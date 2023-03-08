using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SPA_LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 3f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        FindObjectOfType<SPA_ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(nextSceneIndex);
    }
}
