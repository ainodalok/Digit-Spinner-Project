using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bomb : MonoBehaviour {
    public GameObject board;
    public BoardController boardController;
    public TextMeshProUGUI BombLeftTxt;
    public Sprite selectBorder;
    public Sprite normalBorder;

    private bool firstTime = false;
    private bool mouseUp = false;
    private bool picking = false;
    private Coroutine picker;
    private List<Vector2Int> tilesToExplode = new List<Vector2Int>();
    private int x = -1;
    private int y = -1;

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
            mouseUp = true;
    }

    void Awake()
    {
        //CHANGE TO A REAL VALUE LATER
        SafeMemory.SetInt("bombLeft", 1); 
        BombLeftTxt.SetText(SafeMemory.GetInt("bombLeft").ToString());
    }

    public void PickAndExplode()
    {
        if (picking)
        {
            StopCoroutine(picker);
            for (int i = 0; i < tilesToExplode.Count; i++)
            {
                boardController.activeTileObjects[tilesToExplode[i].x][tilesToExplode[i].y].GetComponentInChildren<SpriteRenderer>().sprite = normalBorder;
            }
            EnableColRowMovers(true);
            SafeMemory.SetInt("bombLeft", SafeMemory.GetInt("bombLeft") + 1);
            BombLeftTxt.SetText(SafeMemory.GetInt("bombLeft").ToString());
            picking = false;
        }
        else
        {
            if (SafeMemory.GetInt("bombLeft") > 0)
            {
                picking = true;
                EnableColRowMovers(false);
                SafeMemory.SetInt("bombLeft", SafeMemory.GetInt("bombLeft") - 1);
                BombLeftTxt.SetText(SafeMemory.GetInt("bombLeft").ToString());
                picker = StartCoroutine(ProcessInputs());
            }
        }
    }

    private IEnumerator ProcessInputs()
    {
        while(picking)
        {
            //if (Input.touchCount > 0)
            if (Input.GetMouseButton(0))
            {
                if(!firstTime)
                {
                    firstTime = true;
                    mouseUp = false;
                }
                Vector3 worldPointPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//Input.GetTouch(0).position);
                Vector2 touchPosition = new Vector2(worldPointPosition.x, worldPointPosition.y);
                Collider2D collider = Physics2D.OverlapPoint(touchPosition);
                if (collider != null)
                {
                    if ((x != (int)collider.transform.localPosition.x) || (y != (int)collider.transform.localPosition.y))
                    {
                        x = (int)collider.transform.localPosition.x;
                        y = (int)collider.transform.localPosition.y;
                        RecalculateTilesToExplode(x, y);
                    }
                    Debug.Log(mouseUp);
                    //if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    if (mouseUp)
                    {
                        for (int i = 0; i < tilesToExplode.Count; i++)
                        {
                            boardController.activeTileObjects[tilesToExplode[i].x][tilesToExplode[i].y].GetComponentInChildren<SpriteRenderer>().sprite = normalBorder;
                        }
                        yield return StartCoroutine(boardController.DestroyMatchedTiles(tilesToExplode, true));
                        EnableColRowMovers(true);
                        picking = false;
                        
                    }
                    
                }
                if (mouseUp)
                    firstTime = false;
                
            }
            mouseUp = false;
            if (picking)
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    private void EnableColRowMovers(bool enable)
    {
        ColRowMover[] colRowMovers = board.GetComponentsInChildren<ColRowMover>();
        foreach (ColRowMover comp in colRowMovers)
        {
            comp.enabled = enable;
        }
    }

    private void RecalculateTilesToExplode(int coreX, int coreY)
    {
        for (int i = 0; i < tilesToExplode.Count; i++)
        {
            boardController.activeTileObjects[tilesToExplode[i].x][tilesToExplode[i].y].GetComponentInChildren<SpriteRenderer>().sprite = normalBorder;
        }
        tilesToExplode = new List<Vector2Int>();
        for (int dx = -1; dx < 2; dx++)
        {
            for (int dy = -1; dy < 2; dy++)
            {
                int newX = coreX + dx;
                int newY = coreY + dy;
                if ((newX < BoardLogic.BOARD_SIZE) && (newX >= 0) && (newY < BoardLogic.BOARD_SIZE) && (newY >= 0))
                {
                    tilesToExplode.Add(new Vector2Int(newX, newY));
                    boardController.activeTileObjects[newX][newY].GetComponentInChildren<SpriteRenderer>().sprite = selectBorder;
                }
            }
        }
    }
}
