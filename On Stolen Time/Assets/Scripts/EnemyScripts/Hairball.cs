using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hairball : MonoBehaviour
{
    public float projectilespeed;
    Rigidbody2D hairballrb;
    public float attackDamage = 10;

    public float TimeToLive = 5f;
    private void Start()
    {
        Destroy(gameObject, TimeToLive);
    }
    void Awake()
    {
        Debug.Log("hairball");
        //hairballrb = GetComponent<Rigidbody2D>();
        //hairballrb.AddForce(new Vector2(1, 0) * projectilespeed, ForceMode2D.Impulse);
    }
    // Update is called once per frame
    void Update()
    {
    }
    public void removeForce()
    {
        hairballrb.velocity = new Vector2(0, 0);
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerScript>().takeDamage(attackDamage);
            removeForce();

            Destroy(gameObject);
        }
    }
}
