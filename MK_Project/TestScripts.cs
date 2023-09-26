using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MK_TestProject1
{

    [TestClass]
    public class TestScripts : Libraries
    {
        [TestInitialize]
        public void Init()
        {
            InitializeDriver("chrome");
            LaunchURL();
        }

        [TestMethod]
        [DataRow("Iphone 14 pro max 128gb", "Space Black", "1")]
        public void AddToCart(string Item, string Color, string Qty)
        {

            Console.WriteLine("Step#1: Find the items.");
            Driver.FindElement(By.XPath("//input[@id='gh-ac']")).SendKeys(Item);
            Driver.FindElement(By.XPath("//input[@id='gh-btn']")).Click();
            Wait(5000);

            Console.WriteLine("Step#2: Verify the results.");
            IWebElement dataTable = Driver.FindElement(By.XPath("//*[@id='srp-river-results']/ul"));
            IList<IWebElement> ItemLists = dataTable.FindElements(By.TagName("li"));
            if (ItemLists.Count > 0)
            {
                Console.WriteLine("Search is successfull.");
            }

            foreach (IWebElement iRows in ItemLists)
            {
                string getName_FromLists = iRows.Text;
                if (getName_FromLists.ToLower().Contains("iphone 14 pro max") && getName_FromLists.ToLower().Contains("128gb"))
                {
                    //Console.WriteLine("Found following item:" + getName_FromLists);
                }
            }

            Console.WriteLine("Step#3: Click on first searched item.");
            Driver.FindElement(By.XPath("//*[@id='item528f042d30']/div/div[2]/a")).Click(); ////*[@id='item364831b5a5']/div/div[2]/a
            Wait(5000);


            //Switch to new tab
            Driver.SwitchTo().Window(Driver.WindowHandles[1]);

            //Handle the popup, if exist
            if (Driver.FindElements(By.XPath("//*[@id='s0-1-21-7-5-tooltip-1-1-overlay']/span[2]/span/button")).Count > 0)
            {
                Driver.FindElement(By.XPath("//*[@id='s0-1-21-7-5-tooltip-1-1-overlay']/span[2]/span/button")).Click();
                Wait(3000);
            }
            //Capture Item and its price
            string beforeItem = Driver.FindElement(By.XPath("//*[@id='mainContent']/div[1]/div[1]/h1/span")).Text;
            string beforePrice = Driver.FindElement(By.XPath("//*[@id='mainContent']/div[2]/div/div[1]/div[1]/div/div[2]/div[1]/span")).Text;

            //Split and remove US
            string[] splitUS = beforePrice.Split("US ");
            beforePrice = splitUS[1];
            //Select Color
            if (Driver.FindElements(By.XPath("//select[@id='x-msku__select-box-1000']")).Count > 0 && Driver.FindElement(By.XPath("//select[@id='x-msku__select-box-1000']")).Enabled)
            {
                var DropDownLists = Driver.FindElement(By.XPath("//select[@id='x-msku__select-box-1000']"));
                var selectColor = new SelectElement(DropDownLists);
                selectColor.SelectByText(Color);
            }

            //Select Storage
            if (Driver.FindElements(By.XPath("//select[@id='x-msku__select-box-1001']")).Count > 0 && Driver.FindElement(By.XPath("//select[@id='x-msku__select-box-1001']")).Enabled)
            {
                var DropDownLists = Driver.FindElement(By.XPath("//select[@id='x-msku__select-box-1001']"));
                var selectStorage = new SelectElement(DropDownLists);
                selectStorage.SelectByText("128 GB");
            }

            //Enter Quantity
            if (Driver.FindElements(By.XPath("//input[@id='qtyTextBox']")).Count > 0 && Driver.FindElement(By.XPath("//input[@id='qtyTextBox']")).Enabled)
            {
                Driver.FindElement(By.XPath("//input[@id='qtyTextBox']")).Clear();
                Driver.FindElement(By.XPath("//input[@id='qtyTextBox']")).SendKeys(Qty);
            }

            Driver.FindElement(By.XPath("//span[contains(text(),'Add to cart')]")).Click(); //Click on 'Add to cart' button
            Wait(5000);

            //Click Go to Cart button
            Driver.FindElement(By.XPath("//span[contains(text(),'Go to cart')]")).Click();
            Wait(5000);

            //Capture Item and its Price
            IWebElement dataTable2 = Driver.FindElement(By.XPath("//*[@id='mainContent']/div/div[3]/div[1]/div[2]/div/ul"));
            IList<IWebElement> ItemLists2 = dataTable2.FindElements(By.TagName("li"));
            if (ItemLists2.Count > 0)
            {
                Console.WriteLine("Successfully added into cart.");
            }

            string[] splitCartDetails = ItemLists2[0].Text.Split("\r\n");
            string AfterItem, AfterPrice;
            AfterItem = splitCartDetails[0];
            AfterPrice = splitCartDetails[13]; ;

            //Click Remove for next run
            Driver.FindElement(By.XPath("//*[@id='mainContent']/div/div[3]/div[1]/div[2]/div/ul/li/div/div/div/div[2]/span[2]/button")).Click();
            Wait(5000);

            Assert.AreEqual(beforeItem, AfterItem);
            Assert.AreEqual(beforePrice, AfterPrice);

            Console.WriteLine("Successfully verified Cart for item, expected: " + beforeItem + ", actual: " + AfterItem);
            Console.WriteLine("Successfully verified Cart for price, expected: " + beforePrice + ", actual: " + AfterPrice);
        }

        [TestCleanup]
        public void TestTearDown()
        {
            Driver.Quit();

        }
    }
}
