using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using System.Collections;

[TestFixture]
public class MainMenuTests
{
    [UnityTest]
    public IEnumerator LoadMenuScene()
    {
        // Load the menu scene
        SceneManager.LoadScene("Menu");
        yield return null; // Wait for one frame to ensure the scene is loaded

        // Check if the scene is loaded
        Scene loadedScene = SceneManager.GetActiveScene();
        Assert.AreEqual("Menu", loadedScene.name, "Menu scene is not loaded.");

        // Perform additional checks or assertions specific to the menu scene if needed
    }
}
