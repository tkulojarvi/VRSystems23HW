using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;


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

    // InputData class for handling input
    InputData inputData;

    // Lights for left and right cameras
    public Light rightLight = null;
    public Light leftLight = null;

    // Variables of the color matching
    int state = 0;
    int matchIndex = 0;
    int playerIndex = 0;
    int correctCount = 0;
    Renderer match, playerSelection;

    // temperature
    float selectTemperature = 10000;

    // Flag to check if a button is being held
    bool inputHold = false;


    // NEW VARIABLES
    public GameObject backwallLL;
    public GameObject backwallLR;
    private GameObject backwallRL;
    private GameObject backwallRR;

    
    private string r;
    private string g;
    private string b;
    private string rgbString;
    private bool RGBInputSuccessful = false;
    private bool WallInputSuccessful = false;
    private string wallSelection;

    void Start()
    {
        // Check if the initialization has already been done
        if (init)
            return;

        // Set init to true to prevent further initialization
        init = true;
        // Set main to true to indicate the main functionality is active
        main = true;

        // Get the InputData component attached to the GameObject
        inputData = GetComponent<InputData>();

        // Enable input actions for primary, secondary, and adjust
        primary.Enable();
        secondary.Enable();
        adjust.Enable();

        // Create a new GameObject for the right camera and set its layer
        GameObject right = Instantiate(transform.gameObject, null);
        right.SetLayerRecursively(rightTest.gameObject.layer);
        right.name = "Right Environment";

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
        // Check if the main functionality is active
        if(!main) return;

        // Call the Adjusting method for color adjustment
        //Adjusting();

        // Check if the r key was pressed this frame
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            // Start the process to get RGB values from the user
            StartCoroutine(GetRGBInput());
        }

        // Check if the w key was pressed this frame
        else if(Keyboard.current.wKey.wasPressedThisFrame)
        {
            // Start the process to get input
            StartCoroutine(GetWallInput());
        }

        // Use the obtained values
        if(RGBInputSuccessful == true && WallInputSuccessful == true)
        {
            RGBInputSuccessful = false;
            WallInputSuccessful = false;
            SetWallColor(rgbString, wallSelection);
        }
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

   
    public void SetColorLeft(string color)
    {
        // Check if the input color is not null and set the color of leftTest
        if(color == null) return;
        leftTest.sharedMaterial.color = StringToColor(color);
    }

    
    public void SetColorRight(string color)
    {
        // Check if the input color is not null and set the color of rightTest
        if(color == null) return;
        rightTest.sharedMaterial.color = StringToColor(color);
    }

    
    public void SetLightColorLeft(string color)
    {
        // Check if the input color is not null and set the color of leftLight
        if(color == null) return;
        leftLight.color = StringToColor(color);
    }

    
    public void SetLightColorRight(string color)
    {
        // Check if the input color is not null and set the color of rightLight
        if(color == null) return;
        rightLight.color = StringToColor(color);
    }

    Color TemperatureToColor(float temp)
    {
        // Initialize a black color
        Color color = Color.black;
        // Normalize the temperature
        temp /= 100;

        // Check temperature range and set RGB components accordingly
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
*/
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
/*
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
*/

    // NEW FUNCTIONS BELOW

    public void SetWallColor(string color, string selectedWall)
    {
        // Check if the input color is not null
        if(color == null) return;

        // Debug logs to check input values
        Debug.Log($"Setting color for {selectedWall} to {color}");

        // Get the Renderer components
        Material backwall_LL_Material = backwallLL.GetComponent<Renderer>().material;
        Material backwall_LR_Material = backwallLR.GetComponent<Renderer>().material;
        Material backwall_RL_Material = backwallRL.GetComponent<Renderer>().material;
        Material backwall_RR_Material = backwallRR.GetComponent<Renderer>().material;

        // Set the color of the backwallLeft and backwallRight components
        if(selectedWall == "LL")
        {
            Debug.Log("LL");
            backwall_LL_Material.color = StringToColor(color);
        }

        else if(selectedWall == "LR")
        {
            Debug.Log("LR");
            backwall_LR_Material.color = StringToColor(color);
        }

        else if(selectedWall == "RL")
        {
            Debug.Log("RL");
            backwall_RL_Material.color = StringToColor(color);
        }

        else if(selectedWall == "RR")
        {
            Debug.Log("RR");
            backwall_RR_Material.color = StringToColor(color);
        }
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

    private System.Collections.IEnumerator GetWallInput()
    {
        wallSelection = "";

        yield return new WaitForSeconds(0.5f); // Delay to ensure the space key is not considered for input

        while (true)
        {
            // Check for key presses using Unity's Input System
            if (Keyboard.current.lKey.wasPressedThisFrame) wallSelection += "L";
            if (Keyboard.current.rKey.wasPressedThisFrame) wallSelection += "R";

            // Check for enter key press to break out of the loop
            if (Keyboard.current.enterKey.wasPressedThisFrame) break;

            yield return null; // Wait for the next frame
        }

        // Set flag
        WallInputSuccessful = true;

        yield return null;
    }
}