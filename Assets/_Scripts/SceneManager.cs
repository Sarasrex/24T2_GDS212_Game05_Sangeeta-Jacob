using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{

    public GameObject playButton;

    private void Awake()
    {
        Time.timeScale = 1.0f;
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void Play ()
    {
        SceneManager.LoadSceneAsync(1);
    }

}
