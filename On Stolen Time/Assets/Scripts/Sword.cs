using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    // Start is called before the first frame update
    void Awake()
    {
        inuse = false;
        weaponClass = 1;
        damage = 20;
        attackSpeed = 10;
    }

    public void AttackingWeapon()
    {
        //deal damage specific to swords
    }
}
