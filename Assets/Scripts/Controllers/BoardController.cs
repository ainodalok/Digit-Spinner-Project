using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using GameAnalyticsSDK;

public class BoardController : MonoBehaviour {
    public BoardLogic boardLogic;
    public GameModeManager gameModeManager;

    public GameObject tilePrefab;
    public GameObject prophecyTilePrefab;
    public GameObject ghostTilePrefab;
    public MenuOpener menuOpener;
    public GameOverPanelController gameOverPanelController;
    
    /* Materials used to color tiles depending on the digit */
    public Material[] tileMaterials = new Material[10];
    //public Material[] prophecyTileMaterials = new Material[10];
    private MaterialPropertyBlock[] props = new MaterialPropertyBlock[10];

    public Material comboRed;
    public Material defaultMaterial;

    public TextMeshProUGUI scoreText;
    public Transform[] powerupTransforms;
    public TextMeshProUGUI WMLeftTxt;
    public Button WMLeftBtn;

    [HideInInspector]
    public GameObject[][] prophecyTileObjects = new GameObject[BoardLogic.BOARD_SIZE][];
    [HideInInspector]
    public GameObject[][] activeTileObjects = new GameObject[BoardLogic.BOARD_SIZE][];
    [HideInInspector]
    public GameObject[] ghostTiles = new GameObject[2];

    //[HideInInspector]
    //public int score;
    [HideInInspector]
    public int moveCount = 0;

    [HideInInspector]
    public bool isDestroying = false;

    [HideInInspector]
    public Sequence scalingSequence;
    [HideInInspector]
    public Sequence fallingSequence;
    [HideInInspector]
    public Sequence scalingHiddenSequence;
    [HideInInspector]
    public Sequence shakingSequence;

    public static Vector3 SPAWN_SIZE = new Vector3(0.0f, 0.0f, 1.0f);
    public static Vector3 ACTIVE_SIZE = new Vector3(1.0f, 1.0f, 1.0f);
    public static Vector3 SIZE_STEP = new Vector3(0.1f, 0.1f);
    const float INITIAL_SCALE_DURATION = 0.3f;

    private Vector3[][] prophecyTileScale = new Vector3[BoardLogic.BOARD_SIZE][];

    private float shakeDuration = 0.5f;
    private bool eventFor1000sent = false;
    private bool eventFor5000sent = false;

    void Start ()
    {
        boardLogic = new BoardLogic();
        SetupMaterialPropertyBlocks();
        SetupActiveTiles();
        SetupProphecyTiles();
        SetupGhostTiles();
        SafeMemory.SetInt("score", 0);
        if (GameModeManager.mode == GameMode.Tutorial)
        {
            shakeDuration = 3.0f;
        }
    }

    private void SetupMaterialPropertyBlocks()
    {
        for (int i = 0; i < 10; i++)
        {
            props[i] = new MaterialPropertyBlock();
            props[i].SetColor(ShaderUtilities.ID_OutlineColor, tileMaterials[i].GetColor(ShaderUtilities.ID_OutlineColor));
            props[i].SetColor(ShaderUtilities.ID_UnderlayColor, tileMaterials[i].GetColor(ShaderUtilities.ID_UnderlayColor));
        }
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
                newTile.transform.GetChild(0).GetComponent<TextMeshPro>().text = B64X.Decode(boardLogic.activeTiles[i][j]);

                //newTile.transform.GetChild(0).GetComponent<TextMeshPro>().fontMaterial = tileMaterials[B64X.DecodeInt(boardLogic.activeTiles[i][j])];
                newTile.transform.GetChild(0).GetComponent<TextMeshPro>().renderer.SetPropertyBlock(props[B64X.DecodeInt(boardLogic.activeTiles[i][j])]);


                activeTileObjects[i][j] = newTile;
            }
        }
    }

    private void SetupProphecyTiles()
    {
        for (int i = 0; i < BoardLogic.BOARD_SIZE; i++)
        {
            prophecyTileObjects[i] = new GameObject[BoardLogic.PROPHECY_HEIGHT];
            prophecyTileScale[i] = new Vector3[BoardLogic.PROPHECY_HEIGHT];

            for (int j = 0; j < BoardLogic.PROPHECY_HEIGHT; j++)
            {
                Vector2 position = new Vector2(i, j + BoardLogic.BOARD_SIZE);
                GameObject newTile = Instantiate(prophecyTilePrefab);
                newTile.transform.SetParent(gameObject.transform);
                newTile.name = String.Format("Prophecy Tile ({0}, {1})", i, j);
                newTile.transform.localPosition = position;
                newTile.transform.rotation = Quaternion.identity;
                newTile.transform.GetChild(0).GetComponent<TextMeshPro>().text = B64X.Decode(boardLogic.prophecyTiles[i][j]);

                //newTile.transform.GetChild(0).GetComponent<TextMeshPro>().fontMaterial = prophecyTileMaterials[B64X.DecodeInt(boardLogic.prophecyTiles[i][j])];
                newTile.transform.GetChild(0).GetComponent<TextMeshPro>().renderer.SetPropertyBlock(props[B64X.DecodeInt(boardLogic.prophecyTiles[i][j])]);

                prophecyTileObjects[i][j] = newTile;
                prophecyTileScale[i][j] = ACTIVE_SIZE - j * SIZE_STEP;
            }
        }
    }

    public void ScaleTilesUp()
    {
        if (scalingSequence != null)
        {
            if (scalingSequence.IsActive())
            {
                scalingSequence.Kill();
            }
        }
        scalingSequence = DOTween.Sequence();
        for (int i = 0; i < BoardLogic.BOARD_SIZE; i++)
        {
            for (int j = 0; j < BoardLogic.PROPHECY_HEIGHT; j++)
            {
                scalingSequence.Join(prophecyTileObjects[i][j].transform.DOScale(prophecyTileScale[i][j], INITIAL_SCALE_DURATION).SetEase(Ease.InOutSine));
                if (isDestroying)
                {
                    prophecyTileScale[i][j] = ACTIVE_SIZE - j * SIZE_STEP;
                }
            }
            for (int j = 0; j < BoardLogic.BOARD_SIZE; j++)
            {
                scalingSequence.Join(activeTileObjects[i][j].transform.DOScale(ACTIVE_SIZE, INITIAL_SCALE_DURATION).SetEase(Ease.InOutSine));
            }
        }
        for (int i = 0; i < powerupTransforms.Length; i++)
        {
            powerupTransforms[i].DOScale(ACTIVE_SIZE, INITIAL_SCALE_DURATION)
            .SetEase(Ease.InOutSine)
            .WaitForCompletion();
        }
        scalingSequence.Play();
    }

    public void ScaleTilesDown()
    {   if (scalingSequence != null)
        {
            if (scalingSequence.IsActive())
            {
                scalingSequence.Kill();
            }
        }
        scalingSequence = DOTween.Sequence();
        for (int i = 0; i < BoardLogic.BOARD_SIZE; i++)
        {
            for (int j = 0; j < BoardLogic.PROPHECY_HEIGHT; j++)
            {
                if (isDestroying)
                {
                    prophecyTileScale[i][j] = prophecyTileObjects[i][j].transform.localScale;
                }
                scalingSequence.Join(prophecyTileObjects[i][j].transform.DOScale(0, INITIAL_SCALE_DURATION).SetEase(Ease.InOutSine));
            }
            for (int j = 0; j < BoardLogic.BOARD_SIZE; j++)
            {
                scalingSequence.Join(activeTileObjects[i][j].transform.DOScale(0, INITIAL_SCALE_DURATION).SetEase(Ease.InOutSine));
            }
        }
        for (int i = 0; i < powerupTransforms.Length; i++)
        {
            powerupTransforms[i].DOScale(0, INITIAL_SCALE_DURATION)
            .SetEase(Ease.InOutSine)
            .WaitForCompletion();
        }
        scalingSequence.Play();
    }

    private void SetupGhostTiles()
    {
        ghostTiles[0] = CreateGhostTile(activeTileObjects[0][0], new Vector3(0, BoardLogic.BOARD_SIZE, 0));
        ghostTiles[1] = CreateGhostTile(activeTileObjects[0][BoardLogic.BOARD_SIZE - 1], new Vector3(0, -BoardLogic.BOARD_SIZE, 0));
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
                    B64X.Decode(boardLogic.activeTiles[i][j]);
                //activeTileObjects[i][j].transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().fontMaterial =
                //tileMaterials[B64X.DecodeInt(boardLogic.activeTiles[i][j])];
                activeTileObjects[i][j].transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().renderer.SetPropertyBlock(props[B64X.DecodeInt(boardLogic.activeTiles[i][j])]);
            }

            for (int j = 0; j < BoardLogic.PROPHECY_HEIGHT; j++)
            {
                prophecyTileObjects[i][j].transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text =
                    B64X.Decode(boardLogic.prophecyTiles[i][j]);
                //prophecyTileObjects[i][j].transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().fontMaterial =
                //prophecyTileMaterials[B64X.DecodeInt(boardLogic.prophecyTiles[i][j])];
                prophecyTileObjects[i][j].transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().renderer.SetPropertyBlock(props[B64X.DecodeInt(boardLogic.prophecyTiles[i][j])]);
            }
        }
    }

    private void AddScore(int add, int combo)
    {
        //float modifier = 1.0f;
        SafeMemory.Set("scoreModifier", 1.0f.ToString());
        if (add > 3)
        {
            for (int i = 0; i < (add - 3); i++)
            {
                SafeMemory.Set("scoreModifier", (SafeMemory.GetFloat("scoreModifier") + 0.2f * (i + 1)).ToString());
                //modifier += 0.2f * (i + 1);
            }
        }
        SafeMemory.SetInt("score", SafeMemory.GetInt("score") + (int) (add * 10 * combo * SafeMemory.GetFloat("scoreModifier")));
        //score += add * 10 * combo * modifier
        if (combo > 1)
        {
            StartCoroutine(ShowCombo(combo));
        }
        else
        {
            scoreText.text = string.Format("Score: \n{0}", SafeMemory.Get("score"));
        }
        if (!eventFor1000sent && SafeMemory.GetInt("score") > 1000)
        {
            GameAnalytics.NewDesignEvent("Game:Score:1000");
            eventFor1000sent = true;
        }
        if (!eventFor5000sent && SafeMemory.GetInt("score") > 5000)
        {
            GameAnalytics.NewDesignEvent("Game:Score:5000");
            eventFor5000sent = true;
        }
    }

    private IEnumerator ShowCombo(int combo)
    {
        scoreText.text = string.Format("Combo {0}X!", combo);
        scoreText.fontSharedMaterial = comboRed;
        //scoreText.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, new Color32((byte) (51 * combo), 0, 0, 255));
        scoreText.fontSharedMaterial.SetColor(ShaderUtilities.ID_UnderlayColor, new Color32((byte) ((14 * combo + 185) % 256), 0, 0, 255));
        //scoreText.color = new Color(combo*0.2f, 1.0f - combo * 0.2f, 1.0f - combo * 0.2f, 1.0f);

        yield return scoreText.transform.DOShakePosition(1.0f, combo*15.0f, 1000, 90.0f, false, false).WaitForCompletion();

        //scoreText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        scoreText.fontSharedMaterial = defaultMaterial;
        scoreText.text = string.Format("Score: \n{0}", SafeMemory.Get("score"));
    }

    private GameObject CreateGhostTile(GameObject tile, Vector3 offset)
    {
        GameObject newTile = Instantiate(ghostTilePrefab);
        newTile.transform.SetParent(gameObject.transform);
        newTile.transform.localPosition = tile.transform.localPosition + offset;
        newTile.name = string.Concat(tile.name, " Ghost");
        newTile.transform.rotation = Quaternion.identity;
        newTile.transform.localScale = ACTIVE_SIZE;
        newTile.transform.GetChild(0).GetComponent<TextMeshPro>().text = tile.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text;
        newTile.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().fontMaterial = tile.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().fontSharedMaterial;
        return newTile;
    }

    public void SetEnableBoard(bool enable)
    {
        ParticleSystem[] particleSystem = gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem comp in particleSystem)
        {
            comp.gameObject.GetComponent<ParticleSystemRenderer>().enabled = enable;
            if (comp.gameObject.name == "CircleElectricity" || comp.gameObject.name == "Sparks")
            {
                if (enable)
                {
                    if (comp.isPaused)
                    {
                        comp.Play();
                    }
                }
                else
                {
                    if (comp.isPlaying)
                    {
                        comp.Pause();
                    }
                }
            }
            else
            {
                if (enable)
                {
                    comp.Play();
                }
                else
                {
                    comp.Pause();
                }
            }
        }

        MeshRenderer[] meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer comp in meshRenderers)
        {
            ColRowMover colRowMover = comp.transform.parent.gameObject.GetComponent<ColRowMover>();
            if (colRowMover != null)
            {
                if (!colRowMover.tileToRemove)
                {
                    comp.enabled = enable;
                }
            }
            else
            {
                comp.enabled = enable;
            }
            
        }

        SpriteRenderer[] spriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer comp in spriteRenderers)
        {
            ColRowMover colRowMover = comp.transform.parent.gameObject.GetComponent<ColRowMover>();
            if (colRowMover != null)
            {
                if (!colRowMover.tileToRemove)
                {
                    comp.enabled = enable;
                }
            }
            else
            {
                comp.enabled = enable;
            }
        }

        if (!isDestroying)
        {
            SetEnableTileColliders(enable);
        }
    }

    public void SetEnableColRowMovers(bool enable)
    {
        ColRowMover[] colRowMovers = gameObject.GetComponentsInChildren<ColRowMover>();
        foreach (ColRowMover comp in colRowMovers)
        {
            comp.enabled = enable;
        }
    }

    public void SetEnableTileColliders(bool enable)
    {
        BoxCollider2D[] colliders = gameObject.GetComponentsInChildren<BoxCollider2D>();
        foreach (BoxCollider2D comp in colliders)
        {
            comp.enabled = enable;
        }
    }

    public IEnumerator DestroyMatchedTiles(List<Vector2Int> tilesToRemove, bool powerup = false)
    {
        isDestroying = true;
        int maxDistance = 0;
        int[] fallDistances = new int[BoardLogic.BOARD_SIZE];
        bool particlesPlaying = false;
        SafeMemory.SetInt("combo", 0);
        //int combo = 0;

        if (GameModeManager.mode == GameMode.LimitedTurns)
        {
            if (!powerup)
            {
                (gameModeManager.tracker as TurnCounter).UpdateTurns();
            }
        }

        if (GameModeManager.mode == GameMode.Tutorial)
        {
            if (((gameModeManager.tracker as SectionCounter).sectionCurrent == 1) || ((gameModeManager.tracker as SectionCounter).sectionCurrent == 2))
            {
                for(int i = 0; i < BoardLogic.BOARD_SIZE; i++)
                {
                    for(int j = 0; j < BoardLogic.BOARD_SIZE; j++)
                    {
                        activeTileObjects[i][j].GetComponentInChildren<SpriteRenderer>().sprite = gameModeManager.normalBorder;
                    }
                }
            }
        }

        while (tilesToRemove.Count > 0)
        {
            //Calculating score
            SafeMemory.SetInt("combo", SafeMemory.GetInt("combo") + 1);
            //combo++;
            AddScore(tilesToRemove.Count, SafeMemory.GetInt("combo"));

            /*
            float time = 0;
            while (time < 1.0f)
            {
                yield return null;
                if (!menuOpener.open)
                    time += Time.deltaTime;
                tilesToRemove.ForEach((t) =>
                {
                    activeTileObjects[t.x][t.y].transform.GetChild(0).GetComponent<TextMeshPro>().color =
                    new Color(1.0f, 1.0f, 1.0f) * (1.0f - time) + 
                    time * activeTileObjects[t.x][t.y].transform.GetChild(0).GetComponent<TextMeshPro>().fontSharedMaterial.GetColor(ShaderUtilities.ID_OutlineColor);
                });
            }
            */

            shakingSequence = DOTween.Sequence();
            shakingSequence.Pause();
            tilesToRemove.ForEach((t) =>
            {
                shakingSequence.Join(activeTileObjects[t.x][t.y].transform.DOShakePosition(shakeDuration, 0.1f, 10000, 90.0f, false, false));
            });

            if (!(menuOpener.open || (!menuOpener.open && menuOpener.menuToggles)))
            {
                shakingSequence.Play();
            }
            yield return shakingSequence.WaitForCompletion();

            //Hiding disappearing tiles
            tilesToRemove.ForEach((t) =>
            {
                activeTileObjects[t.x][t.y].GetComponent<ColRowMover>().tileToRemove = true;
                activeTileObjects[t.x][t.y].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                activeTileObjects[t.x][t.y].transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
                //activeTileObjects[t.x][t.y].transform.GetChild(0).GetComponent<TextMeshPro>().color = new Color32(255, 255, 255, 255);
                activeTileObjects[t.x][t.y].transform.GetChild(2).GetComponent<ParticleSystem>().Play();
                particlesPlaying = true;
            });

            while (particlesPlaying)
            {
                particlesPlaying = false;
                for (int i = 0; (i < tilesToRemove.Count) && (!particlesPlaying); i++)
                {
                    particlesPlaying = activeTileObjects[tilesToRemove[i].x][tilesToRemove[i].y].transform.GetChild(2).GetComponent<ParticleSystem>().isPlaying;
                }
                yield return null;
            }
            //Falling down and scaling falling prophecy tiles animation
            fallingSequence = DOTween.Sequence();
            fallingSequence.Pause();
            Vector3 newSize;

            for (int i = 0; i < BoardLogic.BOARD_SIZE; i++)
            {
                for (int j = 0; j < BoardLogic.BOARD_SIZE; j++)
                {
                    fallDistances[i] = tilesToRemove.FindAll(t => (t.x == i) && (t.y < j)).Count;

                    fallingSequence.Join(activeTileObjects[i][j].transform.DOLocalMoveY(j - fallDistances[i], (float)Math.Sqrt(fallDistances[i]) / 2.0f));
                }

                fallDistances[i] = tilesToRemove.FindAll(t => (t.x == i) && (t.y < BoardLogic.BOARD_SIZE)).Count;

                for (int j = 0; j < BoardLogic.PROPHECY_HEIGHT; j++)
                {
                    fallingSequence.Join(prophecyTileObjects[i][j].transform.DOLocalMoveY(BoardLogic.BOARD_SIZE + j - fallDistances[i], (float)Math.Sqrt(fallDistances[i]) / 2.0f));

                    //Adding scaling tweens for prophecy tiles
                    newSize = ACTIVE_SIZE + (fallDistances[i] - j) * SIZE_STEP;
                    if (newSize.x > 1)
                    {
                        newSize = ACTIVE_SIZE;
                    }
                    fallingSequence.Join(prophecyTileObjects[i][j].transform.DOScale(newSize, (float)Math.Sqrt(fallDistances[i]) / 2.0f));
                }

                if (maxDistance < fallDistances[i])
                {
                    maxDistance = fallDistances[i];
                }
            }
            if(!(menuOpener.open || (!menuOpener.open && menuOpener.menuToggles)))
            {
                fallingSequence.Play();
            }
            yield return fallingSequence.WaitForCompletion();
            //Showing hidden tiles and scaling newly appeared prophecy tiles
            scalingHiddenSequence = DOTween.Sequence();
            scalingHiddenSequence.Pause();
            for (int i = 0; i < BoardLogic.BOARD_SIZE; i++)
            {
                for (int j = 0; j < BoardLogic.BOARD_SIZE; j++)
                {
                    activeTileObjects[i][j].transform.localPosition = new Vector3(i, j, 0);
                    activeTileObjects[i][j].GetComponent<ColRowMover>().tileToRemove = false;
                    if (!(menuOpener.open || (!menuOpener.open && menuOpener.menuToggles)))
                    {
                        activeTileObjects[i][j].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                        activeTileObjects[i][j].transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
                    }
                }

                for (int j = 0; j < BoardLogic.PROPHECY_HEIGHT; j++)
                {
                    prophecyTileObjects[i][j].transform.localPosition = new Vector3(i, j + BoardLogic.BOARD_SIZE, 0);
                    if (BoardLogic.PROPHECY_HEIGHT - j <= fallDistances[i])
                    {
                        prophecyTileObjects[i][j].transform.localScale = SPAWN_SIZE;
                        scalingHiddenSequence.Join(prophecyTileObjects[i][j].transform.DOScale(ACTIVE_SIZE - j * SIZE_STEP, 0.1f).SetEase(Ease.InOutSine));
                    }
                    else
                    {
                        prophecyTileObjects[i][j].transform.localScale = ACTIVE_SIZE - j * SIZE_STEP;
                    }
                }
            }

            tilesToRemove = boardLogic.DestroyTiles(tilesToRemove);
            UpdateDigitsBasic();
            if (!(menuOpener.open || (!menuOpener.open && menuOpener.menuToggles)))
            {
                scalingHiddenSequence.Play();
            }
            yield return scalingHiddenSequence.WaitForCompletion();
        }

        if (!(menuOpener.open || (!menuOpener.open && menuOpener.menuToggles)))
        {
            SetEnableTileColliders(true);
        }

        isDestroying = false;

        //gameOverPanelController.animationCoroutine = StartCoroutine(AnimateGameOverNotification());
        if (!gameModeManager.tracker.gameOver && MatchFinder.IsGameOver(boardLogic.activeTiles))
        {
            yield return gameOverPanelController.animationCoroutine = StartCoroutine(AnimateGameOverNotification());
        }
        if ((GameModeManager.mode == GameMode.Tutorial) &&
            (((gameModeManager.tracker as SectionCounter).sectionCurrent == 2) || ((gameModeManager.tracker as SectionCounter).sectionCurrent == 1)))
        {
            (gameModeManager.tracker as SectionCounter).NextSection();
        }

        moveCount++;
    }

    public IEnumerator AnimateGameOverNotification()
    {
        gameOverPanelController.isShowing = true;
        gameModeManager.TimerPauseSafe(true);
        ScaleTilesDown();
        yield return scalingSequence.WaitForCompletion();
        SetEnableBoard(false);
        yield return StartCoroutine(gameOverPanelController.Animate());
        SetEnableBoard(true);
        ScaleTilesUp();
        yield return scalingSequence.WaitForCompletion();
        gameModeManager.TimerPauseSafe(false);
        gameOverPanelController.isShowing = false;
    }
}
