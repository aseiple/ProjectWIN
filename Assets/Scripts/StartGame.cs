using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {
    bool isEnabled = false;
    public PlayerMovement player;
    public MouseLook playerMouse;

    private void OnTriggerStay(Collider other)
    {
        if(Input.GetKey("e"))
        {
            isEnabled = !isEnabled;
            player.locked = !player.locked;
            playerMouse.locked = !playerMouse.locked;
        }
    }

    public void Update()
    {
    }
}
