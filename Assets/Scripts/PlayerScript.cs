using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerScript : MonoBehaviour
{
    private TileNode[,] grid = null;
    [SerializeField] private GameObject bombModel;
    [SerializeField] private AudioSource stepSound;

    public Vector2Int current;

    private bool reachedEnd = false;
    private bool isDead = false;

    private List<GameObject> bombs = new List<GameObject>();
    private float speed = 8f;
    Rigidbody rb;
    // Start is called before the first frame update
    public void StartOnGrid()
    {
        this.transform.position = grid[0,0].worldPosition; //Place player on map
        this.transform.position -= new Vector3(0, 2.8f, 0);
        current = new Vector2Int(0, 0);
        rb = GetComponent<Rigidbody>();
    }

    void Update() //Update for sound
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            stepSound.Play();
        }

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            stepSound.Stop();
        }

        if (Input.GetKeyUp(KeyCode.Space)) //Place bomb
        {
            GameObject bomb = GameObject.Instantiate(bombModel, grid[current.x, current.y].worldPosition, Quaternion.identity);
            bomb.GetComponent<BombScript>().GetGrid(grid, current);
            bombs.Add(bomb);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tile")
        {
            current = other.gameObject.GetComponent<TileScript>().GetPos();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.name)
        {
            case "Enemy(Clone)":
                GotHit();
                break;
            case "Finish":
                reachedEnd = true;
                stepSound.Stop();
                break;
        }
    }

    public bool GetEnd()
    {
        return reachedEnd;
    }

    public void ResEnd()
    {
        reachedEnd=false;
    }

    public bool GetDead()
    {
        return isDead;
    }

    public void ResDead()
    {
        isDead=false;
    }

    public void GotHit()
    {
        isDead = true;
        stepSound.Stop();
    }

    // Update is called once per frame
    void FixedUpdate() //Update for movement
    {
        if (isDead)
            return;

        if (Input.GetKey(KeyCode.W)) //Move up
        {
            this.transform.eulerAngles = new Vector3(-90.0f, 90.0f, -90.0f);
            Vector3 directVect = (new Vector3(0, 0, 1)).normalized * speed * Time.deltaTime;
            rb.MovePosition(transform.position + directVect);
            
        }
        else if (Input.GetKey(KeyCode.S)) //Move down
        {
            this.transform.eulerAngles = new Vector3(-90.0f, 90.0f, 90.0f);
            Vector3 directVect = (new Vector3(0, 0, -1)).normalized * speed * Time.deltaTime;
            rb.MovePosition(transform.position + directVect);
        }
        else if (Input.GetKey(KeyCode.A)) //Move left
        {
            this.transform.eulerAngles = new Vector3(-90.0f, 90.0f, 180.0f);
            Vector3 directVect = (new Vector3(-1, 0, 0)).normalized * speed * Time.deltaTime;
            rb.MovePosition(transform.position + directVect);
        }
        else if (Input.GetKey(KeyCode.D)) //Move right
        {
            this.transform.eulerAngles = new Vector3(-90.0f, 90.0f, 0.0f);
            Vector3 directVect = (new Vector3(1, 0, 0)).normalized * speed * Time.deltaTime;
            rb.MovePosition(transform.position + directVect);
        }
    }

    public void getGrid(TileNode[,] fieldGrid)
    {
        grid = fieldGrid;
    }
}
