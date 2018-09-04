using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps
{
    public const int POWERUP_COUNT = 4;
    public static bool[] initialized = new bool[4] { false, false, false, false };

    public const int DEFAULT_BALANCE = 0;

    //Use this at the beginning of each function that has to do with getting Power Up count
    private static void Init(string powerUpNameLeft)
    {
        int id = -1;
        switch (powerUpNameLeft)
        {
            case "bombLeft":
                id = 0; 
                break;
            case "overtimeLeft":
                id = 1;
                break;
            case "regenLeft":
                id = 2;
                break;
            case "wrongMoveLeft":
                id = 3;
                break;
        }
        if ((id >= 0) && (id < POWERUP_COUNT))
        {
            if (!initialized[id])
            {
                SafeMemory.SetInt(powerUpNameLeft, Storage.GetSafeInt(powerUpNameLeft));
                initialized[id] = true;
            }
        }
    }

    public static void ResetInitialized()
    {
        for (int i = 0; i < POWERUP_COUNT; i++)
        {
            initialized[i] = false;
        }
    }

    public static int GetPowerUpLeft(string powerUpNameLeft)
    {
        Init(powerUpNameLeft);
        return SafeMemory.GetInt(powerUpNameLeft);
    }

    public static void ChangePowerUpLeft(string powerUpNameLeft, int value)
    {
        SafeMemory.SetInt(powerUpNameLeft, value);
        Storage.SetSafeInt(powerUpNameLeft, value);
        Storage.Commit();
        //PlayServicesManager.Init();
        //PlayServicesManager.SaveData();
    }
}
