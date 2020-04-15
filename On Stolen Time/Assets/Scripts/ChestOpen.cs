using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpen : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Items to spawn.")]
    private GameObject[] drops;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            float toSpawn = Random.Range(0, 1.0f);
            int index = Mathf.RoundToInt(toSpawn * drops.Length);
            GameObject dropped = drops[index];
            Instantiate(dropped, collision.transform.position, collision.transform.rotation);
            Destroy(gameObject);
        }
    }

}
