using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SPA_GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;

    SPA_CoinPickup coin;


    void Awake()
    {
        int numGameSessions = FindObjectsOfType<SPA_GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start() 
    {   
        livesText.text = "Lives: " + playerLives.ToString();
        scoreText.text = "Score: " + score.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = "Score: " + score.ToString();
    }

        void TakeLife()
    {
        playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = "Lives: " + playerLives.ToString();
    }

    void ResetGameSession()
    {
        FindObjectOfType<SPA_ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

}
