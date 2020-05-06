using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorUIScript : MonoBehaviour
{
    public GameObject head;
    public GameObject chest;
    public GameObject legs;
    public GameObject boots;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        head.GetComponent<Image>().sprite = player.GetComponent<PlayerScript>().head.GetComponent<SpriteRenderer>().sprite;
        chest.GetComponent<Image>().sprite = player.GetComponent<PlayerScript>().chest.GetComponent<SpriteRenderer>().sprite;
        legs.GetComponent<Image>().sprite = player.GetComponent<PlayerScript>().legs.GetComponent<SpriteRenderer>().sprite;
        boots.GetComponent<Image>().sprite = player.GetComponent<PlayerScript>().boots.GetComponent<SpriteRenderer>().sprite;
    }
}
