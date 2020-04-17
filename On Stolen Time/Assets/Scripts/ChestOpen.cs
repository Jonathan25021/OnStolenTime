using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpen : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Items to spawn.")]
    private GameObject[] drops;

    [SerializeField]
    [Tooltip("Opened sprite.")]
    private Sprite opened;

    private bool closed = true;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && closed)
        {
            float toSpawn = Random.Range(0, 1.0f);
            int index = Mathf.RoundToInt(toSpawn * drops.Length);
            GameObject dropped = drops[index];
            Instantiate(dropped, collision.transform.position, collision.transform.rotation);
            gameObject.GetComponent<SpriteRenderer>().sprite = opened;
            closed = false;
        }
    }

}
