using extOSC;
using UnityEngine;
using UnityEngine.UI;

public class MoveUI : MonoBehaviour
{
    public RectTransform squarePrefab; // Assign the UI Image prefab
    public Transform parentCanvas; // Assign the UI Canvas as parent
    public float moveSpeed = 5f; // Speed for movement
    public bool moveLeft = true; // Toggle for movement direction

    #region Public Vars

    public string Address = "/ineMotion/effect";

    [Header("OSC Settings")]
    public OSCReceiver Receiver;

    #endregion

    #region Unity Methods

    protected virtual void Start()
    {
        Receiver.Bind(Address, ReceivedMessage);
    }

    #endregion

    private void ReceivedMessage(OSCMessage message)
    {
        Debug.LogFormat("Received: {0}", message);
        SpawnNewSquare();
    }

    void SpawnNewSquare()
    {
        // Instantiate a new UI object
        RectTransform newSquare = Instantiate(squarePrefab, parentCanvas);

        // Set a random start Y position
        float randomY = Random.Range(-Screen.height / 2 + 50f, Screen.height / 2 - 10f);

        // Decide the start and target positions based on direction
        float startX = moveLeft ? Screen.width / 2 + 100f : -Screen.width / 2 - 100f;
        float targetX = moveLeft ? -Screen.width / 2 - 100f : Screen.width / 2 + 100f;

        newSquare.anchoredPosition = new Vector2(startX, randomY);

        // Start moving in the chosen direction
        StartCoroutine(MoveSquare(newSquare, targetX));
    }

    System.Collections.IEnumerator MoveSquare(RectTransform square, float targetX)
    {
        Vector2 targetPosition = new Vector2(targetX, square.anchoredPosition.y);

        while (Vector2.Distance(square.anchoredPosition, targetPosition) > 1f)
        {
            square.anchoredPosition = Vector2.Lerp(square.anchoredPosition, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(square.gameObject); // Destroy the object once it moves off-screen
    }
}
