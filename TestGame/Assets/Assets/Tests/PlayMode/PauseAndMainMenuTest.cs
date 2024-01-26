using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenuTests
{
    [Test]
    public void PauseMenu_Pause_Functionality()
    {
        var pauseMenu = new GameObject().AddComponent<PauseMenu>();
        var pauseMenuUI = new GameObject();
        pauseMenu.pauseMenuUI = pauseMenuUI;

        pauseMenu.Pause();

        Assert.IsTrue(PauseMenu.GameIsPaused);
        Assert.IsTrue(pauseMenuUI.activeSelf);
        Assert.AreEqual(Time.timeScale, 0f);
    }

    [Test]
    public void PauseMenu_Resume_Functionality()
    {
        var pauseMenu = new GameObject().AddComponent<PauseMenu>();
        var pauseMenuUI = new GameObject();
        pauseMenu.pauseMenuUI = pauseMenuUI;
        PauseMenu.GameIsPaused = true;
        Time.timeScale = 0f;

        pauseMenu.Resume();

        Assert.IsFalse(PauseMenu.GameIsPaused);
        Assert.IsFalse(pauseMenuUI.activeSelf);
        Assert.AreEqual(Time.timeScale, 1f);
    }


    [UnityTest]
    public IEnumerator PauseMenu_LoadMenu_Functionality()
    {
        SceneManager.LoadScene("Menu");
        yield return null;

        var pauseMenu = new GameObject().AddComponent<PauseMenu>();

        pauseMenu.LoadMenu();

        Assert.IsFalse(PauseMenu.GameIsPaused);
        Assert.AreEqual(Time.timeScale, 1f);
        Assert.AreEqual(SceneManager.GetActiveScene().name, "Menu");
    }

    [Test]
    public void PauseMenu_QuitGame_Functionality()
    {
        var pauseMenu = new PauseMenu();

        pauseMenu.QuitGame();
    }
}
