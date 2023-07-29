//  PlayerPrefsUtility.cs
//  http://kan-kikuchi.hatenablog.com/entry/PlayerPrefsUtility
//
//  Created by kan kikuchi on 2015.10.22.

using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

/// <summary>
/// PlayerPrefsに関する便利クラス
/// </summary>
public static class PlayerPrefsUtility {

    //=================================================================================
    //保存
    //=================================================================================

    // welkin's implementation
    public static void SaveVector3List(List<Vector3> myVectorList, string name){
        for (int i = 0; i < myVectorList.Count; i++)
        {
            PlayerPrefs.SetFloat(name + "X_" + i, myVectorList[i].x);
            PlayerPrefs.SetFloat(name + "Y_" + i, myVectorList[i].y);
            PlayerPrefs.SetFloat(name + "Z_" + i, myVectorList[i].z);
        }
        PlayerPrefs.Save();
    }
    public static List<Vector3> LoadVector3List(string name){
        List<Vector3> myVectorList = new List<Vector3>();
        int i = 0;
        while (PlayerPrefs.HasKey(name + "X_" + i))
        {
            float x = PlayerPrefs.GetFloat(name + "X_" + i);
            float y = PlayerPrefs.GetFloat(name + "Y_" + i);
            float z = PlayerPrefs.GetFloat(name + "Z_" + i);
            myVectorList.Add(new Vector3(x, y, z));
            i++;
        }
        return myVectorList;
    }

    /// <summary>
    /// リストを保存
    /// </summary>
    public static void SaveList<T>(string key , List<T> value){
        string serizlizedList = Serialize<List<T>> (value);
        PlayerPrefs.SetString (key, serizlizedList);
    }

    /// <summary>
    /// ディクショナリーを保存
    /// </summary>
    public static void SaveDict<Key, Value>(string key , Dictionary<Key, Value> value){
        string serizlizedDict = Serialize<Dictionary<Key, Value>> (value);
        PlayerPrefs.SetString (key, serizlizedDict);
    }

    //=================================================================================
    //読み込み
    //=================================================================================

    /// <summary>
    /// リストを読み込み
    /// </summary>
    public static List<T> LoadList<T> (string key){
        //keyがある時だけ読み込む
        if (PlayerPrefs.HasKey (key)) {
            string serizlizedList = PlayerPrefs.GetString (key);
            return Deserialize<List<T>> (serizlizedList);
        }

        return new List<T> ();
    }

    /// <summary>
    /// ディクショナリーを読み込み
    /// </summary>
    public static Dictionary<Key, Value> LoadDict<Key, Value> (string key){
        //keyがある時だけ読み込む
        if (PlayerPrefs.HasKey (key)) {
            string serizlizedDict = PlayerPrefs.GetString (key);
            return Deserialize<Dictionary<Key, Value>> (serizlizedDict);
        }

        return new Dictionary<Key, Value> ();
    }

    //=================================================================================
    //シリアライズ、デシリアライズ
    //=================================================================================

    private static string Serialize<T> (T obj){
        BinaryFormatter binaryFormatter = new BinaryFormatter ();
        MemoryStream    memoryStream    = new MemoryStream ();
        binaryFormatter.Serialize (memoryStream , obj);
        return Convert.ToBase64String (memoryStream   .GetBuffer ());
    }

    private static T Deserialize<T> (string str){
        BinaryFormatter binaryFormatter = new BinaryFormatter ();
        MemoryStream    memoryStream    = new MemoryStream (Convert.FromBase64String (str));
        return (T)binaryFormatter.Deserialize (memoryStream);
    }
}