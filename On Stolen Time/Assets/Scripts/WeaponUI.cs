using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    #region Player
    [SerializeField]
    [Tooltip("The location for the sprite to be displayed.")]
    private Image m_Image;

    [SerializeField]
    [Tooltip("The location for the time to be displayed.")]
    private Text m_Text;

    [SerializeField]
    [Tooltip("The player that the timer corresponds to.")]
    private PlayerScript m_Player;
    #endregion

    private void Start()
    {
        m_Image.sprite = m_Player.primaryWeapon.gameObject.GetComponent<SpriteRenderer>().sprite;
        m_Text.text = "";
        m_Image.transform.position = new Vector2(50, 70);
        m_Text.transform.position = new Vector2(30, 30);
    }

    // Update is called once per frame
    void Update()
    {
        m_Image.sprite = m_Player.primaryWeapon.gameObject.GetComponent<SpriteRenderer>().sprite;
        if (m_Player.primaryWeapon.GetComponent<RangedWeaponScript>().WeaponType() == 1)
        {
            m_Text.text = "Current Mag: " + m_Player.primaryWeapon.GetComponent<RangedWeaponScript>().CurrMag().ToString() + " Total Ammo: " +
                m_Player.primaryWeapon.GetComponent<RangedWeaponScript>().Ammo().ToString();
        }
    }
}
