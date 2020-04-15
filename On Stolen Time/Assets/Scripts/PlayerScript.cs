using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    #region healthVars
    public float maxHealth = 100;
    private float currHealth;
    public float currTimer;
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
    #endregion

    #region combatVars
    public int attackStat;
    public int defenseStat;
    public GameObject primaryWeapon;
    public GameObject secondaryWeapon;
    private int currWeapon;
    private bool rangeAttack;
    private float panRange;
    #endregion

    #region time
    // countdown
    public int startTimer = 600;
    public int rate = 1;
    private GameObject mainCamera;
    private Vector2 rewindedPos;
    private float lastUsed = 0;
    private bool toGo = false;
    private float cost = 0;
    private float rewindedHealth;
    private bool slow = false;
    #endregion


    #region Enemies
    private GameObject[] allEnemies;
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

    #region animation_components
    Animator anim;
    #endregion

    #region UnityFuncs
    void Start()
    {
        anim = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody2D>();
        currStamina = maxStamina;
        currHealth = maxHealth;
        currSlideSpeed = slideSpeed;
        state = State.Normal;
        currTimer = startTimer;
        HealthBar.value = healthRatio();
        StaminaBar.value = staminaRatio();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        rewindedPos = transform.position;
        allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void Update()
    {
        moveMainCamera();
        switch (state)
        {
            case State.Normal:
                movementMaster();
                lookAtMouse();
                rollCheck();
                attackCheck();
                
                break;
            case State.Roll:
                roll();
                break;
            case State.Attack:
                movementMaster();
                break;
        }
        countDown();
        rewind();
        toggleSpeed();
        if (currHealth <= 0)
        {
            Die();
        }
        
    }
    #endregion

    #region timeFuncs
    private void countDown()
    {
        if (currTimer < 0)
        {
            Die();
        }
        currTimer -= rate * Time.deltaTime;
        lastUsed -= Time.deltaTime;
    }

    private void toggleSpeed()
    {
        if (Input.GetKey(KeyCode.T) && !slow)
        {
            foreach (GameObject enemy in allEnemies)
            {
                enemy.GetComponent<EnemyScript>().slow();
            }
            slow = true;
            rate = 2;
        } else if (Input.GetKey(KeyCode.T) && slow)
        {
            foreach (GameObject enemy in allEnemies)
            {
                enemy.GetComponent<EnemyScript>().speed();
            }
            slow = false;
            rate = 1;
        }
        if (Input.GetKey(KeyCode.G) && !slow)
        {
            foreach (GameObject enemy in allEnemies)
            {
                enemy.GetComponent<EnemyScript>().slow();
            }
            baseMoveSpeed = 2;
            slow = true;
            rate = 2;
        } else if (Input.GetKey(KeyCode.G) && slow)
        {
            foreach (GameObject enemy in allEnemies)
            {
                enemy.GetComponent<EnemyScript>().speed();
            }
            baseMoveSpeed = 3;
            slow = false;
            rate = 1;
        }
    }

    private void rewind()
    {
        if (Input.GetKey(KeyCode.X))
        {
            Debug.Log("Oh that's hot!");
            rewindedPos = transform.position;
            rewindedHealth = currHealth;
            toGo = true;
            cost = currTimer;
        } else if (Input.GetKey(KeyCode.Z) && currTimer > 10 && lastUsed <= 0 && toGo)
        {
            Debug.Log("It's rewind time!");
            Debug.Log(rewindedPos);
            Debug.Log(transform.position);
            transform.position = rewindedPos;
            currTimer -= 2 * (cost - currTimer);
            currHealth = rewindedHealth;
            HealthBar.value = healthRatio();
            lastUsed = 2;
        }
    }
    #endregion

    #region movementFuncs
    private void movementMaster()
    {
        sprinting = Input.GetKey(KeyCode.LeftShift) && currStamina > 0 && state != State.Attack;
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
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            anim.SetBool("moving", true);
        }
        else
        {
            anim.SetBool("moving", false);
        }

        if (sprinting)
        {
            playerRB.velocity = movement * sprintMoveSpeed;
            //Debug.Log(sprintMoveSpeed);
            anim.SetFloat("moveSpeed", 2);
        }
        else if (state == State.Attack)
        {
            playerRB.velocity = .5f * movement * baseMoveSpeed;
        }
        else
        {
            playerRB.velocity = movement * baseMoveSpeed;
            //Debug.Log(baseMoveSpeed);
            anim.SetFloat("moveSpeed", 1);
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

    private void moveMainCamera()
    {
        //if (Input.GetKey(KeyCode.F))
        //{
        //    pan(100000);
        //}
        //else
        //{
            Vector3 panPosition = Vector3.Lerp(mainCamera.transform.position, transform.position + new Vector3(0, 0, -10), .125f);
            mainCamera.transform.position = panPosition;
        //}
        
    }

    private void panCheck()
    {
        if (rangeAttack)
        {
            pan(panRange);
        }
    }
    private void pan(float panFactor)
    {
            Vector3 panPosition = Vector3.Lerp(mainCamera.transform.position, transform.position + new Vector3(0, 0, -10) + dir / panFactor * Screen.width, .125f);
            mainCamera.transform.position = panPosition;
    }

    private void rollCheck()
    {
        if (Input.GetKey(KeyCode.Space) && currStamina - rollStaminaCost >= 0 && !movement.Equals(Vector2.zero))
        {
            staminaRegenTimer = 0f;
            currStamina -= rollStaminaCost;
            currSlideSpeed = slideSpeed;
            state = State.Roll;
        }
    }

    private void roll()
    {
        playerRB.freezeRotation = true;
        anim.SetFloat("sSpeed", currSlideSpeed);
        
        //Debug.Log(currSlideSpeed);
        anim.SetBool("rolling", true);
        transform.position += new Vector3(movement.x, movement.y).normalized * currSlideSpeed * Time.deltaTime;
        anim.SetFloat("PosY", transform.position.y);
        anim.SetFloat("PosX", transform.position.x);
        currSlideSpeed -= currSlideSpeed * 3f * Time.deltaTime;
        Debug.Log("Player rolling");
        if (currSlideSpeed < 1f)
        {
            anim.SetBool("rolling", false);
            //Debug.Log(currSlideSpeed);
            anim.SetFloat("sSpeed", currSlideSpeed);
            Debug.Log("Player stopped rolling");
            state = State.Normal;
        }
        playerRB.freezeRotation = false;
    }

    private float staminaRatio()
    {
        return currStamina / maxStamina;
    }
    #endregion

    #region combatFuncs
    private void attackCheck()
    {
        if (Input.GetMouseButton(0))
        {
            state = State.Attack;
            currWeapon = 0;
            anim.SetTrigger("leftAttack");
            attackWith(primaryWeapon);
        }
        else if (Input.GetMouseButton(1))
        {
            state = State.Attack;
            currWeapon = 1;
            //anim.SetTrigger("leftAttack");
            attackWith(secondaryWeapon);
        }

    }

    private void attackWith(GameObject weapon)
    {
        if (weapon.GetComponent<WeaponScript>().WeaponType() == 0)
        {
            
            StartCoroutine(MeleeAttackRoutine(weapon));
        }
        else if (weapon.GetComponent<WeaponScript>().WeaponType() == 1)
        {
            Debug.Log("beggining ranged attack");
            
            StartCoroutine(RangedAttackRoutine(weapon));
        }
        else if (weapon.GetComponent<WeaponScript>().WeaponType() == 2)
        {

        }
    }

    IEnumerator MeleeAttackRoutine(GameObject weapon)
    {
        state = State.Attack;
        Debug.Log("Cast hitbox now");
        playerRB.freezeRotation = true;
        Collider2D[] info = Physics2D.OverlapCircleAll(transform.position - transform.up, 0.5f);
        for (int i = 0; i < info.Length; i++)
        {
            if (info[i].tag == "Enemy")
            {
                info[i].GetComponent<EnemyScript>().TakeDamage(weapon.GetComponent<MeleeWeaponScript>().Damage());
            }
        }
        yield return new WaitForSeconds(weapon.GetComponent<MeleeWeaponScript>().AttackSpeed());
        playerRB.freezeRotation = false;
        state = State.Normal;
    }

    IEnumerator RangedAttackRoutine(GameObject weapon)
    {
        state = State.Attack;
        rangeAttack = true;
        panRange = weapon.GetComponent<RangedWeaponScript>().RangeFactor();
        yield return null;
        //yield return new WaitForSeconds(weapon.GetComponent<RangedWeaponScript>().AttackSpeed());
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
