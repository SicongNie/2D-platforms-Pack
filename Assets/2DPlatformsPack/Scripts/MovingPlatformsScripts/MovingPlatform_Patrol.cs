using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovingPlatform_Patrol : MonoBehaviour
{
    [SerializeField]
    public Transform[] patrolPoints;

    [SerializeField]
    public GameObject PatrolPoint_List;

    private int index;
    private int direction = 1;

    [Range(0f, 50f)]
    [SerializeField]
    [Tooltip("The movespeeld of the platform")]
    public float movespeed;

    [SerializeField]
    public bool isClosed;

    [SerializeField]
    public bool showLine;


    private GameObject point;
    private Vector3 currentTarget;
    PlayerMovement playerMovement;
    Rigidbody2D rb;
    Vector3 moveDirection;
    Rigidbody2D playerRb;

    private void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();


        patrolPoints = new Transform[PatrolPoint_List.transform.childCount];
        for (int i = 0; i < PatrolPoint_List.transform.childCount; i++)
        {
            patrolPoints[i] = PatrolPoint_List.transform.GetChild(i).gameObject.transform;
        }
    }

    private void Start()
    {
        index = 1;
        currentTarget = patrolPoints[1].transform.position;
        DirectionCalculate();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, currentTarget) < 0.05f)
        {
            if (isClosed)
            {
                MoveToNextPoint_Closed();
            }
            else
            {
                MoveToNextPoint();
            }
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDirection * movespeed;
    }

    private void MoveToNextPoint()
    {
        transform.position = currentTarget;
        if (index == patrolPoints.Length - 1)
        {
            direction = -1;
        }
        if (index == 0)
        {
            direction = 1;
        }

        index += direction;
        currentTarget = patrolPoints[index].transform.position;
        DirectionCalculate();
    }

    private void MoveToNextPoint_Closed()
    {
        transform.position = currentTarget;
        index++;
        if (index > patrolPoints.Length - 1)
        {
            index = 0;
        }
        currentTarget = patrolPoints[index].transform.position;
        DirectionCalculate();
    }

    private void DirectionCalculate()
    {
        moveDirection = (currentTarget - transform.position).normalized;
    }

    public void AddPoints()
    {
        string pointname = "Point_" + (PatrolPoint_List.transform.childCount + 1);
        point = new GameObject(pointname);
        point.transform.parent = PatrolPoint_List.transform;
    }

    public void DeletePoint()
    {
        int childCount = PatrolPoint_List.transform.childCount;
        if (childCount > 0)
        {
            Transform lastChild = PatrolPoint_List.transform.GetChild(childCount - 1);
            DestroyImmediate(lastChild.gameObject);
        }
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
            DrawLine();
        }
    }

    private void DrawLine()
    {
        Gizmos.color = Color.yellow;
        if (isClosed)
        {
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                int nextIndex = (i + 1) % patrolPoints.Length;
                Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[nextIndex].position);
            }
        }
        else if (!isClosed)
        {
            for (int i = 0; i < patrolPoints.Length - 1; i++)
            {
                int nextIndex = (i + 1) % patrolPoints.Length;
                Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[nextIndex].position);
            }
        }

    }

}
