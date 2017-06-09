using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {
    private void OnTriggerStay(Collider other)
    {
        if(Input.GetKeyDown("e"))
        {
            Debug.Log("Pressed");
        }
    }
}
