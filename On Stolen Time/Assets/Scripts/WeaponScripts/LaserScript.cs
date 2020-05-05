using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{

    private LineRenderer lineRenderer;
    public Transform LaserHit;
    public bool LaserToggle;
    // Start is called before the first frame update
    void Start()
    {
        LaserToggle = false;
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LaserToggle == true)
        {
            
            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
        }
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, GetComponentInParent<PlayerScript>().dir, Mathf.Infinity, ~(1<<16 | 1<<5));
        
        LaserHit.position = hit.point;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, LaserHit.position);
    }
}
