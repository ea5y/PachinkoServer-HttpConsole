using System;
using System.IO;
using LitJson;
using System.Text;
using System.Security.Cryptography;


public static class IOHelper
{
#region Json
    public static void SaveToJson<T>(object obj, string projectPath)
    {
        var strArray = typeof(T).ToString().Split('.');
        var fileName = strArray[strArray.Length - 1];
        var fileProjectFullName = projectPath + fileName + ".json";
        var msg = string.Format("===>Save to Json file:\n {0}", fileProjectFullName);
        //Debug.Log(msg);
        Console.WriteLine(msg);

        //to json
        string values = JsonMapper.ToJson(obj);

        //Encrypt
        //@TODO

        //write
        CheckPath(projectPath);
        using (FileStream fs = new FileStream(fileProjectFullName, FileMode.Create))
        using (StreamWriter sw = new StreamWriter(fs))
        {
            sw.Write(values);
        }
    }

    public static T ReadFromJson<T>(string projectPath) where T : new ()
    {
        var strArray = typeof(T).ToString().Split('.');
        var fileName = strArray[strArray.Length - 1];
        var fileProjectFullName = projectPath + fileName + ".json";

        var msg = string.Format("===>Read fram Json file:\n {0}", fileProjectFullName);
        Console.WriteLine(msg);

		if(!File.Exists(fileProjectFullName))
		{
            
            Console.WriteLine( string.Format("{0}.json not exists!", fileName));
            SaveToJson<T>(new T(), projectPath);
		}

        //read
        using (FileStream fsr = new FileStream(projectPath + fileName + ".json", FileMode.Open))
        using (StreamReader sr = new StreamReader(fsr))
        {
            var values = sr.ReadToEnd();
            //decrypt
            //@TODO

            //to obj
            var obj = JsonMapper.ToObject<T>(values);
            return obj;
        }
    }

    public static void CheckPath(string path)
    {
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    /// <summary>
    /// Rijndael加密算法
    /// </summary>
    /// <param name="pString">待加密的明文</param>
    /// <param name="pKey">密钥,长度可以为:64位(byte[8]),128位(byte[16]),192位(byte[24]),256位(byte[32])</param>
    /// <param name="iv">iv向量,长度为128（byte[16])</param>
    /// <returns></returns>
    private static string RijndaelEncrypt(string pString, string pKey)
    {
        //密钥
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(pKey);
        //待加密明文数组
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(pString);

        //Rijndael解密算法
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = rDel.CreateEncryptor();

        //返回加密后的密文
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    /// <summary>
    /// ijndael解密算法
    /// </summary>
    /// <param name="pString">待解密的密文</param>
    /// <param name="pKey">密钥,长度可以为:64位(byte[8]),128位(byte[16]),192位(byte[24]),256位(byte[32])</param>
    /// <param name="iv">iv向量,长度为128（byte[16])</param>
    /// <returns></returns>
    private static String RijndaelDecrypt(string pString, string pKey)
    {
        //解密密钥
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(pKey);
        //待解密密文数组
        byte[] toEncryptArray = Convert.FromBase64String(pString);

        //Rijndael解密算法
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = rDel.CreateDecryptor();

        //返回解密后的明文
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return UTF8Encoding.UTF8.GetString(resultArray);
    }

    public static string GetFileMD5(string fileFullName)
    {
        MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();
        //FileStream fs = new FileStream(fileFullName, FileMode.Open, FileAccess.Read, FileShare.Read);
        FileStream fs = new FileStream(fileFullName, FileMode.Open);
        byte[] hash = md5Provider.ComputeHash(fs);
        fs.Close();
        fs.Dispose();
        return System.BitConverter.ToString(hash);
    }
    #endregion
}
