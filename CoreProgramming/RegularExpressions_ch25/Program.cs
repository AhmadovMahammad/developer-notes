using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        /* Regular Expression Basics
        
        Regular expressions, commonly abbreviated as regex, are a versatile tool for identifying patterns within strings. 
        They allow for both simple and complex text searching and manipulation. 
        At their core, regex patterns describe the structure of the text they aim to match, 
        using specific symbols and constructs.


        
        --- Quantifiers and Optional Elements
        
        Quantifiers play a crucial role in defining how many times a particular character, group, or pattern can appear in a match. 
        The ? quantifier is among the most frequently used and 
        indicates that the preceding element is optional—it can appear either 0 times or 1 time in the string.

        For instance, in the pattern colou?r, the character u is optional. This means the regex will match both "color" and "colour". 
        However, if the character u appears more than once consecutively (e.g., "colouur"), 
        the pattern will fail because the ? quantifier explicitly restricts the optional element to at most one occurrence.

        IEnumerable<string> inputs = ["color", "colour", "colouur"];
        string pattern = @"colo?r";

        foreach (string input in inputs)
        {
            Match match = Regex.Match(input, pattern);
            Console.WriteLine($"Input: {input}, Success: {match.Success}");
        }

        Here, Regex.Match is used to search for a match in a given string. 
        The result is a Match object, which contains details about the search, such as whether a match was found and, 
        if so, where in the string it occurred.

        NOTE:
        To apply the ? quantifier to a specific character, it must be placed immediately after that character. For example:
        colou?r applies ? to the u, making it optional.
        
        If you instead wrote colou?r, but intended the ? to apply to o or the entire "ou", the syntax would need to change.
        To apply ? to more than one character (e.g., to make the entire substring "ou" optional), 
        you need to use parentheses to group the characters:

        (ou)? makes the substring "ou" optional.
        Example: col(ou)?r matches "color" and "colour".



        --- Understanding the Match Object

        When using Regex.Match, the returned Match object provides comprehensive information about the search operation. 
        This includes whether 1. the operation was successful, 2. the index where the match begins, 
        3. the length of the match, and 4. the matched value itself. 
        These properties can be particularly useful when processing text programmatically.

        IEnumerable<string> inputs = ["color", "colour", "colouur"];
        string pattern = @"colou?r";

        foreach (string input in inputs)
        {
            Match match = Regex.Match(input, pattern);
            Console.WriteLine($"Success: {match.Success}");
            Console.WriteLine($"Index: {match.Index}");
            Console.WriteLine($"Length: {match.Length}");
            Console.WriteLine($"Value: {match.Value}");
            Console.WriteLine();
        }

        Success: True
        Index: 0
        Length: 5
        Value: color
        
        Success: True
        Index: 0
        Length: 6
        Value: colour
        
        Success: False
        Index: 0
        Length: 0
        Value:



        --- Comparing Regex.Match with Regex.IsMatch

        While Regex.Match returns detailed information about a single match, 
        Regex.IsMatch is a simpler method that merely checks whether the pattern matches any part of the input string. 
        It is a shortcut for scenarios where the only requirement is to confirm the presence or absence of a match.

        Console.WriteLine(Regex.IsMatch("color", @"colou?r")); // True
        Console.WriteLine(Regex.IsMatch("colouur", @"colou?r")); // False



        --- Handling Multiple Matches

        By default, regular expressions operate from left to right and return only the first match. 
        To find subsequent matches, the NextMatch method can be used on a Match object. 
        Alternatively, the Matches method retrieves all matches in a single operation, returning them as a collection.

        ~~~
        Match firstMatch = Regex.Match("One color? There are two colours in my head!", @"colou?rs?");
        Match nextMatch = firstMatch.NextMatch();

        Console.WriteLine(firstMatch.Value); // color
        Console.WriteLine(nextMatch.Value);  // colours

        Note: The Matches method simplifies the process by providing all matches in a single step:

        foreach (Match match in Regex.Matches("One color? There are two colours in my head!", @"colou?rs?"))
        {
            Console.WriteLine(match.Value);
        }

        In both cases, the pattern colou?rs? accounts for optional elements u and s, 
        matching "color" and "colours" in the input string.



        --- Alternators and Alternatives

        Alternators, represented by the | symbol, define multiple alternative subpatterns. 
        These act like a logical OR, allowing one of several options to match. 
        When used in conjunction with parentheses, alternators can target specific sections of a pattern.

        For example, the pattern Jen(ny|nifer)? matches:

        1. "Jen" when the optional part (ny|nifer) is omitted,
        2. "Jenny" when "ny" is selected as the alternative, and
        3. "Jennifer" when "nifer" is chosen.

        Console.WriteLine(Regex.IsMatch("Jenny", @"Jen(ny|nifer)?"));  // True
        Console.WriteLine(Regex.IsMatch("Jennifer", @"Jen(ny|nifer)?")); // True
        Console.WriteLine(Regex.IsMatch("Jen", @"Jen(ny|nifer)?"));    // True



        --- Timeouts in Regex Matching

        Regex operations can be computationally expensive, particularly with complex patterns and large input strings. 
        To safeguard against excessive execution times, you can specify a timeout for the operation. 
        If the matching process exceeds this time limit, a RegexMatchTimeoutException is thrown.

        This is particularly useful when processing user-defined regex patterns, 
        as it prevents issues caused by poorly constructed patterns.

        try
        {
            Regex regex = new Regex(@"\b(\w+\s*)+\b", RegexOptions.None, TimeSpan.FromMilliseconds(500));
            regex.IsMatch("This is a long text string...");
        }
        catch (RegexMatchTimeoutException)
        {
            Console.WriteLine("Regex operation timed out.");
        }

        */

        /* Complied Regular Expressions
        
        When working with regular expressions in C#, 
        you often encounter scenarios where a specific pattern is used repeatedly for matching. 
        While the static methods of the Regex class, like Regex.Match or Regex.IsMatch, are convenient, 
        they can incur additional overhead because the regular expression engine processes the pattern from scratch 
        each time it is invoked.


        When you use RegexOptions.Compiled, the regular expression pattern you provide is turned into a special piece of code 
        that is customized for that pattern. 
        This process happens only once, when the Regex object is created. 
        After that, this special code is reused every time you need to match something with that pattern.

        This reuse makes matching faster, especially when you use the same pattern many times. 
        However, creating the special code takes a little extra time upfront, 
        so it’s most beneficial for situations where you’ll repeatedly use the same pattern.



        --- Example of Compiled Regex

        Regex r = new Regex(@"sausages?", RegexOptions.Compiled);

        // Performing matches using the compiled regex
        Console.WriteLine(r.Match("sausage").Value);   // Output: sausage
        Console.WriteLine(r.Match("sausages").Value);  // Output: sausages
        Console.WriteLine(r.Match("sausage and eggs").Value);  // Output: sausage

        Each call to r.Match reuses the compiled code, making it efficient for repeated matches.



        --- Performance Considerations

        1. When to Use RegexOptions.Compiled:

        If you need to repeatedly match a specific pattern across many strings, or 
        in a performance-sensitive application, using RegexOptions.Compiled is advantageous.
        The dynamic code generation ensures faster execution after the initial compilation cost.

        2. When Not to Use RegexOptions.Compiled:

        If the pattern is only used a few times or in a one-off operation, 
        the overhead of dynamic code generation may outweigh its benefits.
        Regular expressions without compilation are already fast, often completing simple matches in less than a microsecond.

        */

        /* Character Escapes
        
        --- What are Metacharacters?
        Metacharacters are characters in regular expressions that have a special meaning, 
        such as \, *, +, ?, |, {, [, (, ), ^, $, ., and #. 
        
        If you want to use these characters literally in a pattern, 
        you need to escape them by prefixing them with a backslash (\).


        --- Why Escape Metacharacters?
        When metacharacters are unescaped, they trigger special behaviors. 
        Escaping ensures they are treated as literal characters instead.

        Examples:

        1. Matching the literal question mark ?:
        Console.WriteLine(Regex.Match("what?", @"what\?").Success); // True
        Console.WriteLine(Regex.Match("what?", @"what?").Success);  // False

        In the first pattern, \? escapes the question mark, treating it literally.
        In the second pattern, ? is interpreted as a quantifier.


        2. Matching the backslash (\): Without the @ literal in C#, escaping a backslash requires four backslashes:
        Console.WriteLine(Regex.Match("\\", "\\\\").Success); // True


        --- Regex.Escape and Regex.Unescape

        1. User-Generated Search Patterns
        When building applications like search engines, 
        users might provide input that includes characters with special meanings in regular expressions (e.g., *, +, ?). 
        These inputs can unintentionally break your regex pattern. 
        Regex.Escape ensures such inputs are treated as literals.

        Scenario
        A user searches for the file name file?.txt. Since ? is a metacharacter in regex, it would need escaping.

        string userInput = "file?.txt";
        string escapedPattern = Regex.Escape(userInput); // Escapes special characters, file\?\.txt

        // Using escaped pattern in regex
        Console.WriteLine(Regex.IsMatch("file1.txt", escapedPattern)); // False
        Console.WriteLine(Regex.IsMatch("file?.txt", escapedPattern)); // True


        2. Displaying Regex Patterns to Users
        When displaying regex patterns in a user-friendly interface (e.g., documentation or debug output), 
        escaping helps show the regex exactly as it should be written, without misinterpretation.

        Scenario
        Display a regex pattern to match URLs in a formatted string.

        string urlPattern = @"https?:\/\/\w+\.\w+";
        string escapedPattern = Regex.Escape(urlPattern); // Prepare for display
        Console.WriteLine($"The regex pattern to match URLs is: {escapedPattern}");
        
        // To use it, unescape back
        string usablePattern = Regex.Unescape(escapedPattern);
        Console.WriteLine(Regex.IsMatch("http://example.com", usablePattern)); // True


        */


    }
}