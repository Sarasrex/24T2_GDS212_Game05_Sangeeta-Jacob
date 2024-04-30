using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MySceneManager : MonoBehaviour
{
    [SerializeField] private Button continueGameButton;

    private void Start()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            continueGameButton.interactable = false;
        }
    }

    public void OnNewGameClicked()
    {
        DataPersistenceManager.NewGame();
        SceneManager.LoadSceneAsync("JacobTestScene");
    }

    public void OnContinueGameClicked()
    {
        SceneManager.LoadSceneAsync("JacobTestScene");
    }
}
