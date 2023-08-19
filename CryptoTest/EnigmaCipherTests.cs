using FrdCryptoLib.ClassicCrypto.Rotors.Enigma;

namespace CryptoTest
{
    public class EnigmaCipherTests
    {
        [Fact]
        public void Test1()
        {
            string rotor1Wiring = "EKMFLGDQVZNTOWYHXUSPAIBRCJ";
            string rotor2Wiring = "AJDKSIRUXBLHWTMCQGZNPYFVOE";
            string rotor3Wiring = "BDFHJLCPRTXVZNYEIWGAKMUSQO";

            EnigmaCipher enigma = new EnigmaCipher(rotor1Wiring,rotor2Wiring,rotor3Wiring);

            enigma .SetRotorPositions(0,0,0);

            // Add plugboard connections (optional)
            enigma.AddPlugboardConnection('A', 'M');
            enigma.AddPlugboardConnection('F', 'T');
            enigma.AddPlugboardConnection('N', 'M');
            enigma.AddPlugboardConnection('Z', 'V');

            // Your message to encrypt
            string messageToEncrypt = "HELLO";

            // Encrypt the message
            string encryptedMessage = enigma.Encrypt(messageToEncrypt);

            Console.WriteLine(encryptedMessage);


            // Decrypt the message
            string decryptedMessage = enigma.Decrypt(encryptedMessage);
            Console.WriteLine("Original Message: " + messageToEncrypt);
            Console.WriteLine("Encrypted Message: " + encryptedMessage);
            Console.WriteLine("Decrypted Message: " + decryptedMessage);

            Assert.Equal(messageToEncrypt, decryptedMessage);
        }
    }
}