//----------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>Ben</author>
// <date>2016/1/18 12:25:07</date>
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Extension methods for common functions
/// </summary>
public static class StaticExtebsion
{
    #region StringToBytes

    /// <summary>
    /// Strings to bytes.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    public static unsafe byte[] StringToBytes(this string source)
    {
        // exit if null
        if (source.IsNullOrEmpty()) return null;

        //byte[] bytes = new byte[source.Length * sizeof(char)];
        //System.Buffer.BlockCopy(source.ToCharArray(), 0, bytes, 0, bytes.Length);

        byte[] bytes = new byte[source.Length << 1];
        fixed (char* str = source)
        {
            fixed (byte* ptr = bytes)
            {
                byte* bstr = (byte*) str; //一个char两个字节，转成byte好用for循环处理
                for (int i = 0, len = bytes.Length; i < len; ++i)
                {
                    ptr[i] = bstr[i];
                }
            }
        }
        return bytes;
    }

    public static unsafe int StringToBytes(this string source, byte[] bytes, int offset)
    {
        int length = source.Length << 1;

        fixed (char* str = source)
        {
            fixed (byte* ptr = bytes)
            {
                byte* bstr = (byte*) str; //一个char两个字节，转成byte好用for循环处理
                for (int i = offset, len = offset + length; i < len; ++i)
                {
                    ptr[i] = bstr[i];
                }
            }
        }
        return length;
    }

    #endregion

    #region BytesToString

    /// <summary>
    /// Byteses to string.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    public static unsafe string BytesToString(this byte[] source)
    {
        // exit if null
        if (source.IsNullOrEmpty()) return string.Empty;

        //char[] chars = new char[source.Length / sizeof(char)];
        //System.Buffer.BlockCopy(source, 0, chars, 0, source.Length);
        //return new string(chars);
        //Marshal.Copy();

        char[] chars = new char[source.Length >> 1]; //一个char两个字节
        fixed (char* str = chars)
        {
            fixed (byte* ptr = source)
            {
                byte* pstr = (byte*) str; //一个char两个字节，转成byte好用for循环处理
                for (int i = 0; i < source.Length; ++i)
                {
                    pstr[i] = ptr[i];
                }
            }
        }
        return new string(chars);
    }

    #endregion

    #region IsNullOrEmpty

    /// <summary>
    /// Determines whether [is null or empty].
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }

    #endregion

    /// <summary>
    /// Determines whether this instance is empty.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static bool IsEmpty(this string value)
    {
        return value != null && value.Length == 0;
    }

    #region IsNOTNullOrEmpty

    /// <summary>
    /// Returns true if the string is Not null or empty
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsNOTNullOrEmpty(this string value)
    {
        return !string.IsNullOrEmpty(value);
    }

    #endregion

    /// <summary>
    /// Tries the clear.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">The value.</param>
    public static void TryClear<T>(this List<T> value)
    {
        if (null != value) value.Clear();
    }

    /// <summary>
    /// 16位的md5码
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="confusion">if set to <c>true</c> [confusion].  混淆</param>
    /// <returns></returns>
    public static string MD5BIT16(this string value, bool confusion = false)
    {
        MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
        byte[] md5 = MD5.ComputeHash(value.StringToBytes());

        int startIndex = 4;

        char[] chArray = new char[16];
        for (int i = 0; i < 16; i += 2)
        {
            byte b = confusion ? (byte) (md5[startIndex++] ^ 1012 & 0xff) : md5[startIndex++];
            chArray[i] = GetHexValue(b >> 4);
            chArray[i + 1] = GetHexValue(b % 16);
        }
        return new string(chArray, 0, chArray.Length);
    }

    /// <summary>
    /// 32位的MD5码
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="confusion">if set to <c>true</c> [confusion].  混淆</param>
    /// <returns></returns>
    public static string MD5(this string value, bool confusion = false)
    {
        MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
        byte[] md5 = value.StringToBytes();
        if (confusion)
        {
            for (int i = 0; i < md5.Length; ++i)
            {
                md5[i] = (byte)(md5[i] ^ 1012 & 0xff);
            }
        }
        md5 = MD5.ComputeHash(md5);
        StringBuilder hashString = new StringBuilder(32);
        for (int i = 0; i < md5.Length; i++)
        {
            hashString.Append(md5[i].ToString("x2"));
        }

        return hashString.ToString();
    }

    private static char GetHexValue(int i)
    {
        if (i < 10)
        {
            return (char)(i + '0');
        }
        return (char)(i - 10 + 'a');
    }

    /// <summary>
    /// Determines whether [contains] [the specified needle].
    /// </summary>
    /// <param name="sb">The sb.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static bool Contains(this StringBuilder sb, string value)
    {
        return sb.IndexOf(value) != -1;
    }

    /// <summary>
    /// Indexes the of.
    /// </summary>
    /// <param name="sb">The sb.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    public static int IndexOf(this StringBuilder sb, string value)
    {
        if (value.Length == 0)
            return 0; //empty strings are everywhere!
        if (value.Length == 1) //can't beat just spinning through for it
        {
            char c = value[0];
            for (int idx = 0; idx != sb.Length; ++idx)
                if (sb[idx] == c)
                    return idx;
            return -1;
        }
        int m = 0;
        int i = 0;
        int[] T = KMPTable(value);
        while (m + i < sb.Length)
        {
            if (value[i] == sb[m + i])
            {
                if (i == value.Length - 1)
                    return m == value.Length ? -1 : m; //match -1 = failure to find conventional in .NET
                ++i;
            }
            else
            {
                m = m + i - T[i];
                i = T[i] > -1 ? T[i] : 0;
            }
        }
        return -1;
    }

    private static int[] KMPTable(string sought)
    {
        int[] table = new int[sought.Length];
        int pos = 2;
        int cnd = 0;
        table[0] = -1;
        table[1] = 0;
        while (pos < table.Length)
        {
            if (sought[pos - 1] == sought[cnd])
                table[pos++] = ++cnd;
            else if (cnd > 0)
                cnd = table[cnd];
            else
                table[pos++] = 0;
        }
        return table;
    }

    public static bool IsDiskFull(this Exception ex)
    {
        const int ERROR_HANDLE_DISK_FULL = 0x27;
        const int ERROR_DISK_FULL = 0x70;

        int errorCode = Marshal.GetHRForException(ex) & 0xFFFF;
        return errorCode == ERROR_HANDLE_DISK_FULL || errorCode == ERROR_DISK_FULL;
    }

    public static T ConvertReturn<T>(this object obj)
    {
        return (T)obj;
    }

    public static bool IsChinese(this string str)
    {
        UnicodeEncoding unicodeencoding = new UnicodeEncoding();
        byte[] bytes = unicodeencoding.GetBytes(str);
        for (int i = 0; i < bytes.Length; i++)
        {
            i++;
            //如果是中文字符那么高位不为0   
            if (bytes[i] != 0) return true;
        }
        return false;
    }
}
