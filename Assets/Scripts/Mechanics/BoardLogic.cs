using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardLogic {
    public const int BOARD_SIZE = 7;
    public const int PROPHECY_HEIGHT = 5;

    public string[][] activeTiles = new string[BOARD_SIZE][];
    public string[][] prophecyTiles = new string[BOARD_SIZE][];

    System.Random randObj = new System.Random();

    public BoardLogic()
    {
        GenerateActiveTiles();
        while (MatchFinder.IsGameOver(activeTiles))
        {
            GenerateActiveTiles();
        }
        GenerateProphecyTiles();
    }

    public List<Vector2Int> Move(int number, int distance, bool isColumn)
    {
        if (isColumn)
        {
            return MoveColumn(number, distance);
        }
        else
        {
            return MoveRow(number, distance);
        }
    }

    /*
     * returns List - tiles that have to be deleted.
     * Distance can be negative for upwards movement.
     */
    private List<Vector2Int> MoveColumn(int x, int distance)
    {
        string[][] temporaryTiles = Util.CloneArray(activeTiles);

        string[] newColumn = new string[BOARD_SIZE];
        string[] currentColumn = activeTiles[x];

        for (int i = 0; i <= BOARD_SIZE - 1; i++)
        {
            int newIndex = i + distance;

            if (newIndex >= BOARD_SIZE)
            {
                newIndex %= BOARD_SIZE;
            }
            else if (newIndex < 0)
            {
                newIndex = BOARD_SIZE + (newIndex % BOARD_SIZE);
            }

            newColumn[newIndex] = currentColumn[i];
        }

        temporaryTiles[x] = newColumn;

        return TryMove(temporaryTiles);
    }

    /*
     * returns List - tiles that have to be deleted.
     * Distance can be negative for leftwards movement.
     */
    private List<Vector2Int> MoveRow(int y, int distance)
    {
        string[][] temporaryTiles = Util.CloneArray(activeTiles);

        for (int i = 0; i < BoardLogic.BOARD_SIZE; i++)
        {
            int newIndex = i + distance;

            if (newIndex >= BOARD_SIZE)
            {
                newIndex %= BOARD_SIZE;
            }
            else if (newIndex < 0)
            {
                newIndex = BOARD_SIZE + (newIndex % BOARD_SIZE);
            }

            temporaryTiles[newIndex][y] = activeTiles[i][y];
        }

        return TryMove(temporaryTiles);
    }

    /*
    * returns List - tiles that have to be deleted.
    */
    private List<Vector2Int> TryMove(string[][] temporaryTiles)
    {
        List<Vector2Int> tilesToRemove = MatchFinder.FindMatchingTiles(temporaryTiles);

        if (tilesToRemove.Count > 0)
        {
            activeTiles = temporaryTiles;
        }

        return tilesToRemove;
    }

    /*
     * Destroys tiles from activeTiles according to a list of arrays of tile coordinates
     * Fills the board in from the prophecyTiles and fills prophecyTiles with random numbers
     */
    public List<Vector2Int> DestroyTiles(List<Vector2Int> tiles)
    {
        string[] column;

        tiles.ForEach((t) =>
        {
            /* @WARNING change this if you decide to use 0 as one of the possible tile value */
            activeTiles[t.x][t.y] = B64X.EncodeInt(0);
        });

        for (int i = 0; i < BOARD_SIZE; i++)
        {
            column = activeTiles[i];

            for (int j = BOARD_SIZE - 1; j >= 0; j--)
            {
                /* @WARNING change this if you decide to use 0 as one of the possible tile value */
                if (B64X.DecodeInt(column[j]) == 0)
                {
                    SlideDownTo(i, j);
                }
            }
        }

        return MatchFinder.FindMatchingTiles(activeTiles);
    }

    private void SlideDownTo(int x, int y)
    {
        if (y < BOARD_SIZE - 1)
        {
            for (int i = y; i < BOARD_SIZE - 1; i++)
            {
                activeTiles[x][i] = activeTiles[x][i + 1];
            }
        }

        activeTiles[x][BOARD_SIZE - 1] = prophecyTiles[x][0];

        for (int i = 0; i < PROPHECY_HEIGHT - 1; i++)
        {
            prophecyTiles[x][i] = prophecyTiles[x][i + 1];
        }

        prophecyTiles[x][PROPHECY_HEIGHT - 1] = B64X.EncodeInt((randObj.Next(9) + 1));
    }

    private void GenerateActiveTiles()
    {
        int i;

        for (i = 0; i <= BOARD_SIZE - 1; i++)
        {
            activeTiles[i] = new string[BOARD_SIZE];

            for (int j = 0; j <= BOARD_SIZE - 1; j++)
            {
                activeTiles[i][j] = B64X.EncodeInt(0);
            }
        }

        for (i = 0; i <= BOARD_SIZE - 1; i++)
        {
            for (int j = 0; j <= BOARD_SIZE - 1; j++)
            {
                activeTiles[i][j] = B64X.EncodeInt((randObj.Next(9) + 1));

                while (MatchFinder.CheckForInitialMatches(activeTiles, i, j))
                {
                    activeTiles[i][j] = B64X.EncodeInt((randObj.Next(9) + 1));
                }
            }
        }
    }

    private void GenerateProphecyTiles()
    {
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            prophecyTiles[i] = new string[PROPHECY_HEIGHT];

            for (int j = 0; j < PROPHECY_HEIGHT; j++)
            {
                prophecyTiles[i][j] = B64X.EncodeInt((randObj.Next(9) + 1));
            }
        }
    }
}
