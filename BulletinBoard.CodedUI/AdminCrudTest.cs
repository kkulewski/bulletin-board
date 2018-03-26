using System;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace BulletinBoard.CodedUI
{
    /// <summary>
    /// Basic admin authentication and CRUD operations test.
    /// </summary>
    [CodedUITest]
    public class AdminCrudTest
    {
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            Playback.Initialize();
            var bw = BrowserWindow.Launch(new Uri("http://localhost:5000"));
            bw.CloseOnPlaybackCleanup = false;
        }

        [TestMethod]
        public void Admin_CanPerform_CrudActions()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test"
            // from the shortcut menu and select one of the menu items.
            this.UiMap.Admin_LogsIn();
            this.UiMap.Admin_AddsFirstJobType();
            this.UiMap.Admin_AddsSecondJobType();
            this.UiMap.Admin_AddsFirstJobCategory();
            this.UiMap.Admin_ChecksFirstJobCategoryDetails();
            this.UiMap.Admin_AddsSecondJobCategory();
            this.UiMap.Admin_AddsFirstJobOffer();
            this.UiMap.Admin_UpdatesFirstJobOffer();
            this.UiMap.Admin_LogsOut_AndChecksAddedJobOffer();
            this.UiMap.Admin_LogsInAgain();
            this.UiMap.Admin_DeletesJobOffer();
            this.UiMap.Admin_DeletesJobTypes();
            this.UiMap.Admin_DeletesJobCategories();
            this.UiMap.Admin_NavigatesToHome_AndLogsOut();
        }

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        public UIMap UiMap => _map ?? (_map = new UIMap());

        private UIMap _map;
    }
}
