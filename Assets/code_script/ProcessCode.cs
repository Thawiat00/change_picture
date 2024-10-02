using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProcessCode : MonoBehaviour
{
    /*
     

    change image only
     */


    /*
    find design pattern
    
    */

    /*
    use State Pattern:

make status state (State1, State2, State3, State4)
in state will show image different
if state change ,image will change too
    */

    // Variable to hold the image display
    public Image imageDisplay;
    public Sprite[] images; // Array to store images 1 to 4

    // Interface for managing image states
    private IImageState currentState;

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
        currentState = newState;
        currentState.DisplayImage(); // Display the image when the state changes
    }

    // Function to call when clicking to change the image
    public void OnClickChangeImage()
    {
        currentState.OnNext(); // Call OnNext of the current state to switch images
    }

    // Interface for state management
    public interface IImageState
    {
        void DisplayImage(); // Function to display the image
        void OnNext();       // Function to go to the next image
    }
}
