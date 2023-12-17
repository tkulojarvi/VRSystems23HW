using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using System;


public class StereoColor : MonoBehaviour
{
    // Declare InputAction variables for primary, secondary, and adjust actions
    public InputAction primary, secondary, adjust;

    // Arrays to store sets of colors
    public Color[] colorSet1 = null;
    public Color[] colorSet2 = null;
    Color[] colorSet;

    // Material
    public Material rightMaterial;
    public Renderer leftTest;
    public Renderer rightTest;

    // Flags for initialization
    static bool init = false;
    bool main = false;

    // InputData class
    public GameObject inputDataObject;
    InputData inputData;

    // Lights for left and right
    public Light rightLight = null;
    public Light leftLight = null;

    // Variables
    int state = 0;
    int matchIndex = 0;
    int playerIndex = 0;
    int correctCount = 0;
    Renderer match, playerSelection;

    // temperature
    float selectTemperature = 10000;

    // Flag
    bool inputHold = false;

    // wall objects
    public GameObject backwallLL;
    public GameObject backwallLR;
    private GameObject backwallRL;
    private GameObject backwallRR;

    private Camera RRenderTextureCamera;
    public RenderTexture RRenderTexture;

    private float adaptationTimerInput = 0f;

    public GameObject objectToFlash;
    public float flashDuration = 0.5f;
    public int flashCount = 2;
    public float timeBetweenFlashes = 0.5f;

    void Start()
    {

        if (init)
            return;
            
        init = true;
        main = true;

        // Get the InputData component
        inputData = inputDataObject.GetComponent<InputData>();

        primary.Enable();
        secondary.Enable();
        adjust.Enable();

        GameObject right = Instantiate(transform.gameObject, null);
        right.SetLayerRecursively(rightTest.gameObject.layer);

        // Change name
        right.name = "Right Environment";
        right.transform.Find("LeftEnvironmentCam").gameObject.name = "RightEnvironmentCam";
        
        // Set right side camera
        RRenderTextureCamera = right.transform.Find("RightEnvironmentCam").gameObject.GetComponent<Camera>();

        // Set the camera's target texture to the specified render texture
        RRenderTextureCamera.targetTexture = RRenderTexture;

        // set culling mask for right
        int currentCullingMask = RRenderTextureCamera.cullingMask;
        int layer7Mask = 1 << 7;
        int invertedLayer7Mask = ~layer7Mask;
        int newCullingMask = currentCullingMask & invertedLayer7Mask;
        RRenderTextureCamera.cullingMask = newCullingMask;

        currentCullingMask = RRenderTextureCamera.cullingMask;
        int layer8Mask = 1 << 8;
        newCullingMask = currentCullingMask | layer8Mask;
        RRenderTextureCamera.cullingMask = newCullingMask;

        // Store references to the newly created right side back walls
        backwallRL = right.transform.Find("Walls/Backwall Left").gameObject;
        backwallRR = right.transform.Find("Walls/Backwall Right").gameObject;

        // Set the shared material for all renderers in the new GameObject to rightMaterial
        foreach(Renderer r in right.transform.GetComponentsInChildren<Renderer>())
        {
            r.sharedMaterial = rightMaterial;
            rightMaterial.SetFloat("_EditorTest", 0);
        }
    }

    private void Update()
    {
        if(!main) return;

        //Adjusting();
    }
/*
    void Adjusting()
    {
        // If state is equal to 0, then assign the value of leftTest to match; otherwise, assign the value of rightTest to match
        match = state == 0 ? leftTest : rightTest;
        // If state is equal to 0, then assign the value of rightTest to playerSelection; otherwise, assign the value of leftTest to playerSelection.
        playerSelection = state == 0 ? rightTest : leftTest;

        // Adjust the selectTemperature based on the x-axis value of the adjust input
        selectTemperature += adjust.ReadValue<Vector2>().x * Time.deltaTime * 3000;
        // Update the color of the playerSelection renderer using the adjusted temperature
        playerSelection.sharedMaterial.color = colorSet2[playerIndex] * TemperatureToColor(selectTemperature);

        // Check if the primary button is pressed
        if (primary.WasPressedThisFrame())
        {
            // Increment playerIndex and matchIndex cyclically
            playerIndex = (playerIndex + 1) % colorSet1.Length;
            matchIndex = (matchIndex + 1) % colorSet1.Length;
            // Update the color of the match renderer
            match.sharedMaterial.color = colorSet1[matchIndex];
        }
    }

    void Matching()
    {
        // Determine the colorSet, match, and playerSelection based on the current state
        colorSet = state == 0 ? colorSet1 : colorSet2;
        match = state == 0 ? leftTest : rightTest;
        playerSelection = state == 0 ? rightTest : leftTest;

        // Check if the primary button on the right controller is pressed
        if (inputData._rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out bool value) && value)
        {
            // Check if the button is not being held
            if (!inputHold)
            {
                // Increment playerIndex and update the color of playerSelection
                playerIndex = (playerIndex + 1) % colorSet.Length;
                playerSelection.sharedMaterial.color = colorSet[playerIndex];
                inputHold = true;
            }
        }
        // Check if the secondary button on the right controller is pressed
        else if (inputData._rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out bool value2) && value2)
        {
            // Check if the button is not being held
            if (!inputHold)
            {
                // Check if the current matchIndex is within bounds and matches playerIndex
                if (matchIndex < colorSet.Length && matchIndex == playerIndex)
                {
                    // Increment correctCount
                    correctCount++;
                }
                // Increment matchIndex and update the color of the match renderer
                matchIndex++;
                match.sharedMaterial.color = matchIndex >= colorSet.Length ? Color.white : colorSet[matchIndex];
                inputHold = true;
            }
        }
        else
            // Reset inputHold if no buttons are pressed
            inputHold = false;
    }
*/
   
    
    public void SetWallColorLL(string color)
    {
        if(color != null && CheckValidString(color) == true)
        {
            // Get the Renderer components
            Material backwall_LL_Material = backwallLL.GetComponent<Renderer>().material;
            
            // Set the color of the backwallLeft and backwallRight components
            backwall_LL_Material.color = StringToColor(color);
        }
        else
        {
            DisplayErrorMessage();
        }
    }

    public void SetWallColorLR(string color)
    {
        if(color != null && CheckValidString(color) == true)
        {
            // Get the Renderer components
            Material backwall_LR_Material = backwallLR.GetComponent<Renderer>().material;
            
            // Set the color of the backwallLeft and backwallRight components
            backwall_LR_Material.color = StringToColor(color);
        }
        else
        {
            DisplayErrorMessage();
        }
    }

    public void SetWallColorRL(string color)
    {
        if(color != null && CheckValidString(color) == true)
        {
            // Get the Renderer components
            Material backwall_RL_Material = backwallRL.GetComponent<Renderer>().material;
            
            // Set the color of the backwallLeft and backwallRight components
            backwall_RL_Material.color = StringToColor(color);
        }
        else
        {
            DisplayErrorMessage();
        }
    }

    public void SetWallColorRR(string color)
    {
        if(color != null && CheckValidString(color) == true)
        {
            // Get the Renderer components
            Material backwall_RR_Material = backwallRR.GetComponent<Renderer>().material;
            
            // Set the color of the backwallLeft and backwallRight components
            backwall_RR_Material.color = StringToColor(color);
        }
        else
        {
            DisplayErrorMessage();
        }
    }

    public void SetEyeAdaptationColorLeft(string color)
    {
        if(color != null && CheckValidString(color) == true)
        {
            EyeAdaptation.Instance.screenColorL = StringToColor(color);
        }
        else
        {
            DisplayErrorMessage();
        }
    }

    public void SetEyeAdaptationColorRight(string color)
    {
        if(color != null && CheckValidString(color) == true)
        {
            EyeAdaptation.Instance.screenColorR = StringToColor(color);
        }
        else
        {
            DisplayErrorMessage();
        }
    }

    public void EyeAdaptationTimer(string time)
    {
        // Attempt to parse the string into a float
        if (float.TryParse(time, out adaptationTimerInput))
        {
            EyeAdaptation.Instance.fadeDuration = adaptationTimerInput;
        }
        else
        {
            // Parsing failed, handle the error as needed
            DisplayErrorMessage();
        }
    }
    
    public void SetLightColorLeft(string color)
    {
        if(color != null && CheckValidString(color) == true)
        {
            leftLight.color = StringToColor(color);
        }
        else
        {
            DisplayErrorMessage();
        }
    }

    public void SetLightColorRight(string color)
    {
        if(color != null && CheckValidString(color) == true)
        {
            rightLight.color = StringToColor(color);
        }
        else
        {
            DisplayErrorMessage();
        }
    }

    public void SetColorLeft(string color)
    {
        if(color != null && CheckValidString(color) == true)
        {
            leftTest.sharedMaterial.color = StringToColor(color);
        }
        else
        {
            DisplayErrorMessage();
        }
    }

    public void SetColorRight(string color)
    {
        if(color != null && CheckValidString(color) == true)
        {
            rightTest.sharedMaterial.color = StringToColor(color);
        }

        else
        {
            DisplayErrorMessage();
        }
    }

    // Method to load an image asynchronously
    public void LoadImage(string name)
    {
        StartCoroutine(GetImageAsync(name));
    }

    // Coroutine to download an image asynchronously
    IEnumerator GetImageAsync(string name)
    {
        // Create a UnityWebRequest to get the texture from the specified URL
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(name))
        {
            // Send the request and wait for it to complete
            yield return uwr.SendWebRequest();

            // Check if there was an error in the request
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(uwr.error);
                DisplayErrorMessage();
            }
            else
            {
                // Get the downloaded texture
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                // Set the texture and scale of the left and right renderers
                leftTest.sharedMaterial.SetTexture("_Texture2D", texture);
                leftTest.transform.localScale = new Vector3(0.2f, 1f, 0.2f * texture.height / texture.width);
                rightTest.sharedMaterial.SetTexture("_Texture2D", texture);
                rightTest.transform.localScale = new Vector3(0.2f, 1f, 0.2f * texture.height / texture.width);
                Debug.Log("Loaded image");
            }
        }
    }

    Color TemperatureToColor(float temp)
    {
        // Initialize a black color
        Color color = Color.black;
        // Normalize the temperature
        temp /= 100;

        // Check temperature range
        if (temp <= 66) {
            color.r = 1f;
            color.g = Mathf.Clamp((99.4708025861f * Mathf.Log(temp) - 161.1195681661f)/255f, 0, 1.0f);
            color.b = (138.5177312231f * Mathf.Log(temp - 10) - 305.0447927307f) / 255f;
        }
        else {
            color.r = Mathf.Clamp(329.698727446f * Mathf.Pow(temp - 60, -0.1332047592f) / 255f, 0, 1.0f);
            color.g = Mathf.Clamp(288.1221695283f * Mathf.Pow(temp - 60, -0.0755148492f) / 255f, 0, 1.0f);
            color.b = 1f;
        }
        return color;
    }

    Color StringToColor(string color)
    {
        // OPTION 1: RGB FORMAT
        if(ColorModeControl.Instance.toggleRGB.isOn == true)
        {
            string[] rgb = color.Split(',');
            return new Color(int.Parse(rgb[0]) / 255f, int.Parse(rgb[1]) / 255f, int.Parse(rgb[2]) / 255f);
        }

        // OPTION 2: HEX FORMAT
        else if(ColorModeControl.Instance.toggleHEX.isOn == true)
        {
            // Try parsing the color string in HTML format
            Color newCol = Color.white;
            ColorUtility.TryParseHtmlString(color, out newCol);
            
            // Return the parsed color
            return newCol;
        }

        // OPTION 3: HSV FORMAT
        else
        {
            string[] hsv = color.Split(',');

            float h = int.Parse(hsv[0]) / 360f;
            float s = int.Parse(hsv[1]) / 100f;
            float v = int.Parse(hsv[2]) / 100f;

            //Create an RGB color from the HSV values
            Color newCol = Color.HSVToRGB(h, s, v);
            return newCol;
        }
    }

    private bool CheckValidString(string color)
    {
        // Split the string into individual values
        string[] colorValues = color.Split(',');

        // OPTION 1: RGB FORMAT
        if(ColorModeControl.Instance.toggleRGB.isOn == true)
        {
           return IsRGBStringValid(colorValues);
        }

        // OPTION 2: HEX FORMAT
        else if(ColorModeControl.Instance.toggleHEX.isOn == true)
        {
            return IsHexValueValid(color);
        }

        // OPTION 3: HSV FORMAT
        else
        {
            return IsHSVValueValid(colorValues);
        }
    }

    private bool IsRGBStringValid(string[] rgbValues)
    {
        // Check if there are exactly three values
        if (rgbValues.Length != 3)
        {
            return false;
        }

        // Try parsing each value into an integer
        foreach (string value in rgbValues)
        {
            if (!int.TryParse(value, out int intValue))
            {
                return false; // Not a valid integer
            }

            // Check if the integer is within the valid range (0 to 255)
            if (intValue < 0 || intValue > 255)
            {
                return false; // Out of bounds
            }
        }

        // If all checks pass, the RGB string is valid
        return true;
    }

    private bool IsHexValueValid(string hexValue)
    {
        // Remove a possible leading "#" symbol
        if (hexValue[0] == '#')
        {
            hexValue = hexValue.Substring(1);
        }

        // Check if the string has a valid length (6 or 8 characters)
        if (hexValue.Length != 6 && hexValue.Length != 8)
        {
            return false;
        }

        // Check if all characters are valid hex characters
        foreach (char c in hexValue)
        {
            if (!IsHexCharacter(c))
            {
                return false;
            }
        }

        // If all checks pass, the hex value is valid
        return true;
    }

    private bool IsHexCharacter(char c)
    {
        // Check if the character is a valid hex digit (0-9, A-F, a-f)
        return (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f');
    }

    private bool IsHSVValueValid(string[] hsvValues)
    {
        // Check if there are exactly three values
        if (hsvValues.Length != 3)
        {
            return false;
        }

        // Try parsing each value into an integer
        foreach (string value in hsvValues)
        {
            if (!int.TryParse(value, out int intValue))
            {
                return false; // Not a valid integer
            }

            // Check if the integer is within the valid range for HSV
            switch (Array.IndexOf(hsvValues, value))
            {
                case 0: // Hue
                    if (intValue < 0 || intValue > 360)
                    {
                        return false;
                    }
                    break;

                case 1: // Saturation
                case 2: // Value
                    if (intValue < 0 || intValue > 100)
                    {
                        return false;
                    }
                    break;
            }
        }

        // If all checks pass, the HSV value is valid
        return true;
    }

    private void DisplayErrorMessage()
    {
        StartCoroutine(FlashObject());
    }

    IEnumerator FlashObject()
    {
        for (int i = 0; i < flashCount; i++)
        {
            // Activate the object
            objectToFlash.SetActive(true);

            // Wait for a short duration
            yield return new WaitForSeconds(flashDuration);

            // Deactivate the object
            objectToFlash.SetActive(false);

            // Wait for a short duration
            yield return new WaitForSeconds(timeBetweenFlashes);
        }

        // Deactivate the object after the flashing is done
        objectToFlash.SetActive(false);
    }

}