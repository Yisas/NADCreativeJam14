using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorComunicator : MonoBehaviour {

    public BirdCharacterController birdChar;

    public void BoostUp()
    {
        birdChar.BoostUp();
    }

    public void Respawn()
    {
        birdChar.Respawn();
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
