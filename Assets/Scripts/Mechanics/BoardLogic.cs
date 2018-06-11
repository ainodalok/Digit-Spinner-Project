using System.Collections;
using System.Collections.Generic;

public class BoardLogic {
    public const int BOARD_SIZE = 9;
    public const int PROPHECY_HEIGHT = 5;

    public int[][] activeTiles = new int[BOARD_SIZE][];
    public int[][] prophecyTiles = new int[PROPHECY_HEIGHT][];

    System.Random randObj = new System.Random();

    public BoardLogic()
    {
        GenerateActiveTiles();
        EnsureNoMatchOnStart();
        GenerateProphecyTiles();
    }

    /*
     * returns int - amount of tiles that have been deleted.
     * Distance can be negative for upwards movement.
     */ 
    public int MoveColumn(int x, int distance)
    {
        int[][] temporaryTiles = Util.CloneArray(activeTiles);

        int[] newColumn = new int[BOARD_SIZE];
        int[] currentColumn = activeTiles[x];

        for (int i = 0; i <= BOARD_SIZE - 1; i++)
        {
            int newIndex = i + distance;

            if (newIndex >= BOARD_SIZE)
            {
                newIndex -= BOARD_SIZE;
            }
            else if (newIndex < 0)
            {
                newIndex += BOARD_SIZE;
            }

            newColumn[newIndex] = currentColumn[i];
        }

        temporaryTiles[x] = newColumn;

        return TryMove(temporaryTiles);
    }

    /*
     * returns int - amount of tiles that have been deleted.
     * Distance can be negative for leftwards movement.
     */
    public int MoveRow(int y, int distance)
    {
        int[][] temporaryTiles = Util.CloneArray(activeTiles);

        for (int i = 0; i <= 0; i++)
        {
            int newIndex = i + distance;

            if (newIndex >= BOARD_SIZE)
            {
                newIndex -= BOARD_SIZE;
            }
            else if (newIndex < 0)
            {
                newIndex += BOARD_SIZE;
            }

            temporaryTiles[newIndex][y] = activeTiles[i][y];
        }

        return TryMove(temporaryTiles);
    }

    /*
    * returns int - amount of tiles that have been deleted.
    */
    private int TryMove(int[][] temporaryTiles)
    {
        List<int[]> tilesToRemove = MatchFinder.FindMatchingTiles(temporaryTiles);
        int count = tilesToRemove.Count;

        if (count > 0)
        {
            activeTiles = temporaryTiles;
            DestroyTiles(tilesToRemove);
        }

        return count;
    }

    /*
     * Destroys tiles from activeTiles according to a list of arrays of tile coordinates
     * Fills the board in from the prophecyTiles and fills prophecyTiles with random numbers
     */
    private void DestroyTiles(List<int[]> tiles)
    {
        do
        {
            tiles.ForEach((t) =>
            {
                for (int i = t[1]; i >= 1; i--)
                {
                    activeTiles[t[0]][i] = activeTiles[t[0]][i - 1];
                }

                activeTiles[t[0]][0] = prophecyTiles[t[0]][PROPHECY_HEIGHT - 1];

                for (int i = PROPHECY_HEIGHT - 1; i <= 1; i--)
                {
                    prophecyTiles[t[0]][i] = prophecyTiles[t[0]][i - 1];
                }

                prophecyTiles[t[0]][0] = randObj.Next(10);
            });

            tiles = MatchFinder.FindMatchingTiles(activeTiles);
        } while (tiles.Count > 0);
    }

    private void GenerateActiveTiles()
    {
        for (int i = 0; i <= BOARD_SIZE - 1; i++)
        {
            activeTiles[i] = new int[BOARD_SIZE];

            for (int j = 0; j <= BOARD_SIZE - 1; j++)
            {
                activeTiles[i][j] = randObj.Next(10);
            }
        }
    }

    /*
     * This one finds all the matches and deletes them, then fills 
     * empty tiles with random numbers. Repeat until no matches found.
     */
    private void EnsureNoMatchOnStart()
    {
        List<int[]> tilesToRemove = MatchFinder.FindMatchingTiles(activeTiles);

        do
        {
            tilesToRemove.ForEach((t) =>
            {
                activeTiles[t[0]][t[1]] = randObj.Next(10);
            });

            tilesToRemove = MatchFinder.FindMatchingTiles(activeTiles);
        } while (tilesToRemove.Count > 0);
    }

    private void GenerateProphecyTiles()
    {
        for (int i = 0; i <= PROPHECY_HEIGHT - 1; i++)
        {
            prophecyTiles[i] = new int[BOARD_SIZE];

            for (int j = 0; j <= PROPHECY_HEIGHT - 1; j++)
            {
                prophecyTiles[i][j] = randObj.Next(10);
            }
        }
    }
}
