using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    
    void Start()
    {
        // Get the button/slider components
        left = arrowRight.GetComponent<Button>();
        right = arrowLeft.GetComponent<Button>();
        up = arrowUp.GetComponent<Button>();
        down = arrowDown.GetComponent<Button>();
        select = selectUI.GetComponent<Button>();

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
    }

    void Update()
    {
        
    }






    void OnLeftButtonClick()
    {
        // Do something when the left button is clicked
        Debug.Log("Left Button Clicked!");
    }

    void OnRightButtonClick()
    {
        // Do something different when the right button is clicked
        Debug.Log("Right Button Clicked!");
    }

    void OnUpButtonClick()
    {
        // Do something different when the right button is clicked
        Debug.Log("up Button Clicked!");
    }

    void OnDownButtonClick()
    {
        // Do something different when the right button is clicked
        Debug.Log("down Button Clicked!");
    }

    void OnSelectButtonClick()
    {
        // Do something different when the right button is clicked
        Debug.Log("select Button Clicked!");
    }







    void OnLL_RedValueChanged(float value)
    {
        // Do something different when the right button is clicked
        Debug.Log("Slider changed!");
    }

    void OnLL_GreenValueChanged(float value)
    {
        // Do something different when the right button is clicked
        Debug.Log("Slider changed!");
    }

    void OnLL_BlueValueChanged(float value)
    {
        // Do something different when the right button is clicked
        Debug.Log("Slider changed!");
    }

    void OnLR_RedValueChanged(float value)
    {
        // Do something different when the right button is clicked
        Debug.Log("Slider changed!");
    }

    void OnLR_GreenValueChanged(float value)
    {
        // Do something different when the right button is clicked
        Debug.Log("Slider changed!");
    }

    void OnLR_BlueValueChanged(float value)
    {
        // Do something different when the right button is clicked
        Debug.Log("Slider changed!");
    }

    void OnRL_RedValueChanged(float value)
    {
        // Do something different when the right button is clicked
        Debug.Log("Slider changed!");
    }

    void OnRL_GreenValueChanged(float value)
    {
        // Do something different when the right button is clicked
        Debug.Log("Slider changed!");
    }

    void OnRL_BlueValueChanged(float value)
    {
        // Do something different when the right button is clicked
        Debug.Log("Slider changed!");
    }

    void OnRR_RedValueChanged(float value)
    {
        // Do something different when the right button is clicked
        Debug.Log("Slider changed!");
    }

    void OnRR_GreenValueChanged(float value)
    {
        // Do something different when the right button is clicked
        Debug.Log("Slider changed!");
    }

    void OnRR_BlueValueChanged(float value)
    {
        // Do something different when the right button is clicked
        Debug.Log("Slider changed!");
    }






    

    void OnLERValueChanged(float value)
    {
        // Do something different when the right button is clicked
        Debug.Log("Slider changed!");
    }

    void OnLEGValueChanged(float value)
    {
        // Do something different when the right button is clicked
        Debug.Log("Slider changed!");
    }

    void OnLEBValueChanged(float value)
    {
        // Do something different when the right button is clicked
        Debug.Log("Slider changed!");
    }

    void OnRERValueChanged(float value)
    {
        // Do something different when the right button is clicked
        Debug.Log("Slider changed!");
    }

    void OnREGValueChanged(float value)
    {
        // Do something different when the right button is clicked
        Debug.Log("Slider changed!");
    }

    void OnREBValueChanged(float value)
    {
        // Do something different when the right button is clicked
        Debug.Log("Slider changed!");
    }
}
