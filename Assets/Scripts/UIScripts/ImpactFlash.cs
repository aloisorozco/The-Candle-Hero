using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactFlash : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private float flashDuration;

    private SpriteRenderer sr;
    private Material originalMaterial;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalMaterial = sr.material;
    }

    public IEnumerator FlashRoutine()
    {
        sr.material = mat;
        yield return new WaitForSeconds(flashDuration);

        sr.material = originalMaterial;
    }
}
