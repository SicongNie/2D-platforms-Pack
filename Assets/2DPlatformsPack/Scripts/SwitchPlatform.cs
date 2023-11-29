using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class SwitchPlatform : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Duration for the first group to be active")]
    public float group1Timer;

    [SerializeField]
    [Tooltip("Duration for the second group to be active")]
    public float group2Timer;

    [SerializeField]
    [Tooltip("The first group of platforms")]
    public GameObject group1;
    [SerializeField]
    [Tooltip("The second group of platforms")]
    public GameObject group2;

    [SerializeField]
    [Tooltip("Show points for both groups in Gizmos")]
    public bool showPoints;

    [SerializeField]
    [Tooltip("Activate the first group initially if true, otherwise activate the second group")]
    public bool group1First;

    [SerializeField]
    public TextMeshPro timerText;
    [SerializeField]
    [Tooltip("Show the timer on the platform")]
    public bool showTimer;

    private bool isGroup1Active;

    private void Start()
    {
        if (group1First)
        {
            group1.SetActive(false);
            group2.SetActive(true);
            isGroup1Active = false;
        }
        else
        {
            group1.SetActive(true);
            group2.SetActive(false);
            isGroup1Active = true;
        }
    }


    private void Update()
    {
        if (isGroup1Active)
        {
            group1Timer -= Time.deltaTime;
            if (group1Timer <= 0f)
            {
                group1.SetActive(false);
                group2.SetActive(true);

                isGroup1Active = false;
                group1Timer = 5f;
            }
            timerText.text = "Timer: " + group1Timer.ToString("F0");
        }
        else
        {
            group2Timer -= Time.deltaTime;
            if (group2Timer <= 0f)
            {
                group1.SetActive(true);
                group2.SetActive(false);
                isGroup1Active = true;
                group2Timer = 5f;
            }
            timerText.text = "Timer: " + group2Timer.ToString("F0");
        }
        if (showTimer)
        {
            timerText.gameObject.SetActive(true);
        }
        else
        {
            timerText.gameObject.SetActive(false);
        }
    }


    private void OnDrawGizmos()
    {
        if (showPoints)
        {
            DrawGizmosForChild(group1.transform, Color.blue);
            DrawGizmosForChild(group2.transform, Color.red);
        }
        else
        {
            return;
        }
    }

    private void DrawGizmosForChild(Transform child, Color gizmoColor)
    {
        if (child != null)
        {
            Gizmos.color = gizmoColor;

            foreach (Transform obj in child)
            {
                Gizmos.DrawSphere(obj.position, 0.2f); 
            }
        }
    }
}
