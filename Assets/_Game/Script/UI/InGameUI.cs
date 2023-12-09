using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    private GameObject pauseMenu => transform.GetChild(1).gameObject;

    private bool isGamePaused;

    private void Start()
    {
        Time.timeScale = 1.0f;
        pauseMenu.SetActive(false);
        isGamePaused = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            isGamePaused = !isGamePaused;

            if(isGamePaused)
            {
                PauseGame();
            }
            else
            {
                UnpauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void UnpauseGame()
    {
        isGamePaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
