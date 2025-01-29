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

        /* Character Sets
        
        Character sets in regular expressions allow you to define a set or range of characters that 
        a single position in the input string can match. 
        These sets act like a wildcard for a specified group of characters, 
        making them a powerful tool for pattern matching.
        
        Character Set Syntax

        1. Square Brackets []: Enclose a list or range of characters you want to match.
        2. Negated Sets [^]: Matches characters not in the specified set.
        3. Ranges [a-z]: Matches any character within the specified range.

        -------------------------------------------------------------------------------------------------------
        1. Matching Specific Characters: [abc]
        The pattern [abc] matches any one of the characters a, b, or c.

        Example:
        You want to validate if a username contains the letters a, b, or c:

        string input = "userabc";
        string pattern = "[abc]";

        MatchCollection matches = Regex.Matches(input, pattern);
        Console.WriteLine($"Found {matches.Count} matches.");
        foreach (Match match in matches)
        {
            Console.WriteLine($"Matched: {match.Value} at position {match.Index}");
        }

        ~ Output
        Found 3 matches.
        Matched: a at position 4
        Matched: b at position 5
        Matched: c at position 6

        -------------------------
        2. Negating a Set: [^abc]
        The pattern [^abc] matches any character except a, b, or c.

        Example
        Suppose you want to find all characters in a string except the forbidden ones a, b, and c:

        string input = "abcdef";
        string pattern = "[^abc]";

        MatchCollection matches = Regex.Matches(input, pattern);
        foreach (Match match in matches)
        {
            Console.WriteLine($"Matched: {match.Value} at position {match.Index}");
        }

        --- Advanced approach (1)
        If you want to ensure the string does not contain any of the forbidden words, 
        you can combine negated sets with word boundaries.

        string input = "This is a good example of polite behaviour";
        string forbiddenWords = @"\b(bad|evil|vile)\b"; //forbidden words group

        if (!Regex.IsMatch(input, forbiddenWords))
        {
            Console.WriteLine("String is clean!");
        }
        else
        {
            Console.WriteLine("Forbidden words found!");
        }

        \b ensures that these are matched as whole words (e.g., it won't match badly).


        #2 Approach - Dynamic Forbidden Words

        string input = "This is unacceptable!";
        string[] forbiddenWordsArray = { "unacceptable", "prohibited", "restricted" };
        string forbiddenWords = @"\b(" + string.Join("|", forbiddenWordsArray) + @")\b";

        if (Regex.IsMatch(input, forbiddenWords))
        {
            Console.WriteLine("Forbidden words found!");
        }
        else
        {
            Console.WriteLine("String is clean!");
        }


        -------------------------
        3. Matching Ranges: [a-z]
        The pattern [a-z] matches any lowercase letter in the English alphabet.

        Example
        You’re validating a password to ensure it contains at least one lowercase letter:

        string input = "Password123";
        string pattern = "[a-z]";

        bool containsLowercase = Regex.IsMatch(input, pattern);
        Console.WriteLine($"Password contains lowercase: {containsLowercase}");


        -------------------------
        4. Special Character Sets
        
        Regular expressions provide shorthand for common character groups:

            \d: Matches any digit ([0-9]).
            \D: Matches any non-digit ([^0-9]).
            \w: Matches word characters ([a-zA-Z_0-9]).
            \W: Matches non-word characters ([^a-zA-Z_0-9]).
            \s: Matches any whitespace character.
            \S: Matches any non-whitespace character.

        \w indicates a word character, which includes letters, numbers, and the underscore.
        \W matches any nonword character. These work as expected for non-English letters, 
        too, such as Cyrillic.

        Example: Identifying Numbers
        Suppose you’re extracting all numbers from a string:

        string input = "My order numbers are 123, 456, and 789.";
        string pattern = @"\d+";
        string pattern2 = @"[0-9]+";

        MatchCollection matches = Regex.Matches(input, pattern2);
        foreach (var item in matches)
        {
            Console.WriteLine($"Found number: {item}");
        }

        The pattern \d+ matches one or more digits (+ is a quantifier).


        -------------------------------
        5. Unicode Categories with \p{}

        Regex supports matching characters in specific Unicode categories:

            \p{L}: Matches any letter.
            \p{P}: Matches punctuation.
            \p{N}: Matches numbers.

        Example: Detecting Punctuation
        You want to count all punctuation marks in a sentence:

        string input = "Hello, world! How's it going? My order numbers are 123, 456, and 789.";
        string punctPattern = @"\p{P}";
        string numPattern = @"\p{N}";

        MatchCollection punctuations = Regex.Matches(input, punctPattern);
        Console.WriteLine($"Found {punctuations.Count} punctuation marks.");
        foreach (Match match in punctuations)
        {
            Console.WriteLine($"Punctuation: {match.Value}");
        }

        MatchCollection nums = Regex.Matches(input, numPattern);
        Console.WriteLine($"Found {nums.Count} numbers.");
        foreach (Match match in nums)
        {
            Console.WriteLine($"Number: {match.Value}");
        }

        The \p{P} matches any punctuation character.


        ------------------------
        6. Wildcard Match with .

        The dot (.) matches any single character except newline (\n).
        
        Example
        You want to validate file names where the extension can be any three characters:

        string input = "document.pdf";
        string pattern = @"\w+\.\w{3}";

        bool isValidFile = Regex.IsMatch(input, pattern);
        Console.WriteLine($"Is valid file > {isValidFile}");

        --- Pattern Explanation
        \w+ matches the file name (one or more word characters).
        \. matches the literal dot.
        \w{3} matches exactly three characters for the extension.


        --- Notes #1

        string input = "document.pdf";
        string pattern = @"\w+\w{3}";
        
        bool isValidFile = Regex.IsMatch(input, pattern);
        Console.WriteLine($"Is valid file > {isValidFile}");

        ~
        Explanation of Why It Works

        1. The Pattern Breakdown
        \w+: Matches one or more word characters (letters, digits, or underscores).
        \w{3}: Matches exactly three word characters.

        2. Matching Process
        Regex doesn't require the entire string to match the pattern unless you use anchors like 

        ^ (start of string) or 
        $ (end of string). 
        
        By default, Regex.IsMatch looks for any substring within the input string that satisfies the given pattern.

        In your input string "document.pdf":

        - The substring "document" matches \w+ (since it is a series of word characters).
        - The substring "pdf" matches \w{3} (since it is exactly three word characters).
        - Together, "document" satisfies \w+ and "pdf" satisfies \w{3} in sequence.


        --- Notes #2

        ~
        The Role of . in Regular Expressions

        1. Special Meaning of .
        In regular expressions, the dot (.) is a metacharacter that 
        matches any single character except a newline (\n).

        for example:
        
        string pattern = "a.c";
        Console.WriteLine(Regex.IsMatch("abc", pattern)); // True (matches "abc")
        Console.WriteLine(Regex.IsMatch("a1c", pattern)); // True (matches "a1c")
        Console.WriteLine(Regex.IsMatch("a_c", pattern)); // True (matches "a_c")

        2. Matching a Literal Dot
        To match an actual period (.), you must escape its special meaning by using a backslash: \.

        string pattern = @"a\.c";
        Console.WriteLine(Regex.IsMatch("abc", pattern)); // False
        Console.WriteLine(Regex.IsMatch("a.c", pattern)); // True


        ----------------------------------------
        Real-World Problem: Validating Usernames

        You want to ensure usernames meet these rules:
        ~ Only contain alphanumeric characters and underscores.
        ~ Start with a letter.
        ~ Be 3 to 15 characters long.

        string pattern = @"^[a-zA-Z]\w{2,14}$";
        List<string> inputs = ["John123", "Jane_Doe", "A1", "Zebra_77", "Peter_Griffin", "ValidUser", "TheBestUser_9"];

        foreach (string input in inputs)
        {
            bool isMatched = Regex.IsMatch(input, pattern);
            Console.WriteLine($"{input} is valid: {isMatched}");
        }

        
        ~ Pattern Explanation

        ^: Matches the start of the string.
        [a-zA-Z]: The first character must be an alphabetic letter (uppercase or lowercase).
        \w: Matches any word character, which includes:
        
            Letters (a-z, A-Z),
            Digits (0-9),
            Underscore (_).
        
        {2,14}: The username must have between 2 to 14 additional characters (making the total length between 3 and 15 characters).
        $: Matches the end of the string.

        */

        /* What Are Quantifiers?
        Quantifiers in regular expressions define how many times a character, group, or character set can appear in a match.
        
        --- Quantifier Syntax and Meaning

        1. *: Matches zero or more occurrences.
        
        Matches zero or more occurrences of the preceding element.
        This means the element may be completely absent or appear any number of times.
        
        Example:
        Pattern: "ab*c"
        Matches: "ac", "abc", "abbc", "abbbc" (any number of b's, including zero).
        

        2. +: Matches one or more occurrences.
        
        Matches one or more occurrences of the preceding element.
        This means the element must appear at least once.
        
        Example:
        Pattern: "ab+c"
        Matches: "abc", "abbc", "abbbc" (but not "ac" because at least one b is required).


        3. ?: Matches zero or one occurrence.

        Matches zero or one occurrence of the preceding element.
        This is often used for optional elements.
        
        Example:
        Pattern: "colou?r"
        Matches: "color", "colour" (the u is optional).


        4. {n}: Matches exactly n occurrences.
        Matches exactly n occurrences of the preceding element.

        Example:
        Pattern: "a{3}"
        Matches: "aaa" (exactly 3 a's, no more, no less).


        5. {n,}: Matches n or more occurrences.
        Matches at least n occurrences of the preceding element.

        Example:
        Pattern: "a{2,}"
        Matches: "aa", "aaa", "aaaa" (at least 2 a's, but no upper limit).


        6. {n,m}: Matches between n and m occurrences.
        Matches at least n but at most m occurrences of the preceding element.

        Example:
        Pattern: "a{2,4}"
        Matches: "aa", "aaa", "aaaa" (between 2 and 4 a's).


        ~ Quantifier Real-World Examples

        1. File Names with Numbers
        You want to match file names like file.docx, file1.docx, or file123.docx.

          string pattern = @"file[0-9]*\.docx"; 
          string pattern = @"file\d*\.docx"; 
        
        ~ Pattern Explanation

        file: Matches the word file.
        \d*: Matches zero or more digits (0-9) following file.
        \.docx: Matches .docx. (The . is escaped because it is a special character.)


        ~ Code Example

        string pattern = @"file[0-9]*\.docx";
        string pattern2 = @"file\d*\.docx";

        Console.WriteLine(Regex.Match("file123.docx", pattern2).Success); // True
        Console.WriteLine(Regex.Match("file.docx", pattern2).Success);   // True
        Console.WriteLine(Regex.Match("file.txt", pattern2).Success);    // False


        2. Matching Usernames
        Usernames must be alphanumeric, allowing underscores, and be 5–10 characters long.

        string pattern = @"^\w{5,10}$";

        ~ Pattern Explanation
        
        ^: Ensures the match starts at the beginning.
        \w: Matches word characters (letters, digits, underscore).
        {5,10}: Requires between 5 and 10 repetitions.
        $: Ensures the match ends at the end.

        ~ Code Example

        Console.WriteLine(Regex.IsMatch("JohnDoe", @"^\w{5,10}$"));     // True
        Console.WriteLine(Regex.IsMatch("User_1234", @"^\w{5,10}$"));   // True
        Console.WriteLine(Regex.IsMatch("TooLongUsername", @"^\w{5,10}$")); // False


        3. Blood Pressure Readings
        Match blood pressure values like 120/80 or 160/110.

        string pattern = @"\d{2,3}/\d{2,3}";

        ~ Pattern Explanation
        
        \d{2,3}: Matches 2 or 3 digits for systolic/diastolic pressure.
        /: Matches the / separator.

        ~ Code Example

        Regex bp = new Regex(@"\d{2,3}/\d{2,3}");
        Console.WriteLine(bp.Match("It used to be 160/110").Value); // 160/110
        Console.WriteLine(bp.Match("Now it's 115/75").Value);       // 115/75



        4. Matching Repeated Words
        Detect duplicate words like "the the" in a sentence.

        string pattern = @"\b(\w+)\s+\1\b";

        ~ 1. Understanding \b (Word Boundary)
        \b asserts a word boundary, ensuring the match occurs at the start or end of a word.

        Input String	            Pattern	            Match	                    Explanation
        ------------------------------------------------------------------------------------------------------------------------------
        "word"	                    \bword\b	        word	                    Matches the whole word "word".
        "word,"	                    \bword\b	        No	                        Does not match because of the comma.
        "word word"	                \bword	            word	                    Matches both "word" occurrences.
        "sword"	                    \bword	            No	                        Does not match because "word" is part of "sword".

        It ensures that the match happens only at whole words, avoiding partial matches (like "word" inside "sword").


        ~ 2. Understanding \1 (Backreference)
        \1 refers to the first capturing group in the pattern.

        Input String	            Pattern	            Match	                    Explanation
        ------------------------------------------------------------------------------------------------------------------------------
        "the the"	                (\w+)\s+\1	        the                         the	Matches two identical words.
        "the quick the"	            (\w+)\s+\1	        No	                        The words are not identical.
        "123 123"	                (\d+)\s+\1	        123                         123	Matches the same sequence of digits.


        ~ Combining \b and \1

        string input = "the the quick brown fox is is not lazy";
        string pattern = @"\b(\w+)\s+\1\b";

        MatchCollection matches = Regex.Matches(input, pattern);

        foreach (Match match in matches)
        {
            Console.WriteLine($"Duplicate word found: {match.Value}");
        }

        ~ Output
        
        Duplicate word found: the the
        Duplicate word found: is is

        */

        /* Greedy Versus Lazy Quantifiers
        
        Regular expressions use quantifiers to determine how many times a pattern should be repeated.
        By default, quantifiers are greedy, meaning they match as much as possible.
        meaning they try to match as much as possible before allowing the rest of the pattern to continue.

        However, we can make them lazy (also called non-greedy) by adding ?, 
        which forces them to match as little as possible.



        ~ 1. Greedy Quantifiers (Default Behaviors)
        A greedy quantifier tries to match as much text as possible while still allowing the full pattern to match.

        Example: Matching HTML Tags with a Greedy Quantifier
        Consider this HTML string:

        string pattern = "<i>.*</i>";
        string html = "<i>By default</i> quantifiers are <i>greedy</i> creatures";

        MatchCollection matches = Regex.Matches(html, pattern);
        foreach (Match match in matches)
        {
            Console.WriteLine(match.Value);
        }

        ~ Pattern Explanation
        <i> → Matches the opening <i> tag.
        .* → Greedily matches everything until the last possible </i>.
        </i> → Matches the last closing </i> tag in the input.

        ~ What Happens
        The greedy .* quantifier matches the longest possible substring between the first <i> and the last </i> tag,

        ~ Output
        <i>By default</i> quantifiers are <i>greedy</i>



        ~ 2. Lazy Quantifiers (Non-Greedy) (*?, +?, ??, {n,m}?)
        A lazy quantifier stops as soon as it finds a match for the rest of the pattern, instead of consuming everything.

        Example: Fixing the Greedy Match
        To make the quantifier lazy, add a ? after it:


        string pattern = "<i>.*?</i>";
        string html = "<i>By default</i> quantifiers are <i>greedy</i> creatures";

        MatchCollection matches = Regex.Matches(html, pattern);
        foreach (Match match in matches)
        {
            Console.WriteLine(match.Value);
        }

        ~ Pattern Explanation
        <i> → Matches the opening <i> tag.
        .*? → Lazily matches everything until the first possible </i>.
        </i> → Matches the first closing </i> tag in the input.

        ~ What Happens
        The lazy .*? quantifier matches the shortest possible substring between the first <i> and the first </i> tag.

        ~ Output
        <i>By default</i>
        <i>greedy</i>

        */

        /* Lookahead and Lookbehind
        Lookahead and lookbehind are types of zero-width assertions in regular expressions. 
        These assertions allow you to match text based on what comes before or comes after the target pattern, 
        without including that part in the final match result.



        ~ 1. Positive Lookahead (=?expr)
        A positive lookahead checks whether a specific pattern exists ahead of the current position in the string, 
        but does not consume any characters from the string itself. 

        Example: Positive Lookahead
        We want to match a number only if it's followed by the word "miles", 
        but we do not want to include "miles" in the match result:

        string input = "say 25 miles more";
        string pattern = @"\d+\s+(?=miles)";

        Console.WriteLine($"input: {input}. is matched: {Regex.IsMatch(input, pattern)}");

        ~ Pattern Explanation
        \d+ → Matches one or more digits.
        \s+ → Matches one or more whitespace characters.
        (?=miles) → Positive lookahead for the word "miles".

        This is a positive lookahead: it confirms that "miles" follows the number, 
        but doesn't include it in the match.

        ~ NOTES:
        If we append .* to the pattern: \d+\s+(?=miles).*,
        Output would be: 25 miles more

        After the lookahead successfully checks for "miles", 
        it continues matching the rest of the string after "miles", giving us the full "25 miles more".



        ~ 2. Negative Lookahead (?!expr)
        A negative lookahead asserts that a certain pattern does not follow the current position. 
        It is used to ensure that a specific string or pattern is not present after the match.
        
        Example: Negative Lookahead
        We want to match the word "good" only if it's not followed by "however" or "but":

        List<string> inputs = ["good", "good however", "good but", "good however but", "Good work!", "Good work, Thanks!"];
        string pattern = @"(?i)good(?!.*(however|but|although))";

        inputs.ForEach(input =>
        {
            Console.WriteLine($"input: {input}. is matched: {Regex.IsMatch(input, pattern)}");
        });

        ~ Pattern Explanation
        
        (?i) → Case-insensitive matching. (Keep in Mind!)
        good → Matches the word "good".
        (?!.*(however|but)) → ensures that "good" is not followed by "however" or "but" or "although"
        anywhere later in the string.

        ~ Output

        input: good. is matched: True
        input: good however. is matched: False
        input: good but. is matched: False
        input: good however but. is matched: False
        input: Good work!. is matched: True
        input: Good work, Thanks!. is matched: True



        ~ 3. Positive Lookbehind (?<=expr)
        A positive lookbehind asserts that the match must be preceded by a specific pattern. 
        It checks if the specified pattern occurs just before the current position, but does not consume those characters.

        Example: Positive Lookbehind
        We want to match "good" only if it's preceded by "however":

        string pattern = @"(?i)(?<=however.*)good";
        Console.WriteLine(Regex.IsMatch("However good, we...", pattern)); // True
        Console.WriteLine(Regex.IsMatch("Very good, thanks!", pattern)); // False

        ~ Pattern Explanation
        (?i) → Case-insensitive matching.
        (?<=however.*) → Positive lookbehind for "however" followed by any characters.
        good → Matches the word "good".



        ~ 4. Negative Lookbehind (?<!expr)
        A negative lookbehind asserts that the match must not be preceded by a specific pattern.
        It ensures that the specified pattern is absent before the current position.

        Example: Negative Lookbehind
        We want to match "good" only if it's not preceded by "however":

        string pattern = @"(?i)(?<!however.*)good";
        Console.WriteLine(Regex.IsMatch("However good, we...", pattern)); // False
        Console.WriteLine(Regex.IsMatch("Very good, thanks!", pattern)); // True

        ~ Pattern Explanation
        (?i) → Case-insensitive matching.
        (?<!however.*) → Negative lookbehind for "however" followed by any characters.
        good → Matches the word "good".

        */

        // Code Examples

        // Positive Lookahead
        {
            string input = "say 25 miles more";
            string pattern = @"\d+\s+(?=miles)";

            Console.WriteLine($"input: {input}. is matched: {Regex.IsMatch(input, pattern)}");
        }

        // Negative Lookahead
        {
            List<string> inputs = ["good", "good however", "good but", "good however but", "Good work!", "Good work, Thanks!"];
            string pattern = @"(?i)good(?!.*(however|but|although))";

            inputs.ForEach(input =>
            {
                Match match = Regex.Match(input, pattern);
                Console.WriteLine($"input: {input}. match value: {match.Value}. is matched: {Regex.IsMatch(input, pattern)}");
            });
        }

        // Positive Lookbehind
        {
            string pattern = @"(?i)(?<=however.*)good";
            Console.WriteLine(Regex.IsMatch("However good, we...", pattern)); // True
            Console.WriteLine(Regex.IsMatch("Very good, thanks!", pattern)); // False
        }

        // Negative Lookbehind
        {
            string pattern = @"(?i)(?<!however.*)good";
            Console.WriteLine(Regex.IsMatch("However good, we...", pattern)); // False
            Console.WriteLine(Regex.IsMatch("Very good, thanks!", pattern)); // True
        }
    }
}