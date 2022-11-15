using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using ActionChain = OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Chrome;
using ChromeDriverUpdater;

public class WebHelper
{
    ChromeDriver driver;
    bool isInit = false;

    ~WebHelper()
    {
        if (driver != null)
        {
            driver.Close();
            driver.Dispose();
        }
    }

    public void close()
    {
        if (driver != null)
        {
            driver.Close();
            driver.Dispose();
        }
    }

    private void WebInit()
    {
        if (!isInit)
        {
            try
            {
                new ChromeDriverUpdater.ChromeDriverUpdater().Update("C:\\");
            }
            catch (Exception exc)
            {
                // ...
            }

            ChromeOptions options = new ChromeOptions();
            ChromeDriverService service = ChromeDriverService.CreateDefaultService("C:\\");
            driver = new ChromeDriver(service, options);
            driver.Manage().Window.Maximize();
            isInit = true;
        }
    }

    private void GoToFrame(string pFrame)
    {
        // driver.WindowHandles;
        driver.SwitchTo().DefaultContent();
        //Console.WriteLine(driver.FindElements(By.TagName("iFrame")).Count);
        driver.SwitchTo().Frame(driver.FindElements(By.TagName(pFrame))[0]);
    }

    private bool GPortalLoginValidation()
    {
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        try
        {
            IWebElement element = driver.FindElement(By.XPath("/html/body/table/tbody/tr[2]/td/table/tbody/tr[1]/td/font[1]/strong/font"));
            return false;
        }
        catch
        {
            return true;
        }
    }
    public void GPortalLogin(string pID, string pPW, int pCount = 0)
    {
        if (!isInit)
            WebInit();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
        System.Threading.Thread.Sleep(1000);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
        driver.Navigate().GoToUrl("http://gsso.lgensol.com:8001/nls3/cookieLogin.jsp?RTOA=1&UURL=http%3A%2F%2Fgportalapp.lgensol.com%2FssoLogin.do");
        /*while (driver.Url != "http://gportalapp.lgensol.com/portal/main/portalMain.do")
        {
            System.Threading.Thread.Sleep(1000);
            if 
        }
        */
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        driver.SwitchTo().Frame(driver.FindElements(By.TagName("iFrame"))[0]);
        IWebElement element = driver.FindElement(By.Name("userid"));
        element.SendKeys(pID);
        element = driver.FindElement(By.Name("password"));
        element.SendKeys(pPW);
        element.SendKeys("\n");

        if(!GPortalLoginValidation())
        {
            if (pCount == 1)
            {
                driver.Close();
                return;
            }
            GPortalLogin(pID, pPW, 1);
        }

        System.Threading.Thread.Sleep(1000);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
        System.Threading.Thread.Sleep(5000);
        //Login End
    }

    public void GDWFind(string pReportName)
    {
        if (!isInit)
            WebInit();
        //Go to GDW
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
        driver.Navigate().GoToUrl("http://gportalapp.lgensol.com/portal/common/legacy/legacyLink.do?url=http://10.34.221.208:8080/dw/common/login.dev^sLanguage=ko"); 
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
        driver.SwitchTo().Frame(driver.FindElements(By.Id("user_contents"))[0]);
        IWebElement element = driver.FindElement(By.Id("searchMain"));
        element.SendKeys(pReportName);
        element.SendKeys("\n");

        System.Threading.Thread.Sleep(1000);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
        System.Threading.Thread.Sleep(1000);

        driver.SwitchTo().Frame(driver.FindElements(By.Id("contents"))[0]);
        var elements = driver.FindElements(By.XPath("/html/body/div[1]/div/div/div/form[2]/div[2]/div[2]/table/tbody/tr[1]/td[4]/a"));
        elements[0].Click();

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
        System.Threading.Thread.Sleep(5000);
    }

    public void ERPLogin(string pIDNumber)
    {
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
        driver.Navigate().GoToUrl("http://gsso.lgensol.com:8001/sso/ssoErp.jsp?empl_numb=" + pIDNumber + "&flag=gerp"); 
        //driver.FindElements(By.XPath("//*[@id=\"menu-container\"]/li[1]/ul/li[5]/a"));
        //elements[0].Click();
    }

    public void DAVINCILogin(string pLink, string pID, string pPW)
    {
        if (!isInit)
            WebInit();
        
        
        driver.Navigate().GoToUrl(pLink); 
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);

        IWebElement element = driver.FindElement(By.Name("username"));

        element.SendKeys(pID);

        element = driver.FindElement(By.Name("password"));
        
        element.SendKeys(pPW);
        element.SendKeys("\n");

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
    }

    public void DAVINCIDownload(string pReportID)
    {
        WaitElement(By.Id(pReportID));
        IWebElement element = driver.FindElement(By.Id(pReportID));

        element.Click();

        ActionChain.Actions actions = new ActionChain.Actions(driver);
        ActionChain.IAction act = actions.MoveToElement(element).ContextClick().Build();
        act.Perform();

        Thread.Sleep(50);

        element = driver.FindElement(By.XPath("/html/body/div[2]/div[1]/div[2]"));
        element.Click();
        
        Thread.Sleep(50);

        element = driver.FindElement(By.XPath("/html/body/div[3]/div[1]/div[3]"));
        element.Click();

        Thread.Sleep(3000);
    }
    
    public void WebMove(string pURL)
    {
        if (!isInit)
            WebInit();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
        driver.Navigate().GoToUrl(pURL);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
    }

    public void WebClick(string pClick, string pElement, string pType)
    {
        IWebElement element;
        switch (pType)
        {
            case ("XPath") :
                WaitElement(By.XPath(pElement));
                element = driver.FindElement(By.XPath(pElement));
                break;
            case ("Id") :
                WaitElement(By.Id(pElement));
                element = driver.FindElement(By.Id(pElement));
                break;
            case ("ClassName") :
                WaitElement(By.ClassName(pElement));
                element = driver.FindElement(By.ClassName(pElement));
                break;
            default :
                WaitElement(By.XPath(pElement));
                element = driver.FindElement(By.XPath(pElement));
                break;
        }

        switch(pClick)
        {
            case ("LClick") :
                element.Click();
                break;
            case ("RClick") :
                ActionChain.Actions actions = new ActionChain.Actions(driver);
                ActionChain.IAction act = actions.MoveToElement(element).ContextClick().Build();
                act.Perform();
                break;
        }
    }
    
    private void WaitElement(By by)
    {
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
        try
        {
            IWebElement element = driver.FindElement(by);
        }
        catch
        {
            WaitElement(by);
        }
    }
}