using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // Add this to use UI elements

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject deathScreenUI;
    public float showDeathScreenDelay = 1f;
    public Button pauseMenuButton;  // Add this line

    private void Start()
    {
        if (pauseMenuButton != null)
        {
            pauseMenuButton.onClick.AddListener(TogglePause);  // Add this line
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("SampleScene");
    }

    public void OnEnable()
    {
        GameEvents.OnPlayerDeath.AddListener(ShowDeathScreenDelayed);
    }

    public void OnDisable()
    {
        GameEvents.OnPlayerDeath.RemoveListener(ShowDeathScreenDelayed);
    }

    public void ShowDeathScreenDelayed()
    {
        Invoke("ShowDeathScreen", showDeathScreenDelay);
    }

    public void ShowDeathScreen()
    {
        deathScreenUI.SetActive(true);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
