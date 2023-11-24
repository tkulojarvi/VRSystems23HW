using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;


public class RoomSwitch : MonoBehaviour
{
    // Singleton pattern
    public static RoomSwitch Instance;
    // XR Input
    private InputData _inputData;
    // Check if two minutes have passed - it is the minimum time the player is required to stay in the room
    public bool twoMinutes = false;

    // Variables for fade effect
    public Material fadeImage;
    public float fadeSpeed = 1.5f;

    private bool inLoadingScreen = false;

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

        // Set the initial color of the fade image
        fadeImage.color = new Color(0f, 0f, 0f, 255f);

        // Start the fade-in effect when the game starts
        StartCoroutine(StartingFade());
    }

    void Update()
    {
        // Get the currently active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Check for XR input to exit the room
        if(_inputData._rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out bool primaryButtonRight) && primaryButtonRight)
        {
            // add two minutes check here if required
            if(currentScene.name != "TAPE_CUSTOM" && inLoadingScreen == false)
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
        StartCoroutine(FadeOut(tapeNumber));
    }

    public void ExitRoom()
    {
        // Get the currently active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Check if the name of the active scene is not "MainScene"
        if (currentScene.name != "MainScene" && !inLoadingScreen)
        {
            StartCoroutine(FadeIn("MainScene"));
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
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator FadeOut(string sceneName)
    {
        // Set the loading screen flag
        inLoadingScreen = true;

        float alpha = 0f;

        // Fade out
        while (alpha <= 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return new WaitForEndOfFrame();
        }

        // Load the new scene
        SceneManager.LoadScene(sceneName);

        // Disable leaderboard mesh and monitor render
        Leaderboard.Instance.MeshDisabler();
        MonitorRender.Instance.disableMesh();

        // Disable background audio
        MusicManager.Instance.toggleBackgroundAudioOff();

        alpha = 2f;

        // Fade in
        while (alpha >= 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return new WaitForEndOfFrame();
        }

        // Reset the loading screen flag
        inLoadingScreen = false;
    }

    IEnumerator FadeIn(string sceneName)
    {
        // Set the loading screen flag
        inLoadingScreen = true;

        float alpha = 0f;

        // Fade out
        while (alpha <= 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return new WaitForEndOfFrame();
        }

        // Load the new scene
        SceneManager.LoadScene(sceneName);

        // Enable leaderboard mesh and monitor render
        Leaderboard.Instance.MeshEnabler();
        MonitorRender.Instance.enableMesh();

        // Enable background audio
        MusicManager.Instance.toggleBackgroundAudioOn();

        // Reset the two minutes variable
        twoMinutes = false;

        // Set the texture on the monitor
        MonitorRender.Instance.ActivateScreen1();

        alpha = 2f;

        // Fade in
        while (alpha >= 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return new WaitForEndOfFrame();
        }

        // Reset the loading screen flag
        inLoadingScreen = false;
    }
}
