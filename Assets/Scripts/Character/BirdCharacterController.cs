using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdCharacterController : MonoBehaviour
{
    [SerializeField]
    float glidingSpeed;
    [SerializeField]
    float turnAcceleration;

    [SerializeField]
    float horizontalBankAcceleration;
    [SerializeField]
    float horizontalAngularTiltSpeed;
    [SerializeField]
    float horizontalMaxTilt;
    [SerializeField]
    float horizontalTiltSelfCorrectionSpeed; // Value between 0 and 1, determines how fast the character tilts back to rotation 0 when there is no airborne horizontal input

    [SerializeField]
    float verticalAngularTiltSpeed;
    [SerializeField]
    float verticalMaxTilt;
    [SerializeField]
    float verticalTiltSelfCorrectionSpeed; // Value between 0 and 1, determines how fast the character tilts back to rotation 0 when there is no airborne vertical input

    [SerializeField]
    float bounceForce;

    // Input variables
    float horizontalInput = 0;
    float verticalInput = 0;

    // Movement variables
    float tiltSelfcorrectionT = 0;       // t for the lerp back to 0 rotation, when there is no horizontal input
    float lastVerticalSpeed = 0;
    float turnStartTime = 0;

    Rigidbody rb;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        InputCollection();
        VecticalTilt();
        HorizontalTilt();
    }

    private void FixedUpdate()
    {
        // Move character along forward vector
        Vector3 forwardVelocityVector = transform.forward * glidingSpeed;
        rb.velocity = new Vector3(forwardVelocityVector.x, lastVerticalSpeed + (Physics.gravity.y * Time.deltaTime), forwardVelocityVector.z);
        lastVerticalSpeed = rb.velocity.y;

        Turn();
    }

    void InputCollection()
    {
        float previousHorizontalInput = horizontalInput;
        horizontalInput = Input.GetAxis("Horizontal");

        // If signchange or increment from 0, relative to last horizontal input, turning started
        if(Mathf.Sign(horizontalInput) != Mathf.Sign(previousHorizontalInput) || (previousHorizontalInput == 0 && Mathf.Abs(horizontalInput) > 0))
        {
            turnStartTime = Time.time;
        }

        verticalInput = Input.GetAxis("Vertical");
    }

    void HorizontalTilt()
    {
        // Transform euler angle to negative of positive
        float currentRotation = (transform.eulerAngles.z > 180) ? transform.eulerAngles.z - 360 : transform.eulerAngles.z;

        if (horizontalInput != 0)
        {
            tiltSelfcorrectionT = 0;     // Reset lerping back to 0

            // Calculate rotation
            float rotation = -1 * horizontalAngularTiltSpeed * horizontalInput;

            rotation = Mathf.Clamp(rotation + currentRotation, -horizontalMaxTilt, horizontalMaxTilt);

            transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, rotation));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Lerp(currentRotation, 0, tiltSelfcorrectionT)));
            tiltSelfcorrectionT += horizontalTiltSelfCorrectionSpeed;

            // Has to be a percentile for lerp to work
            Mathf.Clamp(tiltSelfcorrectionT, 0, 1);
        }
    }

    void VecticalTilt()
    {
        // Transform euler angle to negative of positive
        float currentRotation = (transform.eulerAngles.x > 180) ? transform.eulerAngles.x - 360 : transform.eulerAngles.x;

        if (verticalInput != 0)
        {
            tiltSelfcorrectionT = 0;     // Reset lerping back to 0

            // Calculate rotation
            float rotation = verticalAngularTiltSpeed * verticalInput;

            rotation = Mathf.Clamp(rotation + currentRotation, -verticalMaxTilt, verticalMaxTilt);

            transform.rotation = Quaternion.Euler(new Vector3(rotation, transform.eulerAngles.y, transform.eulerAngles.z));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(Mathf.Lerp(currentRotation, 0, tiltSelfcorrectionT), transform.eulerAngles.y, transform.eulerAngles.z));
            tiltSelfcorrectionT += verticalTiltSelfCorrectionSpeed;

            // Has to be a percentile for lerp to work
            Mathf.Clamp(tiltSelfcorrectionT, 0, 1);
        }
    }

    void Turn()
    {
        if(Mathf.Abs(horizontalInput) > 0)
        {
            transform.RotateAround(transform.position, Vector3.up, horizontalInput * turnAcceleration * (Time.time - turnStartTime));
            
            // Add horizontal banking acceleration
            rb.velocity = rb.velocity + (transform.right * horizontalBankAcceleration * horizontalInput * (Time.time - turnStartTime));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("Bouncing at " + transform.position);
            lastVerticalSpeed = bounceForce;
        }
    }
}
