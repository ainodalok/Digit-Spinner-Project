using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class LeaderboardBtn : MonoBehaviour {

    public void OpenLeaderboard()
    {
        Social.ShowAchievementsUI();
    }
}
