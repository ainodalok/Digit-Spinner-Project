using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ColRowMover : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {
    private List<GameObject> movingTiles;
    private bool isMovingNow = false;
    /* false when moving a row, true when moving a column */
    private bool isColumnMoving = false;
    private GameObject draggedTile;
    private Vector2 currentMovement;
    private BoardLogic boardLogic;
    private Vector2 initialPosition;
    private float modifier;
    private Vector2 boardStartPoint;

    private void Start()
    {
        boardLogic = gameObject.transform.parent.GetComponent<BoardController>().GetBoardLogic();
        modifier = Screen.width / 8.5f;
        boardStartPoint = new Vector2(modifier * 0.75f, modifier * 1.75f);
    }

    public void OnPointerDown(PointerEventData e)
    {
        draggedTile = gameObject;
            /*
            GetTile(
                (int) Math.Round(e.position.x),
                (int) Math.Round(e.position.y)
            );
            */

        initialPosition = draggedTile.transform.position;
    }

    public void OnBeginDrag(PointerEventData e)
    {
        Debug.Log(e.position.x);
        Debug.Log(e.position.y);
        currentMovement = ((e.position - boardStartPoint) / modifier) - new Vector2(draggedTile.transform.position.x + 5.25f, draggedTile.transform.position.y + 5f);
        movingTiles = new List<GameObject>();

        if (currentMovement.y > currentMovement.x)
        {
            for (int i = 0; i < BoardLogic.BOARD_SIZE; i++)
            {
                movingTiles.Add(GetTile(i, draggedTile.GetComponent<Tile>().y));
            }

            currentMovement.y = 0;
            isColumnMoving = true;
        }
        else
        {
            for (int i = 0; i < BoardLogic.BOARD_SIZE; i++)
            {
                movingTiles.Add(GetTile(draggedTile.GetComponent<Tile>().x, i));
            }

            currentMovement.x = 0;
            isColumnMoving = false;
        }

        isMovingNow = true;
        StartCoroutine(MoveTiles());
    }

    public void OnDrag(PointerEventData e)
    {
        currentMovement = ((e.position - boardStartPoint) / modifier) - new Vector2(draggedTile.transform.position.x + 5.25f, draggedTile.transform.position.y + 3f);

        if (isColumnMoving)
        {
            currentMovement.y = 0;
        }
        else
        {
            currentMovement.x = 0;
        }
    }

    public void OnEndDrag(PointerEventData e)
    {
        isMovingNow = false;
    }

    public void OnPointerUp(PointerEventData e)
    {
        StopCoroutine(MoveTiles());

        if (isColumnMoving)
        {
            //boardLogic.MoveColumn((int)Math.Round(initialPosition.x), (int)Math.Round(e.position.y - initialPosition.y));
        }
        else
        {
            //boardLogic.MoveColumn((int)Math.Round(e.position.y - initialPosition.y), (int)Math.Round(initialPosition.x));
        }
    }

    private IEnumerator MoveTiles()
    {
        while (isMovingNow)
        {
            movingTiles.ForEach((t) =>
            {
                Vector3 temp = t.transform.position;

                temp.x += currentMovement.x;
                temp.y += currentMovement.y;

                t.transform.position = temp;
            });

            currentMovement = new Vector2(0f, 0f);

            yield return null;
        }
    }

    private GameObject GetTile(int x, int y)
    {
        return gameObject.transform.parent.GetComponent<BoardController>().activeTileObjects[x][y];
    }
}
