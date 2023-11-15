using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class DisableHandModel : MonoBehaviour
{
    // USED FOR DISABLING THE HAND MODEL WHEN GRABBING THINGS.
    // GameObjects representing left and right hand models.
    public GameObject leftHandModel;
    public GameObject rightHandModel;


    // Start is called before the first frame update
    void Start()
    {
        // Get the XRGrabInteractable component attached to this GameObject.
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();

        // Subscribe to the selectEntered and selectExited events.
        grabInteractable.selectEntered.AddListener(HideGrabbingHand);
        grabInteractable.selectExited.AddListener(ShowGrabbingHand);
    }

    public void HideGrabbingHand(SelectEnterEventArgs args)
    {
        if(args.interactorObject.transform.tag == "Left Hand")
        {
            leftHandModel.SetActive(false);
        }

        else if(args.interactorObject.transform.tag == "Right Hand")
        {
            rightHandModel.SetActive(false);
        }
    }

    public void ShowGrabbingHand(SelectExitEventArgs args)
    {
        if(args.interactorObject.transform.tag == "Left Hand")
        {
            leftHandModel.SetActive(true);
        }

        else if(args.interactorObject.transform.tag == "Right Hand")
        {
            rightHandModel.SetActive(true);
        }
    }
}
