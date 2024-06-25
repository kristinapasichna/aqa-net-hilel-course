using PlaywrigthUITests.PageObjects;

namespace PlaywrigthUITests.Tests
{
    internal class DynamicPropertiesTests : UITestFixture
    {
        private DemoQADynamicPropertiesPage _demoQADynamicPropertiesPage;

        [SetUp]
        public void SetupDemoQAPage()
        {
            _demoQADynamicPropertiesPage = new DemoQADynamicPropertiesPage(Page);
        }

        [Test, Description("Verify ColorChange button have color black at page init and after 5 sec color red")]
        [Category("UI")]
        public async Task VerifyDynamicColorChange()
        {
            await _demoQADynamicPropertiesPage.GoToDemoQaDynamicPropertiesPage();
            await _demoQADynamicPropertiesPage.GetColorChangeChangeColor("rgb(255, 255, 255)");
            await Task.Delay(5000);
            await _demoQADynamicPropertiesPage.GetColorChangeChangeColor("rgb(220, 53, 69)");
        }

        [Test]
        [Category("UI")]
        public async Task TestEnableAfter()
        {
            await _demoQADynamicPropertiesPage.GoToDemoQaDynamicPropertiesPage();
            await _demoQADynamicPropertiesPage.EnableAfter5sec();
        }

        [Test]
        [Category("UI")]
        public async Task TestVisibleAfter()
        {
            await _demoQADynamicPropertiesPage.GoToDemoQaDynamicPropertiesPage();
            await _demoQADynamicPropertiesPage.VisibleAfter5sec();
        }

        [Test]
        [Category("UI")]
        public async Task TestVisibleAfterClickWait()
        {
            await _demoQADynamicPropertiesPage.GoToDemoQaDynamicPropertiesPage();
            await _demoQADynamicPropertiesPage.VisibleAfter5sec();

        }
    }
}
