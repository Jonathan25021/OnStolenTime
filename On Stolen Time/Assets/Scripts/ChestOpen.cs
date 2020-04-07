using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpen : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Items to spawn.")]
    private GameObject[] drops;

    private void OnCollisionEnter(Collision collision)
    {
        float toSpawn = Random.Range(0, 1.0f);
        int index = Mathf.RoundToInt(toSpawn * drops.Length);
        GameObject dropped = drops[index];
        Instantiate(dropped, this.transform);
    }
}
