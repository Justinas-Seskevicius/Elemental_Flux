using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int goldCoins = 0;

    private void Awake()
    {
        int gameSessionObjects = FindObjectsOfType<GameSession>().Length;
        if (gameSessionObjects > 1)
        {
            Destroy(gameObject);
        } else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            ReduceLives();
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentScene);
        } else
        {
            SceneManager.LoadScene(0);
            Destroy(gameObject);
        }
    }

    private void ReduceLives()
    {
        playerLives--;
    }

    public void IncreaseCoinCounter()
    {
        goldCoins++;
    }
}
