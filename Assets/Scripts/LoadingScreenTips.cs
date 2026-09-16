using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreenManager : MonoBehaviour
{
    public CanvasGroup loadingScreenGroup;  // Attach the CanvasGroup of LoadingScreen
    public RectTransform rotatingImage;     // The image that rotates on Y axis
    public TextMeshProUGUI loadingText;     // Blinking "Loading..." text
    public TextMeshProUGUI factText;        // Accuracy/precision fact text

    public float rotationSpeed = 100f;

    private string[] facts = {
        "High accuracy means hitting near the center!",
        "Precision = how consistent your throws are.",
        "Accuracy ≠ Precision — both matter!",
        "Grouped hits = high precision.",
        "Bullseye every time = high accuracy + high precision!"
    };

    void Start()
    {
        StartCoroutine(LoadingRoutine());
    }

    IEnumerator LoadingRoutine()
    {
        // Block interaction while loading
        loadingScreenGroup.blocksRaycasts = true;
        loadingScreenGroup.interactable = false;

        // Pick a random fact and display
        int randomFactIndex = Random.Range(0, facts.Length);
        factText.text = facts[randomFactIndex];

        // Start blinking loading text
        StartCoroutine(BlinkLoadingText());

        // Start rotating image
        StartCoroutine(RotateImage());

        // Wait for random duration before hiding loading screen
        float randomTime = Random.Range(3f, 7f);
        yield return new WaitForSeconds(randomTime);

        // Hide the loading screen
        loadingScreenGroup.alpha = 0f;
        loadingScreenGroup.blocksRaycasts = false;
        loadingScreenGroup.interactable = false;

        // Optional: Deactivate GameObject if you want
        gameObject.SetActive(false);
    }

    IEnumerator BlinkLoadingText()
    {
        while (true)
        {
            loadingText.alpha = 1f;
            yield return new WaitForSeconds(0.5f);
            loadingText.alpha = 0f;
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator RotateImage()
    {
        while (true)
        {
            if (rotatingImage != null)
            {
                rotatingImage.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
            }
            yield return null;
        }
    }
}
