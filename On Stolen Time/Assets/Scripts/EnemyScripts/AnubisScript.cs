using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnubisScript : EnemyScript
{
    public float attackDamageThump = 40;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerScript>().takeDamage(attackDamageThump);
        }
    }
}
