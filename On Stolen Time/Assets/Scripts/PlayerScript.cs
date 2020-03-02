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
    #endregion

    #region attackVars
    public GameObject primaryWeapon;
    public GameObject secondaryWeapon;
    #endregion

    #region UnityFuncs
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        currStamina = maxStamina;
        currHealth = maxHealth;
    }

    void Update()
    {
        // sprinting and stamina
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
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        if (sprinting)
        {
            playerRB.velocity = movement * sprintMoveSpeed;
        }
        else
        {
            playerRB.velocity = movement * baseMoveSpeed;
        }

        // makes player look at mouse
        dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // primary weapon attack
    }
    #endregion

    #region attackFuncs

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
