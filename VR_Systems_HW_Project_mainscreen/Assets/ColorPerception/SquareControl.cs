using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class SquareControl : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rb_L;
    private Rigidbody rb_R;

    bool moveUp = false;
    bool moveDown = false;
    bool moveCloser = false;
    bool moveAway = false;

    Vector3 upMovement;
    Vector3 downMovement;
    Vector3 rightMovement;
    Vector3 leftMovement;

    public GameObject squareLeft;
    public GameObject squareRight;
    private GameObject right;

    public float resizeAmount = 0.1f;

    void Start()
    {
        rb_L = squareLeft.GetComponent<Rigidbody>();
        rb_R = squareRight.GetComponent<Rigidbody>();

        // Used after adding seperate movement for the right environment, not right now
        right = GameObject.Find("Right Environment");
    }

    void OnEnable()
    {
        // Called when the script is enabled
        // Enable the InputSystem controls
        InputSystem.EnableDevice(Keyboard.current);
    }

    void OnDisable()
    {
        // Called when the script is disabled
        // Disable the InputSystem controls
        InputSystem.DisableDevice(Keyboard.current);
    }

    void Update()
    {
        // KEYBOARD
        // Up
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            moveUp = true;
        }
        // Down
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            moveDown = true;
        }
        // Right
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            moveCloser = true;
        }
        // Left
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            moveAway = true;
        }
        // Resize Up
        else if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            ResizeSquares(1 + resizeAmount);
        }
        // Resize Down
        else if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            ResizeSquares(1 - resizeAmount);
        }

        // Check for key releases
        if (Keyboard.current.upArrowKey.wasReleasedThisFrame)
        {
            moveUp = false;
            upMovement = Vector3.zero;
        }
        else if (Keyboard.current.downArrowKey.wasReleasedThisFrame)
        {
            moveDown = false;
            downMovement = Vector3.zero;
        }
        else if (Keyboard.current.rightArrowKey.wasReleasedThisFrame)
        {
            moveCloser = false;
            rightMovement = Vector3.zero;
        }
        else if (Keyboard.current.leftArrowKey.wasReleasedThisFrame)
        {
            moveAway = false;
            leftMovement = Vector3.zero;
        }

        // Adjust the values continuously while the keys are held down
        if (moveUp)
        {
            upMovement = Vector3.up;
            
            squareLeft.transform.Translate(upMovement.normalized * speed * Time.deltaTime, Space.World);
            squareRight.transform.Translate(upMovement.normalized * speed * Time.deltaTime, Space.World);
        }
        else if (moveDown)
        {
            downMovement = -Vector3.up;
            
            squareLeft.transform.Translate(downMovement.normalized * speed * Time.deltaTime, Space.World);
            squareRight.transform.Translate(downMovement.normalized * speed * Time.deltaTime, Space.World);
        }
        else if (moveCloser)
        {
            rightMovement = -Vector3.right;
            leftMovement = Vector3.right;

            squareLeft.transform.Translate(rightMovement.normalized * speed * Time.deltaTime, Space.World);
            squareRight.transform.Translate(leftMovement.normalized * speed * Time.deltaTime, Space.World);
        }
        else if (moveAway)
        {
            leftMovement = Vector3.right;
            rightMovement = -Vector3.right;

            squareLeft.transform.Translate(leftMovement.normalized * speed * Time.deltaTime, Space.World);
            squareRight.transform.Translate(rightMovement.normalized * speed * Time.deltaTime, Space.World);
        }
    }

    void ResizeSquares(float scale)
    {
        squareLeft.transform.localScale *= scale;
        squareRight.transform.localScale *= scale;
    }
}
