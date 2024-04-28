using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    private bool useEncryption = false;

    private static string aesKeyString = "+zWc7D43KTKKn*&B7!!*G$s9RD=jPTbJ"; // 256-bit encryption
    private static string aesIVString = "fedcba0987654321";
    private static byte[] aesKey = Encoding.UTF8.GetBytes(aesKeyString);
    private static byte[] aesIV = Encoding.UTF8.GetBytes(aesIVString);



    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }   

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                // Load serialized data from file
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (useEncryption)
                {
                    dataToLoad = DecryptString(dataToLoad);
                }

                // Deserialize from Json to C#
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName); // Path.Combine() accounts for different operating systems

        try
        {
            // Create the directory the file will be written to if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Serialize game data to Json
            string dataToStore = JsonUtility.ToJson(data, true);

            if (useEncryption)
            {
                dataToStore = EncryptString(dataToStore);
            }

            // Write serialized data to file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " +  fullPath + "\n" + e);
        }
    }

    // AES encryption
    private string EncryptString(string data)
    {
        // Convert string to bytes
        byte[] buffer = Encoding.UTF8.GetBytes(data);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = aesKey;
            aesAlg.IV = aesIV;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(buffer, 0, buffer.Length);
                    csEncrypt.FlushFinalBlock();
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }
    }

    public static string DecryptString(string encryptedData)
    {
        // Convert base64 string to bytes
        byte[] buffer = Convert.FromBase64String(encryptedData);


        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = aesKey;
            aesAlg.IV = aesIV;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(buffer))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
}
