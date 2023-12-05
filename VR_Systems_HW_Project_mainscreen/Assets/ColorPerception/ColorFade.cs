using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ColorFade : MonoBehaviour
{
    public Image colorScreenImage;  // Drag and drop your UI Image here
    public float fadeDuration = 5f;  // Duration for the screen effect in seconds

    Color screenColor;

    private string r;
    private string g;
    private string b;
    private string rgbString;
    private bool RGBInputSuccessful = false;

    void Start()
    {
        colorScreenImage.color = Color.clear;  // Make sure the image is initially transparent
    }

    void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            // Start the process to get RGB values from the user
            StartCoroutine(GetRGBInput());
        }

        if(RGBInputSuccessful == true)
        {
            RGBInputSuccessful = false;
            screenColor = StringToColor(rgbString);
            StartCoroutine(TurnScreenColor());
        }
    }

    void OnEnable()
    {
        // Called when the script is enabled
        // Enable the InputSystem controls
        InputSystem.EnableDevice(Keyboard.current);
    }

    void OnDisable()
    {
        // Called when the script is disabled
        // Disable the InputSystem controls
        InputSystem.DisableDevice(Keyboard.current);
    }

    IEnumerator TurnScreenColor()
    {
        // Fading in
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;
            colorScreenImage.color = Color.Lerp(Color.clear, screenColor, progress);
            yield return null;
        }

        yield return new WaitForSeconds(fadeDuration);

        // Fading out
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;
            colorScreenImage.color = Color.Lerp(screenColor, Color.clear, progress);
            yield return null;
        }

        // Ensure the final color is completely clear
        colorScreenImage.color = Color.clear;
    }

    private System.Collections.IEnumerator GetRGBInput()
    {
        // Prompt the user to input three different 3-digit numbers representing RGB values
        Debug.Log("Please enter three 3-digit numbers representing RGB values.");

        // Get input for R, G, and B values as strings
        r = "";
        g = "";
        b = "";

        yield return new WaitForSeconds(0.5f); // Delay to ensure the space key is not considered for input

        while (true)
        {
            // Check for numerical key presses (0-9) using Unity's Input System
            if (Keyboard.current.digit0Key.wasPressedThisFrame) r += "0";
            if (Keyboard.current.digit1Key.wasPressedThisFrame) r += "1";
            if (Keyboard.current.digit2Key.wasPressedThisFrame) r += "2";
            if (Keyboard.current.digit3Key.wasPressedThisFrame) r += "3";
            if (Keyboard.current.digit4Key.wasPressedThisFrame) r += "4";
            if (Keyboard.current.digit5Key.wasPressedThisFrame) r += "5";
            if (Keyboard.current.digit6Key.wasPressedThisFrame) r += "6";
            if (Keyboard.current.digit7Key.wasPressedThisFrame) r += "7";
            if (Keyboard.current.digit8Key.wasPressedThisFrame) r += "8";
            if (Keyboard.current.digit9Key.wasPressedThisFrame) r += "9";

            // Check for enter key press to break out of the loop
            if (Keyboard.current.enterKey.wasPressedThisFrame) break;

            yield return null; // Wait for the next frame
        }

        yield return new WaitForSeconds(0.5f); // Delay to ensure the space key is not considered for input

        while (true)
        {
            // Check for numerical key presses (0-9) using Unity's Input System
            if (Keyboard.current.digit0Key.wasPressedThisFrame) g += "0";
            if (Keyboard.current.digit1Key.wasPressedThisFrame) g += "1";
            if (Keyboard.current.digit2Key.wasPressedThisFrame) g += "2";
            if (Keyboard.current.digit3Key.wasPressedThisFrame) g += "3";
            if (Keyboard.current.digit4Key.wasPressedThisFrame) g += "4";
            if (Keyboard.current.digit5Key.wasPressedThisFrame) g += "5";
            if (Keyboard.current.digit6Key.wasPressedThisFrame) g += "6";
            if (Keyboard.current.digit7Key.wasPressedThisFrame) g += "7";
            if (Keyboard.current.digit8Key.wasPressedThisFrame) g += "8";
            if (Keyboard.current.digit9Key.wasPressedThisFrame) g += "9";

            // Check for enter key press to break out of the loop
            if (Keyboard.current.enterKey.wasPressedThisFrame) break;

            yield return null; // Wait for the next frame
        }

        yield return new WaitForSeconds(0.5f); // Delay to ensure the space key is not considered for input

        while (true)
        {
            // Check for numerical key presses (0-9) using Unity's Input System
            if (Keyboard.current.digit0Key.wasPressedThisFrame) b += "0";
            if (Keyboard.current.digit1Key.wasPressedThisFrame) b += "1";
            if (Keyboard.current.digit2Key.wasPressedThisFrame) b += "2";
            if (Keyboard.current.digit3Key.wasPressedThisFrame) b += "3";
            if (Keyboard.current.digit4Key.wasPressedThisFrame) b += "4";
            if (Keyboard.current.digit5Key.wasPressedThisFrame) b += "5";
            if (Keyboard.current.digit6Key.wasPressedThisFrame) b += "6";
            if (Keyboard.current.digit7Key.wasPressedThisFrame) b += "7";
            if (Keyboard.current.digit8Key.wasPressedThisFrame) b += "8";
            if (Keyboard.current.digit9Key.wasPressedThisFrame) b += "9";

            // Check for enter key press to break out of the loop
            if (Keyboard.current.enterKey.wasPressedThisFrame) break;

            yield return null; // Wait for the next frame
        }

        // Use the obtained RGB values
        rgbString = $"{r},{g},{b}";
        Debug.Log($"RGB values entered: {rgbString}");

        RGBInputSuccessful = true;
        
        yield return null;
    }

    Color StringToColor(string color)
    {
        // Check if the color is represented in hexadecimal format
        if (color[0] == '#')
        {
            // Try parsing the color string in HTML format
            Color newCol = Color.white;
            if (ColorUtility.TryParseHtmlString(color, out newCol))
            {
                // Return the parsed color
                return newCol;
            }
        }
        // If not in HTML format, assume RGB format and parse accordingly
        string[] rgb = color.Split(',');
        return new Color(int.Parse(rgb[0]) / 255f, int.Parse(rgb[1]) / 255f, int.Parse(rgb[2]) / 255f);
    }
}
