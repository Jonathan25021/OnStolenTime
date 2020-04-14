using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallMummyScript : EnemyScript
{
    public float attackDamage = 20;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerScript>().takeDamage(attackDamage);
        }
    }
}
