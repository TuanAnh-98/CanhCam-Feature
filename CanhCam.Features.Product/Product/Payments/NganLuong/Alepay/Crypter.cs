using CanhCam.Web.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace CanhCam.Web.ProductUI
{
    public static class Crypter
    {
        //private static string publicKey = ConfigurationManager.AppSettings["publicKey"];
        static RsaKeyParameters rsaKeyParameters;
        static Crypter()
        {
        }
        private static void LoadConfigByStore(string storeCode)
        {
            string encryptKey = AlepayApi.ENCRYPT_KEY;
            if (AlepayApi.ENABLE_KEY_FORSTORE && !string.IsNullOrEmpty(storeCode))
            {
                encryptKey = ConfigHelper.GetStringProperty(AlepayApi.CONFIG_ENCRYPT_KEY + "_" + storeCode, "");
                if (string.IsNullOrEmpty(encryptKey))
                    encryptKey = AlepayApi.ENCRYPT_KEY;
            }
            var keyInfoData = Convert.FromBase64String(encryptKey);
            rsaKeyParameters = PublicKeyFactory.CreateKey(keyInfoData) as RsaKeyParameters;
        }
        public static string Encrypt(object obj, string storeCode)
        {
            LoadConfigByStore(storeCode);
            var payloadBytes = Encoding.UTF8.GetBytes(obj.ToString());

            var cipher = GetAsymmetricBlockCipher(true);
            var encrypted = Process(cipher, payloadBytes);

            var encoded = Convert.ToBase64String(encrypted);
            return encoded;
        }

        public static T Decrypt<T>(string encryptedText, string storeCode)
        {
            LoadConfigByStore(storeCode);
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter> { new TickDateTimeConverter() }
            };

            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);

            var cipher = GetAsymmetricBlockCipher(false);
            var decrypted = Process(cipher, cipherTextBytes);

            var decoded = Encoding.UTF8.GetString(decrypted);

            //var js = new JavaScriptSerializer();
            //return js.Deserialize<T>(decoded, settings);
            return JsonConvert.DeserializeObject<T>(decoded, settings);
        }

        private static IAsymmetricBlockCipher GetAsymmetricBlockCipher(bool forEncryption)
        {
            var cipher = new Pkcs1Encoding(new RsaEngine());
            cipher.Init(forEncryption, rsaKeyParameters);

            return cipher;
        }

        private static byte[] Process(IAsymmetricBlockCipher cipher, byte[] payloadBytes)
        {
            int length = payloadBytes.Length;
            int blockSize = cipher.GetInputBlockSize();

            var plainTextBytes = new List<byte>();
            for (int chunkPosition = 0; chunkPosition < length; chunkPosition += blockSize)
            {
                int chunkSize = Math.Min(blockSize, length - chunkPosition);
                plainTextBytes.AddRange(cipher.ProcessBlock(payloadBytes, chunkPosition, chunkSize));
            }

            return plainTextBytes.ToArray();
        }
    }
}
