﻿using _ = AtataUITests.PageObjects.DemoQAButtonsPage;

namespace AtataUITests.PageObjects
{
    [Url("/buttons")]
    public sealed class DemoQAButtonsPage : DemoQAPage<_>
    {
        [FindByXPath("//*[@id=\"doubleClickBtn\"]")]
        public Button<_> DoubleClickMe { get; private set; }

        [FindByXPath("//*[@id=\"rightClickBtn\"]")]
        public Button<_> RigthClickMe { get; private set; }

        [ScrollTo]
        public Button<_> ClickMe { get; private set; }

        [FindById("dynamicClickMessage")]
        public Text<_> DinamicClickMessage { get; private set; }

        [FindByXPath ("//*[@id=\"rightClickMessage\"]")]
        public Text<_> RightClickMessage { get; private set; }

        [FindByXPath("//*[@id=\"doubleClickMessage\"]")]
        public Text<_> DoubleClickMessage { get; private set; }
    }
}
