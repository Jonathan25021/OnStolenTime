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
    public float Damage = 10;
    public float AttackSpeed = .75f;
    public float AttackRadius = .3f;
    private bool isAttacking = false;
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
                //anim.SetBool("idle", true);
                //anim.SetBool("moving", false);
                enemyRB.velocity = Vector2.zero;
                break;
            case State.Alert:
                chase();
                if (Vector2.Distance(transform.position, player.transform.position) < 1 && !isAttacking)
                {
                    StartCoroutine(attack());
                }
                //anim.SetBool("idle", false);
                //anim.SetBool("moving", true);
                break;
            case State.Attack:
                //anim.SetBool("idle", false);
                //anim.SetBool("moving", false);
                
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
        baseMoveSpeed = baseMoveSpeed / 4;
    }

    public void speed()
    {
        baseMoveSpeed = baseMoveSpeed * 4;
    }

    private IEnumerator attack()
    {
        isAttacking = true;
        Collider2D[] info = Physics2D.OverlapCircleAll(transform.position - transform.up, AttackRadius);
        for (int i = 0; i < info.Length; i++)
        {
            if (info[i].tag == "Player")
            {
                info[i].GetComponent<PlayerScript>().takeDamage(Damage);
            }
        }
        Debug.Log(Time.time);
        yield return new WaitForSeconds(AttackSpeed);
        isAttacking = false;
        yield return null;
    }

    public void SetTarget(GameObject obj)
    {
        player = obj;
        if (obj == null)
        {
            state = State.Idle;
        } else
        {
            state = State.Alert;
        }
    }
    public void TakeDamage(float damage)
    {
        Debug.Log(currHealth);
        enemyRB.velocity = -enemyRB.velocity;
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
