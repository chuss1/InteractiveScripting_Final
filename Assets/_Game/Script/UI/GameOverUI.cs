using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

    public void ReturnToMenu() {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }

    public void ReplayLevel() {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
