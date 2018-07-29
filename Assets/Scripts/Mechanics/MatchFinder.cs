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
    static private string[][] board;

    public static List<Vector2Int> FindMatchingTiles(string[][] b)
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
        int currentNumber = B64X.DecodeInt(board[x][y]);

        Vector2Int currentTile = new Vector2Int(x, y);

        if (currentMatch == null)
        {
            currentMatch = new List<Vector2Int>();
        }

        currentMatch.Add(currentTile);
         
        if (x + 1 < boardSize && B64X.DecodeInt(board[x + 1][y]) - currentNumber == 1)
        {
            nothingFound = false;
            Look(x + 1, y);
        }

        if (y + 1 < boardSize && B64X.DecodeInt(board[x][y + 1]) - currentNumber == 1)
        {
            nothingFound = false;
            Look(x, y + 1);
        }

        if (x - 1 >= 0 && B64X.DecodeInt(board[x - 1][y]) - currentNumber == 1)
        {
            nothingFound = false;
            Look(x - 1, y);
        }

        if (y - 1 >= 0 && B64X.DecodeInt(board[x][y - 1]) - currentNumber == 1)
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

    //run when generating board to check for matches around a certain tile
    public static bool CheckForInitialMatches(string[][] b, int x, int y)
    {
        board = b;
        bool result = false;
        //List of coordinates of tiles that are by 1 greater than tile (x, y);
        List<Vector2Int> greater = SearchAround(x, y, B64X.DecodeInt(board[x][y]) + 1);
        //List of coordinates of tiles that are by 1 lesser than tile (x, y);
        List<Vector2Int> lesser = SearchAround(x, y, B64X.DecodeInt(board[x][y]) - 1);

        if (greater.Count > 0 && lesser.Count > 0)
        {
            result = true;
        }

        if (!result)
        {
            greater.ForEach((t) =>
            {
                if (SearchAround(t.x, t.y, B64X.DecodeInt(board[t.x][t.y]) + 1).Count > 0)
                {
                    result = true;
                }
            });
        }

        if (!result)
        {
            lesser.ForEach((t) =>
            {
                if (SearchAround(t.x, t.y, B64X.DecodeInt(board[t.x][t.y]) - 1).Count > 0)
                {
                    result = true;
                }
            });
        }

        return result;
    }

    //looks for goal in neighbours of tile (x, y) and tells its coordinates
    private static List<Vector2Int> SearchAround(int x, int y, int goal)
    {
        List<Vector2Int> result = new List<Vector2Int>();

        if (goal < 1 || goal > 9)
        {
            return result;
        }

        if (x > 0 && B64X.DecodeInt(board[x - 1][y]) == goal)
        {
            result.Add(new Vector2Int(x - 1, y));
        }
        if (y > 0 && B64X.DecodeInt(board[x][y - 1]) == goal)
        {
            result.Add(new Vector2Int(x, y - 1));
        }
        if (x < board.Length - 1 && B64X.DecodeInt(board[x + 1][y]) == goal)
        {
            result.Add(new Vector2Int(x + 1, y));
        }
        if (y < board.Length - 1 && B64X.DecodeInt(board[x][y + 1]) == goal)
        {
            result.Add(new Vector2Int(x, y + 1));
        }    

        return result;
    }

    //searches for goal around a certain tile, but at a 1 tile distance from the tile
    private static List<Vector2Int> SearchOneTileAway(int x, int y, int goal)
    {
        List<Vector2Int> result = new List<Vector2Int>();

        if (goal < 1 || goal > 9)
        {
            return result;
        }

        if (x > 1 && B64X.DecodeInt(board[x - 2][y]) == goal)
        {
            result.Add(new Vector2Int(x - 2, y));
        }
        if (y > 1 && B64X.DecodeInt(board[x][y - 2]) == goal)
        {
            result.Add(new Vector2Int(x, y - 2));
        }
        if (x < board.Length - 2 && B64X.DecodeInt(board[x + 2][y]) == goal)
        {
            result.Add(new Vector2Int(x + 2, y));
        }
        if (y < board.Length - 2 && B64X.DecodeInt(board[x][y + 2]) == goal)
        {
            result.Add(new Vector2Int(x, y + 2));
        }

        return result;
    }

    /* checks if there is a possible move with current active tiles */
    public static bool IsGameOver(string[][] b)
    {
        board = b;
        bool result = true;

        for (int i = 0; i < board.Length; i++)
        {
            for (int j = 0; j < board[0].Length; j++)
            {
                if (CheckTileForMatches(i, j))
                {
                    result = false;

                    break;
                }
            }

            if (!result)
            {
                break;
            }
        }

        return result;
    }

    private static bool CheckTileForMatches(int i, int j)
    {
        bool result = false;

        //search board for matches of 2 and two to find them a friend 
        SearchAround(i, j, B64X.DecodeInt(board[i][j]) + 1).ForEach((t) =>
        {
            //if tiles stand vertically
            if (t.x == i)
            {
                //check left column
                if (t.x > 1)
                {
                    if (board[t.x - 1].Contains(B64X.EncodeInt(B64X.DecodeInt(board[i][j]) - 1)) ||
                        board[t.x - 1].Contains(B64X.EncodeInt(B64X.DecodeInt(board[t.x][t.y]) + 1)))
                    {
                        result = true;
                    }
                }

                //check right column
                if (t.x < board[0].Length - 1)
                {
                    if (board[t.x + 1].Contains(B64X.EncodeInt(B64X.DecodeInt(board[i][j]) - 1)) ||
                        board[t.x + 1].Contains(B64X.EncodeInt(B64X.DecodeInt(board[t.x][t.y]) + 1)))
                    {
                        result = true;
                    }
                }

                //check row below
                if (j > t.y)
                {
                    if (t.y > 1)
                    {
                        if (RowContains(t.y - 1, B64X.EncodeInt(B64X.DecodeInt(board[t.x][t.y]) + 1)))
                        {
                            result = true;
                        }
                    }
                }
                else
                {
                    if (j > 1)
                    {
                        if (RowContains(j - 1, B64X.EncodeInt(B64X.DecodeInt(board[i][j]) - 1)))
                        {
                            result = true;
                        }
                    }
                }

                //check row above
                if (j > t.y)
                {
                    if (j < board.Length - 1)
                    {
                        if (RowContains(j + 1, B64X.EncodeInt(B64X.DecodeInt(board[i][j]) - 1)))
                        {
                            result = true;
                        }
                    }
                }
                else
                {
                    if (t.y < board.Length - 1)
                    {
                        if (RowContains(t.y + 1, B64X.EncodeInt(B64X.DecodeInt(board[t.x][t.y]) + 1)))
                        {
                            result = true;
                        }
                    }
                }
            }
            //if tiles stand horizontally
            else
            {
                //check left column
                if (i < t.x)
                {
                    if (i > 1)
                    {
                        if (board[i - 1].Contains(B64X.EncodeInt(B64X.DecodeInt(board[i][j]) - 1)))
                        {
                            result = true;
                        }
                    }
                }
                else
                {
                    if (t.x > 1)
                    {
                        if (board[t.x - 1].Contains(B64X.EncodeInt(B64X.DecodeInt(board[t.x][t.y]) + 1)))
                        {
                            result = true;
                        }
                    }
                }

                //check right column
                if (i < t.x)
                {
                    if (t.x < board.Length - 1)
                    {
                        if (board[t.x + 1].Contains(B64X.EncodeInt(B64X.DecodeInt(board[t.x][t.y]) + 1)))
                        {
                            result = true;
                        }
                    }
                }
                else
                {
                    if (i < board.Length - 1)
                    {
                        if (board[i + 1].Contains(B64X.EncodeInt(B64X.DecodeInt(board[i][j]) - 1)))
                        {
                            result = true;
                        }
                    }
                }

                //check row below
                if (t.y > 1)
                {
                    if (RowContains(t.y - 1, B64X.EncodeInt(B64X.DecodeInt(board[i][j]) - 1)) ||
                        RowContains(t.y - 1, B64X.EncodeInt(B64X.DecodeInt(board[t.x][t.y]) + 1)))
                    {
                        result = true;
                    }
                }

                //check row above
                if (t.y < board.Length - 1)
                {
                    if (RowContains(t.y + 1, B64X.EncodeInt(B64X.DecodeInt(board[i][j]) - 1)) ||
                        RowContains(t.y + 1, B64X.EncodeInt(B64X.DecodeInt(board[t.x][t.y]) + 1)))
                    {
                        result = true;
                    }
                }
            }
        });

        SearchOneTileAway(i, j, B64X.DecodeInt(board[i][j]) + 2).ForEach((t) =>
        {
            string requiredDigit = B64X.EncodeInt((B64X.DecodeInt(board[i][j]) + B64X.DecodeInt(board[t.x][t.y])) / 2);

            //if tiles stand vertically
            if (t.x == i)
            {
                if (RowContains((j + t.y) / 2, requiredDigit))
                {
                    result = true;
                }
            }
            //if tiles stand horizontally
            else
            {
                if (board[(i + t.x) / 2].Contains(requiredDigit))
                {
                    result = true;
                }
            }
        });

        return result;
    }

    private static bool RowContains(int row, string search)
    {
        bool result = false;

        for (int i = 0; i < board.Length; i++)
        {
            if (board[i][row] == search)
            {
                result = true;

                break;
            }
        }

        return result;
    }

    /* 
     * TEST FUNC
    public static bool TestGameOverCondition()
    {
        int[][] board = new int[4][]
        {
            new int[]{9, 9, 9, 9},
            new int[]{9, 9, 9, 2},
            new int[]{9, 3, 9, 9},
            new int[]{9, 4, 9, 9}
        };

        return IsGameOver(board);
    }
    */
}
