using UnityEngine;
using System;
using UnityEngine.UI;
using nuitrack;

namespace NuitrackSDK.NuitrackDemos
{
    public class S16_GesturesBehavior : MonoBehaviour
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
                    exceptionsLogger.AddEntry(newEntry);
          
                    //DisplayGestureText(newEntry);
                    HandleGesture(gesture);
                }
            }

            // Smoothly transition background color
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
    }
}
