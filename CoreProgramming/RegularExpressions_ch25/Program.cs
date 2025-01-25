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

    }
}