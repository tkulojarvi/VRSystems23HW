using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

/*
 * RoomSwitch
 * 
 * Overview:
 * This script facilitates the switching between different rooms. It utilizes a Singleton pattern
 * to ensure there is only one instance of the RoomSwitch script, persists across scene changes, and handles inputs
 * for room switching. It also manages the visibility of UI elements using the Leaderboard script's MeshDisabler and MeshEnabler functions.
 * 
 * Components:
 * - twoMinutes: A boolean flag indicating whether two minutes have passed in the current room.
 * 
 * Functions:
 * - Awake(): Initializes the script as a Singleton, ensuring there is only one instance across scene changes.
 * - Update(): Checks for inputs to handle room switching.
 * - EnterRoom(string tapeNumber): Loads a new scene (room) based on the provided tapeNumber and disables the leaderboard mesh.
 * - ExitRoom(): Exits the current room and returns to the main scene, enabling the leaderboard mesh.
 * 
 */

public class RoomSwitch : MonoBehaviour
{
    // Singleton pattern
    public static RoomSwitch Instance;

    // Check if two minutes have passed - it is the minimum time the player is required to stay in the room
    public bool twoMinutes = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        // KEYBOARD CONTROLS
        // Spacebar = Exit room
        if (Input.GetKeyDown(KeyCode.Space) && twoMinutes == true && currentScene.name != "TAPE_CUSTOM")
        {
            ExitRoom();
        }
        // NMB1 = ROOM 1
        else if(Input.GetKeyDown(KeyCode.Alpha1) && currentScene.name == "MainScene")
        {
           EnterRoom("TAPE_1");
        }
        // NMB2 = ROOM 2
        else if(Input.GetKeyDown(KeyCode.Alpha2) && currentScene.name == "MainScene")
        {
            EnterRoom("TAPE_2");
        }
        // NMB3 = ROOM 3
        else if(Input.GetKeyDown(KeyCode.Alpha3) && currentScene.name == "MainScene")
        {
            EnterRoom("TAPE_3");
        }
        // NMB4 = ROOM 4
        else if(Input.GetKeyDown(KeyCode.Alpha4) && currentScene.name == "MainScene")
        {
            EnterRoom("TAPE_4");
        }
        // NMB5 = ROOM 5
        else if(Input.GetKeyDown(KeyCode.Alpha5) && currentScene.name == "MainScene")
        {
            EnterRoom("TAPE_5");
        }   
        // NMB6 = ROOM 6
        else if(Input.GetKeyDown(KeyCode.Alpha6) && currentScene.name == "MainScene")
        {
            EnterRoom("TAPE_6");
        }
        // NMB7 = ROOM 7
        else if(Input.GetKeyDown(KeyCode.Alpha7) && currentScene.name == "MainScene")
        {
            EnterRoom("TAPE_7");
        }
        // NMB8 = ROOM 8
        else if(Input.GetKeyDown(KeyCode.Alpha8) && currentScene.name == "MainScene")
        {
            EnterRoom("TAPE_8");
        }
        // NMB9 = ROOM CUSTOM
        else if(Input.GetKeyDown(KeyCode.Alpha9) && currentScene.name == "MainScene")
        {
            EnterRoom("TAPE_CUSTOM");
        }
    }

    public void EnterRoom(string tapeNumber)
    {
        // tapeNumber is the name of the scene to load
        SceneManager.LoadScene(tapeNumber);

        // disable leaderboard mesh
        Leaderboard.Instance.MeshDisabler();
    }

    public void ExitRoom()
    {
        // Get the currently active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Check if the name of the active scene is "MainScene"
        if (currentScene.name == "MainScene")
        {
            // code for when MainScene is active
            Debug.Log("The active scene is MainScene.");

            // enable leaderboard mesh
            Leaderboard.Instance.MeshEnabler();
        }

        else
        {
            // exit the room when the eject button is pressed
            SceneManager.LoadScene("MainScene");

            // enable leaderboard mesh
            Leaderboard.Instance.MeshEnabler();

            // Reset variable for next room
            twoMinutes = false;
        }
    }
}
