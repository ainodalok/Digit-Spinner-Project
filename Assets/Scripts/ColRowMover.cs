using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using TMPro;
using DG.Tweening;

public class ColRowMover : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    /* false when moving a row, true when moving a column */
    private bool isColumnMoving = false;
    private int number;
    private Vector2 currentMovement;
    private Vector2 oldPosition;
    private Vector3 initialPosition;
    private BoardController bc;

    [HideInInspector]
    public bool tileToRemove = false;

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
            number = Mathf.RoundToInt(initialPosition.x);
            isColumnMoving = true;
        }
        else if (Mathf.Abs(currentMovement.y) < Mathf.Abs(currentMovement.x))
        {
            number = Mathf.RoundToInt(initialPosition.y);
            isColumnMoving = false;
        }
        ChangeGhostTiles();
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

            while (bc.activeTileObjects[number][0].transform.localPosition.y < -0.5f)
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

            while (bc.activeTileObjects[number][BoardLogic.BOARD_SIZE - 1].transform.localPosition.y > BoardLogic.BOARD_SIZE - 0.5f)
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

            while (bc.activeTileObjects[0][number].transform.localPosition.x < -0.5f)
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

            while (bc.activeTileObjects[BoardLogic.BOARD_SIZE - 1][number].transform.localPosition.x > BoardLogic.BOARD_SIZE - 0.5f)
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
        bc.SetEnableTileColliders(false);
        if (isColumnMoving)
        {
            for (int i = 0; i < BoardLogic.BOARD_SIZE; i++)
            {
                GameObject t = bc.activeTileObjects[number][i];
                t.transform.localPosition = new Vector3(t.transform.localPosition.x, Mathf.Round(t.transform.localPosition.y), t.transform.localPosition.z);
                t.name = string.Format("Tile ({0}, {1})", t.transform.localPosition.x, t.transform.localPosition.y);
                bc.ghostTiles[0].transform.localPosition = new Vector3(bc.ghostTiles[0].transform.localPosition.x,
                                                           Mathf.Round(bc.ghostTiles[0].transform.localPosition.y),
                                                                       bc.ghostTiles[0].transform.localPosition.z);
                bc.ghostTiles[1].transform.localPosition = new Vector3(bc.ghostTiles[1].transform.localPosition.x,
                                                           Mathf.Round(bc.ghostTiles[1].transform.localPosition.y),
                                                                       bc.ghostTiles[1].transform.localPosition.z);
            }

            overallMovement = Mathf.RoundToInt(gameObject.transform.localPosition.y - initialPosition.y);
        }
        else
        {
            for (int i = 0; i < BoardLogic.BOARD_SIZE; i++)
            {
                GameObject t = bc.activeTileObjects[i][number];
                t.transform.localPosition = new Vector3(Mathf.Round(t.transform.localPosition.x), t.transform.localPosition.y, t.transform.localPosition.z);
                t.name = string.Format("Tile ({0}, {1})", t.transform.localPosition.x, t.transform.localPosition.y);
                bc.ghostTiles[0].transform.localPosition = new Vector3(Mathf.Round(bc.ghostTiles[0].transform.localPosition.x), 
                                                                                   bc.ghostTiles[0].transform.localPosition.y, 
                                                                                   bc.ghostTiles[0].transform.localPosition.z);
                bc.ghostTiles[1].transform.localPosition = new Vector3(Mathf.Round(bc.ghostTiles[1].transform.localPosition.x),
                                                                                   bc.ghostTiles[1].transform.localPosition.y,
                                                                                   bc.ghostTiles[1].transform.localPosition.z);
            }

            overallMovement = Mathf.RoundToInt(gameObject.transform.localPosition.x - initialPosition.x);
        }

        if (overallMovement != 0)
        {
            List<Vector2Int> tilesToRemove = bc.GetBoardLogic().Move(number, overallMovement, isColumnMoving);

            if (tilesToRemove.Count == 0 || tilesToRemove == null)
            {
                MoveBack(overallMovement);
                bc.SetEnableTileColliders(true);
                return;
            }

            StartCoroutine(DestroyMatchedTiles(tilesToRemove));
        }
        else
        {
            bc.SetEnableTileColliders(true);
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
        bc.isDestroying = true;
        int maxDistance = 0;
        int[] fallDistances = new int[BoardLogic.BOARD_SIZE];
        bool particlesPlaying = false;

        bc.GetGameModeManager().UpdateTurns();

        while (tilesToRemove.Count > 0)
        {
            //Hiding disappearing tiles
            tilesToRemove.ForEach((t) =>
            {
                bc.activeTileObjects[t.x][t.y].GetComponent<ColRowMover>().tileToRemove = true;
                bc.activeTileObjects[t.x][t.y].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                bc.activeTileObjects[t.x][t.y].transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
                bc.activeTileObjects[t.x][t.y].transform.GetChild(2).GetComponent<ParticleSystem>().Play();
                particlesPlaying = true;
            });

            while (particlesPlaying)
            {
                particlesPlaying = false;
                tilesToRemove.ForEach((t) =>
                {
                    particlesPlaying = bc.activeTileObjects[t.x][t.y].transform.GetChild(2).GetComponent<ParticleSystem>().IsAlive();
                });
                yield return null;
            };
            //yield return new WaitForSeconds(0.3f);

            //Falling down animation
            Sequence fallingSequence = DOTween.Sequence();
            for (int i = 0; i < BoardLogic.BOARD_SIZE; i++)
            {
                for (int j = 0; j < BoardLogic.BOARD_SIZE; j++)
                {
                    fallDistances[i] = tilesToRemove.FindAll(t => (t.x == i) && (t.y < j)).Count;

                    fallingSequence.Join(bc.activeTileObjects[i][j].transform.DOLocalMoveY(j - fallDistances[i], (float)Math.Sqrt(fallDistances[i]) / 2.0f));
                }

                fallDistances[i] = tilesToRemove.FindAll(t => (t.x == i) && (t.y < BoardLogic.BOARD_SIZE)).Count;

                for (int j = 0; j < BoardLogic.PROPHECY_HEIGHT; j++)
                {
                    fallingSequence.Join(bc.prophecyTileObjects[i][j].transform.DOLocalMoveY(BoardLogic.BOARD_SIZE + j - fallDistances[i], (float)Math.Sqrt(fallDistances[i]) / 2.0f));
                }

                if (maxDistance < fallDistances[i])
                {
                    maxDistance = fallDistances[i];
                }
            }
            fallingSequence.Play();
            yield return fallingSequence.WaitForCompletion();
            
            bc.AddScore(tilesToRemove.Count * 10);

            //Showing hidden tiles and scaling newly appeared prophecy tiles
            Sequence scalingSequence = DOTween.Sequence();
            for (int i = 0; i < BoardLogic.BOARD_SIZE; i++)
            {
                for (int j = 0; j < BoardLogic.BOARD_SIZE; j++)
                {
                    bc.activeTileObjects[i][j].transform.localPosition = new Vector3(i, j, 0);
                    bc.activeTileObjects[i][j].GetComponent<ColRowMover>().tileToRemove = false;
                    if (!bc.menuOpener.open)
                    {
                        bc.activeTileObjects[i][j].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                        bc.activeTileObjects[i][j].transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
                    }
                }

                for (int j = 0; j < BoardLogic.PROPHECY_HEIGHT; j++)
                {
                    bc.prophecyTileObjects[i][j].transform.localPosition = new Vector3(i, j + BoardLogic.BOARD_SIZE, 0);
                    if (BoardLogic.PROPHECY_HEIGHT - j <= fallDistances[i])
                    {
                        scalingSequence.Join(bc.prophecyTileObjects[i][j].transform.DOScale(0.0f, 0.1f).From());
                    }
                }
            }

            tilesToRemove = bc.GetBoardLogic().DestroyTiles(tilesToRemove);
            bc.UpdateDigitsBasic();
            scalingSequence.Play();
            yield return scalingSequence.WaitForCompletion();
        }

        bc.isDestroying = false;
        if (!bc.menuOpener.open)
        {
            bc.SetEnableTileColliders(true);
        }
    }

    private GameObject GetTile(int x, int y)
    {
        return bc.activeTileObjects[x][y];
    }

    private void ChangeGhost(GameObject ghost, GameObject tile, Vector3 offset)
    {
        ghost.transform.SetParent(bc.transform);
        ghost.transform.localPosition = tile.transform.localPosition + offset;
        ghost.name = string.Concat(tile.name, " Ghost");
        ghost.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = tile.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text;
    }

    private void ChangeGhostTiles()
    {
        if (isColumnMoving)
        {
            ChangeGhost(bc.ghostTiles[0], GetTile(number, 0), new Vector3(0, BoardLogic.BOARD_SIZE, 0));
            ChangeGhost(bc.ghostTiles[1], GetTile(number, BoardLogic.BOARD_SIZE - 1), new Vector3(0, -BoardLogic.BOARD_SIZE, 0));
        }
        else
        {
            ChangeGhost(bc.ghostTiles[0], GetTile(0, number), new Vector3(BoardLogic.BOARD_SIZE, 0, 0));
            ChangeGhost(bc.ghostTiles[1], GetTile(BoardLogic.BOARD_SIZE - 1, number), new Vector3(-BoardLogic.BOARD_SIZE, 0, 0));
        }
    }

    //DEBUG FUNCS

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