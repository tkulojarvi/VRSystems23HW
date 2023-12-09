using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class Quit : MonoBehaviour
{
    public static Quit Instance;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    // Function to quit the game
    public void QuitGame()
    {
        // This will only work in a standalone build, not in the Unity Editor
        
        #if UNITY_STANDALONE
            Application.Quit();
        #endif
        
    }
}
