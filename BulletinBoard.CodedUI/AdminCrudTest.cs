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
            UiMap.Admin_LogsIn();
            UiMap.Admin_AddsFirstJobType();
            UiMap.Admin_AddsSecondJobType();
            UiMap.Admin_AddsFirstJobCategory();
            UiMap.Admin_ChecksFirstJobCategoryDetails();
            UiMap.Admin_AddsSecondJobCategory();
            UiMap.Admin_AddsFirstJobOffer();
            UiMap.Admin_UpdatesFirstJobOffer();
            UiMap.Admin_LogsOut_AndChecksAddedJobOffer();
            UiMap.Admin_LogsInAgain();
            UiMap.Admin_DeletesJobOffer();
            UiMap.Admin_DeletesJobTypes();
            UiMap.Admin_DeletesJobCategories();
            UiMap.Admin_NavigatesToHome_AndLogsOut();
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
