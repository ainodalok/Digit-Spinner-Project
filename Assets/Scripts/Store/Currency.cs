﻿using UnityEngine;
using System.Collections;

public class Currency
{
    private static bool initialized = false;
    public const int DEFAULT_BALANCE = 100;

    //Use this at the beginning of each function that has to do with balance
    private static void Init()
    {
        if (!initialized)
        {
            SafeMemory.SetInt("balance", Storage.GetSafeInt("balance", DEFAULT_BALANCE));
            initialized = true;
        }
    }

    public static int GetBalance()
    {
        Init();
        return SafeMemory.GetInt("balance");
    }

    public static void ProcessEndGame()
    {
        Init();
        ChangeBalance(SafeMemory.GetInt("score") / 100);
    }

    public static void ProcessPurchase(int amount)
    {
        if (amount > 0)
        {
            Init();
            ChangeBalance(amount);
        }
    }

    private static void ChangeBalance(int change)
    {
        SafeMemory.SetInt("balance", SafeMemory.GetInt("balance") + change);

        if (SafeMemory.GetInt("balance") < 0)
        {
            SafeMemory.SetInt("balance", 0);
        }

        Storage.SetSafeInt("balance", SafeMemory.GetInt("balance"));
        Storage.Commit();
    }
}
