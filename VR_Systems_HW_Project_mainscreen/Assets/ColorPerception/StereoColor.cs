using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Networking;

public class StereoColor : MonoBehaviour
{
    public Color[] colorSet1 = null;
    public Color[] colorSet2 = null;
    Color[] colorSet;
    public Material rightMaterial;
    public Renderer leftTest;
    public Renderer rightTest;
    static bool init = false;
    bool main = false;

    InputData inputData;

    Light rightLight = null;
    Light leftLight = null;

    int state = 0;
    int matchIndex = 0;
    int playerIndex = 0;
    int correctCount = 0;
    Renderer match, playerSelection;
    bool inputHold = false;

    void Start()
    {
        if (init)
            return;
        init = true;
        main = true;

        inputData = GetComponent<InputData>();

        GameObject right = Instantiate(transform.gameObject, null);
        right.SetLayerRecursively(rightTest.gameObject.layer);
        foreach(Renderer r in right.transform.GetComponentsInChildren<Renderer>())
        {
            r.sharedMaterial = rightMaterial;
        }
        foreach(Light l in right.GetComponentsInChildren<Light>())
        {
            rightLight = l;
            l.cullingMask *= 2;
            l.color = Color.white;
        }
        leftLight = GetComponentInChildren<Light>();
    }

    private void Update()
    {
        if(!main) return;

        colorSet = state==0 ? colorSet1 : colorSet2;
        match = state==0 ? leftTest : rightTest;
        playerSelection = state==0 ? rightTest : leftTest;

        if(inputData._rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out bool value) && value)
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
            if(!inputHold)
            {
                if(matchIndex < colorSet.Length && matchIndex == playerIndex)
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
