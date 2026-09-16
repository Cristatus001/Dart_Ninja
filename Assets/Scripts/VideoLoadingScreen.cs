using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class VideoLoadingScreen : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Assign in Inspector
    public string videoFileName = "loading.mp4"; // Video file name in StreamingAssets folder
    public GameObject uiElements; // Assign the root UI GameObject here to disable interactions

    private EventSystem eventSystem; // Reference to the EventSystem
    private ShurikenThrow shurikenThrowScript; // Reference to the ShurikenThrow script

    private void Start()
    {
        // Get the EventSystem in the scene
        eventSystem = FindObjectOfType<EventSystem>();

        // Get reference to the ShurikenThrow script
        shurikenThrowScript = FindObjectOfType<ShurikenThrow>();

        if (shurikenThrowScript != null)
        {
            // Disable the shuriken throwing script at the start
            shurikenThrowScript.enabled = false;
        }

        // Construct the path for StreamingAssets (Works in WebGL)
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
        videoPlayer.url = videoPath;
        videoPlayer.Play();

        // Disable interactions with UI and EventSystem
        SetUIInteractions(false);

        // Subscribe to the Video End Event
        videoPlayer.loopPointReached += OnVideoEnd;

    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // Hide the video player and its GameObject
        videoPlayer.gameObject.SetActive(false);

        // Enable interactions with UI
        SetUIInteractions(true);

        // Enable the shuriken throwing script
        if (shurikenThrowScript != null)
        {
            shurikenThrowScript.enabled = true;
        }

    }

    // Function to enable or disable UI interactions and EventSystem
    private void SetUIInteractions(bool enable)
    {
        // Disable/Enable all UI canvases and event system
        Canvas[] canvases = uiElements.GetComponentsInChildren<Canvas>(true);
        foreach (var canvas in canvases)
        {
            canvas.enabled = enable;
        }

        // Disable/Enable the EventSystem (this blocks all input events like clicks)
        if (eventSystem != null)
        {
            eventSystem.enabled = enable;
        }
    }

    /*IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // Wait until the scene is fully loaded
            if (operation.progress >= 0.9f)
            {
                // Allow activation only when the video is done
                if (!videoPlayer.isPlaying)
                {
                    operation.allowSceneActivation = true;
                }
            }
            yield return null;
        }*//*
    }*/
}
