using UnityEngine;
using System.Collections;

/// <summary>
/// Calculates the pixel scale requireed for pixel perfect art.
/// </summary>
public class OrtographicCalculator : MonoBehaviour
{

    // Variables required for the calculator.
    [Header("Camera")]
    private Camera mainCamera;

    [Header("Pixel Density")]
    private float screenWidth;
    private float screenHeight;
    [SerializeField]
    private float tileSize = 100f;

    // Use this for initialization
    void Start()
    {
        // Gets the main camera.
        mainCamera = Camera.main;

        // Gets the screen info based on the screen.
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        // Used to update the camera size.
        UpdateCameraSize(1);
    }

    // Debug Tools used to change the camera size.
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Alpha1))
        {
            UpdateCameraSize(1);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            UpdateCameraSize(2);
        }

        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            UpdateCameraSize(4);
        }
    }

    // Function used to update the camera size at runtime.s
    void UpdateCameraSize(byte multiplier)
    {
        // Calculates the ortographic size for the camera.
        float screenSize = ((screenWidth / screenHeight) * 2);
        float ortoSize = (screenWidth / ((screenSize) * tileSize * multiplier));
        mainCamera.orthographicSize = ortoSize;
    }
}