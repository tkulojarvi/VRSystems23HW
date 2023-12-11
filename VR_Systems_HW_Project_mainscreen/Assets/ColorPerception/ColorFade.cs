using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ColorFade : MonoBehaviour
{
    public static ColorFade Instance; // instance

    public Toggle eyeToggle;
    public GameObject instructions;
    public Image leftPreview;  // preview images (non-VR)
    public Image rightPreview;


    public Camera leftStereoCam;
    public Camera rightStereoCam;
    private Color originalColorL;
    private Color originalColorR;


    public float fadeDuration = 5f;  // Duration for the screen effect in seconds
    public Color screenColorL = Color.black;  // Left side eye color
    public Color screenColorR = Color.black;  // Right side eye color
    
    void Awake()
    {
        // Ensure there is only one instance of the script
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
        originalColorL = leftStereoCam.backgroundColor;
        originalColorR = rightStereoCam.backgroundColor;
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

    void Update()
    {
        Preview();
        
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StartCoroutine(TurnScreenColor());
        }
    }

    IEnumerator TurnScreenColor()
    {
        // Fading in
        float timer = 0f;
        
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;
            leftStereoCam.backgroundColor = Color.Lerp(originalColorL, screenColorL, progress);
            rightStereoCam.backgroundColor = Color.Lerp(originalColorR, screenColorR, progress);
            yield return null;
        }

        yield return new WaitForSeconds(fadeDuration);

        // Fading out
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;
            leftStereoCam.backgroundColor = Color.Lerp(screenColorL, originalColorL, progress);
            rightStereoCam.backgroundColor = Color.Lerp(screenColorR, originalColorR, progress);
            yield return null;
        }

        // Ensure the final color is completely clear
        leftStereoCam.backgroundColor = originalColorL;
        rightStereoCam.backgroundColor = originalColorR;
    }

    void Preview()
    {
        leftPreview.color = screenColorL;
        rightPreview.color = screenColorR;

        if(eyeToggle.isOn == true)
        {
            leftPreview.enabled = true;
            rightPreview.enabled = true;
            instructions.SetActive(true);
        }

        else
        {
            leftPreview.enabled = false;
            rightPreview.enabled = false;
            instructions.SetActive(false);
        }
    }
}
