using System;
using System.Windows.Forms;

namespace houseOmatic {
    [Serializable]
    public class CSVError : Exception {
        public CSVError() : base()
        { }
        public CSVError(string msg) : base(msg)
        { }
        public CSVError(string msg, Exception inner) : base(msg, inner)
        { }
    }

    static class HouseOMatic {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ShowUpdatedReadme();

            Application.Run(new HouseForm());
        }

        /// <summary>
        /// Check if the readme has been updated, and if so,
        /// show the latest version.
        /// </summary>
        static void ShowUpdatedReadme()
        {
            Properties.Settings.Default.Upgrade();
            Properties.Settings.Default.Save();
            System.IO.FileInfo fi = new System.IO.FileInfo("ReadMe.htm");
            if (fi.Exists == false)
                return;
            var lastReadMeUpdate = fi.LastWriteTimeUtc.ToBinary();
            if (lastReadMeUpdate > Properties.Settings.Default.lastShowedReadme) {
                try {
                    System.Diagnostics.Process.Start("ReadMe.htm");
                } catch {
                    MessageBox.Show("I was going to show you the new readme, but there was an error =/", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                Properties.Settings.Default.lastShowedReadme = lastReadMeUpdate + 1;
                Properties.Settings.Default.Save();
            }
        }
    }
}
