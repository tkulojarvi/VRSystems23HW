using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public static class RoomSwitch
{

    // This class is used to handle movement between rooms/scenes.
    // It is static
    // 

    public static void EnterRoom(string tapeNumber)
    {
        // tapeNumber is the name of the scene to load

        // implement wait time here  

        SceneManager.LoadScene(tapeNumber);
    }

    public static void ExitRoom()
    {
        // Get the currently active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Check if the name of the active scene is "MainScene"
        if (currentScene.name == "MainScene")
        {
            // code for when MainScene is active
            Debug.Log("The active scene is MainScene.");
            
        }
        else
        {
            // exit the room when the eject button is pressed
            SceneManager.LoadScene("MainScene");
        }
    }
}
