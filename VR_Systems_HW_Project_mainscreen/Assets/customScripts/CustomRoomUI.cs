using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.InputSystem;

using UnityEngine.XR;

public class CustomRoomUI : MonoBehaviour
{
    // Position, acceleration
    public GameObject xrOrigin;
    private Flow flow;
    // Optic Flow
    public GameObject gridSpawner;
    public Material lineMaterial;
    private RealtimeGrid realtimeGrid;

    // An array to hold references to your TextMeshPro objects
    public TextMeshProUGUI[] textObjects;

    // Small menu UI elements
    public GameObject smallMenuObject;
    public TextMeshProUGUI smallMenuTitle, smallMenuText;
    
    // An array to store the values for each TextMeshPro object
    private float[] values;
    private int[] intValues;
    // Index of the currently selected TextMeshPro object
    private int currentIndex = 0;

    // Variables for handling key states and timers
    private bool increaseKeyDown = false;
    private bool decreaseKeyDown = false;
    private float timer = 0f;
    private float stage2timer = 0f;
    private float delay = 0.1f;
    private float stage2delay = 1;

    // UI game objects
    public GameObject ParameterUI;
    public GameObject Platform;
    public GameObject TimerClock;

    // Index and arrays for small menu options
    int smallMenuIndex = 0;
    int[] smallMenu = { 3,  //speed
                        0,  //rotation
                        0,  //direction
                        0,  //acceleration
                        2,  //grid density
                        1 };//object shape
    string[] smallMenuNames = { "Speed", "Rotation", "Direction", "Acceleration", "Grid Density", "Object Shape" };

    // Check for whether to exit to custom room menu or main menu
    private bool inMenu;
    
    // Variables for fixing the bug where the exit button fires twice
    private bool buttonPressedThisFrame = false;
    private float debounceCooldown = 0.5f; // Adjust the cooldown time as needed
    private float cooldownTimer = 0f;

    // XR Input
    private InputData _inputData;
    float lastSelectionTime = 0f; // Initialize the last selection time

    // Add a variable to control the blinking speed
    public float blinkSpeed = 0.5f;

    // Array to store references to each blinking coroutine
    private Coroutine[] blinkCoroutines;

    void Start()
    {
        // Initialize the array
        blinkCoroutines = new Coroutine[textObjects.Length];

        // Get a reference to the InputData script
        _inputData = GetComponent<InputData>();

        inMenu = true;

        // Get a reference
        flow = xrOrigin.GetComponent<Flow>();
        realtimeGrid = gridSpawner.GetComponent<RealtimeGrid>();

        // Initialize the array to store values for each TextMeshPro object
        InitializeValues();
        // Set up the initial display of TextMeshPro objects
        UpdateDisplayedValue();
        // Set up the initial selection state
        UpdateSelection();
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
        // Cooldown for button presses
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            return; // Exit the Update method prematurely
        }

        // Show/hide small menu
        smallMenuObject.SetActive(!inMenu);

        // XR Input
        // XR Input for left controller
        if (_inputData._leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out Vector2 leftAnalog))
        {
            float threshold = 0.5f; // Threshold for recognizing input
            float cooldownTime = 0.25f; // Set the cooldown time to 1 second

            if((leftAnalog.x > threshold || leftAnalog.y < -threshold) && Time.time - lastSelectionTime >= cooldownTime)
            {
                // Execute only if the x-value is greater than the threshold and enough time has passed
                SelectNextTextObject(1);
                lastSelectionTime = Time.time; // Update the last selection time
            }
            else if((leftAnalog.x < -threshold || leftAnalog.y > threshold) && Time.time - lastSelectionTime >= cooldownTime)
            {
                SelectNextTextObject(-1);
                lastSelectionTime = Time.time; // Update the last selection time
            }
        }
        
        // XR Input for right controller
        if (_inputData._rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out Vector2 rightAnalog))
        {
            float threshold = 0.5f;

            if (rightAnalog.y > threshold || rightAnalog.x > threshold) 
            {
                increaseKeyDown = true;
            }
            else if(rightAnalog.y < -threshold || rightAnalog.x < -threshold)
            {
                decreaseKeyDown = true;
            }

            // Check for joystick release
            // else = in the middle aka not pushing joystick
            else
            {
                increaseKeyDown = false;
                decreaseKeyDown = false;
                timer = 0f; // Reset the timer when the joystick is released
                stage2timer = 0f;
                delay = 0.1f;
            }
        }

        // KEYBOARD
        // Keyboard might not work while controllers are active!
        // Right
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            SelectNextTextObject(1);
        }
        // Left
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            SelectNextTextObject(-1);
        }
        // Up
        else if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            increaseKeyDown = true;
        }
        // Down
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            decreaseKeyDown = true;
        }

        // Check for key releases
        if (Keyboard.current.upArrowKey.wasReleasedThisFrame)
        {
            increaseKeyDown = false;
            timer = 0f; // Reset the timer when the key is released
            stage2timer = 0f;
            delay = 0.1f;
        }
        else if (Keyboard.current.downArrowKey.wasReleasedThisFrame)
        {
            decreaseKeyDown = false;
            timer = 0f; // Reset the timer when the key is released
            stage2timer = 0f;
            delay = 0.1f;
        }

        // Adjust the values continuously while the keys are held down
        if (increaseKeyDown)
        {
            IncreaseKey(1);
        }
        else if (decreaseKeyDown)
        {
            IncreaseKey(-1);
        }

        // Check for enter and exit
        // XR Input
        if(_inputData._leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out bool primaryButtonLeft) && primaryButtonLeft)
        {
            AssignValues();
            StartScene();
        }
        if(_inputData._rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out bool primaryButtonRight) && primaryButtonRight)
        {
            if (!buttonPressedThisFrame)
            {
            ExitScene();
            buttonPressedThisFrame = true;
            cooldownTimer = debounceCooldown;
            }
        }

        // Keyboard
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            // If ENTER key pressed, START SCENE WITH THE USER PARAMETERS.
            AssignValues();
            StartScene();
        }
        else if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // If SPACE key pressed, EXIT SCENE WITH THE USER PARAMETERS.
            ExitScene();
        }

        // Reset the flag when the button is not pressed
        buttonPressedThisFrame = false;
    }

    // Method to handle continuous key presses for increasing values
    void IncreaseKey(int amount)
    {
        if(stage2timer >= stage2delay)
        {
            delay = 0;
                
        }
        if (timer >= delay)
        {
            IncreaseValue(amount);
            timer = 0f; // Reset the timer after the delay
        }
        else
        {
            timer += Time.deltaTime; // Increment the timer
            stage2timer += Time.deltaTime; // Increment the speed up timer
        }
    }

    // Method to select the next or previous TextMeshPro object
    void SelectNextTextObject(int dir)
    {
        if (!inMenu)
        {
            smallMenuIndex = (smallMenuIndex + dir + smallMenu.Length) % smallMenu.Length;
        }
        // Increment the currentIndex, and loop around if necessary
        else
            currentIndex = (currentIndex + dir + textObjects.Length) % textObjects.Length;
        // Update the selection state
        UpdateSelection();
    }

    // Method to handle small menu selections
    string SmallMenuSelection(int change = 0)
    {
        smallMenu[0] = (int)flow.speed;
        smallMenu[1] = (int)flow.rotation;
        if(smallMenuIndex == 2) //direction
        {
            smallMenu[2] = (smallMenu[2] + change + 3) % 3;
            xrOrigin.transform.rotation = smallMenu[2] == 0 ? Quaternion.Euler(0, 0, 0) : smallMenu[2] == 1 ? Quaternion.Euler(90,0,0) : Quaternion.Euler(0,90,0);
            return smallMenu[2] == 0 ? "Forward" : smallMenu[2] == 1 ? "Vertical" : "Horizontal";
        }
        if(smallMenuIndex == 3) //acceleration
        {
            smallMenu[3] = (smallMenu[3] + change + 3) % 3;
            flow.acceleration = smallMenu[3] == 0 ? 0 : 3;
            flow.angularAcceleration = smallMenu[3] == 2 ? 20 : 0;
            return smallMenu[3] == 0 ? "None" : smallMenu[3] == 1 ? "Velocity" : "Rotation";
        }
        if (smallMenuIndex == 4 && change != 0) //grid density
        {
            smallMenu[4] = Mathf.Clamp(smallMenu[4] + change, 1, 3);
            realtimeGrid.gap = 4 - smallMenu[4];
            realtimeGrid.refresh = true;
        }
        else if (smallMenuIndex == 5 && change != 0)
        {
            smallMenu[5] = Mathf.Clamp(smallMenu[5] + change, 1, 10);
            lineMaterial.SetFloat("_Cutoff", (11 - smallMenu[5]) * 0.1f);
        }
        else
        {
            smallMenu[smallMenuIndex] += change;
            flow.speed = smallMenu[0];
            flow.rotation = smallMenu[1];
        }
        return smallMenu[smallMenuIndex].ToString();
    }

    // Method to update the visual selection state of TextMeshPro objects
    void UpdateSelection()
    {
        if (!inMenu)
        {
            smallMenuTitle.text = smallMenuNames[smallMenuIndex];
            smallMenuText.text = SmallMenuSelection();
        }

        // Update the visual selection state of TextMeshPro objects
        for (int i = 0; i < textObjects.Length; i++)
        {
            // Get the Renderer component of the background object
            Renderer backgroundRenderer = textObjects[i].transform.Find("Background").GetComponent<Renderer>();

            // Check if the TextMeshPro object is selected
            if (i == currentIndex)
            {
                // If not already blinking, start the blinking coroutine
                if (blinkCoroutines[i] == null)
                {
                    blinkCoroutines[i] = StartCoroutine(Blink(backgroundRenderer));
                }
            }
            else
            {
                // If currently blinking, stop the blinking coroutine and set the color to white
                if (blinkCoroutines[i] != null)
                {
                    StopCoroutine(blinkCoroutines[i]);
                    blinkCoroutines[i] = null;
                    backgroundRenderer.material.color = Color.white;
                }
            }
        }
    }

    IEnumerator Blink(Renderer renderer)
    {
        // Infinite loop for blinking effect
        while (true)
        {
            // Set the color to red
            renderer.material.color = Color.red;
            // Wait for a short duration
            yield return new WaitForSeconds(blinkSpeed);

            // Set the color to white
            renderer.material.color = Color.white;
            // Wait for the same duration
            yield return new WaitForSeconds(blinkSpeed);
        }
    }

    // Method to handle continuous value increase
    void IncreaseValue(int amount)
    {
        if (!inMenu)
        {
            SmallMenuSelection(amount);
            UpdateSelection();
            return;
        }
        if (currentIndex == 12 || currentIndex == 13 || currentIndex == 14)
        {
            intValues[currentIndex] += amount; // Increment int values
        }

        else if(currentIndex == 24)
        {
            values[currentIndex] = Mathf.Clamp(values[currentIndex] + amount * 0.1f, 0.0f, 1.0f); // Increment and clamp float values
        }

        else
        {
            values[currentIndex] += amount * 0.1f; // Increment float values by 0.1
        }

        UpdateDisplayedValue();
    }

    // Method to update the displayed values of TextMeshPro objects
    void UpdateDisplayedValue()
    {
        if (currentIndex == 12 || currentIndex == 13 || currentIndex == 14)
        {
            textObjects[currentIndex].text = intValues[currentIndex].ToString(); // Display as int
        }
        else
        {
            textObjects[currentIndex].text = values[currentIndex].ToString("F1"); // Display as float with one decimal place
        }
    }

    void AssignValues()
    {
        Quaternion newRotation = Quaternion.Euler(values[0], values[1], values[2]);
        xrOrigin.transform.rotation = newRotation;

        flow.gap = values[3];
        flow.speed = values[4];
        flow.rotation = values[5];

        flow.acceleration = values[6];
        flow.angularAcceleration = values[7];
        flow.linearTime = values[8];
        flow.accelerationTime = values[9];
        flow.randomAccelerationSwing = values[10];
        flow.decelerationTime = values[11];

        realtimeGrid.gap = intValues[12];
        realtimeGrid.safearea = intValues[13];
        realtimeGrid.size = intValues[14];
        realtimeGrid.offset.x = values[15];
        realtimeGrid.offset.y = values[16];
        realtimeGrid.offset.z = values[17];
        realtimeGrid.alternatingOffset.x = values[18];
        realtimeGrid.alternatingOffset.y = values[19];
        realtimeGrid.alternatingOffset.z = values[20];
        realtimeGrid.randomAmount.x = values[21];
        realtimeGrid.randomAmount.y = values[22];
        realtimeGrid.randomAmount.z = values[23];

        // shape
        lineMaterial.SetFloat("_Cutoff", values[24]);
    }

    // Initialize default values
    void InitializeValues()
    {
        intValues = new int[textObjects.Length];
        values = new float[textObjects.Length];

        values[3] = 5;
        values[4] = 3;

        values[8] = 1;
        values[9] = 3;
        values[10] = 0.5f;
        values[11] = 0.3f;

        intValues[12] = 2;
        intValues[13] = 2;
        intValues[14] = 10;
        values[18] = 1;
        values[19] = 1;
        values[20] = 1;
        values[21] = 0.5f;
        values[22] = 0.5f;
        values[23] = 0.5f;

        values[24] = 1;

        for (int i = 0; i < textObjects.Length; i++)
        {
            textObjects[i].text = values[i].ToString();
        }
    }

    // Method to stop optic flow 
    void StopMovementInMenu()
    {
        Quaternion newRotation = Quaternion.Euler(0, 0, 0);
        xrOrigin.transform.rotation = newRotation;

        Vector3 newPosition = new Vector3(0, 0, 50);
        xrOrigin.transform.position = newPosition;

        flow.enabled = false;
    }

    // Method to start the scene and apply user-selected parameters
    void StartScene()
    {
        if (!inMenu)
            return;
        flow.enabled = true;

        Platform.SetActive(false);
        ParameterUI.SetActive(false);
        TimerClock.SetActive(true);

        realtimeGrid.refresh = true;

        inMenu = false;
        UpdateSelection();
    }

    // Method to exit the scene or return to the menu based on the current state
    void ExitScene()
    {
        // EXIT TO MENU
        if(inMenu == false)
        {
            Platform.SetActive(true);
            ParameterUI.SetActive(true);
            TimerClock.SetActive(false);
            

            inMenu = true;
            StopMovementInMenu();
        }

        // EXIT TO MAIN MENU
        else if(inMenu == true)
        {
            RoomSwitch.Instance.ExitRoom();
        }
    }
}
