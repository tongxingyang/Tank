﻿using System;
using System.Collections.Generic;

/* *****************************************************************************
 * File:    ListExtensions.cs
 * Author:  Philip Pierce - Tuesday, September 09, 2014
 * Description:
 *  Extensions for Lists, Arrays, Dictionaries, etc
 *  
 * History:
 *  Tuesday, September 09, 2014 - Created
 * ****************************************************************************/

/// <summary>
/// Extensions for Lists, Arrays, Dictionaries, etc
/// </summary>
public static class ListExtensions
{
    #region IsNullOrEmpty

    /// <summary>
    /// Returns true if the array is null or empty
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty<T>(this T[] data)
    {
        return data == null || data.Length == 0;
    }

    /// <summary>
    /// Returns true if the list is null or empty
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty<T>(this List<T> data)
    {
        return data == null || data.Count == 0;
    }

    /// <summary>
    /// Returns true if the dictionary is null or empty
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty<T1, T2>(this Dictionary<T1, T2> data)
    {
        return data == null || data.Count == 0;
    }

    // IsNullOrEmpty

    #endregion

    #region DequeueOrNull

    /// <summary>
    /// deques an item, or returns null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="q"></param>
    /// <returns></returns>
    public static T DequeueOrNull<T>(this Queue<T> q)
    {
        try
        {
            return (q.Count > 0) ? q.Dequeue() : default(T);
        }

        catch (Exception)
        {
            return default(T);
        }
    }

    // DequeueOrNull

    #endregion
}
