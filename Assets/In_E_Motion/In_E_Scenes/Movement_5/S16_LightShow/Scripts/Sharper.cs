using UnityEngine;
using System;
using UnityEngine.UI;
using nuitrack;

namespace NuitrackSDK.NuitrackDemos
{
    public class Sharper : MonoBehaviour
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
            Users users = NuitrackManager.sensorsData[sensorId].Users;


            foreach (UserData user in users)
            {
                if (user != null && user.GestureType != null)
                {
                    nuitrack.GestureType gesture = user.GestureType.Value;
                    string newEntry = "Dancer " + user.ID + ": " + Enum.GetName(typeof(nuitrack.GestureType), (int)gesture);
                    DisplayGestureText(newEntry);
                    HandleGesture(gesture);
                }
            }

            // Sharp and fast color transition
            if (backgroundRenderer != null)
            {
                backgroundRenderer.material.color = Color.Lerp(backgroundRenderer.material.color, targetColor, Time.deltaTime * 5f);
            }
        }


        void HandleGesture(nuitrack.GestureType gesture)
        {
            switch (gesture)
            {
                case nuitrack.GestureType.GestureWaving:
                    TriggerEffect(new Color(0.5f, 0.5f, 1f), 0); // Soft Blue
                    break;
                case nuitrack.GestureType.GestureSwipeLeft:
                    TriggerEffect(new Color(0.5f, 1f, 0.5f), 1); // Soft Green
                    break;
                case nuitrack.GestureType.GestureSwipeRight:
                    TriggerEffect(new Color(1f, 0.5f, 0.5f), 2); // Soft Red
                    break;
                case nuitrack.GestureType.GestureSwipeUp:
                    TriggerEffect(new Color(0.5f, 1f, 1f), 3); // Soft Cyan
                    break;
                case nuitrack.GestureType.GestureSwipeDown:
                    TriggerEffect(new Color(1f, 1f, 0.5f), 4); // Soft Yellow
                    break;
                case nuitrack.GestureType.GesturePush:
                    TriggerEffect(new Color(1f, 0.5f, 1f), 5); // Soft Magenta
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

        void DisplayGestureText(string text)
        {
            if (canvas != null && textPrefab != null)
            {
                GameObject textObj = Instantiate(textPrefab, canvas.transform);
                Text uiText = textObj.GetComponent<Text>();
                if (uiText != null)
                {
                    uiText.text = text;
                    uiText.fontSize = 50;
                    uiText.color = Color.white;
                    uiText.alignment = TextAnchor.MiddleCenter;
                    RectTransform rectTransform = textObj.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = new Vector2(UnityEngine.Random.Range(-Screen.width / 2, Screen.width / 2), UnityEngine.Random.Range(-Screen.height / 2, Screen.height / 2));
                    rectTransform.SetParent(canvas.transform, false);
                    Destroy(textObj, 4f);
                }
            }
        }
    }
}
