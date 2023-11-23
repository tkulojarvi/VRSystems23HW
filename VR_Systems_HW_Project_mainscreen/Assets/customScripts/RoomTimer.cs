using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
