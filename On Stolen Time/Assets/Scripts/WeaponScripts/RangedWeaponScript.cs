﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeaponScript : WeaponScript
{
    public int _ammo;
    public float _rangeFactor;
    public int _magSize;
    public int _currMag;
    public float reloadTime;
    public Sprite TopDown;
    public Sprite Icon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public IEnumerator Reload()
    {
        if (_ammo <= 0)
        {
            yield return null;
        }
        else if (_ammo < _currMag)
        {
            yield return new WaitForSeconds(reloadTime);
            _currMag = _ammo;
            _ammo = 0;
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(reloadTime);
            _ammo -= (_magSize - _currMag);
            _currMag = _magSize;
            yield return null;
        }
        Debug.Log("reloading");
    }

    public int CurrMag()
    {
        return _currMag;
    }

    public int Ammo()
    {
        return _ammo;
    }

    public float RangeFactor()
    {
        return _rangeFactor;
    }

    public void TopDownSprite()
    {
        transform.GetComponent<SpriteRenderer>().sprite = TopDown;
    }

    public void IconSprite()
    {
        transform.GetComponent<SpriteRenderer>().sprite = Icon;
    }

    abstract public IEnumerator Fire(Vector3 position, Vector3 dir);

    override public int WeaponType()
    {
        return 1;
    }
}
