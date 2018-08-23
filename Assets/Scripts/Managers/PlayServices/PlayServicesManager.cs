using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using System.Collections.Generic;
using GooglePlayGames.BasicApi.SavedGame;
using System;

public class PlayServicesManager
{
    private static bool initialized = false;
    private static bool authenicated = false;
    private static ISavedGameMetadata metaData;
    public static bool isSigningIn = false;

    private const float SERVICE_SIGN_IN_REST_TIME = 5.0f;
    private const string ENCRYPTION_PASSWORD = "12dSNADO)*U@IASD!(!";
    private const string SAVE_GAME_NAME = "DigitSpinnerSave";

    public static void Init()
    {
        if (!initialized)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
                .EnableSavedGames()
                .Build();

            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();

            initialized = true;
            SignIn();
        }
    }

    public static void SignIn()
    {
        isSigningIn = true;
        if (!authenicated)
        {
            Social.localUser.Authenticate((bool success) => {
                if (success)
                {
                    authenicated = true;
                }
                isSigningIn = false;
            });
        }
    }

    public static void TryLoadCloudSave()
    {
        if (!authenicated || !initialized)
        {
            return;
        }

        //TODO: discuss the choice
        PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution(
            SAVE_GAME_NAME,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseUnmerged,
            ReadAndSaveLocally
        );
    }


    public static void SaveData()
    {
        if (!authenicated || !initialized)
        {
            return;
        }

        //TODO: discuss the choice
        PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution(
            SAVE_GAME_NAME,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseUnmerged,
            ReadAndUpdate
        );
    }

    private static void ReadAndUpdate(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            metaData = game;
            PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(game, UpdateCloudData);
        }
        else
        {
            // handle error
        }
    }

    private static void ReadAndSaveLocally(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            metaData = game;
            PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(game, UpdateLocalData);
        }
        else
        {
            // handle error
        }
    }

    private static void UpdateCloudData(SavedGameRequestStatus status, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Dictionary<string, string> dataToSave = new Dictionary<string, string>();
            Dictionary<string, string> oldData = Util.ByteArrayToDict(data);

            //here you should specify fields from local storage you want to be saved
            //and keep it SAFE
            string balanceEncrypted;
            try
            {
                balanceEncrypted = AES.Encrypt(
                    (Int32.Parse(AES.Decrypt(oldData["balance"], ENCRYPTION_PASSWORD)) + Storage.GetSafeInt("balanceChange")).ToString(),
                    ENCRYPTION_PASSWORD
                );
            }
            catch (Exception e)
            {
                balanceEncrypted = AES.Encrypt("0", ENCRYPTION_PASSWORD);
            }
            dataToSave.Add("balance", balanceEncrypted);

            SaveToCloud(Util.DictionaryToByteArray(dataToSave));
        }
        else
        {
            // handle error
        }
    }

    private static void SaveToCloud(byte[] gameData)
    {
        SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder().
        WithUpdatedDescription("Saved game at " + DateTime.Now);
        Texture2D screenshot = GetScreenshot();
        if (screenshot != null)
        {
            byte[] pngData = screenshot.EncodeToPNG();
            builder = builder.WithUpdatedPngCoverImage(pngData);
        }
        SavedGameMetadataUpdate metadataUpdate = builder.Build();
        PlayGamesPlatform.Instance.SavedGame.CommitUpdate(metaData, metadataUpdate, gameData, OnSavedGameWritten);
    }

    private static void UpdateLocalData(SavedGameRequestStatus status, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Dictionary<string, string> cloudData = Util.ByteArrayToDict(data);
            try
            {
                Storage.SetSafeInt("balance", Int32.Parse(AES.Decrypt(cloudData["balance"], ENCRYPTION_PASSWORD)));
                SafeMemory.SetInt("balance", Storage.GetSafeInt("balance"));
            }
            catch (Exception e)
            {
                Storage.SetSafeInt("balance", Currency.DEFAULT_BALANCE);
                SafeMemory.SetInt("balance", Currency.DEFAULT_BALANCE);
            }
        }
        else
        {
            // handle error
        }
    }

    private static void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Storage.SetSafeInt("balanceChange", 0);
        }
    }

    private static Texture2D GetScreenshot()
    {
        Texture2D screenShot = new Texture2D(1080, 1920);
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, (Screen.width / 1080) * 1920), 0, 0);
        return screenShot;
    }
}
