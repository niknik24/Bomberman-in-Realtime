using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GridScript : MonoBehaviour
{
    [SerializeField] private GameObject tileModel;
    [SerializeField] private GameObject ironModel;
    [SerializeField] private GameObject woodModel;

    [SerializeField] private Transform landscape;
    [SerializeField] private GameObject field;
    [SerializeField] private int gridDelta = 10;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;

    private TileNode[,] grid = null;
    private List<GameObject> enemies = new List<GameObject>();

    private float enemyTimer = 5;

    public UnityEvent onPlayerDead;
    public UnityEvent onVictory;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 size = landscape.GetComponent<Renderer>().bounds.size;
        int sizeX = (int)(size.x / gridDelta);
        int sizeZ = (int)(size.z / gridDelta);
        grid = new TileNode[sizeX, sizeZ];
        for (int x = 0; x < sizeX; ++x)
            for (int z = 0; z < sizeZ; ++z)
            {
                Vector3 position = new Vector3(x * gridDelta, 0, z * gridDelta);
                position.y = 2;
                if (x % 2 != 0 && z % 2 != 0) 
                    grid[x, z] = new TileNode(ironModel, false, false, position, new Vector2Int(x,z), field); //Spawn iron boxes
                else
                {
                    
                    if (UnityEngine.Random.Range(0, 5) ==4 && x!=0 && x< grid.GetLength(0)-2)
                    {
                        grid[x, z] = new TileNode(woodModel, false, true, position, new Vector2Int(x, z), field, tileModel); //Spawn wooden boxes
                    }
                    else
                        grid[x, z] = new TileNode(tileModel, true, true, position, new Vector2Int(x, z), field);//Spawn empty tiles    
                }
            }
        player.GetComponent<PlayerScript>().getGrid(grid);
        player.GetComponent<PlayerScript>().StartOnGrid();
        player.GetComponent<PlayerScript>().enabled = false; //Place Player and wait for start

        GameObject startEnemy1 = GameObject.Instantiate(enemy, new Vector3(0f,0f,0f), Quaternion.identity);
        startEnemy1.GetComponent<EnemyScript>().StartWithGrid(grid);
        startEnemy1.GetComponent<EnemyScript>().SetTarget(player.GetComponent<PlayerScript>().current);

        enemies.Add(startEnemy1);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        player.GetComponent<PlayerScript>().enabled = true;
        Time.timeScale = 1;
    }

    public void Restart() //Respawn whole map
    {
        foreach (var tile in grid)
        {
            Destroy(tile.body);
        }
        grid = null;

        foreach (var enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }

        enemies = new List<GameObject>();

        foreach (GameObject expl in GameObject.FindGameObjectsWithTag("Explosion"))
        {
            Destroy(expl);
        }

        foreach (GameObject bomb in GameObject.FindGameObjectsWithTag("Bomb"))
        {
            Destroy(bomb);
        }

        player.GetComponent<PlayerScript>().ResDead();
        player.GetComponent<PlayerScript>().ResEnd();
        enemyTimer = 5;
        Start();
        Resume();
    }

    public void TimeOut() // If time ran out
    {
        player.GetComponent<PlayerScript>().GotHit();
        player.GetComponent<PlayerScript>().enabled = false;
        Time.timeScale = 0;
        onPlayerDead.Invoke();
    }




    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerScript>().GetDead()) //Check if player is dead
        {
            Time.timeScale = 0;
            player.GetComponent<PlayerScript>().enabled = false;
            onPlayerDead.Invoke();
            return;
        }

        if (player.GetComponent<PlayerScript>().GetEnd()) //Check if player exited
        {
            Time.timeScale = 0;
            player.GetComponent<PlayerScript>().enabled = false;
            onVictory.Invoke();
            return;
        }

        int j = 0;
        while (j < enemies.Count)
        {
            if (enemies[j].GetComponent<EnemyScript>().dead) //Check if enemies are dead and delete them
            {
                enemies.RemoveAt(j);
            }
            else
                j++;
        }

        foreach (var enemy in enemies)
        {
            enemy.GetComponent<EnemyScript>().SetTarget(player.GetComponent<PlayerScript>().current);
        }

      
        if (enemyTimer > 0)
            enemyTimer-= Time.deltaTime;
        else
        {   //Spawn new enemy every 5 seconds
            GameObject timeEnemy = GameObject.Instantiate(enemy, new Vector3(0f, 0f, 0f), Quaternion.identity);
            timeEnemy.GetComponent<EnemyScript>().StartWithGrid(grid);
            enemies.Add(timeEnemy);
            enemyTimer = 5;
        }

    }
}
