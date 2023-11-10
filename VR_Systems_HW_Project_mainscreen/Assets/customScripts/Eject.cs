using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.Interaction.Toolkit;

public class Eject : MonoBehaviour
{
    // Eject is for exiting a Scene/Room.
    // When the player presses the button on the VCR, this code executes and player moves to the main scene.
    // Note, player can only move to the main scene with this

    void Start()
    {
        GetComponent<XRSimpleInteractable>().selectEntered.AddListener(x => EjectTape());
    }

    public void EjectTape()
    {
        // exit the room when the eject button is pressed
        RoomSwitch.ExitRoom();
    }
}
