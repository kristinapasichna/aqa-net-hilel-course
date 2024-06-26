﻿using AtataUITests.PageObjects;

namespace AtataUITests.Tests
{
    internal class DynamicPropertiesTests
    {
        [Test]
        [Description("Verify ColorChange button have color black at page init and after 5 sec color red")]
        [Category("UI")]
        public void TestChangeColor()
        {
            Go.To<DemoQADynamicPropertiesPage>().
                ColorChange.Css["color"].Should.Be("rgba(255, 255, 255, 1)").
                ColorChange.Css["color"].Should.Be("rgba(220, 53, 69, 1)");
        }

        [Test]
        [Category("UI")]
        public void TestEnableAfter()
        {
            Go.To<DemoQADynamicPropertiesPage>().
                Enable5Sec.Should.BeDisabled().
                Enable5Sec.Should.WithinSeconds(5).BeEnabled();
        }

        [Test]
        [Category("UI")]
        public void TestVisibleAfter()
        {
            Go.To<DemoQADynamicPropertiesPage>().
                VisibleAfter.Should.WithinSeconds(4).Not.BeVisible().
                VisibleAfter.Should.BeVisible();
        }

        [Test]
        [Category("UI")]
        public void TestVisibleAfterClickWait()
        {
            Go.To<DemoQADynamicPropertiesPage>().
                VisibleAfter.Click().
                VisibleAfter.Should.BeFocused();
        }
    }
}
