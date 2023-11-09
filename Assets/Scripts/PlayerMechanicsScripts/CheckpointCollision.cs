using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointCollision : MonoBehaviour
{

    public PlayerMovement player;
    public RespawnPlayer respawn;
    public ResourceManager resourceManager;
    [SerializeField] private ImpactFlash impactFlash;

    private void Start()
    {
        player = GetComponent<PlayerMovement>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            player.SetGlobalLight();
            collision.GetComponent<CircleCollider2D>().enabled = false;
            collision.GetComponentInChildren<ParticleSystem>().Play();
            respawn.setRespawn(collision.transform);
            resourceManager.AddCountCandle(1);
            StartCoroutine(impactFlash.FlashRoutine());
        }
    }

}
