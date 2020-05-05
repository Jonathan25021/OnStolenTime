using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupRadiusScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pickup"))
        {
            Debug.Log("added item");
            GetComponentInParent<PlayerScript>().PickupItems.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Pickup"))
        {
            int remove = GetComponentInParent<PlayerScript>().PickupItems.IndexOf(collision.gameObject);
            GetComponentInParent<PlayerScript>().PickupItems.RemoveAt(remove);
        }
    }
}
