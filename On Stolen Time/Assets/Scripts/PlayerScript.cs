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
    public float slideSpeed = 10f;
    private float currSlideSpeed;
    public float rollCooldownMax = .5f;
    private float rollCooldown;
    // countdown
    public int startTimer = 600;
    public GameObject mainCamera;

    private float timeInRangeAttack;
    private int mouseButton;
    #endregion

    #region combatVars
    public int attackStat;
    public int defenseStat;
    public GameObject primaryWeapon;
    public GameObject secondaryWeapon;
    private int currWeapon;
    private bool rangeAttack;
    private float panRange;
    private GameObject sword;
    public GameObject helmet;
    public GameObject chestplate;
    public GameObject leggings;
    public GameObject boots;
    #endregion

    #region time
    // countdown
    public int rate = 1;
    private Vector2 rewindedPos;
    private float lastUsed = 0;
    private bool toGo = false;
    private float cost = 0;
    private float rewindedHealth;
    private bool slow = false;
    private GameObject rewindedFirstWeapon;
    private GameObject rewindedSecondWeapon;
    #endregion

    #region pickups
    private bool touching;
    private GameObject pick;
    private Transform pickLoc;
    private bool first;
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
        Normal, Roll, Attack, Aim, Fire
    }

    #region animation_components
    Animator anim;
    #endregion

    #region UnityFuncs
    void Start()
    {
        primaryWeapon = Instantiate(primaryWeapon) as GameObject;
        secondaryWeapon = Instantiate(secondaryWeapon) as GameObject;
        primaryWeapon.transform.parent = this.transform;
        secondaryWeapon.transform.parent = this.transform;
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
        rollCooldown = 0;
        sword = transform.GetChild(0).gameObject;
        sword.transform.position = new Vector3(0, 0, 1000000000);
        rewindedPos = transform.position;
        first = true;
        touching = false;
    }

    void Update()
    {
        if (allEnemies == null)
        {
            allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            Debug.Log(allEnemies.Length);
        }
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
                rollCheck();
                break;
            case State.Aim:
                movementMaster();
                lookAtMouse();
                rollCheck();
                RangeAttackCheck();
                break;
            case State.Fire:
                movementMaster();
                lookAtMouse();
                rollCheck();
                break;
        }
        countDown();
        rewind();
        toggleSpeed();
        pickItem();
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
        if (Input.GetKeyDown(KeyCode.T) && !slow)
        {
            slow = true;
            rate = 4;
            foreach (GameObject enemy in allEnemies)
            {
                Debug.Log("ENEMIES BEGONE");
                enemy.GetComponent<EnemyScript>().Slow();
            }
        } else if (Input.GetKeyDown(KeyCode.T) && slow)
        {
            Debug.Log("BYE");
            slow = false;
            rate = 1;
            foreach (GameObject enemy in allEnemies)
            {
                enemy.GetComponent<EnemyScript>().Speed();
            }
        }
        //if (Input.GetKeyDown(KeyCode.G) && !slow)
        //{
        //    baseMoveSpeed = 2;
        //    slow = true;
        //    rate = 2;
        //    foreach (GameObject enemy in allEnemies)
        //    {
        //        enemy.GetComponent<EnemyScript>().Slow();
        //    }
        //} else if (Input.GetKeyDown(KeyCode.G) && slow)
        //{
        //    baseMoveSpeed = 3;
        //    slow = false;
        //    rate = 1;
        //    foreach (GameObject enemy in allEnemies)
        //    {
        //        enemy.GetComponent<EnemyScript>().Speed();
        //    }
        //}
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
            rewindedFirstWeapon = primaryWeapon;
            rewindedSecondWeapon = secondaryWeapon;
        } else if (Input.GetKey(KeyCode.Z) && currTimer > 10 && lastUsed <= 0 && toGo)
        {
            Debug.Log("It's rewind time!");
            Debug.Log(rewindedPos);
            Debug.Log(transform.position);
            transform.position = rewindedPos;
            currTimer -= 2 * (cost - currTimer);
            currHealth = rewindedHealth;
            HealthBar.value = healthRatio();
            primaryWeapon = rewindedFirstWeapon;
            secondaryWeapon = rewindedSecondWeapon;
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
        if (Input.GetKey(KeyCode.F))
        {
            pan();
        }
        else
        {
            Vector3 panPosition = Vector3.Lerp(mainCamera.transform.position, transform.position + new Vector3(0, 0, -10), .125f);
            mainCamera.transform.position = panPosition;
        }
        
    }
    
    private void pan()
    {
            Vector3 panPosition = Vector3.Lerp(mainCamera.transform.position, transform.position + new Vector3(0, 0, -10) + dir.normalized*4, .125f);
            mainCamera.transform.position = panPosition;
    }

    private void rollCheck()
    {
        if (rollCooldown > 0)
        {
            rollCooldown -= Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Space) && currStamina - rollStaminaCost >= 0 && !movement.Equals(Vector2.zero) && rollCooldown <= 0)
        {
            anim.SetBool("moving", false);
            anim.SetTrigger("rolling");
            staminaRegenTimer = 0f;
            currStamina -= rollStaminaCost;
            currSlideSpeed = slideSpeed;
            rollCooldown = rollCooldownMax;
            state = State.Roll;
        }
    }

    private void roll()
    {
        var angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        playerRB.freezeRotation = true;
        transform.position += new Vector3(movement.x, movement.y).normalized * currSlideSpeed * Time.deltaTime;
        currSlideSpeed -= currSlideSpeed * 6f * Time.deltaTime;
        Debug.Log(currSlideSpeed);
        if (currSlideSpeed < 1f)
        {
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
            mouseButton = 0;
            state = State.Attack;
            attackWith(primaryWeapon);
        }
        else if (Input.GetMouseButton(1))
        {
            mouseButton = 1;
            state = State.Attack;
            attackWith(secondaryWeapon);
        }

    }

    private void attackWith(GameObject weapon)
    {
        if (weapon.GetComponent<WeaponScript>().WeaponType() == 0)
        {

            anim.SetTrigger("leftAttack");
            StartCoroutine(MeleeAttackRoutine(weapon));
        }
        else if (weapon.GetComponent<WeaponScript>().WeaponType() == 1)
        {
            state = State.Aim;
            timeInRangeAttack = 0;
        }
        else if (weapon.GetComponent<WeaponScript>().WeaponType() == 2)
        {

        }
    }

    IEnumerator MeleeAttackRoutine(GameObject weapon)
    {
        state = State.Attack;
        playerRB.freezeRotation = true;
        Collider2D[] info = Physics2D.OverlapCircleAll(transform.position - transform.up * .8f, 0.5f);
        Vector3 start = new Vector3(1, 0, 1);
        Vector3 end = new Vector3(1, 2, 1);
        float ELAtime = 0;
        while (ELAtime < weapon.GetComponent<MeleeWeaponScript>().AttackSpeed() / 2)
        {
            ELAtime += Time.deltaTime;
            sword.transform.localScale = Vector3.Lerp(start, end, 2f * ELAtime / weapon.GetComponent<MeleeWeaponScript>().AttackSpeed());
            yield return null;
        }
        Debug.Log("finish attack");
        for (int i = 0; i < info.Length; i++)
        {
            if (info[i].tag == "Enemy")
            {
                info[i].GetComponent<EnemyScript>().TakeDamage(weapon.GetComponent<MeleeWeaponScript>().Damage());
            }
        }
        playerRB.freezeRotation = false;
        sword.transform.localScale = new Vector3(0, 0, 0);
        Debug.Log("finish attack");
        yield return new WaitForSeconds(weapon.GetComponent<MeleeWeaponScript>().AttackSpeed() / 2);
        state = State.Normal;
    }

    private void RangeAttackCheck()
    {
        if (Input.GetMouseButtonUp(mouseButton))
        {
            Debug.Log("Fire");
            if (mouseButton == 0)
            {
                StartCoroutine(RangedAttackRoutine(primaryWeapon));
            }
            else
            {
                StartCoroutine(RangedAttackRoutine(secondaryWeapon));
            }
            
            
        }
        if (timeInRangeAttack < 1f)
        {
            timeInRangeAttack += Time.deltaTime;
        }
        else
        {
            pan();
        }
    }

    IEnumerator RangedAttackRoutine(GameObject weapon)
    {
        anim.SetTrigger("leftAttack");
        state = State.Fire;
        yield return StartCoroutine(weapon.GetComponent<RangedWeaponScript>().Fire(transform.position, dir));
        state = State.Normal;
    }

    #endregion

    #region healthFuncs
    public void takeDamage(float damageVal)
    {
        StartCoroutine(DamageIndicator());
        float adjustedDamage = damageVal * (defenseStat/100);
        currHealth -= adjustedDamage;
        Debug.Log("hurt; current health = " + currHealth);
        if (state.Equals(State.Roll))
        {
            return;
        }
        currHealth -= Mathf.Max(0,(damageVal - totalFlatDamageModifier()) * (1 - totalPercentDamageModifier()));
        HealthBar.value = healthRatio();
        if (currHealth <= 0)
        {
            Die();
        }
    }

    private float totalPercentDamageModifier()
    {
        return helmet.GetComponent<ArmorScript>().percentDamageModifier
            + chestplate.GetComponent<ArmorScript>().percentDamageModifier
            + leggings.GetComponent<ArmorScript>().percentDamageModifier
            + boots.GetComponent<ArmorScript>().percentDamageModifier;
    }

    private float totalFlatDamageModifier()
    {
        return helmet.GetComponent<ArmorScript>().flatDamageModifier
            + chestplate.GetComponent<ArmorScript>().flatDamageModifier
            + leggings.GetComponent<ArmorScript>().flatDamageModifier
            + boots.GetComponent<ArmorScript>().flatDamageModifier;
    }
    IEnumerator DamageIndicator()
    {
        SpriteRenderer sprite = this.gameObject.GetComponent<SpriteRenderer>();
        sprite.color = new Color(192, 44, 44);
        Debug.Log(sprite.color);
        //yield return new WaitForSeconds(.3f);
        //sprite.color = new Color(255, 255, 255);
        yield return null;
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
        Debug.Log("died");
        //GameObject.FindWithTag("GameController").GetComponent<GameManager>().StartGame();
        Destroy(gameObject);
        Debug.Log("player died");
    }

    #endregion

    #region pickup
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pickup") && !touching)
        {
            Debug.Log("TOUCH");
            touching = true;
            pick = collision.gameObject;
            pickLoc = collision.transform;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pickup") && touching)
        {
            Debug.Log("NO!");
            touching = false;
            pick = null;
            pickLoc = null;
        }
    }

    private void pickItem()
    {
        if (Input.GetKeyDown(KeyCode.E) && touching)
        {
            touching = false;
            if (first)
            {
                Debug.Log("HI");
                Vector3 pos = pickLoc.position;
                Quaternion rot = pickLoc.rotation;
                Instantiate(primaryWeapon, pos, rot);
                first = false;
            } else
            {
                Transform old = primaryWeapon.transform;
                old.position = new Vector3(pickLoc.transform.position.x, pickLoc.transform.position.y, pickLoc.transform.position.z);
            }
            primaryWeapon = pick;
            Transform newT = pick.transform;
            newT.position = new Vector3(0, 0, 1000000000);
            pick = null;
            pickLoc = null;
            Debug.Log("picked");
        }
    }
    #endregion
}
