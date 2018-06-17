using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchFinder
{
    //List of all found "matches" - sequences of 3 and more ascending tiles
    static private List<List<Vector2Int>> matchList;
    //A match that is currently being added to and found new members for
    static private List<Vector2Int> currentMatch;
    static private int[][] board;

    public static List<Vector2Int> FindMatchingTiles(int[][] b)
    {
        matchList = new List<List<Vector2Int>>();
        board = b;

        for (int i = 0; i <= board.Length - 1; i++)
        {
            for (int j = 0; j <= board[0].Length - 1; j++)
            {
                currentMatch = null;
                Look(i, j);
            }
        }

        List<Vector2Int> matchingTiles = new List<Vector2Int>();

        matchList.ForEach((match) =>
        {
            match.ForEach((tile) =>
            {
                if (!matchingTiles.Contains(tile))
                {
                   matchingTiles.Add(tile);
                }
            });
        });

        return matchingTiles;
    }

    private static void Look(int x = 0, int y = 0)
    {
        bool nothingFound = true;
        int boardSize = board[0].Length;
        int currentNumber = board[x][y];

        Vector2Int currentTile = new Vector2Int(x, y);

        if (currentMatch == null)
        {
            currentMatch = new List<Vector2Int>();
        }

        currentMatch.Add(currentTile);
         
        if (x + 1 < boardSize && board[x + 1][y] - currentNumber == 1)
        {
            nothingFound = false;
            Look(x + 1, y);
        }

        if (y + 1 < boardSize && board[x][y + 1] - currentNumber == 1)
        {
            nothingFound = false;
            Look(x, y + 1);
        }

        if (x - 1 >= 0 && board[x - 1][y] - currentNumber == 1)
        {
            nothingFound = false;
            Look(x - 1, y);
        }

        if (y - 1 >= 0 && board[x][y - 1] - currentNumber == 1)
        {
            nothingFound = false;
            Look(x, y - 1);
        }

        //This means a sequence is complete - could not find any continuation.
        if (nothingFound && currentMatch.Count >= 3)
        {
            matchList.Add(new List<Vector2Int>(currentMatch));
        }

        //remove current tile from the current match
        currentMatch.RemoveAt(currentMatch.Count - 1);
    }
}
