using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Time delay before the block falls")]
    public float fallDelay;
    [SerializeField]
    [Tooltip("Time delay before the block destorys")]
    public float destoryDelay;

    [SerializeField]
    FallingPlatformRespawn fallingPlatformRespawn;

    private Rigidbody2D rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fallingPlatformRespawn = GetComponentInParent<FallingPlatformRespawn>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        fallingPlatformRespawn.isDestroyed = true;
        yield return new WaitForSeconds(destoryDelay);
        gameObject.SetActive(false);
    }

}
