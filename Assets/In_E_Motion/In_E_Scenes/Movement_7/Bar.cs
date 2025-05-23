using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NuitrackSDK;
using NuitrackSDK.SensorEnvironment;

public class Bar : MonoBehaviour
{
    public int sensorId;
    public GameObject markPrefab;
    private RectTransform rectTransform;

    [SerializeField] nuitrack.JointType rootJoint2 = nuitrack.JointType.Torso;

    // For glitchy flickering on appearance/disappearance
    private Dictionary<int, Coroutine> flickerCoroutines = new Dictionary<int, Coroutine>();
    private float flickerMinInterval = 0.05f;  // Minimum time interval for glitching
    private float flickerMaxInterval = 0.1f;   // Maximum time interval for glitching (slightly quicker flicker)
    private float jitterAmount = 10f;  // Amount of position jitter for glitch effect

    private Dictionary<int, GameObject> userBars = new Dictionary<int, GameObject>(); // Store bars per user
    private Dictionary<int, Vector3> previousPositions = new Dictionary<int, Vector3>(); // To store previous positions for smoothing

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        Users users = NuitrackManager.sensorsData[sensorId].Users;
        HashSet<int> activeUserIDs = new HashSet<int>(); // Track currently detected users

        foreach (UserData user in users)
        {
            if (user != null && user.Skeleton != null)
            {
                int userId = user.ID;
                activeUserIDs.Add(userId);

                // If this user doesn't already have a bar, create one
                if (!userBars.ContainsKey(userId))
                {
                    GameObject newMark = Instantiate(markPrefab, rectTransform);
                    userBars[userId] = newMark;
                    previousPositions[userId] = newMark.transform.position; // Initialize the previous position
                    StartFlickerEffect(newMark, userId, true); // Start the flicker effect when the bar appears
                }

                // Update the position of the user's mark
                ProcessSkeleton(user.Skeleton, userBars[userId], userId);
            }
        }

        // Remove bars for users who are no longer tracked
        List<int> usersToRemove = new List<int>();
        foreach (int userId in userBars.Keys)
        {
            if (!activeUserIDs.Contains(userId))
            {
                // Start the flicker effect when the bar disappears
                StartFlickerEffect(userBars[userId], userId, false);
                Destroy(userBars[userId], 0.5f); // Reduce the delay for destruction so the flicker effect is quicker
                usersToRemove.Add(userId);
            }
        }

        // Remove from dictionary after the flicker is done
        foreach (int userId in usersToRemove)
        {
            userBars.Remove(userId);
            previousPositions.Remove(userId);
        }
    }

    void ProcessSkeleton(UserData.SkeletonData skeleton, GameObject mark, int userId)
    {
        Vector3 rootPos = Quaternion.Euler(0f, 180f, 0f) * skeleton.GetJoint(rootJoint2).Position * 700;

        // Smooth the motion using Vector3.Lerp
        Vector3 smoothedPosition = Vector3.Lerp(previousPositions[userId], rootPos, Time.deltaTime * 10f);  // 10f is a smoothing factor
        MoveMark(mark, smoothedPosition);
        previousPositions[userId] = smoothedPosition; // Store the new position
    }

    private void MoveMark(GameObject mark, Vector3 pos)
    {
        if (mark != null)
        {
            RectTransform markRect = mark.GetComponent<RectTransform>();
            markRect.anchoredPosition = pos;
        }
    }

    // Start the glitchy flicker effect for a particular user mark
    private void StartFlickerEffect(GameObject mark, int userId, bool isAppearing)
    {
        if (!flickerCoroutines.ContainsKey(userId))
        {
            flickerCoroutines[userId] = StartCoroutine(GlitchyFlickerCoroutine(mark, isAppearing));
        }
    }

    // Stop the glitchy flicker effect for a particular user mark
    private void StopFlickerEffect(int userId)
    {
        if (flickerCoroutines.ContainsKey(userId))
        {
            StopCoroutine(flickerCoroutines[userId]);
            flickerCoroutines.Remove(userId);
        }
    }

    // Coroutine to handle the glitchy flicker effect when appearing or disappearing
    private IEnumerator GlitchyFlickerCoroutine(GameObject mark, bool isAppearing)
    {
        CanvasGroup canvasGroup = mark.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = mark.AddComponent<CanvasGroup>(); // Add CanvasGroup if not already present
        }

        // Flickering effect: will glitch during appearance or disappearance
        float duration = 0.5f; // Shorter flicker duration for more aggressive flicker
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Aggressive glitch: Randomly change alpha between 0 and 1 for quick flickering
            float alpha = Mathf.PingPong(Time.time * 3f, 1f); // Faster flicker (3f is the frequency)
            canvasGroup.alpha = alpha;

            // Random jitter effect on position for glitching
            Vector3 randomJitter = new Vector3(
                Random.Range(-jitterAmount, jitterAmount),
                Random.Range(-jitterAmount, jitterAmount),
                0
            );
            mark.transform.localPosition += randomJitter;

            // Random delay between flickers for glitchiness
            float glitchInterval = Random.Range(flickerMinInterval, flickerMaxInterval);
            yield return new WaitForSeconds(glitchInterval);

            elapsedTime += Time.deltaTime;
        }

        // If it's disappearing, fully fade out (optional)
        if (!isAppearing)
        {
            canvasGroup.alpha = 0f;
        }

        // Stop the flicker effect after completion
        if (!isAppearing)
        {
            Destroy(mark);  // Actually destroy the mark after fading out
        }
    }
}
