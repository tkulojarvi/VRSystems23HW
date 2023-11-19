using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.InputSystem;

/*
 * CustomRoomUI
 * 
 * Overview:
 * This script is designed to control the custom room user interface.
 * The UI allows the user to modify various parameters that affect the environment.
 * The user can modify position, acceleration, and optic flow through individual parameters.
 * The script utilizes TextMeshPro objects to display and interact with the user for parameter adjustment.
 * It also interfaces with the InputSystem to capture keyboard inputs.
 * The script enables the user to navigate through different parameters, increase/decrease their values,
 * and start/exit the scene with the configured parameters.
 *
 * Components:
 * - xrOrigin: Reference to the GameObject representing the XR origin.
 * - flow: Reference to the Flow script attached to the xrOrigin, controlling position and acceleration.
 * - gridSpawner: Reference to the GameObject responsible for spawning a grid in the scene.
 * - lineMaterial: Material used to control the shape of the grid.
 * - realtimeGrid: Reference to the RealtimeGrid script controlling optic flow -related parameters.
 * - textObjects: Array of TextMeshProUGUI objects used to display and interact with parameter values.
 * - values: Array storing float values for each parameter.
 * - intValues: Array storing integer values for specific parameters.
 * - currentIndex: Index of the currently selected TextMeshPro object.
 * - increaseKeyDown, decreaseKeyDown: Flags indicating whether the increase or decrease key is currently held.
 * - timer, stage2timer: Timers used to control the speed of value adjustments.
 * - delay, stage2delay: Delays between value adjustments, with stage2delay indicating a faster adjustment.
 * - ParameterUI, Platform, TimerClock, LeftHand, RightHand: GameObjects representing UI elements in the scene.
 * - inMenu: Flag indicating whether the user is currently in the parameter adjustment menu.
 * 
 * Functions:
 * - Start(): Initializes references, sets up the initial display, and selection state.
 * - OnEnable(), OnDisable(): Enable and disable InputSystem controls for keyboard input.
 * - Update(): Handles keyboard input for navigation, value adjustments, and scene start/exit.
 * - IncreaseKey(), DecreaseKey(): Incrementally adjust parameter values while keys are held.
 * - SelectNextTextObject(), SelectPreviousTextObject(): Change the selected parameter for adjustment.
 * - UpdateSelection(): Updates the visual selection state of TextMeshPro objects.
 * - IncreaseValue(), DecreaseValue(): Adjust the selected parameter's value based on its type.
 * - UpdateDisplayedValue(): Updates the displayed value of the selected parameter.
 * - AssignValues(): Assigns the configured values to the corresponding components in the scene.
 * - InitializeValues(): Initializes default values for parameters and updates the UI display.
 * - StopMovementInMenu(): Stops the optic flow movement when in the parameter adjustment menu.
 * - StartScene(): Initiates the scene with the configured parameter values.
 * - ExitScene(): Exits either to the main menu or back to the parameter adjustment menu, based on the current state.
 */

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
    
    // An array to store the values for each TextMeshPro object
    private float[] values;
    private int[] intValues;

    // Index of the currently selected TextMeshPro object
    private int currentIndex = 0;
    private bool increaseKeyDown = false;
    private bool decreaseKeyDown = false;
    private float timer = 0f;
    private float stage2timer = 0f;
    private float delay = 0.1f; // Adjust the delay as needed
    private float stage2delay = 2;

    public GameObject ParameterUI;
    public GameObject Platform;
    public GameObject TimerClock;
    public GameObject LeftHand;
    public GameObject RightHand;

    private bool inMenu;

    void Start()
    {
        inMenu = true;

        // Get a referemce to the flow script to change values
        flow = xrOrigin.GetComponent<Flow>();
        // Get a reference to the gridspawner script to change values
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
        // Check for key presses
        //rightArrowKey
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            SelectNextTextObject();
        }
        //leftArrowKey
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            SelectPreviousTextObject();
        }
        //upArrowKey
        else if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            increaseKeyDown = true;
        }
        //downArrowKey
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
            IncreaseKey();
        }
        else if (decreaseKeyDown)
        {
            DecreaseKey();
        }

        // Check for enter and exit
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
    }

    void IncreaseKey()
    {
        if(stage2timer >= stage2delay)
        {
            delay = 0.01f;
                
        }
        if (timer >= delay)
        {
            IncreaseValue();
            timer = 0f; // Reset the timer after the delay
        }
        else
        {
            timer += Time.deltaTime; // Increment the timer
            stage2timer += Time.deltaTime; // Increment the speed up timer
        }
    }

    void DecreaseKey()
    {
        if(stage2timer >= stage2delay)
        {
            delay = 0.01f;
                
        }
        if (timer >= delay)
        {
            DecreaseValue();
            timer = 0f; // Reset the timer after the delay
        }
        else
        {
            timer += Time.deltaTime; // Increment the timer
            stage2timer += Time.deltaTime; // Increment the speed up timer
        }        
    }

    void SelectNextTextObject()
    {
        // Increment the currentIndex, and loop around if necessary
        currentIndex = (currentIndex + 1) % textObjects.Length;
        // Update the selection state
        UpdateSelection();
    }

    void SelectPreviousTextObject()
    {
        // Decrement the currentIndex, and loop around if necessary
        currentIndex = (currentIndex - 1 + textObjects.Length) % textObjects.Length;
        // Update the selection state
        UpdateSelection();
    }

    void UpdateSelection()
    {
        // Update the visual selection state of TextMeshPro objects
        for (int i = 0; i < textObjects.Length; i++)
        {
            // Get the Renderer component of the background object
            Renderer backgroundRenderer = textObjects[i].transform.Find("Background").GetComponent<Renderer>();

            // Set the material color based on whether the TextMeshPro object is selected or not
            backgroundRenderer.material.color = (i == currentIndex) ? Color.grey : Color.white;
        }
    }

    void IncreaseValue()
    {
        if (currentIndex == 12 || currentIndex == 13 || currentIndex == 14)
        {
            intValues[currentIndex]++; // Increment int values
        }

        else if(currentIndex == 24)
        {
            values[currentIndex] = Mathf.Clamp(values[currentIndex] + 0.1f, 0.0f, 1.0f); // Increment and clamp float values
        }

        else
        {
            values[currentIndex] += 0.1f; // Increment float values by 0.1
        }

        UpdateDisplayedValue();
    }

    void DecreaseValue()
    {
        if (currentIndex == 12 || currentIndex == 13 || currentIndex == 14)
        {
            intValues[currentIndex]--; // Decrement int values
        }

        else if(currentIndex == 24)
        {
            values[currentIndex] = Mathf.Clamp(values[currentIndex] - 0.1f, 0.0f, 1.0f); // Decrement and clamp float values
        }

        else
        {
            values[currentIndex] -= 0.1f; // Decrement float values by 0.1
        }

        UpdateDisplayedValue();
    }

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

        values[24] = 1;

        for (int i = 0; i < textObjects.Length; i++)
        {
            textObjects[i].text = values[i].ToString();
        }
    }

    void StopMovementInMenu()
    {
        Quaternion newRotation = Quaternion.Euler(0, 0, 0);
        xrOrigin.transform.rotation = newRotation;

        Vector3 newPosition = new Vector3(0, 0, 50);
        xrOrigin.transform.position = newPosition;

        flow.enabled = false;
    }

    void StartScene()
    {
        flow.enabled = true;

        Platform.SetActive(false);
        ParameterUI.SetActive(false);
        //TimerClock.SetActive(true);
        LeftHand.SetActive(false);
        RightHand.SetActive(false);

        realtimeGrid.refresh = true;

        inMenu = false;
    }

    void ExitScene()
    {
        // EXIT TO MENU
        if(inMenu == false)
        {
            Platform.SetActive(true);
            ParameterUI.SetActive(true);
            //TimerClock.SetActive(false);
            LeftHand.SetActive(true);
            RightHand.SetActive(true);

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
