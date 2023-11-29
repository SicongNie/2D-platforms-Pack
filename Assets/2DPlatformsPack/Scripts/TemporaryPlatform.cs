using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TemporaryPlatform : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Duration for the platform to disappear")]
    public float disappearDuration;
    private float disappearTimer;
    [SerializeField]
    [Tooltip("Duration for the platform to reappear")]
    public float reappearDuration;
    private float reappearTimer;

    [SerializeField]
    [Tooltip("Interval between blink effects")]
    public float blinkInterval;

    [SerializeField]
    [Tooltip("Time at which the blinking begins")]
    public float beginToBlink;

    [SerializeField]
    public TextMeshPro timerText;
    [SerializeField]
    [Tooltip("Show the timer on the platform")]
    public bool showTimer;

    private bool isDisapearing;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    [SerializeField]
    private bool isBlinking = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        disappearTimer = disappearDuration;
        reappearTimer = reappearDuration;
    }

    private void Update()
    {
        if (!isDisapearing)
        {
            disappearTimer -= Time.deltaTime;
            if (disappearTimer <= 0f)
            {
                spriteRenderer.enabled = false;
                boxCollider.enabled = false;
                disappearTimer = disappearDuration;
                isDisapearing = true;
            }
            if (!isBlinking)
            {
                StartCoroutine(DoBlinkEffect());
            }
            timerText.text = disappearTimer.ToString("F0");
        }
        else
        {
            reappearTimer -= Time.deltaTime;
            if (reappearTimer <= 0f)
            {
                spriteRenderer.enabled = true;
                boxCollider.enabled = true;
                reappearTimer = reappearDuration;
                isDisapearing = false;
            }
            timerText.text = reappearTimer.ToString("F0");
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

    private IEnumerator DoBlinkEffect()
    {
        isBlinking = true;
        while (disappearTimer > 0f && disappearTimer < beginToBlink)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
        isBlinking = false;
    }
}
