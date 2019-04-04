using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationController : MonoBehaviour {

    public GameObject enemyPrefab;
    public float width = 10f;
    public float height = 5f;
    public float speed = 5f;
    public float spawnDelay = 0.5f;

    private bool movingRight = true;
    private float xmin;
    private float xmax;

	// Use this for initialization
	void Start ()
    {
        float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera));
        Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceToCamera));

        xmin = leftBoundary.x;
        xmax = rightBoundary.x;

        SpawnUntilFull();
    }

    void SpawnEnemies()
    {
        foreach (Transform child in transform)
        {
            GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject; // spawns enemy on child GameObject
                                                                                                                      // Quaternion.identity - no rotation
            enemy.transform.parent = child; // childs enemy to EnemyFormation
        }
    }

    void SpawnUntilFull()
    {
        Transform freePosition = NextFreePosition();

        if (freePosition)
        {
            GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity);
            enemy.transform.parent = freePosition;
        }

        if (NextFreePosition())
        {
            Invoke("SpawnUntilFull", spawnDelay); // calls SpawnUntilFull after a delay of spawnDelay value
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }


    // Update is called once per frame
    void Update () {
        if (movingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        float leftEdgeOfFormation = transform.position.x - (0.5f * width);
        float rightEdgeOfFormation = transform.position.x + (0.5f * width);

        if (leftEdgeOfFormation < xmin)
        {
            movingRight = true; 
        }
        
        else if (rightEdgeOfFormation > xmax)
        {
            movingRight = false; // flips direction
        }

        if (AllMembersAreDead())
        {
            Debug.Log("Empty");
            SpawnUntilFull();
        }

	}

    Transform NextFreePosition()
    {
        foreach(Transform childPositionGameObject in transform)
        {
            if (childPositionGameObject.childCount == 0)
            {
                return childPositionGameObject;
            }
        }

        return null;
    }

    bool AllMembersAreDead() // method for detecting if all enemies are destroyed
    {
        foreach(Transform childPositionGameObject in transform)
        {
            if (childPositionGameObject.childCount > 0)
            {
                return false;
            }
        }

        return true;
    }
}
