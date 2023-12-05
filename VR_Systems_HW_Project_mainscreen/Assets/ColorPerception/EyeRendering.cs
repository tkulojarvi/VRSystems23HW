using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;

public class EyeRendering : MonoBehaviour
{
    public Camera leftEyeCamera;
    public Camera rightEyeCamera;

    [SerializeField] public LayerMask leftEyeCullingMask;
    [SerializeField] public LayerMask rightEyeCullingMask;

    void Start()
    {
        // Ensure cameras exist in the scene
        if (leftEyeCamera == null || rightEyeCamera == null)
        {
            Debug.LogError("Left or right eye camera not assigned!");
            return;
        }

        // Copy camera settings
        //leftEyeCamera.CopyFrom(Camera.main);
        //rightEyeCamera.CopyFrom(Camera.main);

        // Set culling masks
        leftEyeCamera.cullingMask = leftEyeCullingMask;
        rightEyeCamera.cullingMask = rightEyeCullingMask;

        Debug.Log("Left Eye Culling Mask: " + leftEyeCullingMask.value);
        Debug.Log("Right Eye Culling Mask: " + rightEyeCullingMask.value);
    }

    void Update()
    {
        // Position and rotate the cameras based on the HMD
        leftEyeCamera.transform.position = Camera.main.transform.position;
        leftEyeCamera.transform.rotation = Camera.main.transform.rotation;

        rightEyeCamera.transform.position = Camera.main.transform.position;
        rightEyeCamera.transform.rotation = Camera.main.transform.rotation;

        // Set the cameras to render only left or right eye
        leftEyeCamera.stereoTargetEye = StereoTargetEyeMask.Left;
        rightEyeCamera.stereoTargetEye = StereoTargetEyeMask.Right;

        // Render left eye
        leftEyeCamera.Render();

        // Render right eye
        rightEyeCamera.Render();

        // Reset render target to the main screen
        Graphics.SetRenderTarget(null);
    }
}

