using System;
using System.Collections.Generic;
using System.Text;

namespace FrdCryptoLib.Core
{
    internal interface IEncryption
    {
        byte[] Encrypt(byte[] data);
        
    }
}
