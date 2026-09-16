using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShurikenThrow : MonoBehaviour
{
    public GameObject shurikenPrefab;
    public Transform crosshair;
    private int shurikenCount = 5;
    private List<Vector3> shurikenPositions = new List<Vector3>();
    public List<GameObject> shurikenImages; // Assign the shuriken images manually in the Inspector
    public GameObject shurikenPanel; // Assign the shuriken panel manually in the Inspector
    public float panelDelay = 2.0f; // Delay in seconds before enabling the panel

    void Start()
    {
        // Enable all shuriken images at the start of the game
        foreach (GameObject image in shurikenImages)
        {
            image.SetActive(true);
        }
        shurikenPanel.SetActive(false); // Disable the shuriken panel at the start
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && shurikenCount > 0 && !IsPointerOverUI())
        {
            Vector3 throwPosition = crosshair.position;
            Instantiate(shurikenPrefab, throwPosition, Quaternion.identity);
            shurikenPositions.Add(throwPosition);
            shurikenCount--;

            // Disable the corresponding shuriken image
            if (shurikenCount < shurikenImages.Count)
            {
                shurikenImages[shurikenCount].SetActive(false);
            }

            OnShurikenHit();

            // Enable the shuriken panel if all shurikens are thrown
            if (shurikenCount == 0)
            {
                StartCoroutine(EnablePanelAfterDelay(panelDelay));
            }
        }
    }

    bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    void OnShurikenHit()
    {
        // Camera shake effect (adjust the parameters as needed)
        CameraShake.Instance.ShakeCamera(0.1f, 0.2f); // Adjust the intensity and duration of the shake
    }

    IEnumerator EnablePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        shurikenPanel.SetActive(true);
    }
}
