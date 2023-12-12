using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EyeAdaptation : MonoBehaviour
{
    public static EyeAdaptation Instance; // instance

    public Toggle eyeToggle;
    public GameObject instructions;
    public Image leftPreview;  // preview images (non-VR)
    public Image rightPreview;


    public Image LScreenImage;  // Drag and drop your UI Image here
    public Image RScreenImage;  // Drag and drop your UI Image here

    public GameObject LeftCamera;
    public GameObject RightCamera;


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
        LScreenImage.color = Color.clear;  // Make sure the image is initially transparent
        RScreenImage.color = Color.clear;  // Make sure the image is initially transparent
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
            Debug.Log("spacekey");
            StartCoroutine(TurnScreenColor());
        }
    }

    IEnumerator TurnScreenColor()
    {
        LeftCamera.SetActive(true);
        RightCamera.SetActive(true);

        LScreenImage.color = screenColorL;
        RScreenImage.color = screenColorR;

        yield return new WaitForSeconds(fadeDuration);

        // Ensure the final color is completely clear
        LScreenImage.color = Color.clear;
        RScreenImage.color = Color.clear;

        LeftCamera.SetActive(false);
        RightCamera.SetActive(false);

        Debug.Log("complete");
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
