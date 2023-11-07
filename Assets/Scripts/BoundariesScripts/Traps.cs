using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().playerHealth = 0;
        }
        else if (other.gameObject.name == "GreyGoo")
        {
            other.gameObject.GetComponent<GreyGoo>().enemyHealth = 0;
        }
    }
}
