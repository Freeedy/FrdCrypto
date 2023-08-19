using FrdCryptoLib.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace FrdCryptoLib.ClassicCrypto.Rotors.Enigma
{/// <summary>
/// Add CustomReflectors 
/// </summary>
    public class EnigmaCipher
    {
        private EnigmaRotor rotor1;
        private EnigmaRotor rotor2;
        private EnigmaRotor rotor3;
        private EnigmaPlugboard plugboard;

        public EnigmaCipher(byte[] rotor1Wiring, byte[] rotor2Wiring, byte[] rotor3Wiring)
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

        public void AddPlugboardConnection(byte from, byte to)
        {
            plugboard.AddPlug(from, to);
        }



        public byte Process(byte input)
        {
            byte processedByte = input;

            // Pass through the plugboard
            processedByte = plugboard.Process(processedByte);

            // Pass through the rotors in forward direction
            processedByte = rotor3.ProcessForward(processedByte);
            processedByte = rotor2.ProcessForward(processedByte);
            processedByte = rotor1.ProcessForward(processedByte);

            // Pass through the reflector
            // Note: In this example, the reflector is fixed and symmetric.
            // You can use a custom reflector for different implementations.
            // For the real Enigma machine, the reflector was not symmetric.
            // However, for simplicity, we use a symmetric reflector here.
            processedByte = ReflectorB.Process(processedByte);

            // Pass through the rotors in backward direction
            processedByte = rotor1.ProcessBackward(processedByte);
            processedByte = rotor2.ProcessBackward(processedByte);
            processedByte = rotor3.ProcessBackward(processedByte);

            // Pass through the plugboard again
            processedByte = plugboard.Process(processedByte);

            // Rotate the rotors
            rotor1.Rotate();
            if (rotor1.position == rotor1.wiring.Length - 1)
            {
                rotor2.Rotate();
                if (rotor2.position == rotor2.wiring.Length - 1)
                {
                    rotor3.Rotate();
                }
            }

            return processedByte;
        }
        // Fixed symmetric reflector 'B' used in this example
        private static class ReflectorB
        {
            private static readonly byte[] wiring =
             {
                0x59, 0x52, 0x55, 0x48,
                0x51, 0x53, 0x4C, 0x44,
                0x50, 0x58, 0x4E, 0x47,
                0x4F, 0x57, 0x59, 0x48,
                0x58, 0x55, 0x53, 0x50,
                0x41, 0x49, 0x42, 0x52,
                0x43, 0x4A
            };
            public static byte Process(byte input)
            {
                int index = input % wiring.Length;
                return wiring[index];
            }
        }

        public byte[] Encrypt(byte[] data)
        {
            byte[] encryptedData = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                encryptedData[i] = Process(data[i]);
            }
            return encryptedData;
        }

        public byte[] Decrypt(byte[] data)
        {
            SetRotorPositions(0, 0, 0);

            byte[] decryptedData = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                decryptedData[i] = Process(data[i]);
            }
            return decryptedData;
        }
    }

    public class EnigmaRotor
    {
        public byte[] wiring;
        public int position;

        public EnigmaRotor(byte[] wiring)
        {
            this.wiring = wiring;
            position = 0;
        }

        public void SetPosition(int position)
        {
            this.position = (position + wiring.Length) % wiring.Length;
        }

        public byte ProcessForward(byte input)
        {
            int index = (input + position) % wiring.Length;
            return wiring[index];
        }

        public byte ProcessBackward(byte input)
        {
            int index = (Array.IndexOf(wiring, input) - position + wiring.Length) % wiring.Length;
            return wiring[index];
        }

        public void Rotate()
        {
            position = (position + 1) % wiring.Length;
        }
    }



    public class EnigmaPlugboard
    {
        private Dictionary<byte, byte> mapping;

        public EnigmaPlugboard()
        {
            mapping = new Dictionary<byte, byte>();
        }

        public void AddPlug(byte from, byte to)
        {
            mapping[from] = to;
            mapping[to] = from;
        }

        public byte Process(byte input)
        {
            if (mapping.TryGetValue(input, out byte output))
                return output;
            return input;
        }
    }




}
