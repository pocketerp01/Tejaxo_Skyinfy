using System;
using System.Collections.Generic;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Collections;

public class EncryptDecrypt
{
    static byte[] bytes = ASCIIEncoding.ASCII.GetBytes("ZeroCool");
    public static string Encrypt(string originalString)
    {
        string encrypt = string.Empty;
        try
        {
            
            if (String.IsNullOrEmpty(originalString))
            {
                throw new ArgumentNullException("The string which needs to be encrypted can not be null.");
            }




            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(originalString);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            encrypt = Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }
        catch (Exception ex)
        {
            ex.Data.Clear();
        }
        return encrypt;
    }
    public static string Decrypt(string cryptedString)
    {
        string decrypt = string.Empty;
        try
        {
            if (String.IsNullOrEmpty(cryptedString))
            {
                throw new ArgumentNullException("The string which needs to be decrypted can not be null.");
            }
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            cryptedString = cryptedString.Replace(" ", "+");
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cryptedString));
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);
            decrypt = reader.ReadToEnd();
        }
        catch (Exception ex)
        {
            ex.Data.Clear();
        }
        return decrypt;
    }
    public static string Encrypt_new(string Source)
    {
        try
        {
            string strRet = null;
            string strSub = null;
            ArrayList arrOffsets = new ArrayList();
            int intCounter = 0;
            int intMod = 0;
            int intVal = 0;
            int intNewVal = 0;

            arrOffsets.Insert(0, 96);
            arrOffsets.Insert(1, 59);
            arrOffsets.Insert(2, 36);
            arrOffsets.Insert(3, 55);
            arrOffsets.Insert(4, 73);
            arrOffsets.Insert(5, 76);

            strRet = "";

            for (intCounter = 0; intCounter <= Source.Length - 1;
            intCounter++)
            {
                strSub = Source.Substring(intCounter, 1);
                intVal =
                (int)System.Text.Encoding.ASCII.GetBytes(strSub)[0];
                intMod = intCounter % arrOffsets.Count;
                intNewVal = intVal +
                Convert.ToInt32(arrOffsets[intMod]);
                intNewVal = intNewVal % 256;
                strRet = strRet + intNewVal.ToString("X2");
            }
            return strRet;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public static string Decrypt_new(string Source)
    {
        try
        {
            ArrayList arrOffsets = new ArrayList();
            int intCounter = 0;
            int intMod = 0;
            int intVal = 0;
            int intNewVal = 0;
            string strOut = null;
            string strSub = null;
            string strSub1 = null;
            string strDecimal = null;

            arrOffsets.Insert(0, 96);
            arrOffsets.Insert(1, 59);
            arrOffsets.Insert(2, 36);
            arrOffsets.Insert(3, 55);
            arrOffsets.Insert(4, 73);
            arrOffsets.Insert(5, 76);

            strOut = "";
            for (intCounter = 0; intCounter <= Source.Length - 1;
            intCounter += 2)
            {
                strSub = Source.Substring(intCounter, 1);
                strSub1 = Source.Substring((intCounter + 1), 1);
                intVal = int.Parse(strSub,
                System.Globalization.NumberStyles.HexNumber) * 16 + int.Parse(strSub1,
                System.Globalization.NumberStyles.HexNumber);
                intMod = (intCounter / 2) % arrOffsets.Count;
                intNewVal = intVal -
                Convert.ToInt32(arrOffsets[intMod]) + 256;
                intNewVal = intNewVal % 256;
                strDecimal = ((char)intNewVal).ToString();
                strOut = strOut + strDecimal;
            }
            return strOut;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}