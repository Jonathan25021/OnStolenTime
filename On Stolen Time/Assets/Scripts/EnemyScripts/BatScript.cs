using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatScript : EnemyScript
{
    public float attackDamage = 5;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerScript>().takeDamage(attackDamage);
        }
    }


}
