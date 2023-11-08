using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform[] backgrounds;      // Array of backgrounds to parallax
    public float[] parallaxScales;       // The proportion of camera movement to move the backgrounds
    public float smoothing = 1f;         // The smoothing of the parallax effect

    private Transform mainCamera;        // Reference to the main camera's transform
    private Vector3 previousCamPos;      // The position of the camera in the previous frame

    void Awake()
    {
        mainCamera = Camera.main.transform;
    }

    void Start()
    {
        previousCamPos = mainCamera.position;

        if (backgrounds.Length != parallaxScales.Length)
        {
            Debug.LogError("Backgrounds and parallaxScales arrays must have the same length!");
        }
    }

    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            // The parallax is the opposite of the camera movement because the previous frame is multiplied by the scale
            float parallax = (previousCamPos.x - mainCamera.position.x) * parallaxScales[i];

            // Set a target position which is the current position plus the parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            // Create a target position which is the background's current position with its target x position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            // Smoothly interpolate between the current position and the target position
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        // Set the previousCamPos to the camera's position at the end of the frame
        previousCamPos = mainCamera.position;
    }
}
