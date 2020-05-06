using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class PickupScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    abstract public int PickupType(); // 0 for weapons, 1 for armor
}
