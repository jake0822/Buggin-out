using UnityEngine;

public class MainCameraRotationMirror : MonoBehaviour
{
    public float rotationSpeed = 5f;  // Speed at which the camera rotates based on mouse movement
    public float minVerticalAngle = -30f;  // Min vertical rotation limit
    public float maxVerticalAngle = 60f;   // Max vertical rotation limit

    private float rotationX = 0f; // Current horizontal rotation (around Y-axis)
    private float rotationY = 0f; // Current vertical rotation (around X-axis)

    void Update()
    {
        // Get mouse input (horizontal and vertical)
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        // Update the rotation angles based on mouse input
        rotationX += mouseX; // Horizontal rotation (left/right)
        rotationY -= mouseY; // Vertical rotation (up/down)

        // Clamp the vertical rotation to avoid flipping the camera over
        rotationY = Mathf.Clamp(rotationY, minVerticalAngle, maxVerticalAngle);

        // Apply the rotation to the camera's transform
        transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0f);
    }
}