using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    public Vector2Int current;
    private Vector2Int target;

    public bool dead = false;

    private TileNode[,] grid = null;
    private Vector2Int prev;

    Rigidbody rb;
    private float speed = 8f;

    public void StartWithGrid(TileNode[,] fieldGrid) //Initialize enemy on grid
    {
        grid = fieldGrid;
        List<int> list = new List<int>();
        for (int i = 0; i < grid.GetLength(1); i++)
            if (i %2 == 0)
                list.Add(i);
        current = new Vector2Int(grid.GetLength(0) - 1, list[UnityEngine.Random.Range(0, list.Count)]) ;
        this.transform.position = grid[current.x, current.y].worldPosition;
        this.transform.position += new Vector3(0, 2f, 0);
        this.transform.eulerAngles = new Vector3(90.0f, 0, 0);
        target = new Vector2Int(0, 0);
        rb = GetComponent<Rigidbody>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tile")
        {
            prev = current;
            current = other.gameObject.GetComponent<TileScript>().GetPos();
            grid[current.x, current.y].hasEnemy = true;
            grid[prev.x, prev.y].hasEnemy = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Tile")
        {
            grid[prev.x, prev.y].hasEnemy = false;
        }
    }


    // Update is called once per frame
    void FixedUpdate() //Check if tile has explosion
    {
        if (dead) return;

        Vector2Int step = Move();
        rb.velocity = Vector3.zero;
        if (step.x != -1)
        {
            Vector3 directVect = (grid[step.x, step.y].worldPosition - this.transform.position);
            directVect.y = 0;
            if (step == current)
            {
                directVect = directVect.normalized * speed/100 * Time.deltaTime;
                rb.MovePosition(transform.position + directVect);
                return;
            }
            
            directVect = directVect.normalized * speed * Time.deltaTime;
            rb.MovePosition(transform.position + directVect);
            rb.velocity = Vector3.zero;
        }
    }

    public void SetTarget(Vector2Int _target) //Set target of enemy
    {
        target = _target;
    }

    

    public void GotHit()
    {
        dead = true;
        grid[current.x, current.y].hasEnemy = false;
        this.GetComponent<Collider>().enabled = false;
        transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.red;
        Destroy(this.gameObject, 0.5f);
    }

    public Vector2Int Move()
    {
        Vector2Int currentTile = target;
        List<Vector2Int> tiles = new List<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> trace = FindPath();
        Vector2Int min = new Vector2Int(-1, -1);
        if (trace == null)
            return new Vector2Int(-1, -1);
        
        if (!trace.ContainsKey(currentTile)) // Check if enemy can reach target. If not then go as close as possible
        {
            List<Vector2Int> keyX = new List<Vector2Int>(trace.Keys);
            
            float minx = 100f;
            foreach (var key in keyX) {
                if (Vector3.Distance(grid[target.x, target.y].worldPosition, grid[key.x, key.y].worldPosition) < minx)
                {
                    min = key;
                    minx = Vector3.Distance(grid[target.x, target.y].worldPosition, grid[key.x, key.y].worldPosition);
                }
            }
            currentTile = min;
            tiles.Add(currentTile);
        }
        
        while (currentTile.x != -1) {
            currentTile = trace[currentTile];
            tiles.Add(currentTile);
        }

        if (tiles.Count <= 3) {
            if (trace.ContainsKey(target))
                return target;
            else
                return min;
        }

        Vector2Int step = tiles[tiles.Count - 3];
        if (grid[step.x, step.y].hasEnemy)
            return current;
        return step;
    }

    public Dictionary<Vector2Int, Vector2Int> FindPath() //Finding path to target
    {
        TileNode start = grid[current.x, current.y];
        Dictionary<Vector2Int, Vector2Int> trace = new Dictionary<Vector2Int, Vector2Int>(); // Recollection of paths
        Queue<Vector2Int> tiles = new Queue<Vector2Int>(); // All tiles

        tiles.Enqueue(current);
        trace.Add(current, new Vector2Int(-1, -1));
        

        while (tiles.Count > 0)
        {
            Vector2Int currentTile = tiles.Dequeue();

            if (currentTile == target)
                break;

            var neighbours = GetNeighbours(currentTile);
            foreach (var tile in neighbours)
                if (grid[tile.x, tile.y].walkable)
                {
                    if (trace.ContainsKey(tile))
                        continue;
                    trace.TryAdd(tile, currentTile);
                    tiles.Enqueue(tile);
                }
        }
        return trace;
    }

    private List<Vector2Int> GetNeighbours(Vector2Int current) //Getting neighbours of tile
    {
        List<Vector2Int> tiles = new List<Vector2Int>();
        if (current.x != 0)
            if (grid[current.x - 1, current.y].walkable)
                tiles.Add(new Vector2Int(current.x - 1, current.y));
        if (current.x < grid.GetLength(0)-1)
            if (grid[current.x + 1, current.y].walkable)
                tiles.Add(new Vector2Int(current.x + 1, current.y));
        if (current.y != 0)
            if (grid[current.x, current.y - 1].walkable)
            tiles.Add(new Vector2Int(current.x, current.y - 1));
        if (current.y < grid.GetLength(1)-1)
            if (grid[current.x, current.y + 1].walkable)
            tiles.Add(new Vector2Int(current.x, current.y + 1));
        return tiles;
    }
}
