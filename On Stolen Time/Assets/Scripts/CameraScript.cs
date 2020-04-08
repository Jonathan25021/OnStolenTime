using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
