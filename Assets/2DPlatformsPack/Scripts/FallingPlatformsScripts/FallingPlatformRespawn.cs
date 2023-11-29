using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformRespawn : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The position where the block will respawn")]
    public Transform respawnPoint;

    [SerializeField]
    [Tooltip("The block object that will respawn")]
    public GameObject block;

    [SerializeField]
    [Tooltip("Time delay before the block respawns")]
    public float respawnDelay;

    private Rigidbody2D blokcRb;
    [HideInInspector]
    public bool isDestroyed;
    private float respawnTimer = 0f;

    private void Start()
    {
        isDestroyed = false;
        blokcRb = block.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isDestroyed)
        {
            respawnTimer += Time.deltaTime;

            if (respawnTimer >= respawnDelay)
            {
                SpawnBlock();
                isDestroyed = false;
                respawnTimer = 0f;
            }
        }
    }

    void SpawnBlock()
    {
        if (block != null && respawnPoint != null)
        {
            block.transform.position = respawnPoint.position;
            blokcRb.bodyType = RigidbodyType2D.Kinematic;
            block.SetActive(true);
        }
    }

}
