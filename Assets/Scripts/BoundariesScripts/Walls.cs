using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: attach script to walls & boundaries
public class WallsScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //TODO: set up walls game objects in level and rigid bodies for them
    public void OnCollisionEnter(Collision other)
    {
        other.gameObject.GetComponent<PlayerController>().playerSpeed.x = 0;
    }
}
