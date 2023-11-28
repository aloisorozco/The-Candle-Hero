using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    [SerializeField] public Vector2 force = new Vector2(0f, 0f);

    public void Update()
    {
        this.GetComponent<Rigidbody2D>().AddForce(force);
    }
}
