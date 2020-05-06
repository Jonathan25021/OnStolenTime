using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorScript : PickupScript
{
    public float flatDamageModifier;
    public float percentDamageModifier;
    public int type; //0 for head, 1 for chest, 2 for legs, 3 for feet
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public float DamageModifier(float damage)
    {
        return Mathf.Max((damage - flatDamageModifier) * (1 - percentDamageModifier), 0);
    }
    public int Type()
    {
        return type;
    }
    override public int PickupType()
    {
        return 1;
    }
}
