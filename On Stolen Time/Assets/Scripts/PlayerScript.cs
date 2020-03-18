using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    #region healthVars
    public float maxHealth = 100;
    private float currHealth;
    private float currTimer;
    #endregion

    #region movemenentVars
    private Vector2 movement;
    public float baseMoveSpeed = 3;
    public float sprintMoveSpeed = 5;
    private Rigidbody2D playerRB;
    private Vector3 dir;
    // stamina
    private bool sprinting;
    public float maxStamina = 100;
    private float currStamina;
    private float staminaRegenTimer = 0.0f;
    public float staminaDrainPerFrame = 20.0f;
    public float staminaRegenPerFrame = 30.0f;
    public float staminaTimeToRegen = 3.0f;
    // roll
    public float rollStaminaCost = 40;
    public float slideSpeed = 3f;
    private float currSlideSpeed;
    // countdown
    public int startTimer = 600;
    #endregion

    #region combatVars
    public int attackStat;
    public int defenseStat;
    public GameObject primaryWeapon;
    public GameObject secondaryWeapon;
    public Weapon[] Weapons;
    public Weapon currWeapon;
    private float dealingDamage;
    public int currWeaponIndex;
    #endregion

    #region UI
    public Slider HealthBar;
    public Slider StaminaBar;
    #endregion

    private State state;
    private enum State
    {
        Normal, Roll, Attack
    }

    #region UnityFuncs
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        currStamina = maxStamina;
        currHealth = maxHealth;
        currSlideSpeed = slideSpeed;
        state = State.Normal;
        currTimer = startTimer;
        currWeaponIndex = 0;
        HealthBar.value = healthRatio();
        StaminaBar.value = staminaRatio();
    }

    void Update()
    {
        switch (state)
        {
            case State.Normal:
                movementMaster();
                lookAtMouse();
                rollCheck();
                attackCheck();
                swapCheck();
                break;
            case State.Roll:
                roll();
                break;
            case State.Attack:
                attackNow();
                break;
        }
        countDown();
        if (currHealth == 0)
        {
            Die();
        }

        // primary weapon attack
    }
    #endregion

    #region timeFuncs
    private void countDown()
    {
        currTimer -= Time.deltaTime;
        if (currTimer < 0)
        {
            Die();
        }
    }
    #endregion

    #region movementFuncs
    private void movementMaster()
    {
        sprinting = Input.GetKey(KeyCode.LeftShift) && currStamina > 0;
        if (sprinting && !playerRB.velocity.Equals(Vector2.zero))
        {
            currStamina = Mathf.Clamp(currStamina - (staminaDrainPerFrame * Time.deltaTime), 0.0f, maxStamina);
            staminaRegenTimer = 0.0f;
        }
        else if (currStamina < maxStamina)
        {
            if (staminaRegenTimer >= staminaTimeToRegen)
                currStamina = Mathf.Clamp(currStamina + (staminaRegenPerFrame * Time.deltaTime), 0.0f, maxStamina);
            else
                staminaRegenTimer += Time.deltaTime;
        }

        // movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        movement = new Vector2(moveHorizontal, moveVertical);
        if (sprinting)
        {
            playerRB.velocity = movement * sprintMoveSpeed;
        }
        else
        {
            playerRB.velocity = movement * baseMoveSpeed;
        }
        StaminaBar.value = staminaRatio();
    }

    private void lookAtMouse()
    {
        // makes player look at mouse
        dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void rollCheck()
    {
        if (Input.GetKey(KeyCode.R) && currStamina - rollStaminaCost >= 0 && !movement.Equals(Vector2.zero))
        {
            staminaRegenTimer = 0f;
            currStamina -= rollStaminaCost;
            currSlideSpeed = slideSpeed;
            state = State.Roll;
        }
    }

    private void roll()
    {
        transform.position += new Vector3(movement.x, movement.y) * 3f * Time.deltaTime;
        currSlideSpeed -= currSlideSpeed * 3f * Time.deltaTime;
        Debug.Log("Player rolling");
        if (currSlideSpeed < 1f)
        {
            state = State.Normal;
        }
    }

    private float staminaRatio()
    {
        return currStamina / maxStamina;
    }
    #endregion

    #region combatFuncs
    private void attackNow()
    {
        Debug.Log("Attacking now");
        dealingDamage = Weapons[currWeaponIndex].damage * (attackStat / 100);
    }

    private void swapCheck()
    {
        if (Input.GetKey(KeyCode.P) && Weapons.Length > 0)
        {
            Debug.Log("can swap weapons");
            swapWeapon();
            //swap  weapon at [0] with weapon at [1] in array? Constantly access weapon[0]? future = more weapons to cycle through?
        }
    }
    
    //kludgy weapon swap
    private void swapWeapon()
    {
        if (currWeaponIndex == 0)
        {
            Weapons[currWeaponIndex].inuse = false;
            currWeaponIndex = 1;
            Weapons[currWeaponIndex].inuse = true;
            Debug.Log("weapon swapped");
        } else
        {
            Debug.Log("weapon swapped!");
            Weapons[currWeaponIndex].inuse = false;
            currWeaponIndex = 0;
            Weapons[currWeaponIndex].inuse = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            GetComponentInParent<EnemyScript>().TakeDamage(dealingDamage);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponentInParent<EnemyScript>().TakeDamage(0);
        }

    }
    private void attackCheck()
    {
        if (Input.GetMouseButton(0))
        {
            attackWith(primaryWeapon);
        }
        else if (Input.GetMouseButton(1))
        {
            state = State.Attack;
            attackWith(secondaryWeapon);
        }

    }

    private void attackWith(GameObject weapon)
    {
        if (weapon.GetComponent<WeaponScript>().WeaponType() == 0)
        {
            StartCoroutine(MeleeAttackRoutine(weapon.GetComponent<MeleeWeaponScript>().damage));
        }
        else if (weapon.GetComponent<WeaponScript>().WeaponType() == 1)
        {

        }
        else if (weapon.GetComponent<WeaponScript>().WeaponType() == 1)
        {

        }
    }

    IEnumerator MeleeAttackRoutine(float damage)
    {
        state = State.Attack;
        Debug.Log("Cast hitbox now");
        Collider2D[] info = Physics2D.OverlapCircleAll(transform.position - transform.up, 1);
        for (int i = 0; i < info.Length; i++)
        {
            if (info[i].tag == "Enemy")
            {
                info[i].GetComponent<EnemyScript>().TakeDamage(damage);
            }
        }
        yield return new WaitForSeconds(.75f);
        state = State.Normal;
    }
    #endregion

    #region healthFuncs
    public void takeDamage(float damageVal)
    {
        float adjustedDamage = damageVal * (defenseStat/100);
        currHealth -= adjustedDamage;
        Debug.Log("hurt; current health = " + currHealth);
        if (state.Equals(State.Roll))
        {
            return;
        }
        currHealth -= damageVal;
        HealthBar.value = healthRatio();
        if (currHealth <= 0)
        {
            Die();
        }
    }

    public void heal(float healVal)
    {
        currHealth = Mathf.Min(maxHealth, currHealth + healVal);
        HealthBar.value = healthRatio();
    }
    
    private float healthRatio()
    {
        return currHealth / maxHealth;
    }

    private void Die()
    {
        GameObject.FindWithTag("GameController").GetComponent<GameManager>().StartGame();
        //Destroy(this.gameObject);
        Debug.Log("player died");
    }
    #endregion
}
