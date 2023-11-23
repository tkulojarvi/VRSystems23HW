using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorRender : MonoBehaviour
{
    public GameObject screen1texture;
    public GameObject screen2texture;
    public GameObject objectDisable;

    // Singleton
    public static MonitorRender Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ActivateScreen1()
    {
        screen1texture.SetActive(true);
        screen2texture.SetActive(false);
    }

    public void ActivateScreen2()
    {
        screen2texture.SetActive(true);
        screen1texture.SetActive(false);
    }

    public void disableMesh()
    {
        objectDisable.SetActive(false);
    }

    public void enableMesh()
    {
        objectDisable.SetActive(true);
    }
}
