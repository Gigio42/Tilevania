using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPA_ScenePersist : MonoBehaviour
{

    void Awake()
    {
        int numScenePersists = FindObjectsOfType<SPA_ScenePersist>().Length;
        if (numScenePersists > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ResetScenePersist()
    {
        Destroy(gameObject);
    }
}
