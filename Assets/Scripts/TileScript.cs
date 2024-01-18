using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    private Vector2Int gridPos;
    // Start is called before the first frame update
    public void StartGrid(Vector2Int pos)
    {
        gridPos = pos;
    }

    public Vector2Int GetPos()
    {
        return gridPos;
    }
}
