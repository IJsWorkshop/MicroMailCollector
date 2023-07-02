using System.Diagnostics;
using OpenQA.Selenium;
using System;

namespace ConsoleApp1
{
    public static class SeleniumDriver
    {
        public static IWebDriver Wait(this IWebDriver webDriver, TimeSpan ts, bool debugVisible=false) 
        {
            // delay
            var stopwatch = Stopwatch.StartNew();

            stopwatch.Start();

            while (stopwatch.Elapsed.TotalSeconds <= ts.Seconds)
            {
                if(debugVisible) Debug.WriteLine(stopwatch.Elapsed.TotalSeconds);
            }
            stopwatch.Stop();

            return webDriver;
        }

        static void Delay(TimeSpan ts, bool debugVisible=false) 
        {
            var stopwatch = Stopwatch.StartNew();

            stopwatch.Start();

            while (stopwatch.Elapsed.TotalSeconds <= ts.Seconds) 
            {
                if (debugVisible) Debug.WriteLine(stopwatch.Elapsed.TotalSeconds);
            }

            stopwatch.Stop();
        }

        public static bool IsVisible(this IWebDriver webDriver, By locator, TimeSpan timeOut, bool debugVisible=false) 
        {
            bool isDisplayed = false;

            var sw = Stopwatch.StartNew();

            sw.Start();

            while (!isDisplayed || sw.Elapsed.Seconds <= timeOut.Seconds) 
            {
                IWebElement ele = null;

                try
                {
                    ele = webDriver.FindElement(locator);
                }
                catch (Exception e)  
                {
                    if (debugVisible) Debug.WriteLine($"timeout :{sw.Elapsed.Seconds} : {e.Message}");
                }

                if (ele != null && ele.Displayed) 
                {
                    isDisplayed = true; 
                    sw.Stop();
                    return true;
                }

                Delay(TimeSpan.FromSeconds(1));
            }

            sw.Stop();

            return true;
        }

    }
}
