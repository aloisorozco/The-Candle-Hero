using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candles : MonoBehaviour
{

    public void ResetCandles()
    {
        CircleCollider2D[] candleColliders = GetComponentsInChildren<CircleCollider2D>();

        foreach (CircleCollider2D candleCollider in candleColliders)
        {
            candleCollider.enabled = true;
        }
    }
}
