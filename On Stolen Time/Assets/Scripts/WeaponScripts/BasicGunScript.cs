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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override IEnumerator Fire(Vector3 position, Vector3 dir)
    {
        if(CurrMag() == 0)
        {
            Reload();
        }
        Debug.DrawRay(position, dir, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(position, dir);
        if (hit && hit.transform.CompareTag("Enemy"))
        {
            hit.transform.GetComponent<EnemyScript>().TakeDamage(_damage);
        }
        yield return new WaitForSeconds(_attackSpeed);
    }

}
