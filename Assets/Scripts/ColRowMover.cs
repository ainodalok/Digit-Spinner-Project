using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class ColRowMover : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    /* false when moving a row, true when moving a column */
    private bool isColumnMoving = false;
    private int number;
    private Vector2 currentMovement;
    private Vector2 oldPosition;
    private Vector3 initialPosition;
    private BoardController bc;

    void Start()
    {
        bc = gameObject.GetComponentInParent<BoardController>();
    }

    public void OnBeginDrag(PointerEventData e)
    {
        initialPosition = gameObject.transform.localPosition;
        currentMovement = e.delta;
        if (Mathf.Abs(currentMovement.y) > Mathf.Abs(currentMovement.x))
        {
            number = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(e.position).x) + 3;

            for (int i = 0; i < BoardLogic.BOARD_SIZE; i++)
            {
                GameObject tile = GetTile(number, i);

                if (i == 0)
                {
                    bc.ghostTiles[0] = CreateGhostTile(tile, new Vector3(0, BoardLogic.BOARD_SIZE, 0));
                }
                else if (i == BoardLogic.BOARD_SIZE - 1)
                {
                    bc.ghostTiles[1] = CreateGhostTile(tile, new Vector3(0, -BoardLogic.BOARD_SIZE, 0));
                }
            }
            
            isColumnMoving = true;
        }
        else if (Mathf.Abs(currentMovement.y) < Mathf.Abs(currentMovement.x))
        {
            number = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(e.position).y) + 5;

            for (int i = 0; i < BoardLogic.BOARD_SIZE; i++)
            {
                GameObject tile = GetTile(i, number);

                if (i == 0)
                {
                    bc.ghostTiles[0] = CreateGhostTile(tile, new Vector3(BoardLogic.BOARD_SIZE, 0, 0));
                }
                else if (i == BoardLogic.BOARD_SIZE - 1)
                {
                    bc.ghostTiles[1] = CreateGhostTile(tile, new Vector3(-BoardLogic.BOARD_SIZE, 0, 0));
                }
            }
            
            isColumnMoving = false;
        }
        oldPosition = (Vector2)(Camera.main.ScreenToWorldPoint(e.position));
    }

    public void OnDrag(PointerEventData e)
    {
        Vector3 offsetVector;
        currentMovement = (Vector2)(Camera.main.ScreenToWorldPoint(e.position));
        
        if (isColumnMoving)
        {
            offsetVector = new Vector3(0, currentMovement.y - oldPosition.y, 0);

            for (int i = 0; i < bc.ghostTiles.Length; i++)
            {
                bc.ghostTiles[i].transform.localPosition += offsetVector;
            }

            for (int i = 0; i < BoardLogic.BOARD_SIZE; i++)
            {
                bc.activeTileObjects[number][i].transform.localPosition += offsetVector;
            }

            if (bc.activeTileObjects[number][0].transform.localPosition.y < -0.5f)
            {
                Vector3 temp = bc.activeTileObjects[number][0].transform.localPosition;
                bc.activeTileObjects[number][0].transform.localPosition = bc.ghostTiles[0].transform.localPosition;
                bc.ghostTiles[0].transform.localPosition = temp;

                bc.ShiftInsert(number, true, true);

                bc.ghostTiles[1].transform.localPosition = new Vector3(
                    bc.activeTileObjects[number][BoardLogic.BOARD_SIZE - 1].transform.localPosition.x,
                    bc.activeTileObjects[number][BoardLogic.BOARD_SIZE - 1].transform.localPosition.y + 1,
                    bc.activeTileObjects[number][BoardLogic.BOARD_SIZE - 1].transform.localPosition.z
                    );
                bc.ghostTiles[1].transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text =
                    bc.activeTileObjects[number][0].transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text;

                GameObject tempObj = bc.ghostTiles[0];
                bc.ghostTiles[0] = bc.ghostTiles[1];
                bc.ghostTiles[1] = tempObj;
            }

            if (bc.activeTileObjects[number][BoardLogic.BOARD_SIZE - 1].transform.localPosition.y > BoardLogic.BOARD_SIZE - 0.5f)
            {
                Vector3 temp = bc.activeTileObjects[number][BoardLogic.BOARD_SIZE - 1].transform.localPosition;
                bc.activeTileObjects[number][BoardLogic.BOARD_SIZE - 1].transform.localPosition = bc.ghostTiles[1].transform.localPosition;
                bc.ghostTiles[1].transform.localPosition = temp;

                bc.ShiftInsert(number, false, true);

                bc.ghostTiles[0].transform.localPosition = new Vector3(
                    bc.activeTileObjects[number][0].transform.localPosition.x,
                    bc.activeTileObjects[number][0].transform.localPosition.y - 1,
                    bc.activeTileObjects[number][0].transform.localPosition.z
                    );
                bc.ghostTiles[0].transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text =
                    bc.activeTileObjects[number][BoardLogic.BOARD_SIZE - 1].transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text;

                GameObject tempObj = bc.ghostTiles[0];
                bc.ghostTiles[0] = bc.ghostTiles[1];
                bc.ghostTiles[1] = tempObj;
            }
        }
        else
        {
            offsetVector = new Vector3(currentMovement.x - oldPosition.x, 0, 0);

            for (int i = 0; i < bc.ghostTiles.Length; i++)
            {
                bc.ghostTiles[i].transform.localPosition += offsetVector;
            }

            for (int i = 0; i < BoardLogic.BOARD_SIZE; i++)
            {
                bc.activeTileObjects[i][number].transform.localPosition += offsetVector;
            }

            if (bc.activeTileObjects[0][number].transform.localPosition.x < -0.5f)
            {
                Vector3 temp = bc.activeTileObjects[0][number].transform.localPosition;
                bc.activeTileObjects[0][number].transform.localPosition = bc.ghostTiles[0].transform.localPosition;
                bc.ghostTiles[0].transform.localPosition = temp;

                bc.ShiftInsert(number, true, false);

                bc.ghostTiles[1].transform.localPosition = new Vector3(
                    bc.activeTileObjects[BoardLogic.BOARD_SIZE - 1][number].transform.localPosition.x + 1,
                    bc.activeTileObjects[BoardLogic.BOARD_SIZE - 1][number].transform.localPosition.y,
                    bc.activeTileObjects[BoardLogic.BOARD_SIZE - 1][number].transform.localPosition.z
                    );
                bc.ghostTiles[1].transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text =
                    bc.activeTileObjects[0][number].transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text;

                GameObject tempObj = bc.ghostTiles[0];
                bc.ghostTiles[0] = bc.ghostTiles[1];
                bc.ghostTiles[1] = tempObj;
            }

            if (bc.activeTileObjects[BoardLogic.BOARD_SIZE - 1][number].transform.localPosition.x > BoardLogic.BOARD_SIZE - 0.5f)
            {
                Vector3 temp = bc.activeTileObjects[BoardLogic.BOARD_SIZE - 1][number].transform.localPosition;
                bc.activeTileObjects[BoardLogic.BOARD_SIZE - 1][number].transform.localPosition = bc.ghostTiles[1].transform.localPosition;
                bc.ghostTiles[1].transform.localPosition = temp;

                bc.ShiftInsert(number, false, false);

                bc.ghostTiles[0].transform.localPosition = new Vector3(
                    bc.activeTileObjects[0][number].transform.localPosition.x - 1,
                    bc.activeTileObjects[0][number].transform.localPosition.y,
                    bc.activeTileObjects[0][number].transform.localPosition.z
                    );
                bc.ghostTiles[0].transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text =
                    bc.activeTileObjects[BoardLogic.BOARD_SIZE - 1][number].transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text;

                GameObject tempObj = bc.ghostTiles[0];
                bc.ghostTiles[0] = bc.ghostTiles[1];
                bc.ghostTiles[1] = tempObj;
            }
        }

        oldPosition = currentMovement;
    }

    public void OnEndDrag(PointerEventData e)
    {
        int overallMovement;

        if (isColumnMoving)
        {
            foreach (GameObject t in bc.activeTileObjects[number])
            {
                RectTransform tileRectPos = t.transform.GetComponent<RectTransform>();
                tileRectPos.localPosition = new Vector3(tileRectPos.localPosition.x, Mathf.Round(tileRectPos.localPosition.y), tileRectPos.localPosition.z);
                t.name = string.Format("Tile ({0}, {1})", tileRectPos.localPosition.x, tileRectPos.localPosition.y);
            }

            for (int i = 0; i < bc.ghostTiles.Length; i++)
            {
                Destroy(bc.ghostTiles[i]);
            }

            overallMovement = Mathf.RoundToInt(gameObject.transform.localPosition.y - initialPosition.y);
        }
        else
        {
            for (int i = 0; i < BoardLogic.BOARD_SIZE; i++)
            {
                GameObject t = bc.activeTileObjects[i][number];
                RectTransform tileRectPos = t.transform.GetComponent<RectTransform>();
                tileRectPos.localPosition = new Vector3(Mathf.Round(tileRectPos.localPosition.x), tileRectPos.localPosition.y, tileRectPos.localPosition.z);
                t.name = string.Format("Tile ({0}, {1})", tileRectPos.localPosition.x, tileRectPos.localPosition.y);
            }

            for (int i = 0; i < bc.ghostTiles.Length; i++)
            {
                Destroy(bc.ghostTiles[i]);
            }

            overallMovement = Mathf.RoundToInt(gameObject.transform.localPosition.x - initialPosition.x);
        }

        if (overallMovement != 0)
        {
            List<Vector2Int> tilesToRemove = bc.GetBoardLogic().Move(number, overallMovement, isColumnMoving);

            if (tilesToRemove.Count == 0 || tilesToRemove == null)
            {
                MoveBack(overallMovement);
                return;
            }

            StartCoroutine(DestroyMatchedTiles(tilesToRemove));
        }
    }

    private void MoveBack(int distance)
    {
        bc.ShiftBy(number, -distance, isColumnMoving);

        for (int i = 0; i < BoardLogic.BOARD_SIZE; i ++)
        {
            GameObject tile;

            if (isColumnMoving)
            {
                tile = bc.activeTileObjects[number][i];
                tile.transform.localPosition += new Vector3(0, -distance, 0);

                if (tile.transform.localPosition.y >= BoardLogic.BOARD_SIZE)
                {
                    tile.transform.localPosition += new Vector3(0, -BoardLogic.BOARD_SIZE, 0);
                }

                if (tile.transform.localPosition.y < 0)
                {
                    tile.transform.localPosition += new Vector3(0, BoardLogic.BOARD_SIZE, 0);
                }
            }
            else
            {
                tile = bc.activeTileObjects[i][number];
                tile.transform.localPosition += new Vector3(-distance, 0, 0);


                if (tile.transform.localPosition.x >= BoardLogic.BOARD_SIZE)
                {
                    tile.transform.localPosition += new Vector3(-BoardLogic.BOARD_SIZE, 0, 0);
                }

                if (tile.transform.localPosition.x < 0)
                {
                    tile.transform.localPosition += new Vector3(BoardLogic.BOARD_SIZE, 0, 0);
                }
            }
        }
    }

    private IEnumerator DestroyMatchedTiles(List<Vector2Int> tilesToRemove)
    {
        while (tilesToRemove.Count > 0)
        {
            yield return new WaitForSeconds(3.7f);

            bc.AddScore(tilesToRemove.Count * 10);
            tilesToRemove = bc.GetBoardLogic().DestroyTiles(tilesToRemove);
            bc.UpdateDigitsBasic();
        }
    }

    private GameObject GetTile(int x, int y)
    {
        return gameObject.transform.parent.GetComponent<BoardController>().activeTileObjects[x][y];
    }

    private GameObject CreateGhostTile(GameObject tile, Vector3 offset)
    {
        GameObject newTile = Instantiate(tile);
        newTile.transform.SetParent(tile.transform.parent);
        newTile.transform.localPosition = tile.transform.localPosition + offset;
        newTile.name = string.Concat(gameObject.name, " Ghost");
        newTile.GetComponent<TileController>().isGhost = true;

        return newTile;
    }

    private void LogColumn(int number)
    {
        string s = "";

        for (int i = 0; i < bc.activeTileObjects[number].Length; i++)
        {
            s += bc.activeTileObjects[number][i].name;    
        }

        Debug.Log(s);
    }

    private void LogRow(int number)
    {
        string s = "";

        for (int i = 0; i < bc.activeTileObjects[number].Length; i++)
        {
            s += bc.activeTileObjects[i][number].name;
        }

        Debug.Log(s);
    }
}
