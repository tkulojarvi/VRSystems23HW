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

    // InputData class
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
        // Check if the main functionality is active
        if(!main) return;

        // Call the Adjusting method for color adjustment
        Adjusting();
    }

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

    public Color RGBValuesToColor(float r, float g, float b)
    {
        float redValue = r;   // red float value
        float greenValue = g; // green float value
        float blueValue = b;  // blue float value

        // Normalize
        Color rgbColor = new Color(redValue / 255f, greenValue / 255f, blueValue / 255f);

        return rgbColor;
    }

    public void SetWallColor(Color color, string selectedWall)
    {
        // Get the Renderer components
        Material backwall_LL_Material = backwallLL.GetComponent<Renderer>().material;
        Material backwall_LR_Material = backwallLR.GetComponent<Renderer>().material;
        Material backwall_RL_Material = backwallRL.GetComponent<Renderer>().material;
        Material backwall_RR_Material = backwallRR.GetComponent<Renderer>().material;

        // Set the color of the backwallLeft and backwallRight components
        if(selectedWall == "LL")
        {
            backwall_LL_Material.color = color;
        }

        else if(selectedWall == "LR")
        {
            backwall_LR_Material.color = color;
        }

        else if(selectedWall == "RL")
        {
            backwall_RL_Material.color = color;
        }

        else if(selectedWall == "RR")
        {
            backwall_RR_Material.color = color;
        }
    }
}