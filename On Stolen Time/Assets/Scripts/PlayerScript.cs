using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    #region healthVars
    public float maxHealth = 100;
    private float currHealth;
    #endregion

    #region movemenentVars
    private Vector2 movement;
    public float baseMoveSpeed = 1;
    public float sprintMoveSpeed = 2;
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
    public GameObject primaryWeapon;
    public GameObject secondaryWeapon;
    #endregion

    private State state;
    private enum State
    {
        Normal, Roll, Attack,
    }

    #region UnityFuncs
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        currStamina = maxStamina;
        currHealth = maxHealth;
        currSlideSpeed = slideSpeed;
        state = State.Normal;
    }

    void Update()
    {
        switch (state)
        {
            case State.Normal:
                movementMaster();
                lookAtMouse();
                rollCheck();
                break;
            case State.Roll:
                roll();
                break;
        }
        // roll
        

        // primary weapon attack
    }
    #endregion

    #region movemenetFuncs
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
        currSlideSpeed -= currSlideSpeed * 2f * Time.deltaTime;
        Debug.Log(currSlideSpeed);
        if (currSlideSpeed < 1f)
        {
            Debug.Log("done Rolling");
            state = State.Normal;
        }
    }
    #endregion

    #region combatFuncs
    #endregion

    #region healthFuncs
    public void takeDamage(float damageVal)
    {
        currHealth -= damageVal;
    }

    public void heal(float healVal)
    {
        currHealth = Mathf.Min(maxHealth, currHealth + healVal);
    }
    #endregion
}
