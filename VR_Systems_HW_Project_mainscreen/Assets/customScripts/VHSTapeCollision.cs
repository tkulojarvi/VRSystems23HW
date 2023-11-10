using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is attached to the invisible GameObject that represents the VHS insertion hole.
// It is used for correctly pulling the tape in once the player moves it into range, and orients the tape correctly.

// LOGIC:
// 1) detect if tape is colliding with the vhs player via OnTriggerEnter
// 2) detect if the orientation of the tape is the same as the orientation of the insertion hole.
// 3) if so, disable physics from the tape and move it via it's transform
// 4) the movement happens in Update
// 5) once the tape is in the final position, stop Update
// 6) finally check the name of the tape object, to determine the room number player moves to
// 7) pass data to RoomSwitch to handle room movement

public class VHSTapeCollision : MonoBehaviour
{

    // VCR
    public float orientationThreshold = 5.0f; // Define a threshold for the acceptable orientation difference.
    private Vector3 targetPosition; // The final position where the tape should be placed.
    private Quaternion targetRotation; // The final rotation the tape should have.
    public float movementSpeed = 0.5f; // Adjust the movement speed as needed.
    private bool isPullingTape = false; // Flag to indicate if the tape is being pulled.
    bool correctOrientation = false;    // orientation check
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

            
            //Debug.Log("Colliding object's name: " + tapeName);

            // detect if the orientation of the tape is the same as the orientation of this object
            correctOrientation = checkOrientation();

            if(correctOrientation == true) {

                // if so, pull the tape in
                //Debug.Log("correct or");
                pullInTape();
            }
            
        }
    }

    private bool checkOrientation()
    {
        // Calculate the orientation difference between the tape and the hole.
        float angleDifference = Quaternion.Angle(transform.rotation, tape.transform.rotation);

        //Debug.Log(angleDifference);

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
    }

    private void checkNumber()
    {
        // tape name

        // player moves to the room with that number - NOT IMMEDIATELY, WAIT FEW SECONDS
        // handle this in another script but call function here and pass room number
        //Debug.Log("success");
        RoomSwitch.EnterRoom(tapeName);
    }
}
