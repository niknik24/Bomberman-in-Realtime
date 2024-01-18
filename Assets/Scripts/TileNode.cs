using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNode //MonoBehaviour
{
    public bool canDestroy;
    public bool walkable;
    public Vector3 worldPosition;
    private GameObject prefab;
    public Vector2Int gridPos;
    public GameObject body;
    public bool hasEnemy;


    public TileNode(GameObject _prefab, bool _walkable, bool _canDestroy, Vector3 position, Vector2Int _gridPos, GameObject Parent, GameObject TilePrefab = null)
    {
        prefab = _prefab;
        walkable = _walkable;
        worldPosition = position;
        gridPos = _gridPos;
        canDestroy = _canDestroy;
        body = GameObject.Instantiate(prefab, worldPosition, Quaternion.identity);
        body.transform.SetParent(Parent.transform, true);
        if (prefab.name == "Tile")
            body.GetComponent<TileScript>().StartGrid(gridPos);
        if (prefab.name == "WoodBlock")
        {
            GameObject tileBody = GameObject.Instantiate(TilePrefab, worldPosition, Quaternion.identity);
            tileBody.transform.SetParent(Parent.transform, true);
            tileBody.GetComponent<TileScript>().StartGrid(gridPos);
        }

    }
}
