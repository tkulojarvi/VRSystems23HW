using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;

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
    // XR Input
    private InputData _inputData;
    // Check if two minutes have passed - it is the minimum time the player is required to stay in the room
    public bool twoMinutes = false;

    // Variables for fade effect
    public Image fadeImage;
    public float fadeSpeed = 1.5f;

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

    void Start()
    {
        // Get a reference to the InputData script
        _inputData = GetComponent<InputData>();
        fadeImage.color = new Color(0f, 0f, 0f, 255f);

        StartCoroutine(StartingFade());

        Debug.Log("start");
    }

    void Update()
    {
        // REMEMEBER TO ADD TWOMINUTES CHECK BACK

        Scene currentScene = SceneManager.GetActiveScene();

        if(_inputData._rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out bool primaryButtonRight) && primaryButtonRight)
        {
            if(currentScene.name != "TAPE_CUSTOM")
            {
                ExitRoom();
            }
        }

        // KEYBOARD CONTROLS
        // Spacebar = Exit room
        if (Input.GetKeyDown(KeyCode.Space) && currentScene.name != "TAPE_CUSTOM")
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
        StartCoroutine(FadeAndLoadScene(tapeNumber));

        // disable leaderboard mesh
        StartCoroutine(LeaderboardFadeOut());
    }

    public void ExitRoom()
    {
        // Get the currently active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Check if the name of the active scene is not "MainScene"
        if (currentScene.name != "MainScene")
        {
            StartCoroutine(FadeAndLoadScene("MainScene"));

            // enable leaderboard mesh
            StartCoroutine(LeaderboardFadeIn());

            // Reset variable for next room
            twoMinutes = false;
        }
    }

    IEnumerator FadeAndLoadScene(string sceneName)
    {
        float alpha = 0f;

        // Fade out
        while (alpha <= 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        // Load the new scene
        SceneManager.LoadScene(sceneName);

        // Fade in
        while (alpha >= 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
    }

    IEnumerator StartingFade()
    {
        float alpha = 3f;

        // Fade in
        while (alpha >= 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
    }

    IEnumerator LeaderboardFadeOut()
    {
        float alpha = 0f;
        
        // Fade out
        while (alpha <= 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        // disable leaderboard mesh
        Leaderboard.Instance.MeshDisabler();
    }

    IEnumerator LeaderboardFadeIn()
    {
        float alpha = 0f;

        while (alpha >= 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        // enable leaderboard mesh
        Leaderboard.Instance.MeshEnabler();
    }
}
