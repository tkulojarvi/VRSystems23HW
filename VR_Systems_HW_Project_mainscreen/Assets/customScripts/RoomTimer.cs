using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/*
 * RoomTimer
 * 
 * Overview:
 * This script manages the timer functionality for a room. It tracks the elapsed time
 * the player spends in a room, updates the timer text on the UI, and triggers an event when the timer updates.
 * Additionally, it checks whether two minutes have passed and sets a flag in the RoomSwitch script accordingly.
 * 
 * Components:
 * - timerText: TextMeshPro (TMP) text element to display the timer on the UI.
 * - currentTime: The current elapsed time in seconds.
 * - sceneIndex: The build index of the active scene.
 * 
 * Events:
 * - TimerUpdate: Delegate event to notify when the timer updates.
 * 
 * Functions:
 * - Start(): Initializes the script by getting the active scene's build index and the initial timer value.
 * - Update(): Updates the timer based on elapsed time, triggers the TimerUpdate event, and checks for the two-minute threshold.
 * - UpdateTimerText(): Updates the UI timer text with the current elapsed time.
 * 
 */

public class RoomTimer : MonoBehaviour
{
    public TMP_Text timerText;
    private float currentTime;
    private int sceneIndex;

    // Declare an event to notify when the timer updates
    public delegate void TimerUpdate(float time);
    public static event TimerUpdate OnTimerUpdate;
    

    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        currentTime = Leaderboard.Instance.timerValues[sceneIndex];
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= 120f) // 2 minutes
        {
            // when 2 minutes have passed
            RoomSwitch.Instance.twoMinutes = true;
        }

        // Trigger the event when the timer updates
        OnTimerUpdate?.Invoke(currentTime);

        UpdateTimerText();
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        string timerString = string.Format("<mspace=1em>{0:00}:{1:00}</mspace>", minutes, seconds);
        // Adjust the mspace value as needed for your desired spacing

        // Update the UI text element
        timerText.text = timerString;
    }
}
