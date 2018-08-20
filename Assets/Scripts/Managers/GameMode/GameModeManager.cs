using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public enum GameMode
{
    None,
    TimeAttack,
    LimitedTurns,
    Tutorial
};

public class GameModeManager : MonoBehaviour {
    public static GameMode mode = GameMode.None;
    public ObjectiveTracker tracker;

    //Tutorial
    public GameObject TutorialPanel;
    public GameObject TutorialTxt;
    public Sprite greenBorder;
    public Sprite normalBorder;
    [HideInInspector] [System.NonSerialized]
    public bool tutorialShown = false;
    [HideInInspector][System.NonSerialized]
    public bool tutorialOpens = false;

    public BoardController boardController;
    public TextMeshProUGUI BombLeftTxt;
    public TextMeshProUGUI OTLeftTxt;
    public TextMeshProUGUI RegenLeftTxt;
    public TextMeshProUGUI WMLeftTxt;
    public TutorialNextBtn tutorialNextBtn;
    public EndGameBtn endGameBtn;

    private Tweener scaleTutorial;
    private float heightTutorial = 0.0f;

    // Use this for initialization
    void Awake () {
        mode = GameObject.FindWithTag("SceneLoadManager").GetComponent<SceneLoadManager>().currentGameMode;
        UpdateSceneForMode();
	}

    private void UpdateSceneForMode()
    {
        if (mode == GameMode.TimeAttack)
        {
            tracker = gameObject.transform.GetChild(1).gameObject.GetComponent<Timer>();
        }
        else if (mode == GameMode.LimitedTurns)
        {
            tracker = gameObject.transform.GetChild(1).gameObject.GetComponent<TurnCounter>();
        }
        else if (mode == GameMode.Tutorial)
        {
            tracker = gameObject.transform.GetChild(1).gameObject.GetComponent<SectionCounter>();
        }

        tracker.enabled = true;
    }

    public void TimerPauseSafe(bool enabled)
    {
        if (mode == GameMode.TimeAttack)
        {
            if (SafeMemory.GetInt("time") > 0)
            {
                (tracker as Timer).SetEnableTimer(!enabled);
            }
        }
    }

    public IEnumerator ShowTutorialMessage(bool enable)
    {
        if (mode == GameMode.Tutorial)
        {
            if (enable && !tutorialShown)
            {
                switch ((tracker as SectionCounter).sectionCurrent)
                {
                    case 1:
                        heightTutorial = 1500.0f;
                        TutorialTxt.GetComponent<TextMeshProUGUI>().text = "The playing field consists of Blue and Red tiles. " +
                            "You can shift an entire row or column of Blue tiles by any amount you want. Digits that go beyond the playing field will appear on the opposite side.  " +
                            "If you do not match any numbers, the row or column you moved will return to its initial position. " +
                            "When you match the sequence it will explode and give you score.The more tiles you destroy in one turn the more score you will get. " +
                            "After you execute a combo the empty space will be filled by red tiles falling down and becoming blue.\n\nNow try it yourself, move the green tiles right.";
                        SafeMemory.SetInt("bombLeft", 0);
                        BombLeftTxt.SetText(SafeMemory.GetInt("bombLeft").ToString());
                        SafeMemory.SetInt("overtimeLeft", 0);
                        OTLeftTxt.SetText(SafeMemory.GetInt("overtimeLeft").ToString());
                        SafeMemory.SetInt("regenLeft", 0);
                        RegenLeftTxt.SetText(SafeMemory.GetInt("regenLeft").ToString());
                        SafeMemory.SetInt("wrongMoveLeft", 0);
                        WMLeftTxt.SetText(SafeMemory.GetInt("wrongMoveLeft").ToString());
                        boardController.activeTileObjects[0][5].GetComponentInChildren<SpriteRenderer>().sprite = greenBorder;
                        boardController.activeTileObjects[1][5].GetComponentInChildren<SpriteRenderer>().sprite = greenBorder;
                        boardController.activeTileObjects[2][5].GetComponentInChildren<SpriteRenderer>().sprite = greenBorder;
                        break;
                    case 2:
                        heightTutorial = 1100.0f;
                        TutorialTxt.GetComponent<TextMeshProUGUI>().text = "In the game you can get a Combo. Combo is when sequences are completed by red tiles falling in. " +
                            "The longer the Combo the bigger the multiplier you get.\n\nSee for yourself, move green tiles up.\n\n" +
                            "Press continue and find the only possible sequence on the board. See how it will trigger 2x Combo with the red tiles falling down.";
                        boardController.boardLogic.LoadActiveTiles(TutorialBoards.active2);
                        boardController.boardLogic.LoadProphecyTiles(TutorialBoards.prophecy2);
                        boardController.UpdateDigitsBasic();
                        boardController.activeTileObjects[1][1].GetComponentInChildren<SpriteRenderer>().sprite = greenBorder;
                        boardController.activeTileObjects[1][2].GetComponentInChildren<SpriteRenderer>().sprite = greenBorder;
                        boardController.activeTileObjects[1][3].GetComponentInChildren<SpriteRenderer>().sprite = greenBorder;
                        boardController.activeTileObjects[1][4].GetComponentInChildren<SpriteRenderer>().sprite = greenBorder;
                        break;
                    case 3:
                        heightTutorial = 1100.0f;
                        TutorialTxt.GetComponent<TextMeshProUGUI>().text = "There are 4 power-ups in the game, that can get you out of sticky situations. " +
                            "Power-ups can be bought for in-game currency. You can get the currency by playing the game, or by purchasing it in the store.\n\n" +
                            "First power-up will fill the board with random numbers.\n\nPress continue and use the first power-up to see how it works.";
                        SafeMemory.SetInt("regenLeft", 1);
                        RegenLeftTxt.SetText(SafeMemory.GetInt("regenLeft").ToString());
                        break;
                    case 4:
                        heightTutorial = 850.0f;
                        TutorialTxt.GetComponent<TextMeshProUGUI>().text = "Second power-up is different in every mode. In \"Time Attack\" it will add extra 20 seconds. " +
                            "In \"Limited Turns\" it will add extra 2 turns.\n\nPress continue and use the second power-up to proceed to the next section of tutorial.";
                        SafeMemory.SetInt("overtimeLeft", 1);
                        OTLeftTxt.SetText(SafeMemory.GetInt("overtimeLeft").ToString());
                        break;
                    case 5:
                        heightTutorial = 850.0f;
                        TutorialTxt.GetComponent<TextMeshProUGUI>().text = "Third power-up will allow you to make a wrong move. " +
                            "This means that you can move a row or column but even if you don’t make a combo it will not snap to its initial position.\n\n" +
                            "Press continue and use the third power-up to see how it works.";
                        SafeMemory.SetInt("wrongMoveLeft", 1);
                        WMLeftTxt.SetText(SafeMemory.GetInt("wrongMoveLeft").ToString());
                        break;
                    case 6:
                        heightTutorial = 850.0f;
                        TutorialTxt.GetComponent<TextMeshProUGUI>().text = "Fourth power-up will explode a square of three by three tiles. " +
                            "You can press the tile in the middle to center the explosion, or you can drag the square around for more precise targeting.\n\n" +
                            "Press continue and use the fourth power - up to see how it works.";
                        SafeMemory.SetInt("bombLeft", 1);
                        BombLeftTxt.SetText(SafeMemory.GetInt("bombLeft").ToString());
                        break;
                    case 7:
                        heightTutorial = 850.0f;
                        TutorialTxt.GetComponent<TextMeshProUGUI>().text = "The game ends when you run out of time or turns, depending on the game mode you are playing. " +
                            "If you want to end the game earlier you can press \"Give Up\" button twice.\n\nPress continue and press on \"Give Up\" button twice to end the game.";
                        break;
                    case 8:
                        heightTutorial = 900.0f;
                        TutorialTxt.GetComponent<TextMeshProUGUI>().text = "Pro tips:\n\nYou might want to pay close attention to ones and nines during the game, as they tend to accumulate.\n\n" +
                            "In addition due to previous property of ones and nines it is statistically harder to make moves after a large combo.";
                        tutorialNextBtn.GetComponent<Button>().onClick.RemoveAllListeners();
                        tutorialNextBtn.GetComponent<Button>().onClick.AddListener(() => StartCoroutine(endGameBtn.EndGame()));
                        break;
                }
                TutorialPanel.SetActive(enable);
            }
            if (!enable)
            {
                tutorialNextBtn.transform.localScale = new Vector3(0.0f, 0.0f);
                TutorialTxt.transform.localScale = new Vector3(0.0f, 0.0f);
                if (scaleTutorial != null)
                {
                    if (scaleTutorial.IsActive())
                    {
                        scaleTutorial.Kill();
                    }
                }
                scaleTutorial = DOTween.To(x => TutorialPanel.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, x), heightTutorial, 0, 0.3f).SetEase(Ease.Linear);
                yield return scaleTutorial.WaitForCompletion();
            }
            if (enable && !boardController.menuOpener.open)
            {
                TutorialPanel.SetActive(true);
                if (scaleTutorial != null)
                {
                    if (scaleTutorial.IsActive())
                    {
                        scaleTutorial.Kill();
                    }
                }
                scaleTutorial = DOTween.To(x => TutorialPanel.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, x), 0, heightTutorial, 0.3f).SetEase(Ease.Linear);
                yield return scaleTutorial.WaitForCompletion();
                tutorialNextBtn.transform.localScale = new Vector3(1.0f, 1.0f);
                TutorialTxt.transform.localScale = new Vector3(1.0f, 1.0f);
            }
            yield return null;
        }
    }
}
