using UnityEngine;

namespace Zombies
{
    // Controls our player's view and handles Player/Camera rotation.
    public class ViewComponent : MonoBehaviour
    {
        // Our target view FOV.
        [SerializeField] float viewFOV;

        // Our view look sensitivity.
        [SerializeField] float lookSensitivity;

        // Invert Y-Axis movement.
        [SerializeField] bool invertY;

        // Our current camera attached to our view.
        [SerializeField] Camera viewCamera;

        // We don't need to clamp the yaw like we do the pitch. Although it should just reset between (0 & 360) or (180 & -180)
        private const float MAX_PITCH = 89f;

        // X = pitch, Y = yaw
        Vector2 lookAngles;
        Vector2 mouseDelta;

        void Start()
        {

        }

        void Update()
        {
            // Lock the cursor. (This should probably be done in the UI eventually)
            LockInput();

            // Set the view camera's field of view.
            SetFov(viewFOV);

            // Update the view camera's rotation.
            UpdateCameraRotation();
        }

        // Locks the cursor to the window.
        void LockInput()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Abstracted this incase we do ADS on guns we will want to Lerp the FOV down by the ADS FOV scale.
        void SetFov(float fov, float t = 1f)
        {
            viewCamera.fieldOfView = fov;
        }

        void UpdateCameraRotation()
        {
            // Get mouse delta.
            mouseDelta = new Vector2(Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime, Input.GetAxis("Mouse X") * lookSensitivity * Time.deltaTime);

            // Setup look angles. (Could totally remove the Invert Y-Axis check) 
            lookAngles.x += invertY ? mouseDelta.x : -mouseDelta.x;
            lookAngles.x = Mathf.Clamp(lookAngles.x, -MAX_PITCH, MAX_PITCH);
            lookAngles.y = mouseDelta.y;

            // Rotate the camera on the x-axis.
            viewCamera.transform.localRotation = Quaternion.Euler(lookAngles.x, 0f, 0f);

            // Rotate the player on the y-axis.
            transform.Rotate(Vector3.up * lookAngles.y);
        }
    }
}