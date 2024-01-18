using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // camera -> mouse sensitivity 
    public float camSensX;
    public float camSensY;

    // camera rotation
    float rotationX;
    float rotationY;

    public Transform orientation;

    // Player transform
    public Transform playerPos;
    public Transform crouchPos;

    bool crouched = false;
    public PlayerControl playerControl;

    void Start()
    {
        // locking cursor to centre of the screen and making it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        CheckCrouching();        
        
        // setting cam position to player position
        if (crouched)
        {
            this.transform.position = crouchPos.position;
        }
        else
        {
            this.transform.position = playerPos.position;
        }

        
        // camera rotation by mouse movement
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * camSensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * camSensY;

        // unity camera rotation
        rotationY += mouseX;
        rotationX -= mouseY;

        // clamping rotation when looking up and down
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        orientation.rotation = Quaternion.Euler(0, rotationY, 0);
    }

    private void CheckCrouching()
    {
        if (playerControl.isCrouching)
        {
            crouched = true;
        }
        else
        {
            crouched = false;
        }
    }
}
