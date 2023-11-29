using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConveyorBelt : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("The speed of the belt")]
    [SerializeField]
    public float movespeed;

    [SerializeField]
    [Tooltip("Determines the direction of the movement")]
    public bool isClockwise;

    [SerializeField]
    private Transform direction;


    PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (isClockwise)
        {
            direction.localScale = new Vector3(direction.localScale.x, 1, 1);
        }
        else
        {
            direction.localScale = new Vector3(direction.localScale.x, -1, 1);
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            if (isClockwise)
            {
                rb.position += Vector2.right * movespeed * Time.deltaTime;
            }
            else
            {
                rb.position += Vector2.left * movespeed * Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            float movespeed = playerMovement.speed / 2;
            playerMovement.speed = movespeed;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            float movespeed = playerMovement.speed * 2;
            playerMovement.speed = movespeed;
        }

    }
}
