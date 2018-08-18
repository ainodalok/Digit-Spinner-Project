using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Regenerate : MonoBehaviour {
    public TextMeshProUGUI RegenLeftTxt;
    public BoardController boardController;

    void Awake()
    {
        //CHANGE TO A REAL VALUE LATER
        SafeMemory.SetInt("regenLeft", 1);
        RegenLeftTxt.SetText(SafeMemory.GetInt("regenLeft").ToString());
    }

    public void RegenerateBoard()
    {
        if (SafeMemory.GetInt("regenLeft") > 0)
        {
            boardController.boardLogic.GenerateActiveTiles();
            while (MatchFinder.IsGameOver(boardController.boardLogic.activeTiles))
            {
                boardController.boardLogic.GenerateActiveTiles();
            }
            boardController.UpdateDigitsBasic();
            SafeMemory.SetInt("regenLeft", SafeMemory.GetInt("regenLeft") - 1);
            RegenLeftTxt.SetText(SafeMemory.GetInt("regenLeft").ToString());
        }
    }
}
