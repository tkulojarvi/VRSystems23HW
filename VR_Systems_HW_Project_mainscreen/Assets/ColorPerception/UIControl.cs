using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class UIControl : MonoBehaviour
{
    public GameObject arrowRight;
    public GameObject arrowLeft;
    public GameObject arrowUp;
    public GameObject arrowDown;
    private Button left;
    private Button right;
    private Button up;
    private Button down;

    public GameObject selectUI;
    private Button select;

    public GameObject backButton;
    private Button back;

    public GameObject LL_ALL_SLIDERS;
    public GameObject LR_ALL_SLIDERS;
    public GameObject RL_ALL_SLIDERS;
    public GameObject RR_ALL_SLIDERS;

    public GameObject LL_Slider_Red;
    public GameObject LL_Slider_Green;
    public GameObject LL_Slider_Blue;
    
    public GameObject LR_Slider_Red;
    public GameObject LR_Slider_Green;
    public GameObject LR_Slider_Blue;

    public GameObject RL_Slider_Red;
    public GameObject RL_Slider_Green;
    public GameObject RL_Slider_Blue;

    public GameObject RR_Slider_Red;
    public GameObject RR_Slider_Green;
    public GameObject RR_Slider_Blue;

    private Slider LL_Red;
    private Slider LL_Green;
    private Slider LL_Blue;

    private Slider LR_Red;
    private Slider LR_Green;
    private Slider LR_Blue;

    private Slider RL_Red;
    private Slider RL_Green;
    private Slider RL_Blue;

    private Slider RR_Red;
    private Slider RR_Green;
    private Slider RR_Blue;

    public GameObject Left_Eye_ALL_SLIDERS;
    public GameObject Right_Eye_ALL_SLIDERS;
    public GameObject Left_Eye_Slider_Red;
    public GameObject Left_Eye_Slider_Green;
    public GameObject Left_Eye_Slider_Blue;
    public GameObject Right_Eye_Slider_Red;
    public GameObject Right_Eye_Slider_Green;
    public GameObject Right_Eye_Slider_Blue;
    private Slider Left_Eye_Red;
    private Slider Left_Eye_Green;
    private Slider Left_Eye_Blue;
    private Slider Right_Eye_Red;
    private Slider Right_Eye_Green;
    private Slider Right_Eye_Blue;

    public TextMeshProUGUI textMeshSelect;
    private string selectionState;

    private float LL_RedValue;
    private float LL_GreenValue;
    private float LL_BlueValue;
    private float LR_RedValue;
    private float LR_BlueValue;
    private float LR_GreenValue;
    private float RL_RedValue;
    private float RL_GreenValue;
    private float RL_BlueValue;
    private float RR_RedValue;
    private float RR_GreenValue;
    private float RR_BlueValue;

    string LLWall = "LL";
    string LRWall = "LR";
    string RLWall = "RL";
    string RRWall = "RR";

    private float Left_Eye_Red_Value;
    private float Left_Eye_Green_Value;
    private float Left_Eye_Blue_Value;
    private float Right_Eye_Red_Value;
    private float Right_Eye_Green_Value;
    private float Right_Eye_Blue_Value;

    private string colorState;

    public GameObject keyboard_ALL;

    public GameObject leftEnvironment;
    private StereoColor stereoColor;
    public GameObject squareController;
    private SquareControl squareControl;

    public GameObject playerPosition;
    public GameObject XROrigin;

    public TextMeshProUGUI textMeshLLR;
    public TextMeshProUGUI textMeshLLG;
    public TextMeshProUGUI textMeshLLB;
    

    void Start()
    {
        // Get the button/slider components
        left = arrowRight.GetComponent<Button>();
        right = arrowLeft.GetComponent<Button>();
        up = arrowUp.GetComponent<Button>();
        down = arrowDown.GetComponent<Button>();
        select = selectUI.GetComponent<Button>();
        back = backButton.GetComponent<Button>();

        LL_Red = LL_Slider_Red.GetComponent<Slider>();
        LL_Green = LL_Slider_Green.GetComponent<Slider>();
        LL_Blue = LL_Slider_Blue.GetComponent<Slider>();

        LR_Red = LR_Slider_Red.GetComponent<Slider>();
        LR_Green = LR_Slider_Green.GetComponent<Slider>();
        LR_Blue = LR_Slider_Blue.GetComponent<Slider>();

        RL_Red = RL_Slider_Red.GetComponent<Slider>();
        RL_Green = RL_Slider_Green.GetComponent<Slider>();
        RL_Blue = RL_Slider_Blue.GetComponent<Slider>();

        RR_Red = RR_Slider_Red.GetComponent<Slider>();
        RR_Green = RR_Slider_Green.GetComponent<Slider>();
        RR_Blue = RR_Slider_Blue.GetComponent<Slider>();

        Left_Eye_Red = Left_Eye_Slider_Red.GetComponent<Slider>();
        Left_Eye_Green = Left_Eye_Slider_Green.GetComponent<Slider>();
        Left_Eye_Blue = Left_Eye_Slider_Blue.GetComponent<Slider>();
        Right_Eye_Red = Right_Eye_Slider_Red.GetComponent<Slider>();
        Right_Eye_Green = Right_Eye_Slider_Green.GetComponent<Slider>();
        Right_Eye_Blue = Right_Eye_Slider_Blue.GetComponent<Slider>();
        
        // Add listeners to the buttons/sliders
        left.onClick.AddListener(OnLeftButtonClick);
        right.onClick.AddListener(OnRightButtonClick);
        up.onClick.AddListener(OnUpButtonClick);
        down.onClick.AddListener(OnDownButtonClick);
        select.onClick.AddListener(OnSelectButtonClick);
        back.onClick.AddListener(OnBackButtonClick);

        LL_Red.onValueChanged.AddListener(OnLL_RedValueChanged);
        LL_Green.onValueChanged.AddListener(OnLL_GreenValueChanged);
        LL_Blue.onValueChanged.AddListener(OnLL_BlueValueChanged);

        LR_Red.onValueChanged.AddListener(OnLR_RedValueChanged);
        LR_Green.onValueChanged.AddListener(OnLR_GreenValueChanged);
        LR_Blue.onValueChanged.AddListener(OnLR_BlueValueChanged);

        RL_Red.onValueChanged.AddListener(OnRL_RedValueChanged);
        RL_Green.onValueChanged.AddListener(OnRL_GreenValueChanged);
        RL_Blue.onValueChanged.AddListener(OnRL_BlueValueChanged);

        RR_Red.onValueChanged.AddListener(OnRR_RedValueChanged);
        RR_Green.onValueChanged.AddListener(OnRR_GreenValueChanged);
        RR_Blue.onValueChanged.AddListener(OnRR_BlueValueChanged);

        Left_Eye_Red.onValueChanged.AddListener(OnLERValueChanged);
        Left_Eye_Green.onValueChanged.AddListener(OnLEGValueChanged);
        Left_Eye_Blue.onValueChanged.AddListener(OnLEBValueChanged);
        Right_Eye_Red.onValueChanged.AddListener(OnRERValueChanged);
        Right_Eye_Green.onValueChanged.AddListener(OnREGValueChanged);
        Right_Eye_Blue.onValueChanged.AddListener(OnREBValueChanged);

        // Set initial menu
        selectionState = "Main/ColorRGB";

        // Set initial color state
        colorState = "RGB";

        // Set reference to StereoColor
        stereoColor = leftEnvironment.GetComponent<StereoColor>();
        SetInitialColors();

        // Set reference to SquareControl
        squareControl = squareController.GetComponent<SquareControl>();
    }

void SetInitialColors()
{
    // Set default colors
    stereoColor.SetWallColor(stereoColor.RGBValuesToColor(200f, 100f, 100f), LLWall);
    stereoColor.SetWallColor(stereoColor.RGBValuesToColor(100f, 100f, 100f), LRWall);
    stereoColor.SetWallColor(stereoColor.RGBValuesToColor(100f, 100f, 100f), RLWall);
    stereoColor.SetWallColor(stereoColor.RGBValuesToColor(100f, 100f, 100f), RRWall);
}

/*
--------------------------
UP DOWN LEFT RIGHT ARROWS
--------------------------
*/

    void OnLeftButtonClick()
    {
        // CHANGE SELECTION STATE

        switch (selectionState)
        {
            /*
            ----------------
            MAIN
            ----------------
            */
            case "Main/ColorRGB":
                // CHANGE TO START
                textMeshSelect.text = "Start";
                selectionState = "Main/Start";
                break;

            case "Main/ColorHSV":
                // CHANGE TO RGB
                textMeshSelect.text = "ColorRGB";
                selectionState = "Main/ColorRGB";
                break;

            case "Main/EyeAdapt":
                // CHANGE TO HSV
                textMeshSelect.text = "ColorHSV";
                selectionState = "Main/ColorHSV";
                break;

            case "Main/SquareControl":
                // CHANGE TO EYEADAPT
                textMeshSelect.text = "EyeAdapt";
                selectionState = "Main/EyeAdapt";
                break;
            
            case "Main/KeyboardInput":
                // CHANGE TO SQUARE
                textMeshSelect.text = "SquareControl";
                selectionState = "Main/SquareControl";
                break;

            case "Main/Start":
                // CHANGE TO KEYBOARD
                textMeshSelect.text = "KeyboardInput";
                selectionState = "Main/KeyboardInput";
                break;
            /*
            ----------------
            RGB
            ----------------
            */
            
            case "ColorRGB/LL":
                // CHANGE TO RR
                LL_ALL_SLIDERS.SetActive(false);
                RR_ALL_SLIDERS.SetActive(true);
                selectionState = "ColorRGB/RR";
                break;

            case "ColorRGB/LR":
                // CHANGE TO LL
                LR_ALL_SLIDERS.SetActive(false);
                LL_ALL_SLIDERS.SetActive(true);
                selectionState = "ColorRGB/LL";
                break;

            case "ColorRGB/RL":
                // CHANGE TO LR
                RL_ALL_SLIDERS.SetActive(false);
                LR_ALL_SLIDERS.SetActive(true);
                selectionState = "ColorRGB/LR";
                break;

            case "ColorRGB/RR":
                // CHANGE TO RL
                RR_ALL_SLIDERS.SetActive(false);
                RL_ALL_SLIDERS.SetActive(true);
                selectionState = "ColorRGB/RL";
                break;

            /*
            ----------------
            HSV
            ----------------
            */

            /*
            ----------------
            EYE
            ----------------
            */
            case "EyeAdapt/L":
                // CHANGE TO R
                Left_Eye_ALL_SLIDERS.SetActive(false);
                Right_Eye_ALL_SLIDERS.SetActive(true);
                selectionState = "EyeAdapt/R";
                break;

            case "EyeAdapt/R":
                // CHANGE TO L
                Right_Eye_ALL_SLIDERS.SetActive(false);
                Left_Eye_ALL_SLIDERS.SetActive(true);
                selectionState = "EyeAdapt/L";
                break;

            /*
            ----------------
            ARROWS
            ----------------
            */

            // square movement here

            default:
                Debug.Log("DEFAULT");
                break;
        }
    }

    void OnRightButtonClick()
    {
        // Do something different when the right button is clicked
        Debug.Log("Right Button Clicked!");

        // CHANGE SELECTION STATE

        switch (selectionState)
        {
            /*
            ----------------
            MAIN
            ----------------
            */
            case "Main/ColorRGB":
                // CHANGE TO HSV
                textMeshSelect.text = "ColorHSV";
                selectionState = "Main/ColorHSV";
                break;

            case "Main/ColorHSV":
                // CHANGE TO EYEADAPT
                textMeshSelect.text = "EyeAdapt";
                selectionState = "Main/EyeAdapt";
                break;

            case "Main/EyeAdapt":
                // CHANGE TO SQUARE
                textMeshSelect.text = "SquareControl";
                selectionState = "Main/SquareControl";
                break;

            case "Main/SquareControl":
                // CHANGE TO KEYBOARD
                textMeshSelect.text = "KeyboardInput";
                selectionState = "Main/KeyboardInput";
                break;
            
            case "Main/KeyboardInput":
                // CHANGE TO START
                textMeshSelect.text = "Start";
                selectionState = "Main/Start";
                break;

            case "Main/Start":
                // CHANGE TO RGB
                textMeshSelect.text = "ColorRGB";
                selectionState = "Main/ColorRGB";
                break;
            
            /*
            ----------------
            RGB
            ----------------
            */
            case "ColorRGB/LL":
                // CHANGE TO LR
                LL_ALL_SLIDERS.SetActive(false);
                LR_ALL_SLIDERS.SetActive(true);
                selectionState = "ColorRGB/LR";
                break;

            case "ColorRGB/LR":
                // CHANGE TO RL
                LR_ALL_SLIDERS.SetActive(false);
                RL_ALL_SLIDERS.SetActive(true);
                selectionState = "ColorRGB/RL";
                break;

            case "ColorRGB/RL":
                // CHANGE TO RR
                RL_ALL_SLIDERS.SetActive(false);
                RR_ALL_SLIDERS.SetActive(true);
                selectionState = "ColorRGB/RR";
                break;

            case "ColorRGB/RR":
                // CHANGE TO LL
                RR_ALL_SLIDERS.SetActive(false);
                LL_ALL_SLIDERS.SetActive(true);
                selectionState = "ColorRGB/LL";
                break;

            /*
            ----------------
            HSV
            ----------------
            */

            /*
            ----------------
            EYE
            ----------------
            */
            case "EyeAdapt/L":
                // CHANGE TO R
                Left_Eye_ALL_SLIDERS.SetActive(false);
                Right_Eye_ALL_SLIDERS.SetActive(true);
                selectionState = "EyeAdapt/R";
                break;

            case "EyeAdapt/R":
                // CHANGE TO L
                Right_Eye_ALL_SLIDERS.SetActive(false);
                Left_Eye_ALL_SLIDERS.SetActive(true);
                selectionState = "EyeAdapt/L";
                break;

            /*
            ----------------
            ARROWS
            ----------------
            */

            // square movement here

            default:
                Debug.Log("DEFAULT");
                break;
        }
    }

    void OnUpButtonClick()
    {
        // Do something different when the right button is clicked
        Debug.Log("up Button Clicked!");

        // move squares up
    }

    void OnDownButtonClick()
    {
        // Do something different when the right button is clicked
        Debug.Log("down Button Clicked!");

        // move squares down
    }

/*
--------------------------
SELECTION AND EXIT CONTROL
--------------------------
*/

    void OnSelectButtonClick()
    {
         switch (selectionState)
        {
            case "Main/ColorRGB":
                // DISABLE SELECT
                selectUI.SetActive(false);
                // ENABLE WALL SLIDERS
                LL_ALL_SLIDERS.SetActive(true);
                // CHANGE SELECTION STATE
                selectionState = "ColorRGB/LL";
                break;

            case "Main/ColorHSV":
                // DISABLE SELECT
                
                // ENABLE WALL SLIDERS

                // CHANGE SELECTION STATE
                selectionState = "ColorHSV/LL";
                break;

            case "Main/EyeAdapt":
                // DISABLE SELECT
                selectUI.SetActive(false);
                // ENABLE EYE ADAPT SLIDERS
                Left_Eye_ALL_SLIDERS.SetActive(true);
                // CHANGE SELECTION STATE
                selectionState = "EyeAdapt/L";
                break;

            case "Main/SquareControl":
                // DISABLE SELECT
                selectUI.SetActive(false);
                // ENABLE 
                squareControl.squareControlEnabled = true;
                textMeshSelect.text = "Use thumbsticks to move and resize";
                // CHANGE SELECTION STATE
                selectionState = "SquareControl/SquareControl";
                break;

            case "Main/KeyboardInput":
                // DISABLE SELECT
                selectUI.SetActive(false);
                // ENABLE KEYBOARD INPUT
                keyboard_ALL.SetActive(true);
                // CHANGE SELECTION STATE
                selectionState = "Keyboard/Keyboard";
                break;

            case "Main/Start":
                // DISABLE SELECT
                selectUI.SetActive(false);
                // ENTER ROOM
                XROrigin.transform.position = playerPosition.transform.position;
                // CHANGE SELECTION STATE
                selectionState = "InGame";
                break;

            default:
                Debug.Log("DEFAULT");
                break;
        }
    }

    void OnBackButtonClick()
    {
        
        switch (selectionState)
        {
            // RGB
            case "ColorRGB/LL":
                // CHANGE TO Main/ColorRGB
                LL_ALL_SLIDERS.SetActive(false);
                selectUI.SetActive(true);
                selectionState = "Main/ColorRGB";
                break;

            case "ColorRGB/LR":
                // CHANGE TO Main/ColorRGB
                LR_ALL_SLIDERS.SetActive(false);
                selectUI.SetActive(true);
                selectionState = "Main/ColorRGB";
                break;

            case "ColorRGB/RL":
                // CHANGE TO Main/ColorRGB
                RL_ALL_SLIDERS.SetActive(false);
                selectUI.SetActive(true);
                selectionState = "Main/ColorRGB";
                break;

            case "ColorRGB/RR":
                // CHANGE TO Main/ColorRGB
                RR_ALL_SLIDERS.SetActive(false);
                selectUI.SetActive(true);
                selectionState = "Main/ColorRGB";
                break;

            // HSV

            // EYE
            case "EyeAdapt/L":
                // CHANGE TO Main/EYE
                Left_Eye_ALL_SLIDERS.SetActive(false);
                selectUI.SetActive(true);
                selectionState = "Main/EyeAdapt";
                break;

            case "EyeAdapt/R":
                // CHANGE TO Main/EYE
                Right_Eye_ALL_SLIDERS.SetActive(false);
                selectUI.SetActive(true);
                selectionState = "Main/EyeAdapt";
                break;

            // ARROWS
            case "SquareControl/SquareControl":
                // CHANGE TO MAIN/SQUARE
                selectUI.SetActive(true);
                squareControl.squareControlEnabled = false;
                textMeshSelect.text = "SquareControl";
                selectionState = "Main/SquareControl";
                break;

            // KEYBOARD
            case "Keyboard/Keyboard":
                // CHANGE TO Main/Keyboard
                keyboard_ALL.SetActive(false);
                selectUI.SetActive(true);
                selectionState = "Main/KeyboardInput";
                break;

            default:
                Debug.Log("DEFAULT");
                break;
        }
    }

/*
-----------------
RGB WALL VALUE SLIDERS
-----------------
*/

    void OnLL_RedValueChanged(float value)
    {
        // Update color value
        LL_RedValue = value;
        textMeshLLR.text = value.ToString();

        // UPDATE WALL COLOR
        Color color;
        color = stereoColor.RGBValuesToColor(LL_RedValue, LL_GreenValue, LL_BlueValue);
        stereoColor.SetWallColor(color, LLWall);
    }

    void OnLL_GreenValueChanged(float value)
    {
        // Update color value
        LL_GreenValue = value;
        textMeshLLG.text = value.ToString();

        // UPDATE WALL COLOR
        Color color;
        color = stereoColor.RGBValuesToColor(LL_RedValue, LL_GreenValue, LL_BlueValue);
        stereoColor.SetWallColor(color, LLWall);
    }

    void OnLL_BlueValueChanged(float value)
    {
        // Update color value
        LL_BlueValue = value;
        textMeshLLB.text = value.ToString();

        // UPDATE WALL COLOR
        Color color;
        color = stereoColor.RGBValuesToColor(LL_RedValue, LL_GreenValue, LL_BlueValue);
        stereoColor.SetWallColor(color, LLWall);
    }

    void OnLR_RedValueChanged(float value)
    {
        // Update color value
        LR_RedValue = value;

        // UPDATE WALL COLOR
        Color color;
        color = stereoColor.RGBValuesToColor(LR_RedValue, LR_GreenValue, LR_BlueValue);
        stereoColor.SetWallColor(color, LRWall);
    }

    void OnLR_GreenValueChanged(float value)
    {
        // Update color value
        LR_GreenValue = value;

        // UPDATE WALL COLOR
        Color color;
        color = stereoColor.RGBValuesToColor(LR_RedValue, LR_GreenValue, LR_BlueValue);
        stereoColor.SetWallColor(color, LRWall);
    }

    void OnLR_BlueValueChanged(float value)
    {
        // Update color value
        LR_BlueValue = value;

        // UPDATE WALL COLOR
        Color color;
        color = stereoColor.RGBValuesToColor(LR_RedValue, LR_GreenValue, LR_BlueValue);
        stereoColor.SetWallColor(color, LRWall);
    }

    void OnRL_RedValueChanged(float value)
    {
        // Update color value
        RL_RedValue = value;

        // UPDATE WALL COLOR
        Color color;
        color = stereoColor.RGBValuesToColor(RL_RedValue, RL_GreenValue, RL_BlueValue);
        stereoColor.SetWallColor(color, RLWall);
    }

    void OnRL_GreenValueChanged(float value)
    {
        // Update color value
        RL_GreenValue = value;

        // UPDATE WALL COLOR
        Color color;
        color = stereoColor.RGBValuesToColor(RL_RedValue, RL_GreenValue, RL_BlueValue);
        stereoColor.SetWallColor(color, RLWall);
    }

    void OnRL_BlueValueChanged(float value)
    {
        // Update color value
        RL_BlueValue = value;

        // UPDATE WALL COLOR
        Color color;
        color = stereoColor.RGBValuesToColor(RL_RedValue, RL_GreenValue, RL_BlueValue);
        stereoColor.SetWallColor(color, RLWall);
    }

    void OnRR_RedValueChanged(float value)
    {
        // Update color value
        RR_RedValue = value;

        // UPDATE WALL COLOR
        Color color;
        color = stereoColor.RGBValuesToColor(RR_RedValue, RR_GreenValue, RR_BlueValue);
        stereoColor.SetWallColor(color, RRWall);
    }

    void OnRR_GreenValueChanged(float value)
    {
        // Update color value
        RR_GreenValue = value;

        // UPDATE WALL COLOR
        Color color;
        color = stereoColor.RGBValuesToColor(RR_RedValue, RR_GreenValue, RR_BlueValue);
        stereoColor.SetWallColor(color, RRWall);
    }

    void OnRR_BlueValueChanged(float value)
    {
        // Update color value
        RR_BlueValue = value;

        // UPDATE WALL COLOR
        Color color;
        color = stereoColor.RGBValuesToColor(RR_RedValue, RR_GreenValue, RR_BlueValue);
        stereoColor.SetWallColor(color, RRWall);
    }

/*
-----------------
EYE VALUE SLIDERS
-----------------
*/

    void OnLERValueChanged(float value)
    {
        // UPDATE EYE SCREEN COLOR
        Left_Eye_Red_Value = value;

        Color color;
        color = stereoColor.RGBValuesToColor(Left_Eye_Red_Value, Left_Eye_Green_Value, Left_Eye_Blue_Value);
        ColorFade.Instance.EyeAdaptToColor(color);
    }

    void OnLEGValueChanged(float value)
    {
        // UPDATE EYE SCREEN COLOR
        Left_Eye_Green_Value = value;

        Color color;
        color = stereoColor.RGBValuesToColor(Left_Eye_Red_Value, Left_Eye_Green_Value, Left_Eye_Blue_Value);
        ColorFade.Instance.EyeAdaptToColor(color);
    }

    void OnLEBValueChanged(float value)
    {
        // UPDATE EYE SCREEN COLOR
        Left_Eye_Blue_Value = value;

        Color color;
        color = stereoColor.RGBValuesToColor(Left_Eye_Red_Value, Left_Eye_Green_Value, Left_Eye_Blue_Value);
        ColorFade.Instance.EyeAdaptToColor(color);
    }

    void OnRERValueChanged(float value)
    {
        // UPDATE EYE SCREEN COLOR
        Right_Eye_Red_Value = value;
        
        Color color;
        color = stereoColor.RGBValuesToColor(Right_Eye_Red_Value, Right_Eye_Green_Value, Right_Eye_Blue_Value);
        ColorFade.Instance.EyeAdaptToColor(color);
    }

    void OnREGValueChanged(float value)
    {
        // UPDATE EYE SCREEN COLOR
        Right_Eye_Green_Value = value;

        Color color;
        color = stereoColor.RGBValuesToColor(Right_Eye_Red_Value, Right_Eye_Green_Value, Right_Eye_Blue_Value);
        ColorFade.Instance.EyeAdaptToColor(color);
    }

    void OnREBValueChanged(float value)
    {
        // UPDATE EYE SCREEN COLOR
        Right_Eye_Blue_Value = value;

        Color color;
        color = stereoColor.RGBValuesToColor(Right_Eye_Red_Value, Right_Eye_Green_Value, Right_Eye_Blue_Value);
        ColorFade.Instance.EyeAdaptToColor(color);
    }

}
