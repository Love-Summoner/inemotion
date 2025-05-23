using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace NuitrackSDK.NuitrackDemos
{
    public class TextCovers : MonoBehaviour
    {
        public int sensorId;
        [SerializeField] ExceptionsLogger exceptionsLogger;
        [SerializeField] GameObject backgroundQuad;
        [SerializeField] ParticleSystem gestureParticles;
        [SerializeField] AudioSource gestureSound;
        [SerializeField] AudioClip[] gestureSounds;
        [SerializeField] GameObject textPrefab;
        [SerializeField] Canvas canvas;

        private Color targetColor = Color.white;
        private Renderer backgroundRenderer;
        private Boolean endScene = false;

        private float messageInterval = 5f;  // Slower initial interval
        private float minMessageInterval = 0.1f; // Minimum interval to reach (faster speed)
        private float accelerationRate = 0.99f; // Rate of acceleration
        private float timeSinceLastMessage = 0f;
        private bool isDisplayingMessages = true;
        private List<Vector2> screenPositions = new List<Vector2>();

        private string[] errorMessages = {
            "Error 404: Audience not found",
            "Error 500: Internal Server Error",
            "Error 403: Forbidden",
            "Critical Error: System Failure",
            "101: Reality Missing",
            "ERROR !!!!"
        };

        void Start()
        {
            if (exceptionsLogger == null)
                exceptionsLogger = FindObjectOfType<ExceptionsLogger>();

            if (backgroundQuad == null)
                backgroundQuad = GameObject.Find("BackgroundQuad");

            if (backgroundQuad != null)
                backgroundRenderer = backgroundQuad.GetComponent<Renderer>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                endScene = true;
            }

            Users users = NuitrackManager.sensorsData[sensorId].Users;
            StartCoroutine(DisplayErrorMessages());
            StartCoroutine(StartMessageDisplayTimer(8f)); // Stop after 5 seconds

            foreach (UserData user in users)
            {
                if (user != null && user.GestureType != null)
                {
                    nuitrack.GestureType gesture = user.GestureType.Value;
                    HandleGesture(gesture);
                }
            }

            if (backgroundRenderer != null)
            {
                backgroundRenderer.material.color = Color.Lerp(backgroundRenderer.material.color, targetColor, Time.deltaTime * 0.5f);
            }
        }

        void HandleGesture(nuitrack.GestureType gesture)
        {
            switch (gesture)
            {
                case nuitrack.GestureType.GestureWaving:
                    TriggerEffect(new Color(0.5f, 0.5f, 1f), 0);
                    break;
                case nuitrack.GestureType.GestureSwipeLeft:
                    TriggerEffect(new Color(0.5f, 1f, 0.5f), 1);
                    break;
                case nuitrack.GestureType.GestureSwipeRight:
                    TriggerEffect(new Color(1f, 0.5f, 0.5f), 2);
                    break;
                case nuitrack.GestureType.GestureSwipeUp:
                    TriggerEffect(new Color(0.5f, 1f, 1f), 3);
                    break;
                case nuitrack.GestureType.GestureSwipeDown:
                    TriggerEffect(new Color(1f, 1f, 0.5f), 4);
                    break;
                case nuitrack.GestureType.GesturePush:
                    TriggerEffect(new Color(1f, 0.5f, 1f), 5);
                    break;
                default:
                    ResetBackgroundColor();
                    break;
            }
        }

        void TriggerEffect(Color color, int soundIndex)
        {
            targetColor = color;
            if (gestureParticles != null)
            {
                gestureParticles.Play();
            }
            if (gestureSound != null && gestureSounds.Length > soundIndex)
            {
                gestureSound.PlayOneShot(gestureSounds[soundIndex]);
            }
        }

        void ResetBackgroundColor()
        {
            targetColor = Color.white;
        }

        void DisplayErrorText(string text)
        {
            if (canvas != null && textPrefab != null)
            {
                GameObject textObj = Instantiate(textPrefab, canvas.transform);
                Text uiText = textObj.GetComponent<Text>();
                if (uiText != null)
                {
                    uiText.text = text;
                    uiText.fontSize = 50;
                    uiText.color = Color.red;
                    uiText.alignment = TextAnchor.MiddleCenter;

                    RectTransform rectTransform = textObj.GetComponent<RectTransform>();
                    Vector2 newPosition = new Vector2(UnityEngine.Random.Range(-Screen.width / 2, Screen.width / 2), UnityEngine.Random.Range(-Screen.height / 2, Screen.height / 2));
                    rectTransform.anchoredPosition = newPosition;
                    screenPositions.Add(newPosition);
                    rectTransform.SetParent(canvas.transform, false);
                }
            }
        }

        IEnumerator DisplayErrorMessages()
        {
            while (!endScene)
            {
                if (isDisplayingMessages)
                {
                    timeSinceLastMessage += Time.deltaTime;

                    if (timeSinceLastMessage >= messageInterval)
                    {
                        string randomMessage = errorMessages[UnityEngine.Random.Range(0, errorMessages.Length)];
                        DisplayErrorText(randomMessage);
                        timeSinceLastMessage = 0f;

                        if (messageInterval > minMessageInterval)
                        {
                            messageInterval *= accelerationRate;
                        }
                    }
                }
                yield return null;
            }
        }
        IEnumerator StartMessageDisplayTimer(float duration)
        {
            yield return new WaitForSeconds(duration);
            isDisplayingMessages = false; // Stop displaying messages
        }
    }
}
