using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGunScript : RangedWeaponScript
{
    // Start is called before the first frame update
    void Start()
    {
        _currMag = _magSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override IEnumerator Fire(Vector3 position, Vector3 dir)
    {
        Debug.Log(_currMag);
        if (_currMag <= 0)
        {

            yield return StartCoroutine(Reload());
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(position, dir, Mathf.Infinity, (1 << 8 | 1 << 9 | 1 << 15));
            Debug.Log(hit.transform.gameObject);
            if (hit && hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<EnemyScript>().TakeDamage(_damage);
            
            }
            _currMag--;
            Debug.Log(Time.time);
            yield return new WaitForSeconds(_attackSpeed);

        }
            
    }

    

}
