using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdCharacterController : MonoBehaviour
{
    [SerializeField]
    float glidingSpeed;
    [SerializeField]
    float horizontalSpeed;
    [SerializeField]
    float horizontalAngularTiltSpeed;
    [SerializeField]
    float horizontalMaxTilt;
    [SerializeField]
    float horizontalTiltSelfCorrectionSpeed; // Value between 0 and 1, determines how fast the character tilts back to rotation 0 when there is no airborne horizontal input

    // Input variables
    float horizontalInput;

    // Movement variables
    float tiltSelfcorrectionT = 0;       // t for the lerp back to 0 rotation, when there is no horizontal input

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
        HorizontalTilt();
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.forward * glidingSpeed;
        rb.velocity = new Vector3(rb.velocity.x, rb.mass * Physics.gravity.y, rb.velocity.z);

        if (Mathf.Abs(horizontalInput) > 0)
        {
            rb.velocity = rb.velocity + (transform.right) * horizontalSpeed * horizontalInput;
        }
    }

    void InputCollection()
    {
        horizontalInput = Input.GetAxis("Horizontal");
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
}
