using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DebugManager;

public class ProcessCode : MonoBehaviour, IProcessCodeDebug
{
    // Variable to hold the image display
    public Image imageDisplay;
    public Sprite[] images; // Array to store images 1 to 4

    // Interface for managing image states
    private IImageState currentState;
    private bool debugMode = false; // Variable to toggle debug mode
    public Text debugText; // UI Text for showing debug information

    void Start()
    {
        // Check if at least 4 images are assigned
        if (images.Length < 4)
        {
            Debug.LogError("Please assign at least 4 images in the inspector.");
            return;
        }

        // Initialize with the first image state
        SetState(new ImageState(this, 0));
    }

    // Function to change the current state
    public void SetState(IImageState newState)
    {
        if (newState != null) // Check if newState is not null
        {
            currentState = newState;
            currentState.DisplayImage(); // Display the image when the state changes
        }
        else
        {
            Debug.LogWarning("New state is null, cannot set state.");
        }
    }

    // Function to call when clicking to change the image
    public void OnClickChangeImage()
    {
        if (currentState != null) // Check if currentState is not null
        {
            currentState.OnNext(); // Call OnNext of the current state to switch images
        }
        else
        {
            Debug.LogWarning("Current state is null, cannot change image.");
        }
    }

    // Implement the interface to return the current image index for debugging
    public int GetCurrentImageIndex()
    {
        if (currentState is ImageState state) // Check if currentState is of type ImageState
        {
            return state.GetCurrentIndex();
        }
        else
        {
            Debug.LogWarning("Current state is not of type ImageState, returning -1.");
            return -1; // Return -1 if currentState is not an ImageState
        }
    }
}

// Interface for state management
public interface IImageState
{
    void DisplayImage(); // Function to display the image
    void OnNext();       // Function to go to the next image
}
