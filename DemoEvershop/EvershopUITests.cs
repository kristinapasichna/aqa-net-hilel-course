using DemoEverShop.PageObjects;

namespace DemoEverShop
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class ExampleTest : UiTestFixture
    {
        private DemoShopPage _demoShopPage;
        [SetUp]
        public void SetupDemoQaPage()
        {
            _demoShopPage = new DemoShopPage(Page);
        }
        private async Task CustomerUserLogin(string email, string password)
        {
            
            await Page.APIRequest.PostAsync(BaseUrl + "/customer/login", new()
            {
                DataObject = new Dictionary<string, object>()
                {
                    { "email", email},
                    { "password", password }
                }
            });
        }
            
        [Test]
        [TestCase("qatest-kristy@mailinator.com", "Kristy", "Continental 80 shoes", "Pink", "L")]
        public async Task SmokeTest(string email, string password, string productName, string color, string size)
        {
            await CustomerUserLogin(email, password);
            await _demoShopPage.GoToDemoEverShopPage();
            await _demoShopPage.OpenKidsShoesCatalog();
            await _demoShopPage.OpenProductDetails(productName);
            await _demoShopPage.SelectColor(color);
            await _demoShopPage.SelectSize(size);
            await _demoShopPage.ClickAddToCartButton();
            await _demoShopPage.ClickViewCartButton();
            await _demoShopPage.ClickCheckoutButton();
            Assert.That(await _demoShopPage.CheckoutForm.IsVisibleAsync(), "Checkout form is not visible. User is not logged in");
        } 
    }
}
