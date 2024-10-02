using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ProcessCode;

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
        controller.imageDisplay.sprite = controller.images[currentIndex];
    }

    // Function to switch to the next image
    public void OnNext()
    {
        // Loop through images 1 to 4
        int nextIndex = (currentIndex + 1) % controller.images.Length;
        controller.SetState(new ImageState(controller, nextIndex));
    }

    public int GetCurrentIndex()
    {
        return currentIndex;
    }
}


