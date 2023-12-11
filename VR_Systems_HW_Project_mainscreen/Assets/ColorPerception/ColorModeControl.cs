using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ColorModeControl : MonoBehaviour
{
    public static ColorModeControl Instance; // Singleton instance

    public Toggle toggleRGB;
    public Toggle toggleHEX;
    public Toggle toggleHSV;

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
        // Add listener to each toggle
        toggleRGB.onValueChanged.AddListener(delegate { OnToggleValueChanged(toggleRGB); });
        toggleHEX.onValueChanged.AddListener(delegate { OnToggleValueChanged(toggleHEX); });
        toggleHSV.onValueChanged.AddListener(delegate { OnToggleValueChanged(toggleHSV); });
    }

    void OnToggleValueChanged(Toggle activeToggle)
    {
        // Disable all other toggles except the one that was just toggled
        if (activeToggle == toggleRGB)
        {
            toggleHEX.isOn = false;
            toggleHSV.isOn = false;
        }
        else if (activeToggle == toggleHEX)
        {
            toggleRGB.isOn = false;
            toggleHSV.isOn = false;
        }
        else if (activeToggle == toggleHSV)
        {
            toggleRGB.isOn = false;
            toggleHEX.isOn = false;
        }

        // Ensure that at least one toggle is always active
        if (!toggleRGB.isOn && !toggleHEX.isOn && !toggleHSV.isOn)
        {
            activeToggle.isOn = true;
        }
    }
}
