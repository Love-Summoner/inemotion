using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class RandomVideoSpawner : MonoBehaviour
{
    public GameObject videoPrefab;  // Assign the VideoPrefab here
    public Canvas canvas;           // Reference to the canvas to spawn UI elements on

    private List<VideoClip> videoClips;

    void Start()
    {
        // Load all VideoClip assets from Resources/MyVideos folder
        videoClips = new List<VideoClip>(Resources.LoadAll<VideoClip>("PerformerVideos"));

        // Start spawning random videos at intervals
        StartCoroutine(SpawnRandomVideo());
    }

    IEnumerator SpawnRandomVideo()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 5f));  // Random interval between spawns

            if (videoClips.Count > 0)
            {
                // Pick a random video clip
                VideoClip randomClip = videoClips[Random.Range(0, videoClips.Count)];

                // Instantiate videoPrefab and set up the VideoPlayer
                GameObject videoInstance = Instantiate(videoPrefab, canvas.transform);
                VideoPlayer videoPlayer = videoInstance.GetComponent<VideoPlayer>();
                videoPlayer.clip = randomClip;

                // Set random size and position for RawImage
                RectTransform rectTransform = videoInstance.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(Random.Range(100, 500), Random.Range(100, 500));  // Random size
                rectTransform.anchoredPosition = new Vector2(
                    Random.Range(-canvas.pixelRect.width / 2, canvas.pixelRect.width / 2),
                    Random.Range(-canvas.pixelRect.height / 2, canvas.pixelRect.height / 2)
                );  // Random position within the canvas

                videoPlayer.loopPointReached += OnVideoEnd;

                // Play the video
                videoPlayer.Play();
            }
        }
    }

    void OnVideoEnd(VideoPlayer videoPlayer)
    {
        // Destroy the GameObject when the video ends
        Destroy(videoPlayer.gameObject);
    }
}
