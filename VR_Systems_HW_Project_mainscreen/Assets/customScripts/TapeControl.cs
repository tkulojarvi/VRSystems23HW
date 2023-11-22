using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/*
 * TapeControl
 */

public class TapeControl : MonoBehaviour
{
    // Variables to store initial position, rotation, and rigidbody of the tape
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    Rigidbody tapeRigidbody;

    // Variables for tape properties and respawn handling
    public float timerDuration = 5f;
    private float rotationSpeed = 45f;
    private string tapeName;

    // Flag to indicate whether the tape is animated
    public bool rotate = true;
    // Flag to indicate object is grabbed
    public bool grabbed = false;
    private bool inOriginalPos = true;
    private bool respawnCoroutineRunning = false;

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
        // Check if the tape should be rotated
        if (rotate == true)
        {
            // Custom is a special boy
            if (tapeName != "TAPE_CUSTOM")
            {
                // Rotation
                transform.Rotate(-Vector3.forward, rotationSpeed * Time.deltaTime);
            }
        }

        // Check if tape is at original pos
        TapeMovementDetector();
        
        // If not, it has either been grabbed by the player, it is on the table unused,
        // or it has fallen out of reach.
        if (inOriginalPos == false && respawnCoroutineRunning == false)
        {
            // If it is not grabbed, start a timer for respawning it.
            if (grabbed == false)
            {
                // Disable animation
                rotate = false;
                // Enable gravity
                tapeRigidbody.useGravity = true;
                // Start timer
                StartCoroutine(TimerForRespawn());
            }
        }
    }

    void TapeMovementDetector()
    {
        // Check if the current position is different from the initial position

        if (Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            //Debug.Log("Object has moved!");
            inOriginalPos = false;
        }

        else if (Vector3.Distance(transform.position, originalPosition) <= 0.01f)
        {
            //Debug.Log("Object is at the start pos!");
            inOriginalPos = true;
        }
    }

    private IEnumerator TimerForRespawn()
    {
        respawnCoroutineRunning = true;
        
        // Initialize elapsed time
        float elapsedTime = 0f;

        while (elapsedTime < timerDuration)
        {
            // Update the elapsed time
            elapsedTime += Time.deltaTime;
            // Wait for the next frame
            yield return null;
        }

        // If object has still not been grabbed, respawn it.
        if(grabbed == false) 
        {
            respawnCoroutineRunning = false;
            RespawnTape();
        }

        else 
        {
            respawnCoroutineRunning = false;
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
            // Enable gravity
            grabbed = false;
            tapeRigidbody.useGravity = true;
        }
    }
}
