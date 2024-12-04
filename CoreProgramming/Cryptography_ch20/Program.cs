using Microsoft.AspNetCore.Cryptography.KeyDerivation;
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

        --- NOTES:

        SHA1 and SHA256 are two of the HashAlgorithm subtypes provided by .NET.
        Here are all the major algorithms, in ascending order of security (and hash length, in bytes):

        MD5(16) → SHA1(20) → SHA256(32) → SHA384(48) → SHA512(64)

        MD5 and SHA1 are currently the fastest algorithms,
        although the other algorithms are not more than (roughly) two times slower in their current implementations.

        Use at least SHA256 when hashing passwords or other security sensitive data.
        MD5 and SHA1 are considered insecure for this purpose, and are suitable to protect only against accidental corruption, not deliberate tampering.

        */

        /* Hashing Passwords
        Hashing passwords involves transforming them into a fixed-length, irreversible code to securely store them.
        This ensures that even if the database is compromised, plain text passwords remain protected.

        A hashing algorithm transforms input data (e.g., a password) into a fixed-length string of characters, called a hash. Hashing is one-way encryption.

        --- 1. Why Not Just Hash Once?
        Basic hashing has vulnerabilities

        1. Dictionary Attack: Hackers hash every word in a dictionary and compare it to the stored hash.
        2. Rainbow Tables: Precomputed tables of hashes for common passwords make guessing easier.

        --- 2. Adding Salt
        Salt is a random value added to each password before hashing. It ensures that even if two users have the same password, their hashes will differ.

        How Salt Works:

        1. A random byte array (salt) is generated for each user during password creation.
        2. The salt is combined with the password before hashing.

        string password = "strng$5passw0rd";
        byte[] salt = new byte[32];

        RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(salt);

        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] saltedPassword = [.. passwordBytes, .. salt];

        byte[] hashedPassword2 = SHA1.HashData(saltedPassword);
        Console.WriteLine(Convert.ToBase64String(hashedPassword2));

        --- Advantages of Salt:

        1. Rainbow Table Protection: Hackers cannot use precomputed hashes because the salt makes each hash unique.
        2. Added Complexity: Hackers must now determine both the password and the salt.

        --- 3. Stretching (Rehashing)
        Stretching increases computational effort for hashing, by applying the hashing process multiple times.
        The goal is to slow down brute-force attacks.

        Example of Stretching:
        1. Start with the initial hash of the salted password.
        2. Use the result as input to the hash function repeatedly.

        string password = "strng$5passw0rd";
        byte[] salt = new byte[32];

        RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(salt);

        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] saltedPassword = [.. passwordBytes, .. salt];

        byte[] hashedPassword = Array.Empty<byte>();
        for (int i = 0; i < 1000; i++)
        {
            hashedPassword = SHA1.HashData(saltedPassword);
        }

        Console.WriteLine(Convert.ToBase64String(hashedPassword));

        --- 4. Using PBKDF2 for Password Hashing

        PBKDF2 (Password-Based Key Derivation Function 2) is a standard method for hashing and stretching passwords securely. It includes:

        Salting: Adds randomness to the hash.
        Stretching: Allows you to specify the number of iterations.
        Key Derivation: Outputs a secure, fixed-length byte sequence.

        .NET provides the Rfc2898DeriveBytes and the newer KeyDerivation.Pbkdf2 for this purpose.

        ---CODE EXAMPLE:

        string password = "strng$5passw0rd";

        byte[] salt = new byte[32];
        RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(salt);

        byte[] hashedPassword = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256, // hashing algorithm
            iterationCount: 10000, // Number of iterations
            numBytesRequested: 64 // Output length in bytes
        );

        Console.WriteLine(Convert.ToBase64String(hashedPassword));

        Advantages of PBKDF2:
        1. Highly Secure: Combines salting and stretching.
        2. Customizable: You can specify the number of iterations and the output length.
        3. Widely Used: An industry standard for password hashing.

        */

        /* Symmetric Encryption in .NET

        Symmetric encryption is a cryptographic technique where the same key is used for both encryption and decryption.
        This makes it efficient and faster compared to other methods, such as asymmetric encryption.
        However, it introduces the challenge of securely sharing the key between parties, known as the "key exchange problem."

        The .NET Base Class Library (BCL) provides built-in support for symmetric encryption,
        with AES (Advanced Encryption Standard) being the most recommended algorithm due to its balance between speed and security.
        AES supports three key sizes:
            16 bytes (128 bits),
            24 bytes (192 bits),
            and 32 bytes (256 bits).
        All these sizes are currently considered secure for modern encryption needs.

        --- Key and Initialization Vector (IV)
        In symmetric encryption, two essential components are required: a key and an initialization vector (IV).
        The key is a secret sequence of bytes that must remain confidential. If an attacker obtains this key, they can decrypt the data.
        On the other hand, the IV is a sequence of bytes that adds randomness to the encryption process.

        The IV is not secret and is typically sent alongside the encrypted data, such as in a message header.
        Its primary role is to ensure that even if the same plaintext is encrypted multiple times using the same key,
        the resulting ciphertext will differ.
        This makes it harder for attackers to identify patterns in the encrypted data.

        If you do not want the additional security of an IV, you can set it to a fixed value or reuse the same value for all messages.
        However, this weakens the encryption and could make the ciphertext vulnerable to attacks, especially if multiple messages are encrypted with the same key.

        --- How Symmetric Encryption Works in .NET

        To perform symmetric encryption in .NET, the AES algorithm can be used through the Aes class.
        This class is responsible for the mathematical operations of the cipher.
        It supports both encryption and decryption by creating appropriate transforms.

        For encryption, the process involves creating an encryptor with a key and IV, and
        then writing the data to be encrypted using a CryptoStream.
        A CryptoStream is a specialized stream that encrypts or decrypts data as it is read or written.

        // code example:

         string password = "strong%Password";
        byte[] bytes = Encoding.UTF8.GetBytes(password);

        byte[] key = new byte[16];
        byte[] iv = new byte[16];

        // Generate a random key and IV
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(key);
        rng.GetBytes(iv);

        byte[] encryptedBytes;
        byte[] decryptedBytes;

        // Encryption
        using (Aes aes = Aes.Create())
        using (ICryptoTransform encryptor = aes.CreateEncryptor(key, iv))
        using (MemoryStream memoryStream = new MemoryStream())
        {
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(bytes, 0, bytes.Length);
            }

            // Get the encrypted data as a byte array
            encryptedBytes = memoryStream.ToArray();
        }

        Console.WriteLine($"Encrypted Password: {Convert.ToBase64String(encryptedBytes)}");

        // Decryption
        using (Aes aes = Aes.Create())
        using (ICryptoTransform decryptor = aes.CreateDecryptor(key, iv))
        using (MemoryStream memoryStream = new MemoryStream(encryptedBytes))
        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
        using (MemoryStream outputStream = new MemoryStream())
        {
            cryptoStream.CopyTo(outputStream);
            decryptedBytes = outputStream.ToArray();
        }

        Console.WriteLine($"Decrypted Password: {Encoding.UTF8.GetString(decryptedBytes)}");

        --- Important Concepts and Best Practices

        The use of Aes ensures that the encryption process is both secure and efficient. 
        However, certain practices should be followed to maintain the integrity of the encrypted data. 
        1. The key must always be kept confidential, as it is the most critical piece in the encryption process. 
        2. The IV should not be reused across multiple messages, as doing so could expose the encryption to cryptanalysis attacks.

        When encrypting multiple pieces of data, always use a unique IV for each encryption operation. 
        This makes the ciphertext harder to analyze, even if an attacker obtains multiple encrypted messages. 
        If the IV is transmitted with the encrypted data, ensure it is done securely to prevent tampering.

        Symmetric encryption, while fast and efficient, is not suitable for all use cases. 
        For scenarios such as password storage, one-way hashing is preferred. 
        Symmetric encryption is most effective when the key can be securely exchanged or is shared in a controlled environment.


        --- other code examples...

        public class Encryption
        {
            public static string Encrypt(string rawData, byte[] key, byte[] iv)
            {
                byte[] encrypted = Encrypt(Encoding.UTF8.GetBytes(rawData), key, iv);
                return Convert.ToBase64String(encrypted);
            }
        
            private static byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
            {
                using (Aes aes = Aes.Create())
                using (ICryptoTransform encryptor = aes.CreateEncryptor(key, iv))
                {
                    return Crypt(data, encryptor);
                }
            }
        
            public static string Decrypt(string encryptedData, byte[] key, byte[] iv)
            {
                byte[] decrypted = Decrypt(Convert.FromBase64String(encryptedData), key, iv);
                return Encoding.UTF8.GetString(decrypted);
            }
        
            private static byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
            {
                using (Aes aes = Aes.Create())
                using (ICryptoTransform decryptor = aes.CreateDecryptor(key, iv))
                {
                    return Crypt(data, decryptor);
                }
            }
        
            private static byte[] Crypt(byte[] data, ICryptoTransform cryptoTransform)
            {
                MemoryStream stream = new MemoryStream();
                using (CryptoStream cryptoStream = new CryptoStream(stream, cryptoTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                }
        
                return stream.ToArray();
            }
        }

        -----------------

        string password = "strong%Password";
        byte[] bytes = Encoding.UTF8.GetBytes(password);

        byte[] key = new byte[16];
        byte[] iv = new byte[16];

        // Generate a random key and IV
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(key);
        rng.GetBytes(iv);

        string encrypted = Encryption.Encrypt("Yeah!", key, iv);
        Console.WriteLine(encrypted); // Example output: R1/5gYvcxyR2vzPjnT7yaQ==

        string decrypted = Encryption.Decrypt(encrypted, key, iv);
        Console.WriteLine(decrypted); // Output: Yeah!

        */

        string password = "strong%Password";
        byte[] bytes = Encoding.UTF8.GetBytes(password);

        byte[] key = new byte[16];
        byte[] iv = new byte[16];

        // Generate a random key and IV
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(key);
        rng.GetBytes(iv);

        string encrypted = Encryption.Encrypt("Yeah!", key, iv);
        Console.WriteLine(encrypted); // Example output: R1/5gYvcxyR2vzPjnT7yaQ==

        string decrypted = Encryption.Decrypt(encrypted, key, iv);
        Console.WriteLine(decrypted); // Output: Yeah!
    }
}