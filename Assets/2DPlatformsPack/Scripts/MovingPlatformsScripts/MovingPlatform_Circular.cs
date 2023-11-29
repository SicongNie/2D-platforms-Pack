using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform_Circular : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]
    private Transform rotationCenter;

    [Range(0f, 50f)]
    [SerializeField]
    [Tooltip("The radius of the circular path")]
    public float rotationRadius;

    [Range(0f, 50f)]
    [SerializeField]
    [Tooltip("The movespeed of the platform")]
    public float movespeed;

    [SerializeField]
    [Tooltip("Determines the direction of circular movement")]
    public bool isClockwise;

    [Header("Visuals")]
    [SerializeField]
    [Tooltip("Determines whether to show a circular path of the platform")]
    public bool showLine;

    private float posX;
    private float posY;
    private float angle = 0f;

    PlayerMovement playerMovement;

    
    private void Update()
    {
        posX = rotationCenter.position.x + Mathf.Cos(angle) * rotationRadius;
        posY = rotationCenter.position.y + Mathf.Sin(angle) * rotationRadius;
        transform.position = new Vector2(posX, posY);
        if (!isClockwise)
        {
            angle += Time.deltaTime * movespeed;
        }
        else
        {
            angle -= Time.deltaTime * movespeed;
        }
        if (angle >= 360f)
        {
            angle = 0f;
        }

    }

    private void OnDrawGizmos()
    {
        if (showLine)
        {
            DrawPathGizmos();
            rotationCenter.gameObject.SetActive(true);
        }
        else
        {
            rotationCenter.gameObject.SetActive(false);
        }
    }

    private void DrawPathGizmos()
    {
        if (rotationCenter == null)
            return;

        Gizmos.color = Color.red;

        Vector3 previousPosition = rotationCenter.position;
        float gizmoAngle = 0f;
        float gizmoStep = 5f;
        while (gizmoAngle <= 360f)
        {
            float gizmoX = rotationCenter.position.x + Mathf.Cos(gizmoAngle * Mathf.Deg2Rad) * rotationRadius;
            float gizmoY = rotationCenter.position.y + Mathf.Sin(gizmoAngle * Mathf.Deg2Rad) * rotationRadius;
            Vector3 currentPosition = new Vector3(gizmoX, gizmoY, 0f);

            Gizmos.DrawLine(previousPosition, currentPosition);

            previousPosition = currentPosition;
            gizmoAngle += gizmoStep;
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
