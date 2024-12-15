using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Text;

namespace Selenium;

internal class Program
{
    private static void Main(string[] args)
    {
        /* 1. Setting Up Selenium with C#
        Before diving into coding, add Selenium Packages:

            1. Selenium.WebDriver
            2. Selenium.WebDriver.GeckoDriver
            3. Selenium.Support (for advanced features like wait mechanisms). 

        */

        /* 2. Eight Basic Components
        Everything Selenium does is send the browser commands to do something or send requests for information. 
        Most of what you’ll do with Selenium is a combination of these basic commands 

            2.1. Start the Session
            IWebDriver driver = new ChromeDriver();

            2.2. Take action on browser
            driver.Navigate().GoToUrl("https://www.selenium.dev/selenium/web/web-form.html");

            2.3. Request browser information
            var title = driver.Title;

            2.4. Establish Waiting Strategy

            Synchronizing the code with the current state of the browser is one of the biggest challenges with Selenium, 
            and doing it well is an advanced topic.
            
            Essentially you want to make sure that the element is on the page before you attempt to locate it and 
            the element is in an interactable state before you attempt to interact with it.
            
            An implicit wait is rarely the best solution, 
            but it’s the easiest to demonstrate here, so we’ll use it as a placeholder.
            
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);

            2.5. Find an element
            The majority of commands in most Selenium sessions are element related, 
            and you can’t interact with one without first finding an element
                
            var textBox = driver.FindElement(By.Name("my-text"));
            var submitButton = driver.FindElement(By.TagName("button"));

            2.6. Take action on element
            There are only a handful of actions to take on an element, but you will use them frequently.

            textBox.SendKeys("Selenium");
            submitButton.Click();

            2.7. Request element information
            Elements store a lot of information that can be requested.

            var value = message.Text;

            2.8. End the session
            This ends the driver process, which by default closes the browser as well. No more commands can be sent to this driver instance.

            driver.Quit();

        */

        /* 3. Browser: Firefox
        
        --- Start browser in a specified location
        The binary parameter takes the path of an alternate location of browser to use. 
        For example, with this parameter you can use geckodriver to drive Firefox Nightly instead of 
        the production version when both are present on your computer.

        options.BinaryLocation = GetFirefoxLocation();

        --- Profiles
        There are several ways to work with Firefox profiles.

        var options = new FirefoxOptions();
        var profile = new FirefoxProfile();
        options.Profile = profile;
        var driver = new FirefoxDriver(options);

        */

        /* 4. Waiting Strategies
        Perhaps the most common challenge for browser automation is ensuring that 
        the web application is in a state to execute a particular Selenium command as desired. 
        
        The processes often end up in a race condition where sometimes 
        1. the browser gets into the right state first (things work as intended) and 
        2. sometimes the Selenium code executes first (things do not work as intended). 
        
        This is one of the primary causes of flaky tests. 

        NOTE:
        Similarly, in a lot of single page applications, elements get dynamically added to a page or change visibility based on a click. 
        An element must be both present and displayed on the page in order for Selenium to interact with it.

        The first solution many people turn to is adding a sleep statement to pause the code execution for a set period of time. 
        Because the code can’t know exactly how long it needs to wait, this can fail when it doesn’t sleep long enough. 
        
        Alternately, if the value is set too high and a sleep statement is added in every place it is needed, 
        the duration of the session can become prohibitive.

        Selenium provides two different mechanisms for synchronization that are better.
        
        --- Waiting Strategies

        1. Implicit waits
        Selenium has a built-in way to automatically wait for elements called an implicit wait. 
        An implicit wait value can be set either with the timeouts capability in the browser options, 
        or with a driver method (as shown below).
        
        This is a global setting that applies to every element location call for the entire session. 
        The default value is 0, which means that if the element is not found, it will immediately return an error. 

        If an implicit wait is set, the driver will wait for the duration of the provided value before returning the error. 
        Note that as soon as the element is located, the driver will return the element reference and the code will continue executing, 
        so a larger implicit wait value won’t necessarily increase the duration of the session.
        
        Warning: Do not mix implicit and explicit waits. Doing so can cause unpredictable wait times. 
        For example, setting an implicit wait of 10 seconds and an explicit wait of 15 seconds could cause a timeout to occur after 20 seconds.

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

        2. Explicit waits
        Explicit waits are loops added to the code that poll the application for a specific condition to evaluate as true 
        before it exits the loop and continues to the next command in the code. 
        
        If the condition is not met before a designated timeout value, the code will give a timeout error. 
        Since there are many ways for the application not to be in the desired state,
        explicit waits are a great choice to specify the exact condition to wait for in each place it is needed. 
        Another nice feature is that, by default, the Selenium Wait class automatically waits for the designated element to exist.

        Use explicit waits for dynamic elements that load asynchronously.

        // Find and interact with elements
        IWebElement usernameField = driver.FindElement(By.Name("username"));

        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
        wait.Until(d => usernameField.Displayed);

        */

        /* 5. File Upload
        Because Selenium cannot interact with the file upload dialog, it provides a way to upload files without opening the dialog. 
        If the element is an input element with type file, you can use the send keys method to send the full path to the file that will be uploaded. 

        IWebElement fileInput = driver.FindElement(By.CssSelector("input[type=file]"));
        fileInput.SendKeys(uploadFile); // uploadFile is file path
        driver.FindElement(By.Id("file-submit")).Click();

        */

        /* 6. Locator Strategies
        Selenium provides support for these 8 traditional location strategies in WebDriver:
        
        Locator	            Description
        
        class name	        Locates elements whose class name contains the search value (compound class names are not permitted)

        css selector	    Locates elements matching a CSS selector

        id	                Locates elements whose ID attribute matches the search value
        
        name	            Locates elements whose NAME attribute matches the search value
        
        link text	        Locates anchor elements whose visible text matches the search value
        
        partial link text	Locates anchor elements whose visible text contains the search value. 
                            If multiple elements are matching, only the first one will be selected.

        tag name	        Locates elements whose tag name matches the search value
        
        xpath	            Locates elements matching an XPath expression 


        --- Test HTML Snippet --- 

        <html>
        <body>
            <style>
            .information {
              background-color: white;
              color: black;
              padding: 10px;
            }
            </style>
            
            <h2>Contact Selenium</h2>
            
            <form action="/action_page.php">
              <input type="radio" name="gender" value="m" />Male &nbsp;
              <input type="radio" name="gender" value="f" />Female 
              <br><br>
              <label for="fname">First name:</label><br>
              <input class="information" type="text" id="fname" name="fname" value="Jane"><br><br>
              <label for="lname">Last name:</label><br>
              <input class="information" type="text" id="lname" name="lname" value="Doe"><br><br>
              <label for="newsletter">Newsletter:</label>
              <input type="checkbox" name="newsletter" value="1" /><br><br>
              <input type="submit" value="Submit">
            </form> 
            
            <p>To know more about Selenium, visit the official page 
                <a href ="www.selenium.dev">Selenium Official Page</a> 
            </p>
            
        </body>
        </html>

        1. class name
        The HTML page web element can have attribute class. We can see an example in the above shown HTML snippet. 
        We can identify these elements using the class name locator available in Selenium.

        <input class="information" type="text" id="lname" name="lname" value="Doe">
        driver.FindElement(By.ClassName("information"));

        2. id
        We can use the ID attribute of an element in a web page to locate it. 
        Generally the ID property should be unique for each element on the web page. 
        We will identify the Last Name field using it.

        <input class="information" type="text" id="lname" name="lname" value="Doe">
        driver.FindElement(By.Id("lname"));

        3. name
        We can use the NAME attribute of an element in a web page to locate it. 
        Generally the NAME property should be unique for each element on the web page. 
        We will identify the Newsletter checkbox using it.

        <input type="checkbox" name="newsletter" value="1" /><br><br>
        driver.FindElement(By.Name("newsletter"));

        4. link text
        If the element we want to locate is a link, we can use the link text locator to identify it on the web page. 
        The link text is the text displayed of the link.

        <a href ="www.selenium.dev">Selenium Official Page</a>
        driver.FindElement(By.LinkText("Selenium Official Page"));

        5. partial link text
        If the element we want to locate is a link, we can use the partial link text locator to identify it on the web page. 
        The link text is the text displayed of the link. 

        <a href ="www.selenium.dev">Selenium Official Page</a>
        driver.FindElement(By.PartialLinkText("Official Page"));

        6. tag name
        We can use the HTML TAG itself as a locator to identify the web element on the page. 

        driver.FindElement(By.TagName("a"));

        7. xpath (deep dive)

        XPath (XML Path Language) is a powerful tool used to navigate and locate elements within an XML or HTML document. 
        Selenium uses XPath to identify elements in web automation tasks. 
        Let’s explore XPath thoroughly: 

        1. Basics of XPath
        XPath expressions use a path-like syntax to locate nodes in the DOM (Document Object Model). 
        The structure consists of two main types:

            1.1. Absolute XPath:

            Starts from the root (/) of the HTML document.
            Example: /html/body/div[1]/button
            Downside: Fragile; if any part of the DOM changes, the XPath breaks.

            1.2. Relative XPath:

            Starts with //, meaning it can search from anywhere in the DOM.
            Example: //button[text()='Log in']
            Preferred: More robust and flexible than absolute paths.

        2. Key XPath Syntax and Functions

            2.1. Basic Syntax

            // : Selects nodes anywhere in the document.
            Example: //button (Selects all <button> elements).

            / : Selects children directly under a specific node.
            Example: /html/body/div (Selects <div> inside <body>).


            2.2. Attributes
            
            - [@attribute='value']: Matches elements with a specific attribute and value.
            Example: //button[@type='submit'] (Selects <button> with type="submit").

            - [contains(@attribute, 'value')]: Matches elements where the attribute contains a substring.
            Example: //button[contains(@class, 'login')] (Matches buttons where class contains "login").

            - [starts-with(@attribute, 'value')]: Matches elements where the attribute starts with a substring.
            Example: //button[starts-with(@class, '_acan')].


            2.3 Text Content

            - [text()='value']: Matches elements with specific text.
            Example: //button[text()='Log in'].
            
            - [contains(text(), 'value')]: Matches elements containing specific text.
            Example: //button[contains(text(), 'Log')].


            2.4 Hierarchical Relationships

            - Parent: /..
            Example: //div[@class='child']/.. (Selects the parent of a <div> with class "child").
        
            - Children: /child::tag
            Example: //div[@id='parent']/child::button (Selects child <button> inside a <div>).
            
            - Ancestor: ancestor::tag
            Example: //div[@class='child']/ancestor::div (Selects all ancestor <div> elements).
            
            - Following-sibling: following-sibling::tag
            Example: //button[@id='login']/following-sibling::div (Selects the sibling <div> that comes after a <button> with id="login").


        */

        /* 7. Finding Web elements
        
        <ol id="vegetables">
         <li class="potatoes">…
         <li class="onions">…
         <li class="tomatoes"><span>Tomato is a Vegetable</span>…
        </ol>
        <ul id="fruits">
          <li class="bananas">…
          <li class="apples">…
          <li class="tomatoes"><span>Tomato is a Fruit</span>…
        </ul>


        --- 1. Evaluating entire DOM
        When the find element method is called on the driver instance, 
        it returns a reference to the first element in the DOM that matches with the provided locator. 
        This value can be stored and used for future element actions. In our example HTML above, there are two elements that have a class name of “tomatoes” so 
        this method will return the element in the “vegetables” list.

        var vegetable = driver.FindElement(By.ClassName("tomatoes"));

        --- 2. Evaluating a subset of the DOM
        Rather than finding a unique locator in the entire DOM, it is often useful to narrow the search to the scope of another located element. 
        In the above example there are two elements with a class name of “tomatoes” and 
        it is a little more challenging to get the reference for the second one.

        One solution is to locate an element with a unique attribute that is an ancestor of the desired element and 
        not an ancestor of the undesired element, then call find element on that object:

        IWebElement fruits = driver.FindElement(By.Id("fruits"));
        IWebElement fruit = fruits.FindElement(By.ClassName("tomatoes"));
  
        --- 3. Optimized Locator
        A nested lookup might not be the most effective location strategy since it requires two separate commands to be issued to the browser.
        To improve the performance slightly, we can use either CSS or XPath to find this element in a single command.

        var fruit = driver.FindElement(By.CssSelector("#fruits .tomatoes"));

        --- 4. All matching elements
        There are several use cases for needing to get references to all elements that match a locator, rather than just the first one. 
        The plural find elements methods return a collection of element references. 
        If there are no matches, an empty list is returned. 
        In this case, references to all fruits and vegetable list items will be returned in a collection.

        IReadOnlyList<IWebElement> plants = driver.FindElements(By.TagName("li"));

        */

        /* 8. Interaction with Web elements
         
        --- Click
        The element click command is executed on the center of the element. If the center of the element is obscured for some reason, 
        Selenium will return an element click intercepted error.

	    driver.Navigate().GoToUrl("https://www.selenium.dev/selenium/web/inputs.html");
	    IWebElement checkInput = driver.FindElement(By.Name("checkbox_input"));
	    checkInput.Click();

        --- Send keys
        The element send keys command types the provided keys into an editable element. 
        Typically, this means an element is an input element of a form with a text type or an element with a content-editable attribute. 
        If it is not editable, an invalid element state error is returned.

        IWebElement emailInput = driver.FindElement(By.Name("email_input"));
	    emailInput.Clear();
	    
        String email = "admin@localhost.dev";
	    emailInput.SendKeys(email);

        --- Clear
        The element clear command resets the content of an element. This requires an element to be editable, and resettable. 
        Typically, this means an element is an input element of a form with a text type or an element with acontent-editable attribute. 
        If these conditions are not met, an invalid element state error is returned.

        emailInput.Clear();
	    data = emailInput.GetAttribute("value");

        */

        /* 9. Information about web elements
         
        --- Is Displayed
        This method is used to check if the connected Element is displayed on a webpage. 
        Returns a Boolean value, True if the connected element is displayed in the current browsing context else returns false.

        bool isEmailVisible = driver.FindElement(By.Name("email_input")).Displayed;
        Assert.AreEqual(isEmailVisible, true);

        --- Is Enabled
        This method is used to check if the connected Element is enabled or disabled on a webpage. 
        Returns a boolean value, True if the connected element is enabled in the current browsing context else returns false.

        bool isEnabledButton = driver.FindElement(By.Name("button_input")).Enabled;
        Assert.AreEqual(isEnabledButton, true);

        --- Is Selected
        This method determines if the referenced Element is Selected or not. 
        This method is widely used on Check boxes, radio buttons, input elements, and option elements.
        Returns a boolean value, True if referenced element is selected in the current browsing context else returns false.

        bool isSelectedCheck = driver.FindElement(By.Name("checkbox_input")).Selected;
        Assert.AreEqual(isSelectedCheck, true);

        --- Text Content
        Retrieves the rendered text of the specified element.

        string text = driver.FindElement(By.TagName("h1")).Text;
        Assert.AreEqual(text, "Testing Inputs");

        */

        /* 10. Browser Navigation
        
        --- Navigate to
        The first thing you will want to do after launching a browser is to open your website. This can be achieved in a single line:
        
        driver.Url = "https://selenium.dev";
        driver.Navigate().GoToUrl("https://selenium.dev");

        --- Back
        Pressing the browser’s back button:
        driver.Navigate().Back();

        --- Forward
        Pressing the browser’s forward button:
        driver.Navigate().Forward();

        --- Refresh
        Refresh the current page:
        driver.Navigate().Refresh();

        */

        /* 11. Working with windows and tabs
        
        --- Get window handle
        WebDriver does not make the distinction between windows and tabs. If your site opens a new tab or window, Selenium will let you work with it using a window handle. 
        Each window has a unique identifier which remains persistent in a single session. You can get the window handle of the current window by using

        String currHandle = driver.CurrentWindowHandle;

        --- Switching between windows or tabs
        Clicking a link which opens in a new window will focus the new window or tab on screen, 
        but WebDriver will not know which window the Operating System considers active. To work with the new window you will need to switch to it. 
        
        For this feature, we fetch all window handles, and store them in an array. The array position fills in the order the window is launched. 
        So first position will be default browser, and so on.

        // click on link to open a new window
        driver.FindElement(By.LinkText("Open new window")).Click();

        IList<string> windowHandles = new List<string>(driver.WindowHandles);
        driver.SwitchTo().Window(windowHandles[1]);

        --- Closing a window or tab
        When you are finished with a window or tab and it is not the last window or tab open in your browser, 
        you should close it and switch back to the window you were using previously. 
        Assuming you followed the code sample in the previous section you will have the previous window handle stored in a variable.

        //closing current window 
        driver.Close();
        
        //Switch back to the old tab or window
        driver.SwitchTo().Window(windowHandles[0]);

        */

        // simple code example
        //string firefoxDeveloperEditionPath = @"C:\Program Files\Firefox Developer Edition\firefox.exe";

        //FirefoxOptions options = new FirefoxOptions() { BinaryLocation = firefoxDeveloperEditionPath };
        //options.AddArgument("--start-maximized");

        //IWebDriver driver = new FirefoxDriver(options);
        //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

        //try
        //{
        //    // Open a website
        //    driver.Navigate().GoToUrl("https://www.instagram.com/accounts/login/");
        //    Console.WriteLine("Instagram login page opened successfully!");

        //    // Find and interact with elements
        //    IWebElement usernameField = driver.FindElement(By.Name("username"));
        //    IWebElement passwordField = driver.FindElement(By.Name("password"));

        //    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
        //    wait.Until(d => usernameField.Displayed);

        //    // Locate by button's text
        //    IWebElement loginButton = driver.FindElement(By.XPath("//button[@type='submit']"));

        //    // Enter credentials
        //    usernameField.SendKeys("username");
        //    passwordField.SendKeys("password");

        //    // Click the login button
        //    loginButton.Click();

        //    // Verify login
        //    Console.WriteLine("Login attempted. Waiting for confirmation...");

        //    Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
        //    screenshot.SaveAsFile(@"C:\Users\mahammada\Downloads\screenshot.png");

        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine("An error occurred: " + ex.Message);
        //}
        //finally
        //{
        //    driver.Quit();
        //}
    }
}