using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponScript : MonoBehaviour
{
    public float damage;
    public float attackSpeed;
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
}
