using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform_Box : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]
    public Transform centerPoint;

    [Range(0f, 50f)]
    [SerializeField]
    [Tooltip("The movespeed of the platform")]
    public float movespeed;

    [SerializeField]
    [Tooltip("Determines the direction of the movement")]
    public bool isClockwise;

    [Header("Visuals")]
    [SerializeField]
    [Tooltip("Determines whether to show a path of the platform")]
    public bool showLine;

    private bool isPlayed;
    private Vector3[] squarePoints;
    private int currentIndex = 0;
    private float t = 0f;

    PlayerMovement playerMovement;

    private void Awake()
    {
        isPlayed = true;
    }

    private void Start()
    {
        CalculatePatrolPoints();
    }

    private void Update()
    {
        if (squarePoints != null && squarePoints.Length > 0)
        {
            MoveToNextPoint();
        }
    }

    private void CalculatePatrolPoints()
    {
        if (centerPoint != null)
        {
            Vector3 center = (transform.position + centerPoint.position) * 0.5f;
            float halfLength = Mathf.Abs(transform.position.x - centerPoint.position.x) * 0.5f;
            float halfWidth = Mathf.Abs(transform.position.y - centerPoint.position.y) * 0.5f;

            squarePoints = new Vector3[4];
            if (!isClockwise)
            {
                squarePoints[0] = center + new Vector3(-halfLength, -halfWidth, 0f);
                squarePoints[1] = center + new Vector3(halfLength, -halfWidth, 0f);
                squarePoints[2] = center + new Vector3(halfLength, halfWidth, 0f);
                squarePoints[3] = center + new Vector3(-halfLength, halfWidth, 0f);
            }
            else
            {
                if (isClockwise)
                {
                    squarePoints[0] = center + new Vector3(halfLength, halfWidth, 0f);
                    squarePoints[1] = center + new Vector3(halfLength, -halfWidth, 0f);
                    squarePoints[2] = center + new Vector3(-halfLength, -halfWidth, 0f);
                    squarePoints[3] = center + new Vector3(-halfLength, halfWidth, 0f);
                }
            }
        }


    }

    private void MoveToNextPoint()
    {
        transform.position = Vector3.Lerp(squarePoints[currentIndex], squarePoints[(currentIndex + 1) % 4], t);
        t += Time.deltaTime * movespeed;
        if (t >= 1f)
        {
            t = 0f;
            currentIndex = (currentIndex + 1) % 4;
        }
    }


    private void OnDrawGizmos()
    {
        if (showLine)
        {
            if (isPlayed)
            {
                DrawPathGizmos2();
            }
            else if (!isPlayed)
            {
                DrawPathGizmos();
            }
            centerPoint.gameObject.SetActive(true);
        }
        else
        {
           centerPoint.gameObject.SetActive(false);
        }

    }

    private void DrawPathGizmos()
    {
        Gizmos.color = Color.red;
        if (centerPoint != null)
        {
            Vector3 center = (transform.position + centerPoint.position) * 0.5f;

            float halfLength = Mathf.Abs(transform.position.x - centerPoint.position.x) * 0.5f;
            float halfWidth = Mathf.Abs(transform.position.y - centerPoint.position.y) * 0.5f;

            Gizmos.DrawLine(center + new Vector3(-halfLength, -halfWidth, 0f), center + new Vector3(halfLength, -halfWidth, 0f));
            Gizmos.DrawLine(center + new Vector3(-halfLength, -halfWidth, 0f), center + new Vector3(-halfLength, halfWidth, 0f));
            Gizmos.DrawLine(center + new Vector3(halfLength, halfWidth, 0f), center + new Vector3(-halfLength, halfWidth, 0f));
            Gizmos.DrawLine(center + new Vector3(halfLength, halfWidth, 0f), center + new Vector3(halfLength, -halfWidth, 0f));
        }
    }
    private void DrawPathGizmos2()
    {
        if (squarePoints != null && squarePoints.Length > 0)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < squarePoints.Length; i++)
            {
                Gizmos.DrawSphere(squarePoints[i], 0.1f);
                Gizmos.DrawLine(squarePoints[i], squarePoints[(i + 1) % 4]);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        collision.transform.parent = transform;

        playerMovement = collision.GetComponent<PlayerMovement>();
        float movespeed = playerMovement.speed * 3;
        playerMovement.speed = movespeed;


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        collision.transform.parent = null;
        playerMovement = collision.GetComponent<PlayerMovement>();
        float movespeed = playerMovement.speed / 3;
        playerMovement.speed = movespeed;
    }
}
