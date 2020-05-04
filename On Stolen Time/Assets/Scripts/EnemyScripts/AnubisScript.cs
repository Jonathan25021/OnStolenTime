using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnubisScript : EnemyScript
{
    Animator anim;
    public float attackDamage = 50;
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetBool("attacking", true);
            collision.gameObject.GetComponent<PlayerScript>().takeDamage(attackDamage);
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        anim.SetBool("attacking", false);
    }
}
