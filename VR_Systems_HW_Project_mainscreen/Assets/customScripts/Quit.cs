using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class Quit : MonoBehaviour
{

    // XR Input
    private InputData _inputData;

    void Start()
    {
        // Get a reference to the InputData script
        _inputData = GetComponent<InputData>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            
            if(currentScene.name == "MainScene")
            {
                // Save the timer values to a file
                Leaderboard.Instance.Save();

                // Call the quit method to exit the game
                QuitGame();
            }
        }

        if(_inputData._rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.menuButton, out bool menuButton) && menuButton)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            
            if(currentScene.name == "MainScene")
            {
                // Save the timer values to a file
                Leaderboard.Instance.Save();

                // Call the quit method to exit the game
                QuitGame();
            }
        }
    }

    // Function to quit the game
    void QuitGame()
    {
        // This will only work in a standalone build, not in the Unity Editor
        
        #if UNITY_STANDALONE
            Application.Quit();
        #endif
        
    }
}
