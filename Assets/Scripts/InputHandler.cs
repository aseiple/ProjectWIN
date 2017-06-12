using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum games
{
    none,
    space,
    pac,
}

public class InputHandler : MonoBehaviour {
    public GameObject spacePlayer;
    public GameObject pacPlayer;
    public Canvas pauseMenu;

    public games active;

    public float speed;
    public float mouseSens = 100.0f;
    public float clampAngle = 80.0f;
    public float spaceSpeed = 50f;

    private float rotY = 0.0f;
    private float rotX = 0.0f;

    private Vector3 moveDirection = Vector3.zero;
    private bool paused = false;

    void Start () {
        Vector3 rot = GetComponentInChildren<Transform>().transform.localRotation.eulerAngles;
        rotX = rot.x;
        rotY = rot.y;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.enabled = false;
    }
	
	void FixedUpdate () {
		switch (active)
        {
            case games.space:
                float x = -Input.GetAxis("Horizontal") * spaceSpeed;
                Debug.Log(x);
                spacePlayer.transform.localPosition = new Vector3(Mathf.Clamp(spacePlayer.transform.localPosition.x + x, -9.7f, -2.1f), 1, -3);
                break;
            case games.pac:
                break;
            default:
                CharacterController controller = GetComponent<CharacterController>();
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= speed;
                controller.Move(moveDirection * Time.deltaTime);
                transform.position = new Vector3(transform.position.x, 3.58f, transform.position.z);
                break;
        }

        if (active == games.none)
        {
            float mouseY = Input.GetAxis("Mouse X");
            float mouseX = -Input.GetAxis("Mouse Y");

            rotY += mouseY * mouseSens * Time.deltaTime;
            rotX += mouseX * mouseSens * Time.deltaTime;

            rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

            Quaternion localRot = Quaternion.Euler(rotX, rotY, 0.0f);
            GetComponentInChildren<Transform>().transform.rotation = localRot;

            if (Input.GetAxis("Use") > 0)
            {
                RaycastHit hit;
                float hitDistance;
                Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
                Debug.DrawRay(transform.position, forward, Color.green);

                if (Physics.Raycast(transform.position, (forward), out hit))
                {
                    hitDistance = hit.distance;
                    if (hitDistance < 3)
                    {
                        GameObject hitObj = hit.collider.gameObject;
                        active = hitObj.GetComponent<GameCabinet>().thisGame;
                        StartCoroutine(MoveFromTo(transform.position, new Vector3(hitObj.transform.position.x - 1.5f, 3.58f, hitObj.transform.position.z - 1.5f), transform.rotation, new Quaternion(.11f, 0f, 0f, 1f), 0.75f));
                    }
                }
            }
        }
        else
        {
            if (Input.GetAxis("Exit") > 0)
            {
                active = games.none;
            }
        }
    }

    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            paused = TogglePause();
        }
    }

    IEnumerator MoveFromTo (Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRot, float tranTime)
    {
        float t = 0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime / tranTime;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.rotation = Quaternion.Lerp(startRot, endRot, t);
            yield return 0;
        }
    }

    bool TogglePause()
    {
        if (Time.timeScale == 0f)
        {
            Cursor.lockState = CursorLockMode.Locked;
            pauseMenu.enabled = false;
            Time.timeScale = 1f;
            return (false);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            pauseMenu.enabled = true;
            Time.timeScale = 0f;
            return (true);
        }
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
