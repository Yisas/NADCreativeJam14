using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorComunicator : MonoBehaviour {

    private BirdCharacterController birdChar;

    public void BoostUp()
    {
        birdChar.BoostUp();
    }

	// Use this for initialization
	void Start () {
        birdChar = GetComponentInParent<BirdCharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
