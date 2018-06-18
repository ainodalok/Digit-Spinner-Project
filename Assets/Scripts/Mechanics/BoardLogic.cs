using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLogic {
    public const int BOARD_SIZE = 7;
    public const int PROPHECY_HEIGHT = 5;

    public int[][] activeTiles = new int[BOARD_SIZE][];
    public int[][] prophecyTiles = new int[BOARD_SIZE][];

    System.Random randObj = new System.Random();

    public BoardLogic()
    {
        GenerateActiveTiles();
        EnsureNoMatchOnStart();
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
        int[][] temporaryTiles = Util.CloneArray(activeTiles);

        int[] newColumn = new int[BOARD_SIZE];
        int[] currentColumn = activeTiles[x];

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
        int[][] temporaryTiles = Util.CloneArray(activeTiles);

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
    private List<Vector2Int> TryMove(int[][] temporaryTiles)
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
        int[] column;

        tiles.ForEach((t) =>
        {
            /* @WARNING change this if you decide to use 0 as one of the possible tile value */
            activeTiles[t[0]][t[1]] = 0;
        });

        for (int i = 0; i < BOARD_SIZE; i++)
        {
            column = activeTiles[i];

            for (int j = BOARD_SIZE - 1; j >= 0; j--)
            {
                /* @WARNING change this if you decide to use 0 as one of the possible tile value */
                if (column[j] == 0)
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

        prophecyTiles[x][PROPHECY_HEIGHT - 1] = randObj.Next(9) + 1;
    }

    private void GenerateActiveTiles()
    {
        for (int i = 0; i <= BOARD_SIZE - 1; i++)
        {
            activeTiles[i] = new int[BOARD_SIZE];

            for (int j = 0; j <= BOARD_SIZE - 1; j++)
            {
                activeTiles[i][j] = randObj.Next(9) + 1;
            }
        }
    }

    /*
     * This one finds all the matches and deletes them, then fills 
     * empty tiles with random numbers. Repeat until no matches found.
     */
    private void EnsureNoMatchOnStart()
    {
        List<Vector2Int> tilesToRemove = MatchFinder.FindMatchingTiles(activeTiles);

        do
        {
            tilesToRemove.ForEach((t) =>
            {
                activeTiles[t[0]][t[1]] = randObj.Next(9) + 1;
            });

            tilesToRemove = MatchFinder.FindMatchingTiles(activeTiles);
        } while (tilesToRemove.Count > 0);
    }

    private void GenerateProphecyTiles()
    {
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            prophecyTiles[i] = new int[PROPHECY_HEIGHT];

            for (int j = 0; j < PROPHECY_HEIGHT; j++)
            {
                prophecyTiles[i][j] = randObj.Next(9) + 1;
            }
        }
    }
}
