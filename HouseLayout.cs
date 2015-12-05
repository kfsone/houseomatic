using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace houseOmatic
{
    public class HouseLayout : IDisposable
    {
        #region Constants.
        public enum ColumnPosition : int
        {
            TypeID, UID, XPos, ZPos, YPos, XRot, ZRot, YRot, Scale, Crated, Name
        , Max
        };

        public const int SupportedVersionNumber = 6;
        public const string HouseIDTag = "Unique House ID.";
        private string[] ColumnNames = { "TypeID", "HouseID", "XPos", "ZPos", "YPos", "XRot", "ZRot", "YRot", "Scale", "Crated", "Name" };
        private string[] ColumnTypes = { "UInt64", "UInt64", "Decimal", "Decimal", "Decimal", "Decimal", "Decimal", "Decimal", "Decimal", "String", "String" };
        #endregion

        #region Constructors.
        public HouseLayout()
        {   
            pendingChanges = false;
            needSelectionSave = false;
            needFullSave = false;
        }

        ~HouseLayout()
        {
            Dispose(false);
        }

#region IDisposable
        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed) return;
            if (disposing)
            {
                dataset.Dispose();
            }
            disposed = true;
        }
#endregion

        public bool CanSave(bool partial)
        {
            if (pendingChanges)
                return true;
            if (partial)
                return needSelectionSave;
            return needFullSave;
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="newFilename">Name of the file to load.</param>
        public HouseLayout(string newFilename)
        {
            // Save the filename.
            filename = newFilename;

            // Count the line we're currently on.
            lineNo = 0;
            StreamReader sr = null;
            try
            {
                // Lets see if we can open the file.
                try
                {
                    sr = new StreamReader(filename);
                }
                catch (FileNotFoundException)
                {
                    throw new CSVError("The layout file is either missing or not accessible.\n\nDid you erase the file?");
                }

                // First line should be the version number.
                ReadVersionLine(sr);

                // Second line should be the house and layout line.
                ReadLayoutLine(sr);

                // v6: Third line is the unique house ID - presumably to stop people ripping houses off.
                ReadHouseIDLine(sr);

                // Keep going until there doesn't appear to be anything left.
                while (sr.Peek() >= 0)
                {
                    // Get the next row from the stream.
                    string[] row = ReadRow(sr, (int)ColumnPosition.Max);
                    // Check it had the expected number of columns.
                    if (row.Length < (int)ColumnPosition.Max)
                        throw new CSVError(String.Format("Uh oh, there's some data I don't understand on line #{0} of that layout file (one or more columns appeared to be missing).", lineNo));

                    // Create an ID from the first two columns.
                    string id = String.Format("{0},{1}", row[0], row[1]);
                    // Make sure this ID hasn't already been assigned.
                    if(idToRowMap.ContainsKey(id))
                        throw new CSVError(String.Format("Duplicate ID for '{0}' ({1},{2}) on line {3}", row[(int)ColumnPosition.Name], row[0], row[1], lineNo));

                    // Add the id<->row number mapping/
                    idToRowMap[id] = rows.Count;

                    // Add to the list of rows.
                    rows.Add(row);
                }

                if (rows.Count == 0)
                    throw new CSVError("The layout file appears to be empty - was there anything in your house?");
            }
            catch (System.IO.IOException e)
            {
                throw new CSVError(String.Format("There was a system error reading the layout file: {0}", e.Message));
            }
            catch (System.OutOfMemoryException)
            {
                throw new CSVError("The computer ran out of memory trying to load the file. Either the file is too big or there is a bug ('memory in leak') in houseOmatic :(");
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }
        }
        #endregion

        #region Member Functions

        /// <summary>
        /// Regenerate the DataSet object for the GridView.
        /// </summary>
        /// <param name="itemName">If not the empty string, limits items in the grid view to those matching itemName.</param>
        public void BuildDataSet(string itemName)
        {
            // Delete the old data set.
            if (dataset != null)
            {
                dataset.Dispose();
                dataset = new DataSet();
            }

            // How many places to round decimals to
            Decimal decimalPlaces = Decimal.Parse("1.0e8", System.Globalization.NumberStyles.Any);

            // New table to work in.
            DataTable dt = new DataTable(filename);
            try
            {
                // Populate the columns from the ColumnNames list.
                for (int i = 0; i < ColumnNames.Length; ++i)
                {
                    var newCol = new DataColumn(ColumnNames[i], Type.GetType("System." + ColumnTypes[i]));
                    dt.Columns.Add(newCol);
                }

                // Populate the rows.
                for (int r = 0; r < rows.Count; ++r)
                {
                    string[] row = rows[r];
                    // If an itemname is supplied, skip rows which don't match it.
                    if (itemName != "" && row[(int)ColumnPosition.Name] != itemName)
                        continue;

                    // Create a row.
                    DataRow dr = dt.NewRow();
                    // Copy cells across.
                    for (int i = 0; i < ColumnNames.Length; ++i)
                    {
                        string value = row[i];

                        // Truncate decimal columns to N places.
                        if (ColumnTypes[i] == "Decimal")
                        {
                            Decimal dv = Convert.ToDecimal(value);
                            dv = Math.Floor(dv * decimalPlaces) / decimalPlaces;
                            value = Convert.ToString(dv);
                            row[i] = value;
                        }

                        // Strings need quotes trimming off the start/end.
                        if (i == (int)ColumnPosition.Name)
                        {
                            value = value.Trim(new char[] { '"' });
                            rows[r][i] = value;
                        }

                        // Assign the final value to the datarow.
                        dr[ColumnNames[i]] = value;
                    }

                    // Add to the row list.
                    dt.Rows.Add(dr);
                }

                // Create a new dataset to house the table.
                dataset.Tables.Add(dt);

                // There are no changes outstanding at this point.
                pendingChanges = false;

                // However, if this is a subset of a full file,
                // provide the user with the opportunity to save
                // *just* this list.
                needSelectionSave = (itemName != "");
            }
            catch (Exception)
            {
                dt.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Process a change to a column.
        /// </summary>
        /// <param name="gridRowIndex">?</param>
        /// <param name="columnIndex">?</param>
        public void AdjustValue(int gridRowIndex, int columnIndex)
        {
            ///TODO:
            /// Go through and see if there are any changes outstanding.
            pendingChanges = true;
        }

        /// <summary>
        /// Retrieve a list of unique item names in this layout.
        /// </summary>
        /// <returns>Array of item names.</returns>
        public string[] ItemNames()
        {
            List<string> names = new List<string>();
            foreach (var row in rows)
            {
                names.Add(row[(int)ColumnPosition.Name].Trim(new char[] { '"' }));
            }
            names = names.Distinct().ToList();
            names.Sort();
            return names.ToArray();
        }

        /// <summary>
        /// Save the currently viewed data set.
        /// When allData is true, the file is considered to be partial
        /// and the user will need to save all data again if they want
        /// to repopulate the file completely.
        /// </summary>
        /// <param name="allData"></param>
        public void SaveData(bool allData)
        {
            StreamWriter sw = null;
            string tempFilename = filename + ".new";
            try
            {
                sw = new StreamWriter(tempFilename);
            }
            catch (IOException)
            {
                throw new CSVError("I was unable to write to the layout file; do you have it open in another program?");
            }

            try
            {
                // Print the header line.
                sw.WriteLine(String.Format("{0},{1}", SupportedVersionNumber, "Version Number"));
                // Print the layout line.
                sw.WriteLine(String.Format("{0},{1} ", houseID, layoutName));
                // v6: Print the house ID line.
                sw.WriteLine(String.Format("{0}, {1} ", uniqueHouseID, HouseIDTag));
                // Now write all of the data columns currently on display.
                foreach (DataRow row in dataset.Tables[0].Rows)
                {
                    object[] items = row.ItemArray.ToArray();

                    // Update our 'rows' entry in the full data table.
                    int srcIndex = idToRowMap[String.Format("{0},{1}", items[0].ToString(), items[1].ToString())];
                    var srcRow = rows[srcIndex];
                    for (int i = 0; i < items.Length; ++i)
                    {
                        switch (ColumnTypes[i])
                        {
                            case "Decimal":
                            {
                                Decimal d = Convert.ToDecimal(items[i]);
                                srcRow[i] = d.ToString("0.00000000");
                                break;
                            }
                            default:
                                srcRow[i] = items[i].ToString().Trim();
                                break;
                        }
                    }

                    // Quote the name.
                    int NameIdx = (int)ColumnPosition.Name;
                    srcRow[NameIdx] = String.Format("\"{0}\"", items[NameIdx]);

                    // Save the row.
                    sw.WriteLine(String.Join(",", srcRow));
                }

                sw.Close();
            }
            catch (Exception)
            {
                File.Delete(tempFilename);
                throw;
            }

            if (File.Exists(filename))
            {
                string backup01 = filename + ".backup01";
                string backup02 = filename + ".backup02";
                try
                {
                    File.Delete(backup02);
                    File.Move(backup01, backup02);
                }
                catch (IOException) { }
                try
                {
                    File.Move(filename, backup01);
                    File.Move(tempFilename, filename);
                }
                catch (IOException)
                {
                    throw new CSVError("There was a problem saving your file - it has been saved as " + tempFilename + " instead.");
                }
            }

            pendingChanges = false;
            needSelectionSave = false;
            // If we just saved a partial selection,
            // the user needs the opportunity to save
            // the full file back to disk again.
            needFullSave = !allData;
        }

        #endregion

        #region CSV Parsing Routines
        /// <summary>
        /// Read the new row of columns, if any, from the given stream.
        /// </summary>
        /// <param name="sr">Stream to read.</param>
        /// <param name="maxNumColumns">Max number of columns to pay attention to.</param>
        /// <returns>The columns in the row as an array of strings.</returns>
        private string[] ReadRow(StreamReader sr, int maxNumColumns)
        {
            while (sr.Peek() >= 0)
            {
                lineNo++;

                string line = sr.ReadLine();
                line.Trim();
                // Ignore empty and comment lines.
                if (line.Length <= 0 || line[0] == ';' || line[0] == '#')
                    continue;
                string[] columns = line.Split(',');
                if ( columns.Length > maxNumColumns )
                    Array.Resize(ref columns, maxNumColumns) ;
                for (var i = 0; i < columns.Length; ++i)
                    columns[i] = columns[i].Trim();
                return columns;
            }
            return null;
        }

        /// <summary>
        /// The first line of each file is a version declaration.
        /// It should contain a version number followed by "Version Number".
        /// </summary>
        /// <param name="sr">Stream to read from.</param>
        private void ReadVersionLine(StreamReader sr)
        {
            if (sr.Peek() < 0)
                throw new CSVError("The layout file appears to be completely empty! Try saving the layout again.");
            var row = ReadRow(sr, 3);
            if (row.Length != 2)
                throw new CSVError("Unrecognized/unsupported layout file format.");
            if (row[1] != "Version Number")
                throw new CSVError("Doesn't look like an EQ2 layout file to me.");
            UInt64 versionNo = Convert.ToUInt64(row[0]);
            if (versionNo != SupportedVersionNumber)
                throw new CSVError(String.Format("houseOmatic only supports EQ2 Layout version #{0}, the layout file is version #{1}"
                                                    , SupportedVersionNumber
                                                    , row[0]));
        }

        /// <summary>
        /// Read the layout description line (line #2)
        /// </summary>
        /// <param name="sr">Reader to read from.</param>
        private void ReadLayoutLine(StreamReader sr)
        {
            if (sr.Peek() < 0)
                throw new CSVError("Layout file is missing its layout description line.");
            var row = ReadRow(sr, 3);
            if (row.Length != 2)
                throw new CSVError("I didn't understand the second line of the file.");
            houseID = Convert.ToUInt64(row[0]);
            if (houseID == 0)
                throw new CSVError("HouseID of the layout line looks bad.");
            if (row[1].Length == 0)
                throw new CSVError("LayoutName of the layout line looks bad.");
            layoutName = row[1];
        }

        /// <summary>
        /// Read the unique house ID line (line #3)
        /// </summary>
        /// <param name="sr">Reader to read from.</param>
        private void ReadHouseIDLine(StreamReader sr)
        {
            if (sr.Peek() < 0)
                throw new CSVError("Layout file is missing its UniqueHouseID line.");
            var row = ReadRow(sr, 3);
            if (row.Length != 2)
                throw new CSVError("Line#3 doesn't look like the Unique HouseID line I was looking for.");
            uniqueHouseID = Convert.ToUInt64(row[0]);
            if (uniqueHouseID == 0)
                throw new CSVError("Line#3 doesn't look like a propery formatted hosueID line.");
            if (row[1] != HouseIDTag)
                throw new CSVError("Line#3 doesn't appear to be correctly formatted.");
        }
        #endregion

        #region Member Values
        /// <summary>
        /// true/false whether there are changes pending.
        /// </summary>
        private bool pendingChanges;
        /// <summary>
        /// Current line number in the current file.
        /// </summary>
        private int lineNo;
        private bool needSelectionSave;     // true/false is the selection saved?
        private bool needFullSave;          // true/false does all data need saving?
        /// <summary>
        /// name of the file this layout was loaded from.
        /// </summary>
        private string filename;
        /// <summary>
        /// SOE's identifier for this particular house.
        /// </summary>
        private UInt64 houseID;
        /// <summary>
        /// SOE's identifier for this particular instance of the house.
        /// </summary>
        private UInt64 uniqueHouseID;
        /// <summary>
        /// The house layout this was loaded from.
        /// </summary>
        private string layoutName;
        /// <summary>
        /// The rows.
        /// </summary>
        private List<string[]> rows = new List<string[]>();
        /// <summary>
        /// DataSet for displaying the edit view.
        /// </summary>
        private DataSet dataset = new DataSet();
        /// <summary>
        /// Map IDs -> row numbers
        /// </summary>
        private Dictionary<string, int> idToRowMap = new Dictionary<string, int>();
        #endregion

        #region Property Functions
        public string Filename
        {
            get { return filename; }
        }
        public string LayoutName
        {
            get { return layoutName; }
        }
        public DataSet DataSet
        {
            get { return dataset; }
        }
        public bool PendingChanges
        {
            get { return pendingChanges; }
        }
        public bool NeedFullSave
        {
            get { return needFullSave; }
        }
        #endregion
    }
}
