using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TextMeshProUGUI scoreText;  // Reference to the Score UI Text
    private int totalScore = 0;
    public ScoringSystem ScoringSystem;

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

    public void AddScore(int score)
    {
        totalScore += score;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + totalScore.ToString();
        //ScoringSystem.instance.CalculateAccuracyAndPrecision();
        //Debug.Log("Current Score: " + totalScore);


    }
}
