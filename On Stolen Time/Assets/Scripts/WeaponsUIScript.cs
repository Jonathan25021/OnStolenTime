using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsUIScript : MonoBehaviour
{
    public GameObject P;
    public GameObject S;
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        P = Player.GetComponent<PlayerScript>().primaryWeapon;
        S = Player.GetComponent<PlayerScript>().secondaryWeapon;
        transform.GetChild(0).GetComponent<Image>().sprite = P.GetComponent<SpriteRenderer>().sprite;
        transform.GetChild(1).GetComponent<Image>().sprite = S.GetComponent<SpriteRenderer>().sprite;
        if (P.GetComponent<WeaponScript>().WeaponType() == 1)
        {
            int CurrMag = P.GetComponent<BasicGunScript>()._currMag;
            int MaxMag = P.GetComponent<BasicGunScript>()._magSize;
            int Ammo = P.GetComponent<BasicGunScript>()._ammo;
            transform.GetChild(2).GetComponent<Text>().text = CurrMag + "/" + MaxMag + "\n" + Ammo;
        }
        else
        {
            transform.GetChild(2).GetComponent<Text>().text = "";
        }
        if (S.GetComponent<WeaponScript>().WeaponType() == 1)
        {
            int CurrMag = S.GetComponent<BasicGunScript>()._currMag;
            int MaxMag = S.GetComponent<BasicGunScript>()._magSize;
            int Ammo = S.GetComponent<BasicGunScript>()._ammo;
            transform.GetChild(3).GetComponent<Text>().text = CurrMag + "/" + MaxMag + "\n" + Ammo;
        }
        else
        {
            transform.GetChild(3).GetComponent<Text>().text = "";
        }
    }
}
