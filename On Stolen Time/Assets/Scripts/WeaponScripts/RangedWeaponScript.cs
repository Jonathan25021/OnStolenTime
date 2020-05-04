using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeaponScript : WeaponScript
{
    public int _ammo;
    public float _rangeFactor;
    public int _magSize;
    public int _currMag;
    public float reloadTime;
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
            _currMag = _ammo;
            _ammo = 0;
            yield return new WaitForSeconds(reloadTime);
        }
        else
        {
            _ammo -= (_magSize - _currMag);
            _currMag = _magSize;
            yield return new WaitForSeconds(reloadTime);
        }
        Debug.Log("reloading");
    }

    public int CurrMag()
    {
        return _currMag;
    }

    public float RangeFactor()
    {
        return _rangeFactor;
    }

    abstract public IEnumerator Fire(Vector3 position, Vector3 dir);

    override public int WeaponType()
    {
        return 1;
    }
}
