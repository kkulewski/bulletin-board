using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;


namespace BulletinBoard.CodedUI
{
    /// <summary>
    /// Summary description for AdminCrudTest
    /// </summary>
    [CodedUITest]
    public class AdminCrudTest
    {
        private static Process _proc;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            Playback.Initialize();
            var bw = BrowserWindow.Launch(new Uri("http://localhost:5000"));
            _proc = bw.Process;
            bw.CloseOnPlaybackCleanup = false;
        }

        [ClassCleanup]
        public static void CleanUp()
        {
            var bw = BrowserWindow.FromProcess(_proc);
            bw.Close();
        }

        [TestMethod]
        public void Admin_CanPerform_CrudActions()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            this.UIMap.Admin_LogsIn();
            this.UIMap.Admin_AddsFirstJobType();
            this.UIMap.Admin_AddsSecondJobType();
            this.UIMap.Admin_AddsFirstJobCategory();
            this.UIMap.Admin_ChecksFirstJobCategoryDetails();
            this.UIMap.Admin_AddsSecondJobCategory();
            this.UIMap.Admin_AddsFirstJobOffer();
            this.UIMap.Admin_UpdatesFirstJobOffer();
            this.UIMap.Admin_LogsOut_AndChecksAddedJobOffer();
            this.UIMap.Admin_LogsInAgain();
            this.UIMap.Admin_DeletesJobOffer();
            this.UIMap.Admin_DeletesJobTypes();
            this.UIMap.Admin_DeletesJobCategories();
            this.UIMap.Admin_NavigatesToHome_AndLogsOut();
        }

        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:

        ////Use TestInitialize to run code before running each test 
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        ////Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        #endregion

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        public UIMap UIMap
        {
            get
            {
                if (this.map == null)
                {
                    this.map = new UIMap();
                }

                return this.map;
            }
        }

        private UIMap map;
    }
}
