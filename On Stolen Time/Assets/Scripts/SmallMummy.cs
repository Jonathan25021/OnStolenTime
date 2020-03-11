using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallMummy : MonoBehaviour
{
    #region health
    public float maxHealth = 100;
    private float currHealth;
    #endregion

    #region movemenent
    private Vector2 movement;
    public float baseMoveSpeed = 1;
    private Rigidbody2D mummyRB;
    private Vector3 dir;
    #endregion

    #region state

    private State state;
    private enum State
    {
        Normal, Chase, Attack,
    }

    #endregion

    #region playerRelated
    public Transform player;
    #endregion

    #region attack
    public float damage;
    public float smallMummyReach;
    #endregion

    #region UnityFuncs
    void Awake()
    {
        state = State.Normal;
    }
    void Start()
    {
        mummyRB = GetComponent<Rigidbody2D>();
        currHealth = maxHealth;
        StartCoroutine(EnemyMoves());
    }

    void Update()
    { 
    }
    #endregion

    #region FSM
    private void Move()
    {
        Vector2 direction = player.position - transform.position;
        mummyRB.velocity = direction.normalized * baseMoveSpeed;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 t = collision.attachedRigidbody.transform.position;
        StartCoroutine(Chase(t));
    }

    IEnumerator EnemyMoves()
    {
        while (true)
        {
            yield return StartCoroutine(state.ToString());
        }
    }

    IEnumerator Chase(Vector3 t)
    {
        while (gameObject.activeInHierarchy)
        {
            transform.position = Vector3.Lerp(transform.position, t, 0.1f);
            if (transform.position == t)
            {
                state = State.Attack;
            }
            yield return new WaitForSeconds(0.5f);
        }
        Debug.Log("lost player");
        //Attack();
    }

    IEnumerator Normal()
    {
        while (state == State.Normal)
        {
            yield return new WaitForSeconds(1.0f);
            //Debug.Log("Resting mummyface");
        }
    }

    IEnumerator AttackTime()
    {
        while (state == State.Attack)
        {
            Debug.Log("Attacking");
            AttackNow();
        }
        yield return new WaitForSeconds(1.0f);
    }
    #endregion

    #region combatFuncs
    private void AttackNow()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, smallMummyReach, Vector2.zero);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Player"))
            {
                Debug.Log("hit plater");
                //cause damage
                hit.transform.GetComponent<PlayerScript>().takeDamage(damage);
            }
        }
    }
    #endregion

    #region healthFuncs
    public void takeDamage(float damageVal)
    {
        currHealth -= damageVal;
        if (currHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
        Debug.Log("enemy died");
    }
    #endregion
}
