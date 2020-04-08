using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGunScript : RangedWeaponScript
{
    // Start is called before the first frame update
    void Start()
    {
        _ammo = 36;
        _magSize = 6;
        _currMag = _magSize;
        _rangeFactor = 175f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
