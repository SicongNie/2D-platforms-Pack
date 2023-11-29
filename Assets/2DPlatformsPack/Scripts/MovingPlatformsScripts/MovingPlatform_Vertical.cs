using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform_Vertical : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]
    public Transform edgeTop;
    [SerializeField]
    public Transform edgeDown;

    [Range(0f, 50f)]
    [SerializeField]
    [Tooltip("The movespeed of the platform")]
    public float movespeed;

    [SerializeField]
    [Tooltip("Determines if the platform starts from the down edge")]
    public bool startFromDown;

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
    Rigidbody2D playerRb;


    private void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        waitTimer = waitTime;
    }

    private void Start()
    {
        if (!startFromDown)
        {
            currentTarget = edgeTop.position;
        }
        if (startFromDown)
        {
            currentTarget = edgeDown.position;
        }

        DirectionCalculate();
        switchDistance = GetComponent<SpriteRenderer>().bounds.size.y / 2;

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
                    if (currentTarget == edgeTop.position)
                    {
                        currentTarget = edgeDown.position;
                        DirectionCalculate();

                    }
                    else
                    {
                        currentTarget = edgeTop.position;
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
                if (currentTarget == edgeTop.position)
                {
                    currentTarget = edgeDown.position;
                    DirectionCalculate();

                }
                else
                {
                    currentTarget = edgeTop.position;
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
            playerRb.gravityScale = playerRb.gravityScale * 10;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerMovement.isOnPlatform = false;
            playerRb.gravityScale = playerRb.gravityScale / 10;
        }
    }

    private void OnDrawGizmos()
    {
        if (showLine)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(edgeTop.position, edgeDown.position);
            edgeTop.gameObject.SetActive(true);
            edgeDown.gameObject.SetActive(true);
        }
        else
        {
            edgeTop.gameObject.SetActive(false);
            edgeDown.gameObject.SetActive(false);
        }
    }

}
