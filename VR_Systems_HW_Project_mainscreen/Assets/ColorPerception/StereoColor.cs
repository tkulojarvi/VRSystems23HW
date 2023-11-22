using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class StereoColor : MonoBehaviour
{
    public Material rightMaterial;
    public Renderer leftTest;
    public Renderer rightTest;
    static bool init = false;
    bool main = false;

    Light rightLight = null;
    Light leftLight = null;
    int state = 0;

    void Start()
    {
        if (init)
            return;
        init = true;
        main = true;

        leftTest.material.color = new Color(1,1,0.8f);
        rightTest.material.color = new Color(1,1,0.8f);

        GameObject right = Instantiate(transform.gameObject, null);
        right.SetLayerRecursively(7);
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(state == 0)
            {
                state = 1;
                rightLight.color = leftLight.color;
                leftLight.color = Color.white;
            }
            else if (state == 1)
            {
                state = 2;
                leftLight.color = Color.white;
                rightLight.color = Color.white;
            }
            else if(state == 2)
            {
                state = 0;
                leftLight.color = new Color(1, 1, 0.8f);
                rightLight.color = Color.white;
            }
        }
    }
}
