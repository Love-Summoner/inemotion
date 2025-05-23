using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjSpawner : MonoBehaviour
{
    public GameObject fallingObjectPrefab;
    public float spawnDelay = 2f;
    public float spawnHeight = 10f;
    public Vector2 spawnXRange = new Vector2(-50f, 50);
    public float fallSpeed = 20f;
    public bool isSpawning = false; // track if currently spawning

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("Spawner Start called");

        // start if not running
        if (!isSpawning)
        {
            isSpawning = true;
            Debug.Log("Starting Coroutine");
            StartCoroutine(SpawnObjInterval());
        }

    }

    private IEnumerator EnableRigidbodyAfterDelay(Rigidbody rb)
    {
        yield return new WaitForSeconds(0.1f); // Short delay to allow the spawning logic to start
        rb.isKinematic = false;  // Re-enable physics interactions
    }



    IEnumerator SpawnObjInterval()
    {
        while (isSpawning)
        {
            SpawnFallingObject();
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void SpawnFallingObject()
    {
        // random x val w/in range
        float randX = Random.Range(spawnXRange.x, spawnXRange.y);

        // set spawn pos above camera
        Vector3 spawnPosition = new Vector3(randX, spawnHeight, -2f);

        // inst falling obj at spawn pos
        GameObject fallingObj = Instantiate(fallingObjectPrefab, spawnPosition, Quaternion.identity);
        fallingObj.transform.localScale = Vector3.one * 1; //change the size to see easier

        // debug log
        Debug.Log($"Spawned Cube at: {fallingObj.transform.position}");

        Rigidbody rb = fallingObj.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = fallingObj.AddComponent<Rigidbody>();
        }

        rb.useGravity = true;
        rb.mass = 1f;
        
        Collider col = fallingObj.GetComponent<Collider>();
        if (col == null)
        {
            col = fallingObj.AddComponent<BoxCollider>();
        }
        
        PhysicMaterial bouncyMat = new PhysicMaterial();
        bouncyMat.bounciness = 0.8f;
        bouncyMat.frictionCombine = PhysicMaterialCombine.Minimum;
        bouncyMat.bounceCombine = PhysicMaterialCombine.Maximum;
        
        col.material = bouncyMat;

        // beyblade mode
        rb.angularVelocity = new Vector3(
            Random.Range(-5f, 5f),
            Random.Range(-5f, 5f),
            Random.Range(-5f, 5f)
        );

        // destroy after time
        Destroy(fallingObj, 7f);

    }

    // Update is called once per frame
    void Update()
    {
        if (spawnDelay > .2f)
        {
            spawnDelay = spawnDelay * .9999f;
        }
    }
}
