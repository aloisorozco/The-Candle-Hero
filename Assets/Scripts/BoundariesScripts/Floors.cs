using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO: attach script to floor objects & platforms
public class Floors : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //TODO: set up floors game objects in level and rigid bodies for them
    //TODO: test that when the player hits his head on a platform he does not freeze there
    public void OnCollisionEnter(Collision other)
    {
        other.gameObject.GetComponent<PlayerController>().playerVerticalAcceleration = 0f;
        other.gameObject.GetComponent<PlayerController>().isJumping = false;
    }

    public void OnCollisionExit(Collision other)
    {
        other.gameObject.GetComponent<PlayerController>().playerVerticalAcceleration = -0.05f;
    }
}
