using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

public class StereoColor : MonoBehaviour
{
    public InputAction primary, secondary, adjust;

    public Color[] colorSet1 = null;
    public Color[] colorSet2 = null;
    Color[] colorSet;
    public Material rightMaterial;
    public Renderer leftTest;
    public Renderer rightTest;
    static bool init = false;
    bool main = false;

    InputData inputData;

    public Light rightLight = null;
    public Light leftLight = null;

    int state = 0;
    int matchIndex = 0;
    int playerIndex = 0;
    int correctCount = 0;
    Renderer match, playerSelection;

    float selectTemperature = 10000;

    bool inputHold = false;

    void Start()
    {
        if (init)
            return;
        init = true;
        main = true;

        inputData = GetComponent<InputData>();

        primary.Enable();
        secondary.Enable();
        adjust.Enable();

        GameObject right = Instantiate(transform.gameObject, null);
        right.SetLayerRecursively(rightTest.gameObject.layer);
        foreach(Renderer r in right.transform.GetComponentsInChildren<Renderer>())
        {
            r.sharedMaterial = rightMaterial;
            rightMaterial.SetFloat("_EditorTest", 0);
        }
    }

    private void Update()
    {
        if(!main) return;
        Adjusting();
    }

    void Adjusting()
    {
        match = state == 0 ? leftTest : rightTest;
        playerSelection = state == 0 ? rightTest : leftTest;

        selectTemperature += adjust.ReadValue<Vector2>().x * Time.deltaTime * 3000;
        playerSelection.sharedMaterial.color = colorSet2[playerIndex] * TemperatureToColor(selectTemperature);

        if (primary.WasPressedThisFrame())
        {
            playerIndex = (playerIndex + 1) % colorSet1.Length;
            matchIndex = (matchIndex + 1) % colorSet1.Length;
            match.sharedMaterial.color = colorSet1[matchIndex];
        }
    }

    void Matching()
    {
        colorSet = state == 0 ? colorSet1 : colorSet2;
        match = state == 0 ? leftTest : rightTest;
        playerSelection = state == 0 ? rightTest : leftTest;

        if (inputData._rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out bool value) && value)
        {
            if (!inputHold)
            {
                playerIndex = (playerIndex + 1) % colorSet.Length;
                playerSelection.sharedMaterial.color = colorSet[playerIndex];
                inputHold = true;
            }
        }
        else if (inputData._rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out bool value2) && value2)
        {
            if (!inputHold)
            {
                if (matchIndex < colorSet.Length && matchIndex == playerIndex)
                {
                    correctCount++;
                }
                matchIndex++;
                match.sharedMaterial.color = matchIndex >= colorSet.Length ? Color.white : colorSet[matchIndex];
                inputHold = true;
            }
        }
        else
            inputHold = false;
    }

    public void SetColorLeft(string color)
    {
        if(color == null) return;
        leftTest.sharedMaterial.color = StringToColor(color);
    }

    public void SetColorRight(string color)
    {
        if(color == null) return;
        rightTest.sharedMaterial.color = StringToColor(color);
    }

    public void SetLightColorLeft(string color)
    {
        if(color == null) return;
        leftLight.color = StringToColor(color);
    }

    public void SetLightColorRight(string color)
    {
        if(color == null) return;
        rightLight.color = StringToColor(color);
    }

    Color TemperatureToColor(float temp)
    {
        Color color = Color.black;
        temp /= 100;

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
        if (color[0] == '#')
        {
            Color newCol = Color.white;
            if (ColorUtility.TryParseHtmlString(color, out newCol))
            {
                return newCol;
            }
        }
        string[] rgb = color.Split(',');
        return new Color(int.Parse(rgb[0]) / 255f, int.Parse(rgb[1]) / 255f, int.Parse(rgb[2]) / 255f);
    }

    public void LoadImage(string name)
    {
        StartCoroutine(GetImageAsync(name));
    }

    IEnumerator GetImageAsync(string name)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(name))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                // Get downloaded asset bundle
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                leftTest.sharedMaterial.SetTexture("_Texture2D", texture);
                leftTest.transform.localScale = new Vector3(0.2f, 1f, 0.2f * texture.height / texture.width);
                rightTest.sharedMaterial.SetTexture("_Texture2D", texture);
                rightTest.transform.localScale = new Vector3(0.2f, 1f, 0.2f * texture.height / texture.width);
                Debug.Log("Loaded image");
            }
        }
    }
}
