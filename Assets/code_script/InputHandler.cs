using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public ProcessCode processCode; // Reference to the ProcessCode class
    public DialogManager dialogManager; // Reference to the DialogManager class

    void Update()
    {
        // Check if the spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Check if processCode is not null before calling the method
            if (processCode != null)
            {
                // Call the OnClickChangeImage function from ProcessCode to change the image
                processCode.OnClickChangeImage();
            }
            else
            {
                Debug.LogWarning("ProcessCode reference is not set.");
            }

            // Check if dialogManager is not null before calling the method
            if (dialogManager != null)
            {
                // Call the ShowNextMessage function from DialogManager to display the next text
                dialogManager.ShowNextMessage();
            }
            else
            {
                Debug.LogWarning("DialogManager reference is not set.");
            }
        }
    }
}
