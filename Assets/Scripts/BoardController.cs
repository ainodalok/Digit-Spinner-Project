using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class BoardController : MonoBehaviour {
    private BoardLogic boardLogic;
    public GameObject tilePrefab;
    public GameObject[][] activeTileObjects = new GameObject[BoardLogic.BOARD_SIZE][];

	// Use this for initialization
	void Awake () {
        boardLogic = new BoardLogic();

        for (int i = 0; i < BoardLogic.BOARD_SIZE; i++)
        {
            activeTileObjects[i] = new GameObject[BoardLogic.BOARD_SIZE];

            for (int j = 0; j < BoardLogic.BOARD_SIZE; j++)
            {
                Vector2 position = new Vector2(i, j);
                GameObject newTile = Instantiate(tilePrefab);
                newTile.transform.SetParent(gameObject.transform);
                newTile.name = String.Format("Tile ({0}, {1})", i, j);
                newTile.transform.localPosition = position;
                newTile.transform.rotation = Quaternion.identity;
                newTile.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = boardLogic.activeTiles[i][j].ToString();
                newTile.GetComponent<Tile>().x = i;
                newTile.GetComponent<Tile>().y = j;

                activeTileObjects[i][j] = newTile;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public BoardLogic GetBoardLogic()
    {
        return boardLogic;
    }
}
