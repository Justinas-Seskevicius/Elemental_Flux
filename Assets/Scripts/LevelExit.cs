using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelTransitionWait = 2f;
    [SerializeField] float exitSlowMotionTime = 0.2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            StartCoroutine(ExitLevel());
        } else
        {
            Debug.Log(" Collied with something else than the player!! -> " + collision.gameObject.name);
        }  
    }

    IEnumerator ExitLevel()
    {
        Time.timeScale = exitSlowMotionTime;
        yield return new WaitForSecondsRealtime(levelTransitionWait);
        Time.timeScale = 1f;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

}
