using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class TorchScript : MonoBehaviour
{
    public GameObject Light;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > 20)
        {
            Light.GetComponent<Light2D>().enabled = false;
        }
        else
        {
            Light.GetComponent<Light2D>().enabled = true;
        }
    }
}
