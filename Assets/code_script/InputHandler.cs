using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public ProcessCode processCode; // Reference to the ProcessCode class

    void Update()
    {
        // Check if the spacebar or left mouse button is pressed
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            // Call the OnClickChangeImage function from ProcessCode
            processCode.OnClickChangeImage();
        }
    }
}
