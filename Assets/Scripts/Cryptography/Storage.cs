using System;
using UnityEngine;

//Use this to store data on disk. This one is slow.
public class Storage
{
    private const string password = "DSAlkj3432ndsq8@";

    public static string Get(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    public static void Store(string key, string data)
    {
        PlayerPrefs.SetString(key, data);
        PlayerPrefs.Save();
    }

    public static string GetSafe(string key, string defaultValue = null)
    {
        try
        {
            return AES.Decrypt(PlayerPrefs.GetString(key), password);
        }
        catch (Exception e)
        {
            return defaultValue; 
        }
    }

    public static void SetSafe(string key, string data)
    {
        PlayerPrefs.SetString(key, AES.Encrypt(data, password));
    }

    public static int GetSafeInt(string key, int defaultValue = 0)
    {
        try
        {
            return Int32.Parse(AES.Decrypt(PlayerPrefs.GetString(key), password));
        }
        catch (Exception e)
        {
            return defaultValue;
        }
    }

    public static void SetSafeInt(string key, int data)
    {
        PlayerPrefs.SetString(key, AES.Encrypt(data.ToString(), password));
    }

    public static void Commit()
    {
        PlayerPrefs.Save();
    }
}
