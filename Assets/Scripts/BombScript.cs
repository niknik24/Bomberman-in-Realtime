using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class BombScript : MonoBehaviour
{
    public bool destroyed = false;

    [SerializeField] private GameObject explosion;
    private TileNode[,] grid = null;
    private Vector2Int pos;
    private float timer = 3;

    public UnityEvent onExplode;

    public void GetGrid(TileNode[,] fieldGrid, Vector2Int position)
    {
        grid = fieldGrid;
        pos = position;
    }

    public void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            float sec = Mathf.FloorToInt(timer % 60);
            GameObject obj = this.transform.GetChild(2).gameObject;
            TMPro.TextMeshProUGUI text = obj.GetComponent<TMPro.TextMeshProUGUI>();
            text.text = sec.ToString();
        }
        else
        {
            Destroy();
        }
    }

    private void Destroy()
    {
        if (!destroyed)
            Explode();
        destroyed = true;
        
        this.gameObject.GetComponent<MeshRenderer>().enabled = false; // Make body invisble to make audio still play after Destroy
        for (int i = 0; i < transform.childCount-1; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
        Destroy(this.gameObject, 1.5f);
    }

    void Explode()
    {
        onExplode.Invoke(); //To create sound

        for (int i = 0; i < 3; i++)
        {
            if (pos.x + i >= grid.GetLength(0)) // Check for out of bounds
                break;
            
            if (!grid[pos.x + i, pos.y].walkable) // Destory wooden box and no reaction to iron box
            {

                if (grid[pos.x + i, pos.y].canDestroy)
                {
                    grid[pos.x + i, pos.y].walkable = true;
                    Destroy(grid[pos.x + i, pos.y].body);
                }
                break;
            }

            GameObject explode = GameObject.Instantiate(explosion, grid[pos.x+i, pos.y].worldPosition, Quaternion.identity); // Create a visual effect of explosion
            explode.transform.eulerAngles = new Vector3(-90.0f, 0f, 0f);
            ParticleSystem exp = explosion.GetComponent<ParticleSystem>();
            exp.Play();
            Destroy(explode, exp.main.duration);
        }
        for (int i = 0; i < 3; i++)
        {
            if (pos.x - i < 0)
                break;
           
            if (!grid[pos.x - i, pos.y].walkable)
            {

                if (grid[pos.x - i, pos.y].canDestroy)
                {
                    grid[pos.x - i, pos.y].walkable = true;
                    Destroy(grid[pos.x - i, pos.y].body);
                }
                break;
            }

            GameObject explode = GameObject.Instantiate(explosion, grid[pos.x - i, pos.y].worldPosition, Quaternion.identity);
            explode.transform.eulerAngles = new Vector3(-90.0f, 0f, 0f);
            ParticleSystem exp = explosion.GetComponent<ParticleSystem>();
            exp.Play();
            Destroy(explode, exp.main.duration);
        }
        for (int i = 0; i < 3; i++)
        {
            if (pos.y - i < 0)
                break;
            
            if (!grid[pos.x, pos.y - i].walkable)
            {

                if (grid[pos.x, pos.y - i].canDestroy)
                {
                    grid[pos.x, pos.y - i].walkable = true;
                    Destroy(grid[pos.x, pos.y - i].body);
                }
                break;
            }

            GameObject explode = GameObject.Instantiate(explosion, grid[pos.x, pos.y - i].worldPosition, Quaternion.identity);
            explode.transform.eulerAngles = new Vector3(-90.0f, 0f, 0f);
            ParticleSystem exp = explosion.GetComponent<ParticleSystem>();
            exp.Play();
            Destroy(explode, exp.main.duration);
        }
        for (int i = 0; i < 3; i++)
        {
            if (pos.y + i > grid.GetLength(1)-1)
                break;

            if (!grid[pos.x, pos.y + i].walkable)
            {

                if (grid[pos.x, pos.y + i].canDestroy)
                {
                    grid[pos.x, pos.y + i].walkable = true;
                    Destroy(grid[pos.x, pos.y + i].body);
                }
                break;
            }

            GameObject explode = GameObject.Instantiate(explosion, grid[pos.x, pos.y + i].worldPosition, Quaternion.identity);
            explode.transform.eulerAngles = new Vector3(-90.0f, 0f, 0f);
            ParticleSystem exp = explosion.GetComponent<ParticleSystem>();
            exp.Play();
            Destroy(explode, exp.main.duration);
        }
    }
}
