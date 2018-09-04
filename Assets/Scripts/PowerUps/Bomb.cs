using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using GameAnalyticsSDK;

public class Bomb : MonoBehaviour {
    public GameObject board;
    public BoardController boardController;
    public TextMeshProUGUI BombLeftTxt;
    public Sprite selectBorder;
    public Sprite normalBorder;
    public GameModeManager gameModeManager;

    public static bool used = false;

#if UNITY_EDITOR
    private bool firstTime = false;
    private bool mouseUp = false;
#endif
    private bool picking = false;
    private Coroutine picker;
    private List<Vector2Int> tilesToExplode = new List<Vector2Int>();
    private int x = -1;
    private int y = -1;

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
            mouseUp = true;
    }
#endif

    void Start()
    {
        used = false;
        //CHANGE TO A REAL VALUE LATER
        //SafeMemory.SetInt("bombLeft", 1);
        if (GameModeManager.mode != GameMode.Tutorial)
        {
            PowerUps.GetPowerUpLeft("bombLeft");
        }
        else
        {
            SafeMemory.SetInt("bombLeft", 0);
        }
        BombLeftTxt.SetText(SafeMemory.GetInt("bombLeft").ToString());

    }

    public void PickAndExplode()
    {
        if (!used)
        {
            if (picking)
            {
                StopCoroutine(picker);
                for (int i = 0; i < tilesToExplode.Count; i++)
                {
                    boardController.activeTileObjects[tilesToExplode[i].x][tilesToExplode[i].y].GetComponentInChildren<SpriteRenderer>().sprite = normalBorder;
                }
                boardController.SetEnableColRowMovers(true);
                //SafeMemory.SetInt("bombLeft", SafeMemory.GetInt("bombLeft") + 1);
                //BombLeftTxt.SetText(SafeMemory.GetInt("bombLeft").ToString());
                Util.SwapButtonColors(transform.GetComponent<Button>());
                picking = false;
            }
            else
            {
                if (SafeMemory.GetInt("bombLeft") > 0)
                {
                    picking = true;
                    boardController.SetEnableColRowMovers(false);
                    //SafeMemory.SetInt("bombLeft", SafeMemory.GetInt("bombLeft") - 1);
                    //BombLeftTxt.SetText(SafeMemory.GetInt("bombLeft").ToString());
                    Util.SwapButtonColors(transform.GetComponent<Button>());
                    picker = StartCoroutine(ProcessInputs());
                }
            }
        }
    }

    private IEnumerator ProcessInputs()
    {
        while(picking)
        {
#if UNITY_EDITOR
                if (Input.GetMouseButton(0))
#else
                if (Input.touchCount > 0)
#endif
                {
#if UNITY_EDITOR
                if (!firstTime)
                {
                    firstTime = true;
                    mouseUp = false;
                }
#endif
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
#if UNITY_EDITOR
                    if (mouseUp)
#else
                    if (Input.GetTouch(0).phase == TouchPhase.Ended)
#endif
                    {
                        picking = false;
                        for (int i = 0; i < tilesToExplode.Count; i++)
                        {
                            boardController.activeTileObjects[tilesToExplode[i].x][tilesToExplode[i].y].GetComponentInChildren<SpriteRenderer>().sprite = normalBorder;
                        }
                        used = true;
                        if (GameModeManager.mode != GameMode.Tutorial)
                        {
                            PowerUps.ChangePowerUpLeft("bombLeft", PowerUps.GetPowerUpLeft("bombLeft") - 1);
                        }
                        else
                        {
                            SafeMemory.SetInt("bombLeft", 0);
                        }
                        BombLeftTxt.SetText(SafeMemory.GetInt("bombLeft").ToString());
                        Util.SwapButtonColors(transform.GetComponent<Button>());
                        yield return StartCoroutine(boardController.DestroyMatchedTiles(tilesToExplode, true));
                        boardController.SetEnableColRowMovers(true);
                        if (GameModeManager.mode == GameMode.Tutorial)
                        {
                            (gameModeManager.tracker as SectionCounter).NextSection();
                        }
                        else
                        {
                            GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "bombLeft", 1, "Use", "PowerUpUse");
                        }
                    }
                    
                }
#if UNITY_EDITOR
                if (mouseUp)
                    firstTime = false;
                mouseUp = false;
#endif
            }
            if (picking)
            {
                yield return new WaitForEndOfFrame();
            }
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
