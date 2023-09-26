using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MK_TestProject1
{
    public class Libraries
    {

        public static IWebDriver Driver { get; set; }
        static string baseFolder => Directory.GetCurrentDirectory();

        public static void InitializeDriver(string browserName)
        {
            switch (browserName)
            {
                case "ie":
                    //not in scope
                    break;

                case "chrome":
                    ChromeOptions chOptions = new ChromeOptions();
                    chOptions.AddArguments("--disable-popup-blocking");
                    chOptions.AddArguments("--disable-default-apps");
                    try
                    {
                        Driver = new ChromeDriver(baseFolder, chOptions, TimeSpan.FromMinutes(3));
                    }
                    catch (InvalidOperationException)
                    {

                        Console.WriteLine("Chrome WebDriver faild to initialize, please review error message.");
                    }
                    break;

                default:
                    break;
            }
            Driver.Manage().Window.Maximize();
        }

        public static void SuperSetup()
        {
            InitializeDriver("chrome");
        }

        public static void LaunchURL()
        {
            Driver.Navigate().GoToUrl("https://www.ebay.com/");
            Wait(5000);
            if (Driver.FindElements(By.XPath("//input[@id='gh-ac']")).Count > 0)
            {
                Console.WriteLine("Home page is visible and available for testing.");
            }
            else
            {
                Assert.Fail("Home page is NOT visible and unable to continue testing, test aborted.");
            }
        }

        public static void Wait(int timeInMilliseconds)
        {
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.AboveNormal;
            System.Threading.Thread.Sleep(timeInMilliseconds);
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Normal;
        }
    }
}
