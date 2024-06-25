using AtataUITests.Infrastructure;
using AtataUITests.PageObjects;

namespace AtataUITests.Tests
{
    internal class DownloadUploadTests : UITestFixture
    {
        [Test, Description("Donwload file verify file updated")]
        [Category("UI")]
        public void DownloadTest()
        {
            Go.To<DemoQADownloadUploadPage>().
                Download.Click();

            AtataContext.Current.Artifacts.Should.WithinSeconds(10).ContainFile("sampleFile.jpeg");
        }

        [Test, Description("Donwload file then upload same file")]
        [Category("UI")]
        public void UploadTest()
        {
            Go.To<DemoQADownloadUploadPage>().
               Download.Click();

            AtataContext.Current.Artifacts.Should.WithinSeconds(10).ContainFile("sampleFile.jpeg");

            Go.To<DemoQADownloadUploadPage>().
                Upload.Set(HelperMethods.GetArtifactsDirectoryPath() + "\\sampleFile.jpeg").
                UploadedFile.Should.BeVisible();
        }
    }
}
