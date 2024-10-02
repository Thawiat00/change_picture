using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public ProcessCode processCode; // Reference to the ProcessCode class

    void Update()
    {
        // Check if the spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Check if processCode is not null before calling the method
            if (processCode != null)
            {
                // Call the OnClickChangeImage function from ProcessCode
                processCode.OnClickChangeImage();
            }
            else
            {
                Debug.LogWarning("ProcessCode reference is not set.");
            }
        }
    }
}
