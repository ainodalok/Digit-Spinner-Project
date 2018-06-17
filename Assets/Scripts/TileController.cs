using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public bool isGhost = false;
    /* checks if it is close to any side of the board of active tiles
     * and creates a ghost object at the other side in that case
     */ 
	public GameObject CreateGhostIfNecessary()
    {
        float x = gameObject.transform.localPosition.x;
        float y = gameObject.transform.localPosition.y;
        Vector3 offsetVector = new Vector3();

        if (x > BoardLogic.BOARD_SIZE - 1)
        {
            offsetVector.x = -BoardLogic.BOARD_SIZE;
        }
        else if (x < 0)
        {
            offsetVector.x = BoardLogic.BOARD_SIZE;
        }
        else if (y > BoardLogic.BOARD_SIZE - 1)
        {
            offsetVector.y = -BoardLogic.BOARD_SIZE;
        }
        else if (y < 0)
        {
            offsetVector.y = BoardLogic.BOARD_SIZE;
        }

        // means there is a reason to create a ghost
        if (offsetVector.x != 0f || offsetVector.y != 0f)
        {
            GameObject newTile = Instantiate(gameObject);
            newTile.transform.SetParent(gameObject.transform.parent);
            newTile.transform.localPosition = gameObject.transform.localPosition + offsetVector;
            newTile.name = string.Concat(gameObject.name, " Ghost");

            return newTile;
        }

        return null;
    }
}
