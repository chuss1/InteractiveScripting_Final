using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTracker : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private float score;

    private void Start()
    {
        float tempScore = PlayerPrefs.GetFloat("HighScore");
        highScoreText.text = tempScore.ToString();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            ResetHighScore();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            SetHighScore();
        }
    }

    public void SetHighScore()
    {
        float tempScore = score;
        float tempHighScore = PlayerPrefs.GetFloat("HighScore");

        if (tempScore > tempHighScore) 
        {
            PlayerPrefs.SetFloat("HighScore", score);
            highScoreText.text = tempScore.ToString();
        }
    }

    public void ResetHighScore()
    {
        PlayerPrefs.SetFloat("HighScore", 0f);
        float tempScore = PlayerPrefs.GetFloat("HighScore");
        highScoreText.text = tempScore.ToString();
    }

    public void AddScore()
    {
        score++;
        scoreText.text = score.ToString();
        SetHighScore();
    }
}
