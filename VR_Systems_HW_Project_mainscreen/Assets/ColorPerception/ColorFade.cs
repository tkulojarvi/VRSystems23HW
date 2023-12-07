using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ColorFade : MonoBehaviour
{
    public static ColorFade Instance; // instance

    public Image colorScreenImage;  // Drag and drop your UI Image here
    public float fadeDuration = 5f;  // Duration for the screen effect in seconds
    Color screenColor;

    void Start()
    {
        colorScreenImage.color = Color.clear;  // Make sure the image is initially transparent
    }

    public void EyeAdaptToColor(Color screenColor)
    {
        StartCoroutine(TurnScreenColor(screenColor));
    }

    IEnumerator TurnScreenColor(Color screenColor)
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
}
