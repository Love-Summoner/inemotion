using System.Collections;
using UnityEngine;

public class ExpandingSquaresSpawner : MonoBehaviour
{
    public GameObject squarePrefab;
    public float spawnInterval = 5f;

    void Start()
    {
        StartCoroutine(SpawnSquares());
    }

    IEnumerator SpawnSquares()
    {
        while (true)
        {
            SpawnSquare();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnSquare()
    {
        Debug.Log("Spawning a new square!"); // Check if this appears in the console
        GameObject square = new GameObject("ExpandingSquare");
        square.transform.position = new Vector3(0, 0, 10);
        LineRenderer lineRenderer = square.AddComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.positionCount = 5;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        // Define square shape
        float halfHeight = 0.5f;
        float halfWidth = 1f;
        Vector3[] points = new Vector3[]
        {
            new Vector3(-halfWidth, -halfHeight, 0), // Bottom-left
            new Vector3(-halfWidth, halfHeight, 0),  // Top-left
            new Vector3(halfWidth, halfHeight, 0),   // Top-right
            new Vector3(halfWidth, -halfHeight, 0),  // Bottom-right
            new Vector3(-halfWidth, -halfHeight, 0)  // Closing the loop
        };
        lineRenderer.SetPositions(points);

        square.AddComponent<ExpandingSquare>();
    }
}

public class ExpandingSquare : MonoBehaviour
{
    public float growthSpeed = 5f;

    void Start()
    {
        Destroy(gameObject, 5f); // Destroy after 5 seconds
    }

    void Update()
    {
        transform.localScale += Vector3.one * growthSpeed * Time.deltaTime;
    }
}
