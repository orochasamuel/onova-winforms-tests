using Onova;
using Onova.Services;
using System.Reflection;

namespace AppNetOnovaTest
{
    public partial class Form1 : Form
    {
        private readonly IUpdateManager updateManager = new UpdateManager(
        new GithubPackageResolver("orochasamuel", "onova-winforms-tests", "App-*.zip"),
        new ZipPackageExtractor()
    );

        string currentVersion;

        public Form1()
        {
            InitializeComponent();

            currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            lblCurrentVersion.Text = currentVersion;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            // Check for updates
            var check = await updateManager.CheckForUpdatesAsync();

            // If there are none, notify user and return
            if (!check.CanUpdate)
            {
                MessageBox.Show("There are no updates available.");
                return;
            }
            else
            {
                DialogResult result = MessageBox.Show(
                    $"There is an update available. Your current version is {currentVersion}, the latest version is {check.LastVersion}. Would you like to update to the latest version?",
                    "Update Available",
                    MessageBoxButtons.YesNo
                );

                if (result == DialogResult.Yes)
                {
                    // Prepare the latest update
                    await updateManager.PrepareUpdateAsync(check.LastVersion);

                    // Launch updater and exit
                    updateManager.LaunchUpdater(check.LastVersion);
                    Application.Exit();
                }
            }
        }
    }
}