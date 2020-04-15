using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    #region HealthVars
    public float maxHealth = 100;
    private float currHealth;
    #endregion

    #region MovemenetVars
    private Vector2 movement;
    public float baseMoveSpeed = 2;
    private Rigidbody2D enemyRB;
    private Vector3 dir;
    private GameObject player;
    #endregion

    #region AnimationVars
    Animator anim;
    #endregion

    #region AttackVars
    private int damage = 10;
    #endregion

    private State state;
    private enum State
    {
        Idle, Alert, Attack
    }

    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        enemyRB = GetComponent<Rigidbody2D>();
        currHealth = maxHealth;
        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                anim.SetBool("idle", true);
                anim.SetBool("moving", false);
                enemyRB.velocity = Vector2.zero;
                break;
            case State.Alert:
                chase();
                anim.SetBool("idle", false);
                anim.SetBool("moving", true);
                break;
            case State.Attack:
                anim.SetBool("idle", false);
                anim.SetBool("moving", false);
                attack();
                break;
        }
    }
    
    public virtual void chase()
    {
        dir = player.transform.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        movement = dir.normalized;
        enemyRB.velocity = movement.normalized * baseMoveSpeed;
    }

    public void slow()
    {
        baseMoveSpeed = 1;
    }

    public void speed()
    {
        baseMoveSpeed = 2;
    }

    private void attack()
    {
        player.GetComponent<PlayerScript>().takeDamage(damage);
    }

    public void SetTarget(GameObject obj)
    {
        player = obj;
        Debug.Log(player);
        if (obj == null)
        {
            state = State.Idle;
        } else
        {
            state = State.Alert;
        }
        Debug.Log(state);
    }
    public void TakeDamage(float damage)
    {
        currHealth -= damage;
        Debug.Log(currHealth);
        if (currHealth <= 0)
        {
            die();
        }
    }

    private void die()
    {
        Destroy(this.gameObject);
    }
}
