using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class BoardController : MonoBehaviour {
    private BoardLogic boardLogic;
    private GameObject board;

	// Use this for initialization
	void Start () {
        board = Util.FindGameObjectByName("Board");
        boardLogic = new BoardLogic();

        for (int i = 0; i <= BoardLogic.BOARD_SIZE - 1; i++)
        {
            for (int j = 0; j <= BoardLogic.BOARD_SIZE - 1; j++)
            {
                GameObject tile = new GameObject();
                tile.name = String.Format("Tile ({0}, {1})", i.ToString(), j.ToString());

                TextMeshPro textMesh = tile.AddComponent<TextMeshPro>();
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void CreateTextMesh(int x, int y, string text)
    {

    }
}
