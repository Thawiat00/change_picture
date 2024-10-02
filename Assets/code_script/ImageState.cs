using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageState : IImageState
{
    private ProcessCode controller;
    private int currentIndex;

    public ImageState(ProcessCode controller, int index)
    {
        this.controller = controller;
        this.currentIndex = index;
    }

    // Function to display the image
    public void DisplayImage()
    {
        if (controller.images.Length > 0) // Check if there are any images to display
        {
            controller.imageDisplay.sprite = controller.images[currentIndex];
        }
        else // If there are no images, log a warning
        {
            Debug.LogWarning("No images available to display.");
        }
    }

    // Function to switch to the next image
    public void OnNext()
    {
        if (controller.images.Length > 0) // Check if there are any images to switch
        {
            // Loop through images 1 to 4
            int nextIndex = (currentIndex + 1) % controller.images.Length;
            controller.SetState(new ImageState(controller, nextIndex));
        }
        else // If there are no images, log a warning
        {
            Debug.LogWarning("No images available to switch to.");
        }
    }

    public int GetCurrentIndex()
    {
        return currentIndex;
    }
}
