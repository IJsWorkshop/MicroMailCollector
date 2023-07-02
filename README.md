# MicroMailCollector

<p>Collecting email from a Gmail account using Selenium WebDriver</p>

<h4>Things you will need</h4>
<p></p>
<summary>Selenium.WebDriver</summary>
<summary>SeleniumExtras.WaitHelpers</summary>
<summary>You will also need these methods as the Webdriver wait method doesnt work.</summary>
<p></p>
<p>Use the IsVisible method to wait untill the element loads into the DOM.</p>
<p>Use the Wait method to pause then continue.</p>


```C#
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

static void Delay(TimeSpan ts, bool debugVisible = false)
{
    var stopwatch = Stopwatch.StartNew();

    stopwatch.Start();

    while (stopwatch.Elapsed.TotalSeconds <= ts.Seconds)
    {
        if (debugVisible) Debug.WriteLine(stopwatch.Elapsed.TotalSeconds);
    }

    stopwatch.Stop();
}
