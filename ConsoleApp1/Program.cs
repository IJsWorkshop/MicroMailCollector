using static ConsoleApp1.SeleniumDriver;
using System.Collections.Generic;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using OpenQA.Selenium;
using System.Linq;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            // password of email account
            const string passWord = "SuperSecretPassword";
            // chrome path -> file system location
            const string chromeDriverPath = "Your filepath to chrome.exe";

            const string emailaddress = "your_email@gmail.com";
            // Set up ChromeDriver options
            var chromeOptions = new ChromeOptions();
            // create options
            chromeOptions.AddArgument("--start-maximized");
            // setup chromedriver
            var driver = new ChromeDriver(chromeDriverPath, chromeOptions);

            try
            {
                // url
                const string url = @"https://accounts.google.com/signin";
                // navigate to Gmail login page
                driver.Navigate().GoToUrl(url); 
                // find and fill in the email field
                driver.FindElement(By.Id("identifierId")).SendKeys(emailaddress);
                // click the next button
                driver.FindElement(By.Id("identifierNext")).Click();
                // try to detect element for a maximum of 7 seconds
                driver.IsVisible(By.XPath("//*[@id='password']/div/div/div/input"), TimeSpan.FromSeconds(7));
                // send information to element and add keystroke
                driver.FindElement(By.XPath("//*[@id='password']/div/div/div/input")).SendKeys(passWord);
                // find next button and click
                driver.FindElement(By.Id("passwordNext")).Click();
                // wait then load page
                driver.Wait(TimeSpan.FromSeconds(6)).Navigate().GoToUrl("https://mail.google.com/mail/u/0/#inbox");
                // init collection
                var EmailDetails = new Dictionary<string, int>();

                var emailDetailsList = new List<Details>();
                
                var dict = new Dictionary<string, string>();
                // get table with id=':'
                var trows = driver.FindElements(By.XPath("//table[contains(@id,':')]/tbody//tr"));

                int x = 0;
                // iterate table rows
                foreach (var r in trows)
                {
                    var spans = r.FindElements(By.CssSelector("span"));
                    // iterate the spans collected in the table row
                    foreach (var s in spans)
                    {
                        try
                        {
                            // get email
                            var isEmail = s.FindElement(By.ClassName("yP")).GetAttribute("email");

                            dict.TryGetValue(s.Text, out string value);

                            if (value == null)
                            {
                                dict.Add(isEmail, isEmail);
                            }
                        }
                        catch
                        { }

                        try
                        {
                            // remove random artifacts
                            var t = s.Text.TrimStart(' ', '-').Replace("\r\n", "");

                            dict.TryGetValue(t, out string value);

                            if (value == null)
                            {
                                dict.Add(t, t);
                            }
                        }
                        catch
                        { }
                    }

                    // build from dictionary
                    var vals = dict.Values.ToList();

                    // remove null value
                    for (var ndx = vals.Count - 1; ndx > 0; ndx--)
                    {
                        if (vals[ndx] == "")
                        {
                            vals.RemoveAt(ndx);
                        }
                    }

                    // map values from dictionary -> type
                    dict = new Dictionary<string, string>();

                    var d = new Details();
                    d.Sender = vals[1];
                    d.Name = vals[2];
                    d.Title = vals[3];
                    d.Body = vals[4];
                    d.Time = vals[5].ToDateTime();

                    emailDetailsList.Add(d);

                    x++;
                }

                // display collected emails
                Debug.WriteLine("");

                foreach (var item in emailDetailsList)
                {
                    Debug.WriteLine($"Sender :{item.Sender}");
                    Debug.WriteLine($"Name   :{item.Name}");
                    Debug.WriteLine($"Title  :{item.Title}");
                    Debug.WriteLine($"Body   :{item.Body}");
                    Debug.WriteLine($"Time   :{item.Time}");
                    Debug.WriteLine("");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Something went wrong : {e.Message}");
            }
            finally
            {
                // close the browser
                driver?.Quit();
            }
        }

        public class Details
        {
            public string Sender { get; set; } = default;
            public string Name { get; set; } = default;
            public string Title { get; set; } = default;
            public string Body { get; set; } = default;
            public string Time { get; set; } = "Jan 01";
        }
    }
}