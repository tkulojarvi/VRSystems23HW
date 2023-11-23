using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * VHSTapeCollision
 * 
 * Overview:
 * This script is attached to an invisible GameObject representing the VHS insertion hole in a Unity scene.
 * It manages the logic for correctly pulling a virtual tape into the VHS player once the player moves it into range.
 * The script detects the tape's collision, checks its orientation, disables physics, and smoothly moves it to the final position.
 * After reaching the final position, it retrieves the tape's name to determine the room number the player moves to.
 * The script interfaces with the RoomSwitch script to handle room transitions based on the inserted tape.
 * 
 * Components:
 * - orientationThreshold: The acceptable orientation difference between the tape and the insertion hole.
 * - targetPosition, targetRotation: The final position and rotation where the tape should be placed.
 * - movementSpeed: Adjusts the speed at which the tape is moved into the player.
 * - isPullingTape: Flag indicating if the tape is being pulled.
 * - correctOrientation: Flag indicating if the tape's orientation matches the insertion hole's orientation.
 * - tape: Reference to the GameObject representing the virtual tape.
 * - tapeRigidbody: Reference to the Rigidbody component of the virtual tape.
 * - tapeName: The name of the virtual tape, representing the room number.
 * - startPosition: The initial position of the virtual tape.
 * 
 * Functions:
 * - Update(): Checks if the tape is being pulled, smoothly moves it to the target position, and triggers actions upon completion.
 * - OnTriggerEnter(Collider other): Detects when the virtual tape collides with the VHS player and initiates the pulling process.
 * - checkOrientation(): Calculates the orientation difference between the tape and the insertion hole and checks if it's within the threshold.
 * - pullInTape(): Sets up the target position, disables physics on the tape, and starts pulling it into the player.
 * - checkNumber(): Retrieves the tape's name (room number) and calls the RoomSwitch script to handle the room transition.
 * 
 */

public class VHSTapeCollision : MonoBehaviour
{
    public float orientationThreshold = 10.0f; // Define a threshold for the acceptable orientation difference.
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

            // detect if the orientation of the tape is the same as the orientation of this object
            correctOrientation = checkOrientation();

            if(correctOrientation == true) {
                // if so, pull the tape in
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
    }

    private void SetTapePositionToVCR()
    {
        Vector3 tapePosition = tape.transform.position;
        tapePosition.x = this.transform.position.x;
        tapePosition.y = this.transform.position.y;
        tape.transform.position = tapePosition;

        Vector3 tapeRotation = tape.transform.rotation.eulerAngles;
        tapeRotation = this.transform.rotation.eulerAngles;
        tape.transform.rotation = Quaternion.Euler(tapeRotation);
    }

    private void checkNumber()
    {
        RoomSwitch.Instance.EnterRoom(tapeName);
    }
}
