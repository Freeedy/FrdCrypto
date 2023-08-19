using FrdCryptoLib.ClassicCrypto.Rotors.Enigma;
using System.Text;

namespace CryptoTest
{
    public class EnigmaCipherTests
    {
        [Fact]
        public void Test1()
        {
            byte[] rotor1Wiring = new byte[] { 0x00,0x01,0x02 };
            byte[] rotor2Wiring = new byte[] { 0x00, 0x01, 0x02 };
            byte[] rotor3Wiring = new byte[] { 0x00, 0x01, 0x02 };


            EnigmaCipher enigma = new EnigmaCipher(rotor1Wiring,rotor2Wiring,rotor3Wiring);

            enigma .SetRotorPositions(0,0,0);

            // Add plugboard connections (optional)
            enigma.AddPlugboardConnection(0x41, 0x4D); // Example plugboard connection A -> M
            enigma.AddPlugboardConnection(0x46, 0x4F); // Example plugboard connection F -> O
            enigma.AddPlugboardConnection(0x4E, 0x54); // Example plugboard connection N -> T
            enigma.AddPlugboardConnection(0x5A, 0x56); // Example plugboard connection Z -> V

            // Your message to encrypt
            byte[] dataToEncrypt =Encoding.UTF8.GetBytes("Hello");

            // Encrypt the message
            byte[] encryptedMessage = enigma.Encrypt(dataToEncrypt);

            Console.WriteLine(BitConverter.ToString(encryptedMessage));


            // Decrypt the message
            byte[] decryptedMessage = enigma.Decrypt(encryptedMessage);
            Console.WriteLine("Original Message: " + BitConverter.ToString(dataToEncrypt));
            Console.WriteLine("Encrypted Message: " + BitConverter.ToString(encryptedMessage));
            Console.WriteLine("Decrypted Message: " + BitConverter.ToString(decryptedMessage));

            Assert.Equal(dataToEncrypt, decryptedMessage);
        }
    }
}