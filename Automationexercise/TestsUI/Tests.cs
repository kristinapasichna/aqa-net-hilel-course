using AutomationExercise;
using Automationexercise.PageObjects;
using Microsoft.Playwright;


namespace Automationexercise.TestsUI
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class AutomationExerciseTest : UiTestFixture
    {
        private Common _common;
        private ContactUsPage _contactUsPage;
        private ProductsPage _productsPage;
        private ProductDetailsPage _productDetailsPage;
        private Cart _cart;
        private CheckoutPage _checkoutPage;
        private PaymentPage _paymentPage;
        private DeleteAccountPage _deleteAccountPage;

        [SetUp]
        public Task SetUp()
        {
            _common = new Common(Page, BaseUrl);
            _contactUsPage = new ContactUsPage(Page, BaseUrl);
            _productsPage = new ProductsPage(Page, BaseUrl);
            _productDetailsPage = new ProductDetailsPage(Page, BaseUrl);
            _cart = new Cart(Page, BaseUrl);
            _checkoutPage = new CheckoutPage(Page, BaseUrl);
            _paymentPage = new PaymentPage(Page, BaseUrl);
            _deleteAccountPage = new DeleteAccountPage(Page, BaseUrl);
            return Task.CompletedTask;
        }
        
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
            await _contactUsPage.UploadFileAsync(UploadFilePath);
            Page.Dialog += async (_, dialog) =>
            {
                await dialog.AcceptAsync();
            };
            await _contactUsPage.ClickSubmitButton();
            Assert.That(await _contactUsPage.SuccessMessage.IsVisibleAsync(), "Success message is not visible");
            await _common.ClickHomeButton();
            Assert.That(await _common.HomePageTitle.IsVisibleAsync(), "Home page title is not visible");
        }
       
        [Test]
        [Description("Test Case 8: Verify All Products and product detail page")]
        [TestCase("3")]
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

        [Test]
        [Description("Test Case 13: Verify Product quantity in Cart")]
        [TestCase("3", "4")]
        public async Task VerifyProductQuantityInCart(string productName, string quantity)
        {
            await  _common.HomePageTitle.IsVisibleAsync();
            await _productsPage.ClickViewProductButton(productName);
            await _productDetailsPage.SelectQuantity(quantity);
            await _productDetailsPage.ClickAddToCartButton();
            await _cart.GoShoppingButton.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
            await _cart.ClickViewCartButton();
            Assert.That(await _cart.ProductQuantity(productName), Is.EqualTo(quantity), "Product quantity is not visible");
        }
        
        [Test]
        [Description("Test Case 16: Place Order: Login before Checkout")]
        [TestCase("Men Tshirt", "1")]
        public async Task PlaceOrderLoginBeforeCheckout(string productName, string quantity)
        {
            var user = new User();
            await _common.HomePageTitle.IsVisibleAsync();
            await _cart.AddToCart(productName);
            await _cart.ClickViewCartButton();
            Assert.That(await _cart.IsCartPageVisible(), Is.True,"Cart page is not visible");
            await _cart.ClickProceedToCheckoutButton();
            Assert.That(await _checkoutPage.IsAddressDetailsVisible(user), Is.True,"Address details is not visible");
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
