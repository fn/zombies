using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControls : MonoBehaviour
{
    [SerializeField] int sens;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;
    float rotX;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; //Locks the mouse into the window, think of windowed fullscreen vs fullscreen.
    }

    // Update is called once per frame
    void Update()
    {
        //get input
        float mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;

        if (invertY)
            //inverse
            rotX += mouseY;
        else
            //normal
            rotX -= mouseY;

        //clamp the rotX of the x-axis
        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);

        //rotate the camera on the x-axis
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);
        //When working with degrees, use the Quaternion.

        //rotate the player on the y-axis
        transform.parent.Rotate(Vector3.up * mouseX);
        //We already multiplied mouseX by deltaTime, so we don't need to multiply it again. If there are issues, this is the first thing to check.
    }
}
