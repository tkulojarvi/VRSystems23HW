using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class TapeControl : MonoBehaviour
{
    // Variables to store initial position, rotation, and rigidbody of the tape
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    Rigidbody tapeRigidbody;

    // Variables for tape properties and respawn handling
    public float timerDuration = 10f;
    private bool dropped = false;
    private float rotationSpeed = 45f;
    private string tapeName;

    // Flag to indicate whether the tape is animated
    public bool rotate = true;
    // Flag to indicate object is grabbed
    public bool grabbed = false;

    void Start()
    {
        // Store the original position and rotation
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // Get the rigidbody component of the tape
        tapeRigidbody = gameObject.GetComponent<Rigidbody>();

        // Get the name of the tape
        tapeName = gameObject.name;

        // Add event listeners for grab interactions
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(SetGrabbedTrue);
        grabInteractable.selectExited.AddListener(SetGrabbedFalse);
    }

    void Update()
    {
        // ANIMATION

        // Check if the tape is grabbed and should be rotated
        if (rotate == true)
        {
            // Custom is a special boy
            if (tapeName != "TAPE_CUSTOM")
            {
                // Rotation
                transform.Rotate(-Vector3.forward, rotationSpeed * Time.deltaTime);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the tape collided with the floor
        if (other.CompareTag("Respawn Collider"))
        {
            // Set the dropped flag and respawn the tape
            dropped = true;
            RespawnTape();
        }
    }

    void RespawnTape()
    {
        // Handle respawning the tape to it's original position here.
        // Turn off rigidbody physics temporarily
        tapeRigidbody.isKinematic = true;

        // Reset position and rotation
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        // Turn physics back on
        tapeRigidbody.isKinematic = false;
        // Gravity back off
        tapeRigidbody.useGravity = false;

        // Enable rotation animation
        rotate = true;

        // Reset dropped flag
        dropped = false;
    }

    private IEnumerator StartTimerCoroutine()
    {

        // If the player leaves the item on the table, it will despawn/respawn after some time.

        // Initialize elapsed time
        float elapsedTime = 0f;
        
        // Continue until the timer duration is reached, or the tape is grabbed again, or falls
        while (elapsedTime < timerDuration && dropped == false && grabbed == false)
        {
            // Update the elapsed time
            elapsedTime += Time.deltaTime;
            // Wait for the next frame
            yield return null;
        }

        // Respawn the tape if the timer duration is reached and it hasn't been grabbed
        if(dropped == false && grabbed == false)
        {
            RespawnTape();
        }
    }

    // Set the grabbed flag to true when the tape is grabbed
    public void SetGrabbedTrue(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.tag == "Left Hand" || args.interactorObject.transform.tag == "Right Hand")
        {
            // Disable animation
            rotate = false;
            grabbed = true;
        }
    }

    // Set the grabbed flag to false when the tape is let go
    public void SetGrabbedFalse(SelectExitEventArgs args)
    {
        if (args.interactorObject.transform.tag == "Left Hand" || args.interactorObject.transform.tag == "Right Hand")
        {
            // Enable gravity and start the respawn timer
            grabbed = false;
            tapeRigidbody.useGravity = true;
            StartTimerCoroutine();
        }
    }
}
