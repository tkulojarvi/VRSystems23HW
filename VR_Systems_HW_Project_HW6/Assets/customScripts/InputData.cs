using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;

public class InputData : MonoBehaviour
{
    // Reference to the right-hand controller input device
    public InputDevice _rightController;
    // Reference to the left-hand controller input device
    public InputDevice _leftController;
    // Reference to the head-mounted display (HMD) input device
    public InputDevice _HMD;

    void Update()
    {
        // Check if any of the input devices are not valid (e.g., disconnected or not initialized)
        if(!_rightController.isValid || !_leftController.isValid || !_HMD.isValid)
        {
            // If any device is not valid, reinitialize input devices
            InitializeInputDevices();
        }
    }

    private void InitializeInputDevices()
    {
        if(!_rightController.isValid)
        {
            // Initialize the right controller input device with the specified characteristics
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, ref _rightController);
        }
        if(!_leftController.isValid)
        {
            // Initialize the left controller input device with the specified characteristics
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, ref _leftController);
        }
        if(!_HMD.isValid)
        {
            // Initialize the HMD input device with the specified characteristics
            InitializeInputDevice(InputDeviceCharacteristics.HeadMounted, ref _HMD);
        }
    }

    private void InitializeInputDevice(InputDeviceCharacteristics inputCharacteristics, ref InputDevice inputDevice)
    {
        // Create a list to store input devices with the specified characteristics
        List<InputDevice> devices = new List<InputDevice>();

        // Call InputDevices to see if it can find anyd devices with the characteristics we're looking for
        InputDevices.GetDevicesWithCharacteristics(inputCharacteristics, devices);

        // Our hands might not be active and so they will not be generated from the search.
        // We check if any devices are found here to avoid errors.
        if(devices.Count > 0)
        {
            // Assign the first device found to the inputDevice reference
            inputDevice = devices[0];
        }
    }
}
