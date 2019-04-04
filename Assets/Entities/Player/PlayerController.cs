using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject projectile;
    public float projectileSpeed;
    public float firingRate = 0.2f;

    public float speed = 15.0f;
    public float padding = 1.0f; // offset for clamping player
    public float health = 250f;

    public AudioClip fireSound;

    float xmin;
    float xmax;

	// Use this for initialization
	void Start ()
    {
        float distance = transform.position.z - Camera.main.transform.position.z; // distance between player and camera
        Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xmin = leftmost.x + padding;
        xmax = rightmost.x - padding;

    }

	void Fire()
    {
        Vector3 offeset = new Vector3(0, 1f, 0);
        GameObject beam = Instantiate(projectile, transform.position + offeset, Quaternion.identity) as GameObject;
        beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed, 0); // launch laserbeam upwards
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
    }


	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) // GetKeyDown - only works when once while space is pressed 
        {
            InvokeRepeating("Fire", 0.000001f, firingRate); // multi-shots delays
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke("Fire"); // stops fire after releasing space
        }

		if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;    // vector3.left - only moves left in x-direction
                                                                            // Time.delta - movement inependent of framerate
        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        // restricting player to gamespace
        float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z); // resetting transform
	}

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Projectile missile = collider.gameObject.GetComponent<Projectile>();

        if (missile)
        {
            Debug.Log("Missile hit player");
            health -= missile.GetDamage();
            missile.Hit();

            if (health<= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        LevelManager man = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        man.LoadLevel("Win Screen"); // loads win screen when player dies
        Destroy(gameObject); // destroys player
    }
}
