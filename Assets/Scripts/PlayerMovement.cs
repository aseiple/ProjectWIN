using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speedMultiplier;
    public bool locked = false;

    void Start () {
		
	}
	
	void Update () {
        if (!locked) {
            transform.Translate(Input.GetAxis("Horizontal") * speedMultiplier, 0f, Input.GetAxis("Vertical") * speedMultiplier);
        }
	}
}
