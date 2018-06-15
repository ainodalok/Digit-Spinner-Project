using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class BoardController : MonoBehaviour {
    public const int GHOST_TILE_AMOUNT = 4;

    private BoardLogic boardLogic;
    public GameObject tilePrefab;
    public GameObject[][] activeTileObjects = new GameObject[BoardLogic.BOARD_SIZE][];
    public GameObject[][][] ghostTileObjects = new GameObject[BoardLogic.BOARD_SIZE][][];

	void Awake () {
        boardLogic = new BoardLogic();
        SetupActiveTiles();
        SetupGhostTiles();
	}
	
	void Update () {
		
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

    /* 
     * this creates a screen wrapping effect by
     * making an array of 4 "ghost" tiles for each of the active tiles
     * one for each of the possible move directions: NESW
     */ 
    private void SetupGhostTiles()
    {
        for (int i = 0; i < BoardLogic.BOARD_SIZE; i++)
        {
            ghostTileObjects[i] = new GameObject[BoardLogic.BOARD_SIZE][];

            for (int j = 0; j < BoardLogic.BOARD_SIZE; j++)
            {
                //tile we are creating ghosts of
                GameObject tile = activeTileObjects[i][j];
                ghostTileObjects[i][j] = new GameObject[GHOST_TILE_AMOUNT];

                for (int k = 0; k < GHOST_TILE_AMOUNT; k++)
                {
                    /* setting name and offset vector vars for all of the 4 movement directions;
                     * offset vector is a vector that is added to original tile localPosition in order to get
                     * the position of a ghost tile, i.e. for the northern ghost ship Y coordinate should 
                     * be larger by 7 than the original one. 
                     */

                    string side;
                    Vector3 offsetVector;

                    if (k == 0)
                    {
                        offsetVector = new Vector3(0, BoardLogic.BOARD_SIZE, 0);
                        side = "North";
                    }
                    else if (k == 1)
                    {
                        offsetVector = new Vector3(BoardLogic.BOARD_SIZE, 0, 0);
                        side = "East";
                    }
                    else if (k == 2)
                    {
                        offsetVector = new Vector3(0, -BoardLogic.BOARD_SIZE, 0);
                        side = "South";
                    }
                    else
                    {
                        offsetVector = new Vector3(-BoardLogic.BOARD_SIZE, 0, 0);
                        side = "West";
                    }

                    GameObject ghostTile = Instantiate(tile);
                    ghostTile.transform.parent = tile.transform.parent;
                    ghostTile.name = String.Format("{0} Ghost {1}", side, tile.name);
                    ghostTile.transform.localPosition = tile.transform.localPosition + offsetVector;
                    ghostTileObjects[i][j][k] = ghostTile;
                }
            }
        }
    }

    private void DestroyCurrentTiles()
    {

    }

    public BoardLogic GetBoardLogic()
    {
        return boardLogic;
    }
}
