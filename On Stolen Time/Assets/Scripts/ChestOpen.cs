using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpen : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Items to spawn.")]
    public GameObject[] drops;

    public int numDrops;
    [SerializeField]
    [Tooltip("Opened sprite.")]
    public Sprite opened;



    private bool closed = true;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && closed)
        {
            for (int i = 0; i < numDrops; i++)
            {
                int index = Random.Range(0, drops.Length);
                GameObject dropped = drops[index];
                Instantiate(dropped, collision.transform.position, collision.transform.rotation);
            }
           
            gameObject.GetComponent<SpriteRenderer>().sprite = opened;
            closed = false;
        }
    }

}
