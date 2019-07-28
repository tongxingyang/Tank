using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Schema;
using Assets.Tools.Script.File;
using LuaInterface;
using UnityEngine;

/// <summary>
/// 本地存档管理器
/// </summary>
public class SaveManager
{
    private static SaveManager instance;

    public static SaveManager Instance
    {
        get
        {
            if (instance == null)
                instance = new SaveManager();
            return instance;
        }
    }

    /// <summary>
    /// 读取玩家信息存档
    /// </summary>
    /// <param name="saveID">读取存档的ID</param>
    public bool IsPlayData(string saveName)
    {
        return FileTools.Exists(Application.streamingAssetsPath + "/Save/" + saveName + ".save");
    }

    /// <summary>
    /// 读取玩家信息存档
    /// </summary>
    /// <param name="saveID">读取存档的ID</param>
    private bool IsPlayData()
    {
        return FileTools.Exists(Application.streamingAssetsPath + "/Save/" + "PlayerData .save") &&
               FileTools.Exists(Application.streamingAssetsPath + "/Save/" + "SaveData .save") &&
               FileTools.Exists(Application.streamingAssetsPath + "/Save/" + "SettingData .save");
    }

    /// <summary>
    /// 读取存档文件,I/O操作谨慎使用
    /// </summary>
    /// <param name="textPath"></param>
    /// <param name="action"></param>
    public void LoadSave(string savePath, Action<object> action)
    {
        savePath = Application.streamingAssetsPath + savePath;//TODO 路径修改！！！,或使用PlayerPrefs
        string saveData = string.Empty;
        //读取
        if (FileTools.Exists(savePath))
        {
            using (StreamReader streamReader = new StreamReader(new FileStream(savePath, FileMode.Open)))
            {
                saveData = streamReader.ReadToEnd();
            }
            if (saveData.IsTNull())
                return;
            if (AppConst.isEncrypt)
                saveData = RijndaelDecrypt(saveData, "12345678123456781234567812345678");
            action(saveData);
        }
    }

    /// <summary>
    /// 写入存档文件,I/O操作谨慎使用
    /// </summary>
    /// <param name="savePath"></param>
    /// <param name="action"></param>
    public void DoSave(string savePath, object data)
    {
        savePath = Application.streamingAssetsPath + "/Save/" + savePath + ".save";//TODO 路径修改！！！,或使用PlayerPrefs
        using (StreamWriter streamReader = File.CreateText(savePath))
        {
            string saveData = data.ToString();
            if (AppConst.isEncrypt)
                saveData = RijndaelEncrypt(saveData, "12345678123456781234567812345678");
            streamReader.Write(saveData);
        }
    }

    /// <summary>
    /// Rijndael加密算法
    /// </summary>
    /// <param name="str">待加密的明文</param>
    /// <param name="key">密钥,长度可以为:64位(byte[8]),128位(byte[16]),192位(byte[24]),256位(byte[32])</param>
    /// <returns></returns>
    private string RijndaelEncrypt(string str, string key)
    {
        //密钥
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
        //待加密明文数组
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(str);
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
    /// Rijndael解密算法
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
}