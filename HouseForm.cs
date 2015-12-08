using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.IO;
using System.Windows.Forms;

namespace houseOmatic {
    public partial class HouseForm : Form {
        public HouseForm()
        {
            InitializeComponent();
            GuessDefaultPath();
            formName = this.Text;
            try {
                this.Text = formName + " version " + ApplicationDeployment.CurrentDeployment.CurrentVersion;
            } catch (InvalidDeploymentException) {
                this.Text = formName + " [development build]";
            }
        }

        #region Data
        private string formName;
        private int selectedItemIndex = 0;
        private HashSet<int> groupedItems = new HashSet<int>();

        private HouseLayout layout = new HouseLayout();
        #endregion

        /// <summary>
        /// Attempt to guess at the probable path for the
        /// saved_house_layouts folder.
        /// </summary>
        private void GuessDefaultPath()
        {
            /// LAZINESS CAUTION:
            // These are the most likely locations for an English language
            // install of Everquest II.
            string[] basePaths = { "C:\\ProgramData", "C:\\Program Files", "C:\\Program Files (x86)", "C:\\Games" };
            // Where Sony usually puts the game.
            string sonyPath = "Sony Online Entertainment\\Installed Games";
            // I'm an Extended convert myself, so look there first.
            string[] gamePaths = { "EverQuest II Extended", "EverQuest II" };

            foreach (string basePath in basePaths) {
                foreach (string gamePath in gamePaths) {
                    string path = basePath + "\\" + sonyPath + "\\" + gamePath + "\\" + "saved_house_layouts";
                    if (Directory.Exists(path)) {
                        openDlg.InitialDirectory = path;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Handle user clicking the Open button.
        /// </summary>
        /// <param name="sender">Object that generated the event.</param>
        /// <param name="e">The event itself.</param>
        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            // If there are outstanding changes, make sure the user
            // wants to lose any changes they have currently pending
            // before we go ahead and clear things out.
            if (layout.PendingChanges || layout.NeedFullSave) {
                var proceed = MessageBox.Show("If you proceed, your current pending changes will be lost.\n\nAre you sure you want to load a file?"
                                            , "Discard current changes?"
                                            , MessageBoxButtons.YesNo
                                            , MessageBoxIcon.Question);
                if (proceed != DialogResult.Yes)
                    return;
            }

            // Ask the user to locate the file they want to load.
            var result = openDlg.ShowDialog();
            // If they didn't click OK, don't do anything else,
            // keep all the state etc we currently have.
            if (result != DialogResult.OK)
                return;

            try {
                layout = new HouseLayout(openDlg.FileName);

                itemSelect.Items.Clear();
                itemSelect.Items.Add("Show me everything");
                itemSelect.Items.AddRange(layout.ItemNames());
                itemSelect.SelectedIndex = 0;
                itemSelect.Enabled = true;

                ShowLayout("");

                itemSelect.Focus();
            } catch (CSVError err) {
                string msg = $"Unable to load the layout file you selected:\n\n{err.Message}\n\nMake sure the file you selected was an EQ2 Saved House Layout.";
                MessageBox.Show(msg, "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// Update the grid view, optionally limiting the view to just one item type.
        /// </summary>
        /// <param name="selectedName">Empty string, or the name of the item to list.</param>
        private void ShowLayout(string selectedName)
        {
            saveToolStripButton.Enabled = false;
            saveAsToolStripButton.Enabled = true;
            modeLabel.Text = (selectedName == "" ? "File Mode" : "Item Mode");

            groupCountLabel.Text = "";
            groupedItems.Clear();

            itemGrid.Hide();

            layout.BuildDataSet(selectedName);
            var ds = layout.DataSet;

            bs.DataSource = ds;
            bs.DataMember = ds.Tables[0].TableName;
            itemGrid.DataSource = bs;
            itemGrid.Refresh();
            itemGrid.Show();

            saveToolStripButton.Enabled = layout.CanSave(selectedItemIndex > 0);

        }

        /// <summary>
        /// Handle the user selecting an item from the item select drop down.
        /// </summary>
        /// <param name="sender">Object user interacted with.</param>
        /// <param name="e">Event.</param>
        private void itemSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (itemSelect.SelectedIndex == selectedItemIndex)
                return;

            if (layout.PendingChanges == true) {
                if (MessageBox.Show("Changes you've made will be lost. Do you want to continue WITHOUT saving?", "Lose your changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) {
                    itemSelect.SelectedIndex = selectedItemIndex;
                    return;
                }
            }
            selectedItemIndex = itemSelect.SelectedIndex;
            if (itemSelect.SelectedIndex == 0)
                ShowLayout("");
            else
                ShowLayout(itemSelect.Items[itemSelect.SelectedIndex].ToString());
        }

        /// <summary>
        /// Handle the user changing a value in the grid cell. Called *after* validation.
        /// </summary>
        /// <param name="sender">Object that generated the event.</param>
        /// <param name="e">Event.</param>
        private void itemGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore changes to the headers.
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            // Get number of cells selected.
            DataGridView dgv = (DataGridView)sender;
            Int32 selectCount = dgv.GetCellCount(DataGridViewElementStates.Selected);
            for (var i = 0; i < selectCount; ++i) {
                var cell = dgv.SelectedCells[i];
                // Only clone changes in same column.
                if (cell.ColumnIndex != e.ColumnIndex)
                    continue;
                if (cell.RowIndex != e.RowIndex)
                    cell.Value = itemGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                layout.AdjustValue(cell.RowIndex, cell.ColumnIndex);
            }
            layout.AdjustValue(e.RowIndex, e.ColumnIndex);
            saveToolStripButton.Enabled = layout.CanSave(selectedItemIndex > 0);
        }

        /// <summary>
        /// Handle input errors from the user.
        /// </summary>
        /// <param name="sender">object sending the error</param>
        /// <param name="e">error</param>
        private void itemGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show($"Invalid input value: {e.Exception.Message}", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            itemGrid.CancelEdit();
        }

        /// <summary>
        /// Invoked when the user changes a checkbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            string colName = itemGrid.Columns[itemGrid.CurrentCell.ColumnIndex].Name;
            if (colName == "dgvCrated")
                itemGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void itemGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            string colName = itemGrid.Columns[e.ColumnIndex].Name;
            if (colName == "dgvScale") {
                object o = e.FormattedValue;
                var d = Convert.ToDouble(o);
                if (d <= 0 || d > 100) {
                    MessageBox.Show("Scale has to be greater than zero and less than 100, more or less.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.Cancel = true;
                    itemGrid.CancelEdit();
                }
            }
        }

        /// <summary>
        /// Called when user clicks "Save".
        /// </summary>
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            bool allData = (bool)(itemSelect.SelectedIndex == 0);
            layout.SaveData(layout.Filename, allData);
            saveToolStripButton.Enabled = layout.CanSave(selectedItemIndex > 0);
        }
        private void saveAsToolStripButton_Click(object sender, EventArgs e)
        {
            // Ask the user to locate the file they want to load.
            var result = saveAsDlg.ShowDialog();
            // If they didn't click OK, don't do anything else,
            // keep all the state etc we currently have.
            if (result != DialogResult.OK)
                return;

            bool allData = (bool)(itemSelect.SelectedIndex == 0);

            try {
                layout.SaveData(saveAsDlg.FileName, allData);
            } catch (CSVError err) {
                string msg = $"Unable to save the layout file you selected:\n\n{err.Message}\n";
                MessageBox.Show(msg, "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void itemGrid_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            Int32 selectCount = dgv.GetCellCount(DataGridViewElementStates.Selected);
            if (selectCount <= 0)
                return;

            for (var i = HouseLayout.InternalColumns; i < selectCount; ++i) {
                var cell = dgv.SelectedCells[i];
                cell.Style.SelectionBackColor = System.Drawing.Color.SlateGray;
            }

            dgv.SelectedCells[0].Style.SelectionBackColor = System.Drawing.Color.SlateBlue;
        }

        private void itemGrid_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
        }

        private void itemGrid_ToggleItemGroupMembership(object sender, DataGridViewCellEventArgs e)
        {
            if (groupedItems.Contains(e.RowIndex))
                groupedItems.Remove(e.RowIndex);
            else
                groupedItems.Add(e.RowIndex);

            var numGroupedItems = groupedItems.Count;
            if (numGroupedItems == 0) {
                // Nothing grouped, empty label.
                groupCountLabel.Text = "";
            } else {
                groupCountLabel.Text = $"Group of {numGroupedItems:n0}";
            }
        }
    }
}
