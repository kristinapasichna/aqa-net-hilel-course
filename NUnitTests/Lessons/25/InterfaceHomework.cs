﻿namespace NUnitTests.Lessons._25
{

    public interface IMyWebDriver
    {
        void Open(string url);
        void FindElement(string locator);
        void Close();
    }

    public interface IMyWindowsWebDriver
    {
        string GetWindowsVersion();
    }

    public class ChromeDriver : IMyWebDriver, IMyWindowsWebDriver
    {
        public static readonly string DriverName = "Chrome";
        public void Open(string url)
        {
            Console.WriteLine($"Opening {DriverName}");
        }

        public void FindElement(string locator)
        {
            Console.WriteLine($"Find {locator}");
        }

        public void Close()
        {
            Console.WriteLine($"Closing {DriverName}");
        }

        public string GetWindowsVersion()
        {
            return "Windows 10";
        }
    }

    public class SafariDriver : IMyWebDriver
    {
        public static readonly string DriverName = "Safari";
        public void Open(string url)
        {
            Console.WriteLine($"Opening {DriverName}");
        }

        public void Close()
        {
            Console.WriteLine($"Closing {DriverName}");
        }

        public void FindElement(string locator)
        {
            Console.WriteLine($"Find {locator}");
        }

    }

    public class FirefoxDriver : IMyWebDriver, IMyWindowsWebDriver
    {
        public static readonly string DriverName = "Firefox";

        public void FindElement(string locator)
        {
            Console.WriteLine($"Find {locator}");
        }

        public void Open(string url)
        {
            Console.WriteLine($"Opening {DriverName}");
        }

        public string GetWindowsVersion()
        {
            return "Windows 11";
        }

        public void Close()
        {
            Console.WriteLine($"Closing {DriverName}");
        }
    }
}
