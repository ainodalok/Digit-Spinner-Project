using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ColRowMover : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    private List<GameObject> movingTiles;
    /* false when moving a row, true when moving a column */
    private bool isColumnMoving = false;
    private Vector2 currentMovement;
    private BoardController boardController;
    private Vector2 oldPosition;

    private void Start()
    {
        boardController = gameObject.transform.parent.GetComponent<BoardController>();
    }

    public void OnBeginDrag(PointerEventData e)
    {
        currentMovement = e.delta;
        movingTiles = new List<GameObject>();
        if (Mathf.Abs(currentMovement.y) > Mathf.Abs(currentMovement.x))
        {
            for (int i = 0; i < BoardLogic.BOARD_SIZE; i++)
            {
                movingTiles.Add(GetTile(Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(e.position).x)+3, i));
            }
            
            isColumnMoving = true;
        }
        else if (Mathf.Abs(currentMovement.y) < Mathf.Abs(currentMovement.x))
        {
            for (int i = 0; i < BoardLogic.BOARD_SIZE; i++)
            {
                movingTiles.Add(GetTile(i, Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(e.position).y)+5));
            }
            
            isColumnMoving = false;
        }
        oldPosition = (Vector2)(Camera.main.ScreenToWorldPoint(e.position));
    }

    public void OnDrag(PointerEventData e)
    {
        currentMovement = (Vector2)(Camera.main.ScreenToWorldPoint(e.position));
        if (isColumnMoving)
        {
            movingTiles.ForEach((t) =>
            {
                t.transform.position += new Vector3(0, currentMovement.y-oldPosition.y, 0);
            });
        }
        else
        {
            movingTiles.ForEach((t) =>
            {
                t.transform.position += new Vector3(currentMovement.x-oldPosition.x, 0, 0);
            });
        }
        oldPosition = currentMovement;
    }

    public void OnEndDrag(PointerEventData e)
    {

        if (isColumnMoving)
        {
            //boardLogic.MoveColumn((int)Math.Round(initialPosition.x), (int)Math.Round(e.position.y - initialPosition.y));
            movingTiles.ForEach((t) =>
            {
                RectTransform tileRectPos = t.transform.GetComponent<RectTransform>();
                tileRectPos.localPosition = new Vector3(tileRectPos.localPosition.x, Mathf.Round(tileRectPos.localPosition.y), tileRectPos.localPosition.z);
            });
        }
        else
        {
            //boardLogic.MoveColumn((int)Math.Round(e.position.y - initialPosition.y), (int)Math.Round(initialPosition.x));
            movingTiles.ForEach((t) =>
            {
                RectTransform tileRectPos = t.transform.GetComponent<RectTransform>();
                tileRectPos.localPosition = new Vector3(Mathf.Round(tileRectPos.localPosition.x), tileRectPos.localPosition.y, tileRectPos.localPosition.z);
            });
        }
    }

    private GameObject GetTile(int x, int y)
    {
        return gameObject.transform.parent.GetComponent<BoardController>().activeTileObjects[x][y];
    }
}
