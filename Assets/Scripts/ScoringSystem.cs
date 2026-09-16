using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // For scene reload if needed

public class ScoringSystem : MonoBehaviour
{
    public static ScoringSystem instance;
    private List<Vector2> shurikenPositions = new List<Vector2>();
    public List<int> scores = new List<int>();

    public GameManager gameManager; // Reference to GameManager for score updating

    [Header("UI Elements")]
    public GameObject resultPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI accuracyText;
    public TextMeshProUGUI precisionText;
    public TextMeshProUGUI randomFactText;

    [Header("Trophy Images")]
    public Image highHighTrophy;
    public Image lowLowTrophy;
    public Image highLowTrophy;
    public Image lowHighTrophy;

    private int shurikenCount = 5; // Total number of shurikens to be thrown
    private float accuracyThreshold = 2f; // Max distance for high accuracy
    private float precisionThreshold = 1f; // Max spread for high precision

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        resultPanel.SetActive(false); // Hide result panel at the start
        ResetTrophies();  // Make sure trophies are not active by default
        LoadTrophies();   // Load any saved trophies from previous rounds
    }

    public void RegisterShurikenHit(Vector3 position, string tag)
    {
        Vector2 gridPosition = new Vector2(position.x, position.y);
        shurikenPositions.Add(gridPosition);
        int score = 0;

        //Debug.Log("Registering hit for tag: " + tag);

        // Score calculation remains, but accuracy & precision won't use scores
        switch (tag)
        {
            case "Center":
                score = 100;
                break;
            case "Second":
                score = 80;
                break;
            case "Third":
                score = 60;
                break;
            case "Fourth":
                score = 40;
                break;
            case "Finish":
                score = 0; // Black background = miss
                break;
            default:
                //Debug.LogWarning("Unknown tag detected: " + tag);
                break;
        }

        scores.Add(score);
        gameManager.AddScore(score);

        // Increase crosshair movement speed
        CrosshairMovement crosshair = Object.FindFirstObjectByType<CrosshairMovement>();
        if (crosshair != null)
        {
            crosshair.IncreaseSpeed();
        }

        if (shurikenPositions.Count == shurikenCount)
        {
            StartCoroutine(ShowResults());
        }
    }

    IEnumerator ShowResults()
    {
        yield return new WaitForSeconds(2f);
        CalculateAccuracyAndPrecision();
        resultPanel.SetActive(true);
    }

    public void CalculateAccuracyAndPrecision()
    {
        ResetTrophies();
        if (shurikenPositions.Count == 0)
        {
            Debug.Log("No shurikens thrown.");
            return;
        }

        // === Check if all shurikens missed ===
        int missedShurikenCount = scores.FindAll(score => score == 0).Count;
        if (missedShurikenCount == shurikenPositions.Count)
        {
            accuracyText.text = "Accuracy: NIL";
            precisionText.text = "Precision: NIL";
            scoreText.text = "Total Score: 0";
            randomFactText.text = "Try again! All shots missed the board.";
            Debug.Log("All shurikens missed. Accuracy & Precision: NIL");
            return;
        }

        // === Accuracy Calculation (All Must Be Within Threshold) ===
        bool allWithinThreshold = true;
        foreach (Vector2 pos in shurikenPositions)
        {
            float distance = Vector2.Distance(pos, Vector2.zero); // Distance from (0,0)
            if (distance > accuracyThreshold)
            {
                allWithinThreshold = false;
                break;
            }
        }
        string accuracyCategory = allWithinThreshold ? "High" : "Low";

        // === Precision Calculation (Spread from Mean Position) ===
        Vector2 meanPosition = Vector2.zero;
        foreach (Vector2 pos in shurikenPositions)
        {
            meanPosition += pos;
        }
        meanPosition /= shurikenPositions.Count;

        float spread = 0;
        foreach (Vector2 pos in shurikenPositions)
        {
            spread += Vector2.Distance(pos, meanPosition);
        }
        float averageSpread = spread / shurikenPositions.Count;
        string precisionCategory = averageSpread <= precisionThreshold ? "High" : "Low";

        // === Update UI ===
        scoreText.text = "Total Score: " + GetTotalScore();
        accuracyText.text = "Accuracy: " + accuracyCategory;
        precisionText.text = "Precision: " + precisionCategory;
        randomFactText.text = GetRandomFact();

        Debug.Log("Accuracy: " + accuracyCategory);
        Debug.Log("Precision: " + precisionCategory + " (Avg Spread: " + averageSpread + ")");

        // === Save & Load Trophy for this round ===
        string trophyKey = "";
        if (accuracyCategory == "Low" && precisionCategory == "Low")
        {
            trophyKey = "Trophy_Low_Low";
        }
        else if (accuracyCategory == "High" && precisionCategory == "High")
        {
            trophyKey = "Trophy_High_High";
        }
        else if (accuracyCategory == "High" && precisionCategory == "Low")
        {
            trophyKey = "Trophy_High_Low";
        }
        else if (accuracyCategory == "Low" && precisionCategory == "High")
        {
            trophyKey = "Trophy_Low_High";
        }

        if (!string.IsNullOrEmpty(trophyKey) && PlayerPrefs.GetInt(trophyKey, 0) == 0)
        {
            PlayerPrefs.SetInt(trophyKey, 1);
            PlayerPrefs.Save();
            Debug.Log("New trophy earned and saved: " + trophyKey);
        }

        LoadTrophies();
    }


    private void ResetTrophies()
    {
        highHighTrophy.gameObject.SetActive(false);
        lowLowTrophy.gameObject.SetActive(false);
        highLowTrophy.gameObject.SetActive(false);
        lowHighTrophy.gameObject.SetActive(false);
    }

    private void LoadTrophies()
    {
        // Check each trophy save and enable the corresponding trophy if saved.
        if (PlayerPrefs.GetInt("Trophy_High_High", 0) == 1)
        {
            highHighTrophy.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Trophy_Low_Low", 0) == 1)
        {
            lowLowTrophy.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Trophy_High_Low", 0) == 1)
        {
            highLowTrophy.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Trophy_Low_High", 0) == 1)
        {
            lowHighTrophy.gameObject.SetActive(true);
        }
    }
     
    private int GetTotalScore()
    {
        int total = 0;
        foreach (int score in scores)
        {
            total += score;
        }
        return total;
    }
    
    private string GetRandomFact()
    {
        string[] facts = {
            "Did you know? High accuracy means hitting the target consistently!",
            "Fun Fact: Precision is all about consistency, even if the shots aren't on target.",
            "High precision means your throws are grouped closely, even if not in the center.",
            "Accuracy can be high even with low precision if the average is near the bullseye!",
            "In archery, high precision is valued more than high accuracy for consistency."
        };

        int randomIndex = Random.Range(0, facts.Length);
        return facts[randomIndex];
    }

    // Call this method from your Quit button to clear saved trophies.
    public void ClearSaves()
    {
        PlayerPrefs.DeleteKey("Trophy_High_High");
        PlayerPrefs.DeleteKey("Trophy_Low_Low");
        PlayerPrefs.DeleteKey("Trophy_High_Low");
        PlayerPrefs.DeleteKey("Trophy_Low_High");
        PlayerPrefs.Save();
        Debug.Log("Trophy saves cleared.");

        // Optionally, you can reload the scene to start fresh:
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
