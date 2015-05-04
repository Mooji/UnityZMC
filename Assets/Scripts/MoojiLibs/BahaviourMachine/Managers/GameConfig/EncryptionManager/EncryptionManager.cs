using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using UnityEngine;

namespace Mooji
{
    public class EncryptionManager : MonoBehaviour
    {

        private string key = "uwniTq6wza7nU4/cCVxTScpjhlv1Tl5s";
        private string iv = "ld6Et92CmbQ=";
        private TripleDESCryptoServiceProvider tdes = null;

        public void Awake()
        {
            tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = Convert.FromBase64String( key );
            tdes.IV = Convert.FromBase64String( iv );
        }


        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="sourceByteArr"></param>
        /// <returns></returns>
        public byte[]  encryption(byte[] sourceByteArr)
        {
            //  压缩
            byte[] compressByteArr = QuickLZ.compress( sourceByteArr , 1 );
            //  加密
            byte[] encryptedBytes = this.encryptionByteArr( compressByteArr , tdes.Key , tdes.IV );

            return encryptedBytes;
        }

        public byte[] dEncryption( byte[] sourceByteArr )
        {
            //  解密
            byte[] decryptByteArr = this.decryptionByteArr( sourceByteArr , tdes.Key , tdes.IV );
            //  解压
            byte[] unCompressByteArr = QuickLZ.decompress( decryptByteArr );

            return unCompressByteArr;
        }

        private byte[] encryptionByteArr( byte[] sourceStrByteArr , byte[] Key , byte[] IV )
        {
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream( memoryStream , new TripleDESCryptoServiceProvider().CreateEncryptor( Key , IV ) , CryptoStreamMode.Write );
            byte[] toEncrypt = sourceStrByteArr;
            try
            {
                cryptoStream.Write( toEncrypt , 0 , toEncrypt.Length );
                cryptoStream.FlushFinalBlock();
                byte[] encryptedBytes = memoryStream.ToArray();
                return encryptedBytes;
            }
            catch ( CryptographicException err )
            {
                throw new Exception( "加密出错：" + err.Message );
            }
            finally
            {
                cryptoStream.Close();
                memoryStream.Close();
            }
        }
        private byte[] decryptionByteArr( byte[] dataBytes , byte[] Key , byte[] IV )
        {
            MemoryStream memoryStream = new MemoryStream( dataBytes );
            CryptoStream cryptoStream = new CryptoStream( memoryStream , new TripleDESCryptoServiceProvider().CreateDecryptor( Key , IV ) , CryptoStreamMode.Read );
            byte[] decryptBytes = new byte[dataBytes.Length];
            try
            {
                //从解密流中将解密后的数据读到字节数组中
                cryptoStream.Read( decryptBytes , 0 , decryptBytes.Length );
                return decryptBytes;
            }
            catch ( CryptographicException err )
            {
                throw new Exception( "解密出错：" + err.Message );
            }
            finally
            {
                cryptoStream.Close();
                memoryStream.Close();
            }
        }

    }
}
