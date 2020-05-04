using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSightProjectile : MonoBehaviour
{
    public float attackDamage = 5;
    public Transform ProjectileLocation; //from where the projectile will get released
    public GameObject hairball;
    private GameObject player;
    private bool inRange;
    float timer;
    public float timeToReload;
    public float fireSpeed;

    List<GameObject> playerList;

    void Awake()
    {
        playerList = new List<GameObject>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timer > timeToReload)
        {
            // check if there are any targets within range
            if (playerList.Count > 0)
            {
                timer = 0;
                //shoot at each player in range
                foreach (GameObject obj in playerList)
                {
                    //calculate the trajectory vector 
                    float x = obj.transform.position.x - transform.position.x;
                    float y = obj.transform.position.y - transform.position.y;
                    Vector2 FireDirection = new Vector2(x, y);
                    FireDirection = FireDirection.normalized * fireSpeed;

                    // Create the projectile and Access its Rigidbody to add force
                    GameObject newProjectile = Instantiate(hairball, transform.position, transform.rotation);
                    newProjectile.GetComponent<Rigidbody2D>().velocity = FireDirection;
                    //If the projectile exists after 5 seconds, destroy it
                    Destroy(newProjectile, 5);
                }
            }
        }
        else
        {
            timer += Time.deltaTime;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerList.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerList.Remove(collision.gameObject);
        }

    }

    //private void Launchhairball()
    //{
    //    //animation goes here
    //    ProjectileLocation = GetComponentInParent<Rigidbody2D>().transform;
    //    //            bulletShoot.GetComponent<Rigidbody2D>().velocity = (player.transform.position - transform.position).normalized * bulletSpeed;
    //    Instantiate(hairball, ProjectileLocation.position, Quaternion.Euler(new Vector3(0, 0, 0)));
    //}
}
