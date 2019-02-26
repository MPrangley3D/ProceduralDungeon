using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public Transform[] startingPositions;
    public GameObject[] rooms;
    public GameObject[] rightEdgeRooms;
    public GameObject[] leftEdgeRooms;
    public GameObject player;
    public int direction;
    public float moveAmount;
    private float spawnTimer;
    public float startTime;
    public float minX;
    public float maxX;
    public float maxY;
    public float rayDistance = 1f;
    private bool stopGen = false;
    private GameObject lastRoomSpawned;
    private Vector2 checkPreviousDir;
    private string location = "middle";
    private bool retrySpawn = false;
    private bool firstSpawn = true;
    public LayerMask whatIsGround;
    public LayerMask isSpawned;

    private void Start()
    {
        int randStartingPos = Random.Range(0, startingPositions.Length);
        this.transform.position = startingPositions[randStartingPos].position;
        SpawnRoom();
        SpawnPlayer();
    }

    private void Update()
    {
        RayDebugger();

        if (!stopGen)
        {
            if (retrySpawn)
            {
                RetrySpawn();
            }
            
            else if (!retrySpawn)
            {
                Spawner();
            }
        }
        
        else if (stopGen)
        {
            Filler();
        }
        
    }

    private void RayDebugger()
    {
        Debug.DrawRay(this.transform.position, checkPreviousDir * rayDistance, Color.green);
        if (direction == 1 || direction == 2 || direction == 3)
        {
            Debug.DrawRay(this.transform.position, Vector2.right * rayDistance, Color.red);
        }
        else if (direction == 4 || direction == 5 || direction == 6)
        {
            Debug.DrawRay(this.transform.position, Vector2.left * rayDistance, Color.red);
        }
        else if (direction == 7 || direction == 8)
        {
            Debug.DrawRay(this.transform.position, Vector2.up * rayDistance, Color.red);
        }
        
    }

    private void RetrySpawn()
    {
        lastRoomSpawned.GetComponentInChildren<Seppuku>().CommitSeppuku();
        Debug.Log("I am the Retry spawner");
        SpawnRoom();
    }

    private void Spawner()
    {
        if (spawnTimer <= 0)
        {
            Debug.Log("===Spawn New Room===");
            bool invalidPick = PickDirection();
            if (!invalidPick)
            {
                Debug.Log("=[]Valid Direction Found[]=");
                Move();
                Debug.Log("I am the Normal spawner");
                SpawnRoom();
            }
            spawnTimer = startTime;
        }
        else
        {
            spawnTimer -= Time.deltaTime; 
        }
    }

    private bool PickDirection()
    {
        RandomizeNewDirection();
        bool check = LookInDirection();

        if (check)
        {
            Debug.Log("[-] Tried & failed to Move: " + direction+"Check" + check);
        }
        else
        {
            Debug.Log("[+] Direction is Clear!: " + direction+"Check"+check);
        }

        return (check);
    }

    private void RandomizeNewDirection()
    {
        string currentLocation = CheckLocation();

        //Cases where we must go up!
        if (currentLocation == "left" && checkPreviousDir == Vector2.right)
        {
            direction = Random.Range(7, 9);
        }
        else if (currentLocation == "right" && checkPreviousDir == Vector2.left)
        {
            direction = Random.Range(7, 9);
        }
        //cases where we go left or up
        else if (currentLocation == "right")
        {
            direction = Random.Range(4, 9);
        }
        else if (currentLocation == "middle" && checkPreviousDir == Vector2.right)
        {
            direction = Random.Range(4, 9);
        }
        //cases where we go right or up
        else if (currentLocation == "left")
        {
            int[] skipLeft = new int[5] { 1, 2, 3, 7, 8 };
            direction = skipLeft[Random.Range(1, 5)];
        }

        else if (currentLocation == "middle" && checkPreviousDir == Vector2.left)
        {
            int[] skipLeft = new int[5] { 1, 2, 3, 7, 8 };
            direction = skipLeft[Random.Range(1, 5)];
        }
        //we can go left, right, or up
        else if (currentLocation == "middle" && checkPreviousDir == Vector2.down)
        {
            direction = Random.Range(1, 9);
        }
        else
        {
            direction = Random.Range(1, 9);
        }
    }

    private bool LookInDirection()
    {
        bool check;
        //Look Right
        if (direction == 1 || direction == 2 || direction == 3)
        {
            check = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y) + Vector2.right * 4.5f, Vector2.right, rayDistance, whatIsGround);
        }
        //Look Left
        else if (direction == 4 || direction == 5 || direction == 6)
        {
            check = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y) + Vector2.left * 4.5f, Vector2.left, rayDistance, whatIsGround);
        }
        //Look up (Dogs can't)
        else if (direction == 7 || direction == 8)
        {
            check = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y) + Vector2.up * 4.5f, Vector2.up, rayDistance, whatIsGround);
        }
        else
        {
            check = false;
        }
        return (check);
    }

    private void Move()
    {
        if (direction == 1 || direction == 2 || direction == 3)
        {
            StepRight();
        }

        else if (direction == 4 || direction == 5 || direction == 6)
        {
            StepLeft();
        }

        else if (direction == 7 || direction == 8)
        {
            StepUp();
        }
    }

    private void StepRight()
    {
        if (this.transform.position.x < maxX)
        {
            Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
            this.transform.position = newPos;
            checkPreviousDir = Vector2.left;
        }
        else
        {
            StepUp();
        }
    }

    private void StepLeft()
    {
        if (this.transform.position.x > minX)
        {
            Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
            this.transform.position = newPos;
            checkPreviousDir = Vector2.right;
        }
        else
        {
            StepUp();
        }
    }

    private void StepUp()
    {
        if (this.transform.position.y < maxY)
        {
            Vector2 newPos = new Vector2(transform.position.x, transform.position.y + moveAmount);
            this.transform.position = newPos;
            checkPreviousDir = Vector2.down;
        }
        else
        {
            stopGen = true;
        }
    }

    private void SpawnRoom()
    {
        string spawnLocation = CheckLocation();

        if (spawnLocation == "left")
        {
            lastRoomSpawned = Instantiate(leftEdgeRooms[Random.Range(0, leftEdgeRooms.Length)], transform.position, Quaternion.identity);
        }
        else if (spawnLocation == "right")
        {
            lastRoomSpawned = Instantiate(rightEdgeRooms[Random.Range(0, rightEdgeRooms.Length)], transform.position, Quaternion.identity);
        }
        else if (spawnLocation == "middle")
        {
            lastRoomSpawned = Instantiate(rooms[Random.Range(0, rooms.Length)], transform.position, Quaternion.identity);
        }

        Debug.Log("New Room Named:  " + lastRoomSpawned);
        
        StartCoroutine("ValidateNewRoom");
    }

    IEnumerator ValidateNewRoom()
    {
        yield return new WaitForSeconds(0f);
        
        bool checkPrev = Physics2D.Raycast(new Vector2(this.transform.position.x,this.transform.position.y)+checkPreviousDir*4.5f, checkPreviousDir, rayDistance, whatIsGround);
        Debug.DrawRay(this.transform.position, checkPreviousDir * rayDistance, Color.blue);
        Debug.Log("Validating new room - True should destroy " + checkPrev);
        if (checkPrev)
        {
            Debug.Log("Call Destroy this: " + lastRoomSpawned.name);
        }
        
        if (firstSpawn)
        {
            retrySpawn = false;
            firstSpawn = false;
        }
        else
        {
            retrySpawn = checkPrev;
        }
        
    }

    private void SpawnPlayer()
    {
        Debug.Log("Player Spawned");
        player.transform.position = this.transform.position;
    }

    private string CheckLocation()
    {
        if (this.transform.position.x >= maxX)
        {
            location = "right";
        }
        else if (this.transform.position.x <= minX)
        {
            location = "left";
        }
        else
        {
            location = "middle";
        }
        return location;
    }

    private void Filler()
    {
        float yIter = 5;
        float xIter = minX;
        while (xIter <= maxX)
        {
            while (yIter <= maxY)
            {
                BuildColumn(xIter, yIter);
                yIter += 10;
            }
            yIter = 5;
            xIter += 10;
        }
    }

    void BuildColumn(float buildX, float buildY) 
    {
        Vector2 newPos = new Vector2(buildX, buildY);
        this.transform.position = newPos;
        bool check = CheckExisting();
        if (!check)
        {
            Instantiate(rooms[Random.Range(0, rooms.Length)], transform.position, Quaternion.identity);
        }
    }

    bool CheckExisting()
    {
        bool check = Physics2D.Raycast(this.transform.position, Vector2.right, 1.0f, isSpawned);
        return check;
    }
}
