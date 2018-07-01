using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class BoardController : MonoBehaviour {
    private BoardLogic boardLogic;
    public GameObject tilePrefab;
    public GameObject prophecyTilePrefab;

    [HideInInspector]
    public GameObject scoreText;
    [HideInInspector]
    public GameObject[][] prophecyTileObjects = new GameObject[BoardLogic.BOARD_SIZE][];
    [HideInInspector]
    public GameObject[][] activeTileObjects = new GameObject[BoardLogic.BOARD_SIZE][];
    [HideInInspector]
    public GameObject[] ghostTiles = new GameObject[2];

    private int score;

    [HideInInspector]
    public bool isDestroying = false;

    void Awake () {
        boardLogic = new BoardLogic();
        SetupActiveTiles();
        SetupProphecyTiles();
        ghostTiles[0] = CreateGhostTile(activeTileObjects[0][0], new Vector3(0, BoardLogic.BOARD_SIZE, 0));
        ghostTiles[1] = CreateGhostTile(activeTileObjects[0][BoardLogic.BOARD_SIZE - 1], new Vector3(0, -BoardLogic.BOARD_SIZE, 0));
    }

    public BoardLogic GetBoardLogic()
    {
        return boardLogic;
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

    private void SetupProphecyTiles()
    {
        for (int i = 0; i < BoardLogic.BOARD_SIZE; i++)
        {
            prophecyTileObjects[i] = new GameObject[BoardLogic.PROPHECY_HEIGHT];

            for (int j = 0; j < BoardLogic.PROPHECY_HEIGHT; j++)
            {
                Vector2 position = new Vector2(i, j + BoardLogic.BOARD_SIZE);
                GameObject newTile = Instantiate(prophecyTilePrefab);
                newTile.transform.SetParent(gameObject.transform);
                newTile.name = String.Format("Prophecy Tile ({0}, {1})", i, j);
                newTile.transform.localPosition = position;
                newTile.transform.rotation = Quaternion.identity;
                newTile.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = boardLogic.prophecyTiles[i][j].ToString();

                prophecyTileObjects[i][j] = newTile;
            }
        }
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

    public void ShiftBy(int number, int distance, bool isColumn)
    {
        if (isColumn)
        {
            GameObject[] newColumn = new GameObject[BoardLogic.BOARD_SIZE];
            GameObject[] currentColumn = activeTileObjects[number];

            for (int i = 0; i < BoardLogic.BOARD_SIZE; i++)
            {
                int newIndex = i + distance;

                if (newIndex >= BoardLogic.BOARD_SIZE)
                {
                    newIndex %= BoardLogic.BOARD_SIZE;
                }
                else if (newIndex < 0)
                {
                    newIndex = BoardLogic.BOARD_SIZE + (newIndex % BoardLogic.BOARD_SIZE);
                }

                newColumn[newIndex] = currentColumn[i];
            }

            activeTileObjects[number] = newColumn;
        }
        else
        {
            GameObject[][] temporaryTileObjects = Util.CloneGameObjectArray(activeTileObjects);

            for (int i = 0; i < BoardLogic.BOARD_SIZE; i++)
            {
                int newIndex = i + distance;

                if (newIndex >= BoardLogic.BOARD_SIZE)
                {
                    newIndex %= BoardLogic.BOARD_SIZE;
                }
                else if (newIndex < 0)
                {
                    newIndex = BoardLogic.BOARD_SIZE + (newIndex % BoardLogic.BOARD_SIZE);
                }

                temporaryTileObjects[newIndex][number] = activeTileObjects[i][number];
            }

            activeTileObjects = temporaryTileObjects;
        }
    }

    public void UpdateDigitsBasic()
    {
        for (int i = 0; i < BoardLogic.BOARD_SIZE; i++)
        {
            for (int j = 0; j < BoardLogic.BOARD_SIZE; j++)
            {
                activeTileObjects[i][j].transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text =
                    boardLogic.activeTiles[i][j].ToString();
            }

            for (int j = 0; j < BoardLogic.PROPHECY_HEIGHT; j++)
            {
                prophecyTileObjects[i][j].transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text =
                    boardLogic.prophecyTiles[i][j].ToString();
            }
        }
    }

    public void AddScore(int add)
    {
        score += add;

        scoreText.GetComponent<TextMeshProUGUI>().text = string.Format("Score: \n{0}", score);
    }

    private GameObject CreateGhostTile(GameObject tile, Vector3 offset)
    {
        GameObject newTile = Instantiate(tile);
        newTile.transform.SetParent(gameObject.transform);
        newTile.transform.localPosition = tile.transform.localPosition + offset;
        newTile.name = string.Concat(tile.name, " Ghost");
        return newTile;
    }



    public void SetEnableBoardRenderers(bool enable)
    {
        ParticleSystem[] particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem comp in particleSystems)
        {
            if (enable)
            {
                if (comp.gameObject.name == "BarrierRL" || comp.gameObject.name == "BarrierLR")
                {
                    comp.Play();
                }
            }
            else
            {
                comp.Stop();
            }
        }

        MeshRenderer[] meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer comp in meshRenderers)
        {
            comp.enabled = enable;
        }

        SpriteRenderer[] spriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer comp in spriteRenderers)
        {
            comp.enabled = enable;
        }

        SetEnableTileColliders(enable);
    }

    public void SetEnableTileColliders(bool enable)
    {
        BoxCollider2D[] colliders = gameObject.GetComponentsInChildren<BoxCollider2D>();
        foreach (BoxCollider2D comp in colliders)
        {
            comp.enabled = enable;
        }
    }
}
