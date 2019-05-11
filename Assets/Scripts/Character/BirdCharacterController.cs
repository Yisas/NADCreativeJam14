using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdCharacterController : MonoBehaviour
{
    [SerializeField]
    float glidingSpeed;
    [SerializeField]
    float gravityMultiplier;
    [SerializeField]
    float turnAcceleration;
    [SerializeField]
    float maxTurnSpeed;

    [SerializeField]
    float horizontalBankAcceleration;
    [SerializeField]
    float maxHorizontalSpeed;
    [SerializeField]
    float horizontalAngularTiltSpeed;
    [SerializeField]
    float horizontalMaxTilt;
    [SerializeField]
    float horizontalTiltSelfCorrectionSpeed; // Value between 0 and 1, determines how fast the character tilts back to rotation 0 when there is no airborne horizontal input

    [SerializeField]
    float verticalAngularTiltSpeed;
    [SerializeField]
    float verticalDiveMaxTilt;
    [SerializeField]
    float verticalUpwardsMaxTilt;
    [SerializeField]
    float verticalTiltSelfCorrectionSpeed; // Value between 0 and 1, determines how fast the character tilts back to rotation 0 when there is no airborne vertical input

    [SerializeField]
    float wingFlapBoost;
    [SerializeField]
    float bounceForce;

    [SerializeField]
    float cloudYLevel;
    [SerializeField]
    AudioClip cloudEnterSound;
    [SerializeField]
    AudioClip wingFlapSound;

    public Animator anim;

    // Input variables
    float horizontalInput = 0;
    float verticalInput = 0;

    // Movement variables
    bool diving = false;
    float tiltSelfcorrectionT = 0;       // t for the lerp back to 0 rotation, when there is no horizontal input
    float bounceSpeed = 0;
    float turnStartTime = 0;
    float lastVerticalPosition = 0;
    Vector3 originalPosition;

    AudioSource audioSource;
    Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        InputCollection();
        VecticalTilt();
        HorizontalTilt();
        UpdateAnimator();

        diving = verticalInput > 0;

        if (transform.position.y < cloudYLevel && lastVerticalPosition >= cloudYLevel)
        {
            audioSource.PlayOneShot(cloudEnterSound);
        }
        lastVerticalPosition = transform.position.y;
    }

    private void FixedUpdate()
    {
        // Move character along forward vector
        Vector3 forwardVelocityVector = transform.forward * glidingSpeed;
        rb.velocity = new Vector3(forwardVelocityVector.x, forwardVelocityVector.y + bounceSpeed + (Physics.gravity.y * Time.deltaTime * gravityMultiplier), forwardVelocityVector.z);

        // Apply gravity to bounce speed till it fades
        bounceSpeed += (Physics.gravity.y * Time.deltaTime * gravityMultiplier);
        if (bounceSpeed < 0)
            bounceSpeed = 0;

        Turn();
    }

    void InputCollection()
    {
        float previousHorizontalInput = horizontalInput;
        horizontalInput = Input.GetAxis("Horizontal");

        // If signchange or increment from 0, relative to last horizontal input, turning started
        if (Mathf.Sign(horizontalInput) != Mathf.Sign(previousHorizontalInput) || (previousHorizontalInput == 0 && Mathf.Abs(horizontalInput) > 0))
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

            rotation = Mathf.Clamp(rotation + currentRotation, -verticalUpwardsMaxTilt, verticalDiveMaxTilt);

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
        if (Mathf.Abs(horizontalInput) > 0)
        {
            float turnAngleAmount = 0;
            if (Mathf.Abs(horizontalInput * turnAcceleration * (Time.time - turnStartTime)) < maxTurnSpeed)
            {
                turnAngleAmount = horizontalInput * turnAcceleration * (Time.time - turnStartTime);
            }
            else
            {
                turnAngleAmount = maxTurnSpeed * Mathf.Sign(horizontalInput);
            }
            transform.RotateAround(transform.position, Vector3.up, turnAngleAmount);

            // Add horizontal banking acceleration
            float horizontalBankAmount = 0;
            if (Mathf.Abs(horizontalInput * horizontalBankAcceleration * (Time.time - turnStartTime)) < maxHorizontalSpeed)
            {
                horizontalBankAmount = horizontalInput * horizontalBankAcceleration * (Time.time - turnStartTime);
            }
            else
            {
                horizontalBankAmount = maxHorizontalSpeed * Mathf.Sign(horizontalInput);
            }
            rb.velocity = rb.velocity + (transform.right * maxHorizontalSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Bouncy")
        {
            Debug.Log("Bouncing at " + transform.position);
            bounceSpeed = bounceForce;
        }

        if(collision.transform.tag == "Ground")
        {
            transform.position = originalPosition;
        }

        if(collision.transform.tag == "Tree")
        {
            transform.position = originalPosition;
        }
    }

    public void BoostUp()
    {
        if (!diving)
        {
            bounceSpeed += wingFlapBoost;
            audioSource.PlayOneShot(wingFlapSound);
        }
    }

    void UpdateAnimator()
    {
        anim.SetBool("diving", diving);
        anim.SetBool("gliding", Mathf.Abs(horizontalInput) > 0.2f);
    }
}
