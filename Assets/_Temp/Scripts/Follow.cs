using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {

    public Transform target;
    public float speed = 5;
    Vector3 offset;

	void Start()
    {
        offset = transform.position - target.position;
	}
	
	void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * speed);	
	}
}
