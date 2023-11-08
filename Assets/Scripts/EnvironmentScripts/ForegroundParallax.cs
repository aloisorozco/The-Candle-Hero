using UnityEngine;
using Cinemachine;

public class ForegroundParallax : MonoBehaviour
{
    public Transform foreground;        // Reference to the foreground's transform
    public float parallaxSpeed = 0.5f;  // Adjust this to control the parallax effect intensity

    private float startPosX;            // The initial X position of the foreground
    private CinemachineVirtualCamera cinemachineCamera;
    private Vector3 lastCameraPosition; // The position of the camera in the previous frame

    void Start()
    {
        startPosX = foreground.position.x;
        cinemachineCamera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        lastCameraPosition = cinemachineCamera.transform.position;
    }

    void Update()
    {
        // Calculate the horizontal movement of the camera since the last frame
        float deltaX = cinemachineCamera.transform.position.x - lastCameraPosition.x;

        // Check if the camera has moved horizontally
        if (Mathf.Abs(deltaX) > 0.001f)
        {
            // Calculate the parallax effect based on the camera's movement
            float parallaxX = (startPosX - Camera.main.transform.position.x) * parallaxSpeed;

            // Set the target X position for the foreground
            float foregroundTargetPosX = startPosX + parallaxX;

            // Update the foreground's position with the parallax effect
            Vector3 foregroundTargetPos = new Vector3(foregroundTargetPosX, foreground.position.y, foreground.position.z);
            foreground.position = Vector3.Lerp(foreground.position, foregroundTargetPos, Time.deltaTime);
        }

        // Update the starting X position and camera's position for the next frame
        startPosX = foreground.position.x;
        lastCameraPosition = cinemachineCamera.transform.position;
    }
}
