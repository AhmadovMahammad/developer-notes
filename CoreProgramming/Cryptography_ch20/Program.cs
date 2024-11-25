using System.Security.Cryptography;
using System.Text;

namespace Cryptography_ch20;

internal static class Program
{
    public static void Main(string[] args)
    {
        /* Overview of Windows Data Protection (DPAPI)

         Windows Data Protection (DPAPI) is a built-in encryption system in Windows that allows developers to encrypt and decrypt data
         without needing to manage cryptographic keys directly.

         It’s designed to protect sensitive data using a key derived from the user’s credentials (password or system-wide credentials).
         It ensures that sensitive data can only be decrypted by the same user or machine that encrypted it.

         DPAPI provides a simple interface through the ProtectedData class, which has two main methods:
         1_ Protect: Encrypts the data.
         2_ Unprotect: Decrypts the data.
         These methods ensure that data is securely protected without having to manually handle encryption keys.

         --- Protect and Unprotect Methods
        Protect (Encrypt) data: Encrypt your sensitive data with a key derived from the user's password (or machine credentials if using LocalMachine).
        Unprotect (Decrypt) data: Decrypt the previously encrypted data, but only if the correct key is available.

        public static byte[] Protect(byte[] userData, byte[] optionalEntropy, DataProtectionScope scope);
        public static byte[] Unprotect(byte[] encryptedData, byte[] optionalEntropy, DataProtectionScope scope);

        Here is how it works:
        1. userData: the data you want to encrypt (a byte array).
        2. optionalEntropy: Extra data (like a password or salt) that can be added to the encryption process to increase security.
        This makes it harder for an attacker to decrypt the data even if they have the key.
        3. scope: Defines whether the encryption is for the CurrentUser or for the LocalMachine.
        3.1. CurrentUser: The key is tied to the currently logged-in user. Only that user can decrypt the data.
        3.2. LocalMachine: The key is system-wide and can be used by any user on that machine.
        This is useful for services or applications running under multiple user accounts.

        Example Code:

        byte[] key = Encoding.UTF8.GetBytes("this is a password");
        byte[] salt = Encoding.UTF8.GetBytes("this is a salt");
        DataProtectionScope scope = DataProtectionScope.CurrentUser;

        // encrypt the data:
        byte[] encryptedData = ProtectedData.Protect(key, salt, scope);
        Console.WriteLine(Convert.ToBase64String(encryptedData));

        // decrypt the data:
        byte[] decrypted = ProtectedData.Unprotect(encryptedData, salt, scope);
        Console.WriteLine(Encoding.UTF8.GetString(decrypted));

        --- Key points
        The encryption relies on the user’s password or machine-wide key.
        The stronger the password (in the case of CurrentUser), the more secure the encryption.

        With CurrentUser, if an attacker gains physical access to the system or the user’s login credentials,
        they can potentially decrypt the data.

        --- 1) How a Hacker Can Access Encrypted Data

        a) If an attacker gains access to the encrypted data, they might try to decrypt it.
        However, the data is usually not in a readable form because it’s encrypted, and
        they need the key or password used to encrypt it.

        b) The hacker might attempt a brute force attack, which involves trying a large number of possible passwords or
        encryption keys until the right one is found.

        c) Accessing the Key/Password
        If the attacker can access the key used for encryption (e.g., by stealing the user’s credentials or exploiting system vulnerabilities),
        they can simply use that key to decrypt the data directly.
        Keys might be stored in places like:

        1_ System memory (where an attacker might try to extract it using malware or privilege escalation).
        2_ Credential storage (e.g., in files or keychains) that is insecure or poorly protected.

        --- 2) How Salt Affects the Decryption Process

        Salt (random data) is added to the encryption process to increase its security.
        It’s particularly helpful in preventing attackers from easily decrypting data,
        even if they have access to the encrypted data and the encryption method.

        Example Without Salt:
        1. Data "This is a password" is encrypted with the user’s password "password123".
        2. The attacker captures the encrypted data and knows the encryption method.
        3. The attacker tries different passwords in a brute-force attack. Once they guess "password123", they can decrypt the data.
        4. Since the encryption method is deterministic (same input always results in the same output),
        the attacker can reuse the same decryption attempt for any instance of "This is a password".

        Example With Salt:
        1. Data "This is a password" is encrypted with the user’s password "password123", but this time, a random salt (e.g., "RANDOMSALT123") is added.
        2. The attacker captures the encrypted data but doesn’t know the salt.
        3. The attacker must now brute-force both the password and the salt.
        Since the salt is random, they would need to try many different salt values in addition to password guesses.
        This increases the complexity of the attack.
        4. If the salt is long enough and randomly generated,
        it makes brute-forcing computationally infeasible because the attacker needs to guess the salt and the password simultaneously.



        */

        /* Hashing

        hashing is the process of converting input data (of any size) into a fixed-length value called a hashcode using a mathematical algorithm.
        The main properties of hashing are:

        1. Fixed Length: Regardless of the input size, the resulting hashcode is always of a fixed length (e.g., SHA1 produces a 20-byte hash).
        2. Unique Representation: Even a small change in the input (e.g., a single bit) leads to a drastically different hashcode
        (this is called the avalanche effect).
        3. One-Way Function: Hashing also acts as one-way encryption, because it’s difficult-to-impossible to convert a hashcode back into the original data.
        This makes it ideal for storing passwords in a database, because should your database become compromised,

        you don’t want the attacker to gain access to plain-text passwords.
        To authenticate, you simply hash what the user types in and compare it to the hash that’s stored in the database.

        --- Hashing in .NET
        In .NET, you can hash data using the HashAlgorithm subclasses, such as SHA1 and SHA256.
        Here’s a breakdown of the examples:

        a) Hashing a File

        FileStream fs = File.OpenRead("test.txt");
        byte[] hash = SHA1.Create().ComputeHash(fs);
        // SHA1 produces 20 bytes (160 bits). | SHA256 produces 32 bytes (256 bits).

        The ComputeHash method also accepts a byte array, which is convenient for hashing passwords.
        byte[] data = Encoding.UTF8.GetBytes ("stRhong%pword");
        byte[] hash = SHA256.Create().ComputeHash (data);

        ----- Encoding, Decoding problems

        Encoding.UTF8.GetBytes and Encoding.UTF8.GetString methods are designed specifically for
        converting readable text (strings) into byte arrays and back, based on the UTF-8 encoding.

        * Why it works only for regular text:
        Text encoded in UTF-8 follows specific rules to represent characters (e.g., ASCII and Unicode).
        Arbitrary binary data (e.g., a hash, encrypted data) often contains bytes that do not conform to valid UTF-8 character encodings.

        * What happens if you try to decode binary data with GetString:
        You’ll likely get garbled text or an error, because the binary data might include byte sequences that violate UTF-8 rules.

        =============================

        Convert.ToBase64String and Convert.FromBase64String methods are designed to convert any byte array into a safe,
        text-friendly string representation (Base64 encoding) and back.

        * Why it works for binary data:
        Base64 encoding ensures the data is represented only with characters from the safe subset: A-Z, a-z, 0-9, +, /, and = for padding.

        --- Example Comparison

        byte[] binaryData = { 0xFA, 0x45, 0x00, 0x12 };

        // Correct use with Base64 (Binary Data):
        string base64String = Convert.ToBase64String(binaryData);
        byte[] originalData = Convert.FromBase64String(base64String);

        // Incorrect use with UTF-8 (Binary Data):
        string invalidText = Encoding.UTF8.GetString(binaryData);
        byte[] corruptedData = Encoding.UTF8.GetBytes(invalidText);

        // Correct use with UTF-8 (Text):
        string text = "Hello, World!";
        byte[] utf8Bytes = Encoding.UTF8.GetBytes(text);
        string decodedText = Encoding.UTF8.GetString(utf8Bytes);

        */
    }
}