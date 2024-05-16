using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button button;
    void Start() 
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(PlayScene);
    }

    void PlayScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex+1;
        SceneManager.LoadScene(nextSceneIndex);
    }
}
