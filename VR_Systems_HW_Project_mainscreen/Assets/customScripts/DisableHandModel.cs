using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/*
 * DisableHandModel
 * 
 * Overview:
 * This script is designed to disable the hand models when they are grabbing objects.
 * The script allows for hiding the left and right hand models independently when grabbing objects and restoring them when the grab is released.
 * 
 * Components:
 * - leftHandModel, rightHandModel: GameObjects representing the left and right hand models.
 * 
 * Functions:
 * - Start(): Retrieves the XRGrabInteractable component attached to this GameObject,
 *   and subscribes to the selectEntered and selectExited events to trigger hand model visibility changes.
 * - HideGrabbingHand(SelectEnterEventArgs args): Hides the hand model of the interacting hand when an object is grabbed.
 *   Checks the tag of the interacting hand and deactivates the corresponding hand model.
 * - ShowGrabbingHand(SelectExitEventArgs args): Restores the hand model visibility when the grab interaction is released.
 *   Checks the tag of the interacting hand and activates the corresponding hand model.
 * 
 */

public class DisableHandModel : MonoBehaviour
{
    public GameObject leftHandModel;
    public GameObject rightHandModel;

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
