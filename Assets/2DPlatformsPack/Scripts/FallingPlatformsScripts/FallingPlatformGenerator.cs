using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformGenerator : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Prefab used for creating blocks")]
    public GameObject blockPrefab;

    [SerializeField]
    [Tooltip("Number of blocks to generate")]
    public int blockCount;

    private float startX;

    /// <summary>
    /// Generates squares (blocks) based on the provided count.
    /// </summary>
    public void GenerateSquares(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject square = Instantiate(blockPrefab, transform);
            square.transform.SetParent(transform);

            float xPos = startX + i * square.transform.localScale.x;
            square.transform.localPosition = new Vector3(xPos, 0f, 0f);
        }
    }

}
