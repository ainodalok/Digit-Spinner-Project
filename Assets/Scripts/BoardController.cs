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
    public GameObject[] ghostTiles = new GameObject[2];

    void Awake () {
        boardLogic = new BoardLogic();
        SetupActiveTiles();
	}

    private void SetupActiveTiles()
    {
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

                activeTileObjects[i][j] = newTile;
            }
        }
    }

    public BoardLogic GetBoardLogic()
    {
        return boardLogic;
    }

    public void ShiftInsert(int number, bool isFirstElement, bool isColumn)
    {
        if (isColumn)
        {
            if (isFirstElement)
            {
                GameObject temp = activeTileObjects[number][0];

                for (int i = 1; i < BoardLogic.BOARD_SIZE; i++)
                {
                    activeTileObjects[number][i - 1] = activeTileObjects[number][i];
                }

                activeTileObjects[number][BoardLogic.BOARD_SIZE - 1] = temp;
            }
            else
            {
                GameObject temp = activeTileObjects[number][BoardLogic.BOARD_SIZE - 1];

                for (int i = 1; i < BoardLogic.BOARD_SIZE; i++)
                {
                    activeTileObjects[number][BoardLogic.BOARD_SIZE - i] = activeTileObjects[number][BoardLogic.BOARD_SIZE - i - 1];
                }

                activeTileObjects[number][0] = temp;
            }
        }
        else
        {
            if (isFirstElement)
            {
                GameObject temp = activeTileObjects[0][number];

                for (int i = 1; i < BoardLogic.BOARD_SIZE; i++)
                {
                    activeTileObjects[i - 1][number] = activeTileObjects[i][number];
                }

                activeTileObjects[BoardLogic.BOARD_SIZE - 1][number] = temp;
            }
            else
            {
                GameObject temp = activeTileObjects[BoardLogic.BOARD_SIZE - 1][number];

                for (int i = 1; i < BoardLogic.BOARD_SIZE; i++)
                {
                    activeTileObjects[BoardLogic.BOARD_SIZE - i][number] = activeTileObjects[BoardLogic.BOARD_SIZE - i - 1][number];
                }

                activeTileObjects[0][number] = temp;
            }
        }
    }
}
