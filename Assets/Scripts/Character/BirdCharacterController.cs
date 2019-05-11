using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdCharacterController : MonoBehaviour {

    [SerializeField]
    float glidingSpeed;

    Rigidbody rb;


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        rb.velocity = transform.forward * glidingSpeed;
        rb.velocity = new Vector3(rb.velocity.x, rb.mass * Physics.gravity.y, rb.velocity.z);
    }
}
