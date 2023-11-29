using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform_Horizontal : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]
    public Transform edgeLeft;
    [SerializeField]
    public Transform edgeRight;

    [Range(0f, 50f)]
    [SerializeField]
    [Tooltip("The movespeed of the platform")]
    public float movespeed;

    [SerializeField]
    [Tooltip("Determines if the platform starts from the right edge")]
    public bool startFromRight;

    [Header("Visuals")]
    [SerializeField]
    [Tooltip("Determines whether to show a line between edges")]
    public bool showLine = true;

    [Header("Wait Mode")]
    [SerializeField]
    [Tooltip("Enable the platform's wait mode, the platform will pause for a moment when it reaches the edges")]
    public bool WaitMode;
    private bool isWaiting = false;

    [SerializeField]
    [Tooltip("The duration for which the platform waits at the edges in wait mode")]
    public float waitTime;
    private float waitTimer;

    private Vector3 currentTarget;
    private float switchDistance;
    PlayerMovement playerMovement;
    Rigidbody2D rb;
    Vector3 moveDirection;

    private void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        waitTimer = waitTime;
    }

    private void Start()
    {
        if (!startFromRight)
        {
            currentTarget = edgeLeft.position;
        }
        if (startFromRight)
        {
            currentTarget = edgeRight.position;
        }

        DirectionCalculate();
        switchDistance = GetComponent<SpriteRenderer>().bounds.size.x / 2;

    }

    private void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget);
        if (WaitMode)
        {
            if (distanceToTarget < switchDistance)
            {
                isWaiting = true;
                waitTimer -= Time.deltaTime;

                if (waitTimer < 0f)
                {
                    isWaiting = false;
                    if (currentTarget == edgeLeft.position)
                    {
                        currentTarget = edgeRight.position;
                        DirectionCalculate();

                    }
                    else
                    {
                        currentTarget = edgeLeft.position;
                        DirectionCalculate();
                    }
                    waitTimer = waitTime;
                }

            }
        }
        else
        {
            isWaiting = false;
            waitTimer = waitTime;
            if (distanceToTarget < switchDistance)
            {
                if (currentTarget == edgeLeft.position)
                {
                    currentTarget = edgeRight.position;
                    DirectionCalculate();

                }
                else
                {
                    currentTarget = edgeLeft.position;
                    DirectionCalculate();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isWaiting)
        {
            rb.velocity = moveDirection * movespeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void DirectionCalculate()
    {
        moveDirection = (currentTarget - transform.position).normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerMovement.isOnPlatform = true;
            playerMovement.platformRb = rb;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerMovement.isOnPlatform = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (showLine)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(edgeLeft.position, edgeRight.position);
            edgeLeft.gameObject.SetActive(true);
            edgeRight.gameObject.SetActive(true);
        }
        else
        {
            edgeLeft.gameObject.SetActive(false);
            edgeRight.gameObject.SetActive(false);
        }
    }

}
