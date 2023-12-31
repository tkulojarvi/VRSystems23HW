using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VHSTapeCollision : MonoBehaviour
{
    public float orientationThreshold = 10.0f; // Define a threshold for the acceptable orientation difference.
    private Vector3 targetPosition; // The final position where the tape should be placed.
    //private Quaternion targetRotation;
    public float movementSpeed = 0.5f; // Adjust the movement speed as needed.
    private bool isPullingTape = false; // Flag to indicate if the tape is being pulled.
    bool correctOrientation = false; // Flag to indicate if tape orientation is correct.

    // TAPE
    private GameObject tape;    // tape object
    Rigidbody tapeRigidbody;    // THE RIGIDBODY of the tape object
    private string tapeName; // tape number
    private Vector3 startPosition;  // Tape initial position.

    private void Update()
    {
        if (isPullingTape)
        {
            // Smoothly move the tape towards the target position
            tape.transform.position = Vector3.Lerp(tape.transform.position, targetPosition, Time.deltaTime * movementSpeed);
            //tape.transform.rotation = Quaternion.Lerp(tape.transform.rotation, targetRotation, Time.deltaTime * movementSpeed);

            // Check if the tape has reached the target position
            if (Vector3.Distance(tape.transform.position, targetPosition) < 0.05f)
            {
                // The action is completed, stop updating.
                isPullingTape = false;

                // The action is completed, proceed to checkNumber().
                checkNumber();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // detect if tape is colliding with the vhs player
        if (other.CompareTag("VHSTapeTag"))
        {
            // get the object
            tape = other.gameObject;
            // create a string variable for the number of the tape
            tapeName = tape.name;

            // detect if the orientation of the tape is the same as the orientation of this object
            correctOrientation = checkOrientation();

            if(correctOrientation == true) {
                // if so, pull the tape in
                XRGrabInteractable xri = other.GetComponent<XRGrabInteractable>();
                xri.movementType = XRBaseInteractable.MovementType.Kinematic;
                xri.trackPosition = false;
                pullInTape();
            }
        }
    }

    private bool checkOrientation()
    {
        // Calculate the orientation difference between the tape and the hole.
        float angleDifference = Quaternion.Angle(transform.rotation, tape.transform.rotation);

        // Compare the angle difference with the orientation threshold.
        if (angleDifference < orientationThreshold)
        {
            return true; // Orientation is correct.
        }
        else
        {
            return false; // Orientation is incorrect.
        }
    }

    private void pullInTape()
    {
        // set tape to center
        SetTapePositionToVCR();
        
        // Set tape start pos
        startPosition = transform.position;

        // Set target pos
        targetPosition = startPosition + new Vector3(0, 0, -0.2f);
        //targetRotation = transform.rotation;

        // Get a reference to the tape's Rigidbody.
        tapeRigidbody = tape.GetComponent<Rigidbody>();

        // TURN OFF PHYSICS
        tapeRigidbody.isKinematic = true;   
        tapeRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
        tapeRigidbody.constraints |= RigidbodyConstraints.FreezeRotation;

        // Start pulling the tape.
        isPullingTape = true;

        // set texture
        MonitorRender.Instance.ActivateScreen2();
        
        // Play audio cue.
        MusicManager.Instance.PlayAudioOnce();
    }

    private void SetTapePositionToVCR()
    {
        // Set tape position to align with the VCR.
        Vector3 tapePosition = tape.transform.position;
        tapePosition.x = this.transform.position.x;
        tapePosition.y = this.transform.position.y;
        tape.transform.position = tapePosition;

        // Set tape rotation to align with the VCR.
        Vector3 tapeRotation = tape.transform.rotation.eulerAngles;
        tapeRotation = this.transform.rotation.eulerAngles;
        tape.transform.rotation = Quaternion.Euler(tapeRotation);
    }

    private void checkNumber()
    {
        // Inform the RoomSwitch script
        RoomSwitch.Instance.EnterRoom(tapeName);
    }
}
