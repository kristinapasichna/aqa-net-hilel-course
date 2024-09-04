using System.Text;
using AutomationExercise.API;
using AutomationExercise.Models;
using Automationexercise.PageObjects;
using AutomationExercise.TestData;
using Microsoft.Playwright;

namespace Automationexercise.TestsUI
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class AutomationExerciseTest
    {
        private IPage Page { get; set; }
        private IBrowserContext Context { get; set; }
        
        private IBrowser _browser;
        private Common _common;
        private UserApi _userApi;
        private User _user;
        private Cart _cart;
        private CheckoutPage _checkoutPage;
        private ContactUsPage _contactUsPage;
        private DeleteAccountPage _deleteAccountPage;
        private ProductsPage _productsPage;
        private ProductDetailsPage _productDetailsPage;
        private PaymentPage _paymentPage;
        
        [OneTimeSetUp]
        public async Task SetupUser()
        {
            _userApi = new UserApi(Helpers.BaseUrl);
            _user = new User();
                
            var userDetails = await _userApi.GetUserDetailByEmailAsync(_user.Email);

            if (userDetails.Contains(_user.Email) == false)
            {
                var isUserCreated = await _userApi.CreateUserAsync(_user);
                if (isUserCreated == false) throw new Exception("User was not created");
            }
        }
        
        [SetUp]
        public async Task SetUp()
        {
            var playwrightDriver = await Playwright.CreateAsync();
            _browser = await playwrightDriver.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });

            var fileInfo = new FileInfo(Helpers.AuthStorageFilePath);

            if (!fileInfo.Exists)
            {
                Directory.CreateDirectory(fileInfo.Directory!.FullName);
                using (FileStream fs = File.Create(Helpers.AuthStorageFilePath))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("");
                    fs.Write(info, 0, info.Length);
                }
            }

            Context = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize
                {
                    Width = 1920,
                    Height = 1080
                },
                StorageStatePath = Helpers.AuthStorageFilePath
            });

            Page = await Context.NewPageAsync();
            await Page.GotoAsync(Helpers.BaseUrl);
            
            _user = new User();
            _common = new Common(Page, Helpers.BaseUrl);
            _contactUsPage = new ContactUsPage(Page, Helpers.BaseUrl);
            _productsPage = new ProductsPage(Page, Helpers.BaseUrl);
            _productDetailsPage = new ProductDetailsPage(Page, Helpers.BaseUrl);
            _cart = new Cart(Page, Helpers.BaseUrl);
            _checkoutPage = new CheckoutPage(Page, Helpers.BaseUrl);
            _paymentPage = new PaymentPage(Page, Helpers.BaseUrl);
            _deleteAccountPage = new DeleteAccountPage(Page, Helpers.BaseUrl);
     
            var userLoggedIn = Page.Locator($"//li/*[contains(text(),'Logged in as')]/*[contains(text(),'{_user.Name}')]");
            if (await userLoggedIn.IsHiddenAsync())
            {
                await Page.GotoAsync(Helpers.BaseUrl + "/login");
                await Page.Locator("[data-qa='login-email']").FillAsync(_user.Email);
                await Page.Locator("[data-qa='login-password']").FillAsync(_user.Password);
                await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
                
                if (await userLoggedIn.IsVisibleAsync() == false)
                {
                    throw new Exception("User was not logged in");
                }
                await Context.StorageStateAsync(new()
                {
                    Path = Helpers.AuthStorageFilePath
                });
            }
        }
        
        [TearDown]
        public async Task TearDown()
        {
            await Page.CloseAsync();
            await _browser.CloseAsync();
        }
        
        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await Page.CloseAsync();
            await _browser.CloseAsync();
            var userDetails = await _userApi.GetUserDetailByEmailAsync(_user.Email);
            if (userDetails.Contains(_user.Email))
            {
                var isUserDeleted = await _userApi.DeleteUserAsync(_user);
                if (isUserDeleted == null) throw new Exception("User was not deleted");
                Assert.That(isUserDeleted, Is.EqualTo("{\"responseCode\": 200, \"message\": \"Account deleted!\"}"), "User was not deleted");
            }
            else
            {
                Console.WriteLine("User not found");
            }
        }
        
        [Order(1)]
        [Test]
        [Description("Test Case 6: Contact Us Form")]
        [TestCase("John Doe", "jondoe@mailinator.com", "Subject text")]
        public async Task VerifyContactUsForm(string name, string email, string subject)
        {
            await _common.HomePageTitle.IsVisibleAsync();
            await _common.ClickContactUsButton(); 
            Assert.That(await Page.GetByText("Get In Touch").IsVisibleAsync(), "'Get In Touch' title is not visible");
            await _contactUsPage.FillInName(name);
            await _contactUsPage.FillInEmail(email);
            await _contactUsPage.FillInSubject(subject);
            await _contactUsPage.UploadFileAsync(Helpers.UploadFilePath);
            Page.Dialog += async (_, dialog) =>
            {
                await dialog.AcceptAsync();
            };
            await _contactUsPage.ClickSubmitButton();
            Assert.That(await _contactUsPage.SuccessMessage.IsVisibleAsync(), "Success message is not visible");
            await _common.ClickHomeButton();
            Assert.That(await _common.HomePageTitle.IsVisibleAsync(), "Home page title is not visible");
        }
       
        [Order(2)]
        [Test]
        [Description("Test Case 8: Verify All Products and product detail page")]
        [TestCase("Winter Top")]
        public async Task VerifyAllProductsAndProductDetailPage(string productName)
        {
            await _common.HomePageTitle.IsVisibleAsync();
            await _common.ClickProductsButton();
            Assert.That(await Page.GetByText("All Products").IsVisibleAsync(), "'All Products' title is not visible");
            Assert.That(await _productsPage.ProductListIsVisible(), Is.True, "Product list is not visible");
            await _productsPage.ClickViewProductButton(productName);
            Assert.That(Page.Url, Does.Contain("/product_details/"), "Product detail page is not visible");
            Assert.That(await _productDetailsPage.ProductName.IsVisibleAsync(), "Product name is not visible");
            Assert.That(await _productDetailsPage.ProductCategory.IsVisibleAsync(), "Product category is not visible");
            Assert.That(await _productDetailsPage.ProductPrice.IsVisibleAsync(), "Product price is not visible");
            Assert.That(await _productDetailsPage.ProductAvailability.IsVisibleAsync(), "Product availability is not visible");
            Assert.That(await _productDetailsPage.ProductCondition.IsVisibleAsync(), "Product condition is not visible");
            Assert.That(await _productDetailsPage.ProductBrand.IsVisibleAsync(), "Product brand is not visible");
        }
        
        [Order(3)]
        [Test]
        [Description("Test Case 9: Search Product")]
        [TestCase("top")]
        public async Task SearchProduct(string productName)
        {
            await _common.HomePageTitle.IsVisibleAsync();
            await _common.ClickProductsButton();
            Assert.That(await Page.GetByText("All Products").IsVisibleAsync(), "'All Products' title is not visible");
            await _productsPage.SearchProduct(productName);
            Assert.That(await Page.GetByText("SEARCHED PRODUCTS").IsVisibleAsync(), "'SEARCHED PRODUCTS' title is not visible");
            Assert.That(await _productsPage.IsProductListContainsRelatedProducts(productName), Is.True, "Product list is not visible");
        }

        [Order(4)]
        [Test]
        [Description("Test Case 11: Verify Subscription in Cart page")]
        public async Task VerifySubscriptionInCartPage()
        {
            await _common.HomePageTitle.IsVisibleAsync();
            await _common.ClickCartButton();
            await _common.ScrollToFooter();
            Assert.That(await _common.SubscriptionTitle.IsVisibleAsync(), "'SUBSCRIPTION' text is not visible");
            await _common.EnterEmailInSubscriptionInput("jondoe@mailinator.com");
            await _common.ClickSubscriptionArrowButton();
            Assert.That(await _common.SubscribeSuccessMessage.InnerTextAsync(), Contains.Substring("You have been successfully subscribed!"),
                "Subscription success message is not visible");
        }

        [Order(5)]
        [Test]
        [Description("Test Case 13: Verify Product quantity in Cart")]
        [TestCase("Winter Top", "4")]
        public async Task VerifyProductQuantityInCart(string productName, string quantity)
        {
            await _common.HomePageTitle.IsVisibleAsync();
            await _productsPage.ClickViewProductButton(productName);
            await _productDetailsPage.SelectQuantity(quantity);
            await _productDetailsPage.ClickAddToCartButton();
            await _cart.GoShoppingButton.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
            await _cart.ClickViewCartButton();
            Assert.That(await _cart.GetProductQuantity(productName), Is.EqualTo(quantity), "Product quantity is not visible");
        }
        
        [Order(6)]
        [Test]
        [Description("Test Case 16: Place Order: Login before Checkout")]
        [TestCase("Blue Top", "1")]
        public async Task PlaceOrderLoginBeforeCheckout(string productName, string quantity)
        {
            await _common.HomePageTitle.IsVisibleAsync();
            await _cart.AddToCart(productName);
            await _cart.ClickViewCartButton();
            Assert.That(await _cart.IsCartPageVisible(), Is.True,"Cart page is not visible");
            await _cart.ClickProceedToCheckoutButton();
            Assert.That(await _checkoutPage.IsAddressDetailsVisible(_user), Is.True,"Address details is not visible");
            Assert.That(await _checkoutPage.IsReviewYourOrderVisible(productName, quantity), Is.True,"Review your order is not visible");
            await _checkoutPage.EnterDescriptionInCommentField("This is a test order");
            await _checkoutPage.ClickPlaceOrderButton();
            await _paymentPage.EnterPaymentDetails("John Doe", "1234567890123456", "123", "12","25");
            await _paymentPage.ClickPayAndConfirmOrderButton();
            Assert.That(await Page.GetByText("Congratulations! Your order has been confirmed!").IsVisibleAsync(), "Order Placed success message is not visible");
            await _common.ClickDeleteAccountButton();
            Assert.That(await _deleteAccountPage.AccountDeletedMessage.InnerTextAsync(), Contains.Substring("ACCOUNT DELETED!"), "Account deleted message is not visible");
            await _deleteAccountPage.ClickContinueButton();
        }
    }
}