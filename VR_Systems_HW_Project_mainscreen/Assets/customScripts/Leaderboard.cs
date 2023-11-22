using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

/*
 * Leaderboard
 * 
 * Overview:
 * This script manages a leaderboard, recording and displaying the times
 * spent in various scenes. The script is designed as a Singleton to persist across scene changes,
 * and it subscribes to the RoomTimer script's TimerUpdate event to capture the time for each scene.
 * The leaderboard data is stored persistently using PlayerPrefs, and the script includes functions to enable/disable
 * certain mesh renderers for UI elements on the leaderboard screen.
 * 
 * Components:
 * - timerValues: Array storing the times for each scene.
 * - leaderboardText: TextMeshProUGUI component for displaying the leaderboard on the UI.
 * - screen, text, edge, edge2: GameObjects representing UI elements on the leaderboard screen.
 * - meshRenderer: MeshRenderer component used to enable/disable visibility of UI elements.
 * 
 * Functions:
 * - Awake(): Ensures there is only one instance of the Leaderboard script and persists it across scene changes.
 * - Start(): Initializes the script by loading leaderboard data and updating the UI.
 * - OnEnable(), OnDisable(): Subscribes and unsubscribes from the TimerUpdate event to handle completion times.
 * - HandleTimerUpdate(float time): Updates the time for the current scene in the leaderboard array.
 * - UpdateLeaderboardText(): Updates the UI text with the current leaderboard data.
 * - FormatTime(float timeInSeconds): Formats time from seconds to a readable string (MM:SS).
 * - MeshDisabler(), MeshEnabler(): Enable or disable certain mesh renderers to show/hide UI elements on the leaderboard screen.
 * - Save(): Saves the leaderboard data to a persistent file.
 * - Load(): Loads leaderboard data from a persistent file and updates the timerValues array.
 * - OnDestroy(): Saves leaderboard data when the script is destroyed or the application exits.
 * 
 */

public class Leaderboard : MonoBehaviour
{
    public static Leaderboard Instance; // Singleton instance
    string filePath;

    public float[] timerValues = new float[9];
    int sceneIndex;
    public TMP_Text leaderboardText;

    public GameObject screen;
    public GameObject text;
    public GameObject edge;
    public GameObject edge2;
    MeshRenderer meshRenderer;

    void Awake()
    {
        // Ensure there is only one instance of the Leaderboard script
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scene changes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Load();
        UpdateLeaderboardText();
    }

    void OnEnable()
    {
        // Subscribe to the TimerUpdate event
        RoomTimer.OnTimerUpdate += HandleTimerUpdate;
    }

    void OnDisable()
    {
        // Unsubscribe from the TimerUpdate event to prevent memory leaks
        RoomTimer.OnTimerUpdate -= HandleTimerUpdate;
    }

    void HandleTimerUpdate(float time)
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Save the timer value in the leaderboard array
        timerValues[sceneIndex] = time;
        
        // Update the leaderboard text
        UpdateLeaderboardText();
    }

    void UpdateLeaderboardText()
    {
        string leaderboardString = "Leaderboard\n";

        for (int i = 1; i < timerValues.Length; i++)
        {
            int sceneNumber = i;
            leaderboardString += "Scene " + sceneNumber + ": " + FormatTime(timerValues[i]) + "\n";
        }

        leaderboardText.text = leaderboardString;
    }

    string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void MeshDisabler()
    {
        meshRenderer = screen.GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;

        meshRenderer = text.GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;

        meshRenderer = edge.GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;

        meshRenderer = edge2.GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }

    public void MeshEnabler()
    {
        meshRenderer = screen.GetComponent<MeshRenderer>();
        meshRenderer.enabled = true;

        meshRenderer = text.GetComponent<MeshRenderer>();
        meshRenderer.enabled = true;

        meshRenderer = edge.GetComponent<MeshRenderer>();
        meshRenderer.enabled = true;

        meshRenderer = edge2.GetComponent<MeshRenderer>();
        meshRenderer.enabled = true;
    }

    void Save()
    {
        // Specify the file path where you want to save the leaderboard data
        string filePath = Application.persistentDataPath + "/leaderboard.txt";

        // Open a StreamWriter to write data to the file
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // Write the timer values to the file
            for (int i = 1; i < timerValues.Length; i++)
            {
                writer.WriteLine(FormatTime(timerValues[i]));
            }
        }

        Debug.Log("Leaderboard data saved to: " + filePath);
    }

    void Load()
    {
        // Specify the file path from which to load the leaderboard data
        string filePath = Application.persistentDataPath + "/leaderboard.txt";

        // Check if the file exists
        if (File.Exists(filePath))
        {
            // Open a StreamReader to read data from the file
            using (StreamReader reader = new StreamReader(filePath))
            {
                // Read each line from the file and update the timerValues array
                for (int i = 1; i < timerValues.Length; i++)
                {
                    string line = reader.ReadLine();
                    
                    if (!string.IsNullOrEmpty(line))
                    {
                        // Parse the time value from the line and update the timerValues array
                        string[] parts = line.Split(':');

                        int minutes = int.Parse(parts[0]);
                        int seconds = int.Parse(parts[1]);
                        timerValues[i] = minutes * 60 + seconds;
                    }
                }
            }

            Debug.Log("Leaderboard data loaded from: " + filePath);
        }
        else
        {
            Debug.LogWarning("Leaderboard file not found at: " + filePath);
        }
    }

    void OnDestroy()
    {
        Save();
    }
}
