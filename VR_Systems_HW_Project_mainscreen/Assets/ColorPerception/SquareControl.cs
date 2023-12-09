using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.XR;

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

    public float resizeAmount = 0.1f;

    // XR Input
    public GameObject inputDataObject;
    private InputData _inputData;

    public bool squareControlEnabled = false;

    void Start()
    {
        rb_L = squareLeft.GetComponent<Rigidbody>();
        rb_R = squareRight.GetComponent<Rigidbody>();

        _inputData = inputDataObject.GetComponent<InputData>();
    }

    void Update()
    {
        if(squareControlEnabled == true)
        {
            // MOVEMENT
            if (_inputData._leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out Vector2 leftAnalog))
            {
                float threshold = 0.5f; // Threshold for recognizing input

                Debug.Log(leftAnalog.y);

                if((leftAnalog.x > threshold))
                {
                    // RIGHT
                    moveCloser = true;
                }
                else if((leftAnalog.x < -threshold))
                {
                    // LEFT
                    moveAway = true;
                }

                else if (leftAnalog.y > threshold) 
                {
                    // UP
                    moveUp = true;
                }
                else if(leftAnalog.y < -threshold)
                {
                    // DOWN
                    moveDown = true;
                }

                else
                {
                    // Check for joystick release
                    
                    moveUp = false;
                    upMovement = Vector3.zero;
                    moveDown = false;
                    downMovement = Vector3.zero;
                    moveCloser = false;
                    rightMovement = Vector3.zero;
                    moveAway = false;
                    leftMovement = Vector3.zero;
                    
                }
            }

            // RESIZER
            // Up and Down
            if (_inputData._rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out Vector2 rightAnalog))
            {
                float threshold = 0.5f;

                if (rightAnalog.y > threshold || rightAnalog.x > threshold) 
                {
                    // UP
                    ResizeSquares(1 + resizeAmount);
                }
                else if(rightAnalog.y < -threshold || rightAnalog.x < -threshold)
                {
                    // DOWN
                    ResizeSquares(1 - resizeAmount);
                }
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
    }

    void ResizeSquares(float scale)
    {
        squareLeft.transform.localScale *= scale;
        squareRight.transform.localScale *= scale;
    }
}
