using FrdCryptoLib.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace FrdCryptoLib.ClassicCrypto.Rotors.Enigma
{
    public class EnigmaCipher 
    {
        private EnigmaRotor rotor1;
        private EnigmaRotor rotor2;
        private EnigmaRotor rotor3;
        private EnigmaPlugboard plugboard;

        public EnigmaCipher(string rotor1Wiring, string rotor2Wiring, string rotor3Wiring)
        {
            rotor1 = new EnigmaRotor(rotor1Wiring);
            rotor2 = new EnigmaRotor(rotor2Wiring);
            rotor3 = new EnigmaRotor(rotor3Wiring);
            plugboard = new EnigmaPlugboard();
        }

        public void SetRotorPositions(int position1, int position2, int position3)
        {
            rotor1.SetPosition(position1);
            rotor2.SetPosition(position2);
            rotor3.SetPosition(position3);
        }

        public void AddPlugboardConnection(char from, char to)
        {
            plugboard.AddPlug(from, to);
        }



        public char Process(char input)
        {
            char processedChar = char.ToUpper(input);

            // Pass through the plugboard
            processedChar = plugboard.Process(processedChar);

            // Pass through the rotors in forward direction
            processedChar = rotor3.ProcessForward(processedChar);
            processedChar = rotor2.ProcessForward(processedChar);
            processedChar = rotor1.ProcessForward(processedChar);

            // Pass through the reflector
            // Note: In this example, the reflector is fixed and symmetric.
            // You can use a custom reflector for different implementations.
            // For the real Enigma machine, the reflector was not symmetric.
            // However, for simplicity, we use a symmetric reflector here.
            processedChar = ReflectorB.Process(processedChar);

            // Pass through the rotors in backward direction
            processedChar = rotor1.ProcessBackward(processedChar);
            processedChar = rotor2.ProcessBackward(processedChar);
            processedChar = rotor3.ProcessBackward(processedChar);

            // Pass through the plugboard again
            processedChar = plugboard.Process(processedChar);

            // Rotate the rotors
            rotor1.Rotate();
            if (rotor1.position == rotor1.wiring.IndexOf('Q'))
            {
                rotor2.Rotate();
                if (rotor2.position == rotor2.wiring.IndexOf('E'))
                {
                    rotor3.Rotate();
                }
            }

            return processedChar;
        }

        // Fixed symmetric reflector 'B' used in this example
        private static class ReflectorB
        {
            private static readonly string wiring = "YRUHQSLDPXNGOKMIEBFZCWVJAT";

            public static char Process(char input)
            {
                int index = input - 'A';
                return wiring[index];
            }
        }

        public string Encrypt(string message)
        {
            char[] encryptedChars = new char[message.Length];
            for (int i = 0; i < message.Length; i++)
            {
                encryptedChars[i] = Process(message[i]);
            }
            return new string(encryptedChars);
        }

        public string Decrypt(string message)
        {
            // To decrypt, we need to reset rotor positions to the starting position
            SetRotorPositions(0, 0, 0);

            char[] decryptedChars = new char[message.Length];
            for (int i = 0; i < message.Length; i++)
            {
                decryptedChars[i] = Process(message[i]);
            }
            return new string(decryptedChars);
        }
    }

    public class EnigmaRotor
    {
        public string wiring;
        public int position;

        public EnigmaRotor(string wiring)
        {
            this.wiring = wiring;
            position = 0;
        }

        public void SetPosition(int position)
        {
            this.position = (position + wiring.Length) % wiring.Length;
        }

        public char ProcessForward(char input)
        {
            int index = (input - 'A' + position) % 26;
            return wiring[index];
        }

        public char ProcessBackward(char input)
        {
            int index = (wiring.IndexOf(input) - position + wiring.Length) % 26;
            return (char)('A' + index);
        }

        public void Rotate()
        {
            position = (position + 1) % wiring.Length;
        }
    }


    public class EnigmaPlugboard
    {
        private Dictionary<char, char> mapping;

        public EnigmaPlugboard()
        {
            mapping = new Dictionary<char, char>();
        }

        public void AddPlug(char from, char to)
        {
            mapping[from] = to;
            mapping[to] = from;
        }

        public char Process(char input)
        {
            if (mapping.TryGetValue(input, out char output))
                return output;
            return input;
        }
    }


}
