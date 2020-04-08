using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponScript : MonoBehaviour
{
    public float _damage;
    public float _attackSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Returns 0 for melee weapons, 1 for ranged weapons, 2 for shields
    public abstract int WeaponType();

    public float Damage()
    {
        return _damage;
    }

    public float AttackSpeed()
    {
        return _attackSpeed;
    }
}
