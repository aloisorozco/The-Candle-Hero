using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emberScript : MonoBehaviour
{
    public GameObject flame;
    public CircleCollider2D cc;
    void Start()
    {
        cc = GetComponent<CircleCollider2D>();
    }

    public void SetInactive()
    {
        cc.enabled = false;
        flame.SetActive(false);
    }
}
