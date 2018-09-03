using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdRemove
{
    private static bool initialized = false;

    private static void Init()
    {
        if (!initialized)
        {
            SafeMemory.SetInt("adRemove", Storage.GetSafeInt("adRemove"));
            initialized = true;
        }
    }

    public static void Enable()
    {
        SafeMemory.SetInt("adRemove", 1);
        Storage.SetSafeInt("adRemove", 1);
        Storage.Commit();
    }

    public static bool Get()
    {
        Init();
        if (SafeMemory.GetInt("adRemove") == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
