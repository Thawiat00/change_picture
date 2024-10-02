using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugManager : MonoBehaviour
{
    public TextMeshProUGUI normalText; // UI Text for normal display (non-debug)
    public TextMeshProUGUI debugText; // UI Text to display debug information
   
    public Button debugButton; // Button to toggle debug mode

    private bool isDebugMode = false;
    private IProcessCodeDebug processCodeDebug; // Reference to ProcessCode through the interface

    void Start()
    {
        // Hook the button click event to toggle debug mode
        debugButton.onClick.AddListener(ToggleDebugMode);

        // Initially hide the debug text and show the normal text
        debugText.gameObject.SetActive(false);
        normalText.gameObject.SetActive(true);

        // Get reference to ProcessCode via interface
        processCodeDebug = FindObjectOfType<ProcessCode>() as IProcessCodeDebug;
        if (processCodeDebug == null)
        {
            Debug.LogError("No ProcessCode found implementing IProcessCodeDebug interface.");
        }
    }

    // Toggle the debug mode
    private void ToggleDebugMode()
    {
        isDebugMode = !isDebugMode;

        if (isDebugMode)
        {
            // Show debug text and hide normal text
            debugText.gameObject.SetActive(true);
            normalText.gameObject.SetActive(false);

            if (processCodeDebug != null)
            {
                // Display the current image index in the debug text
                debugText.text = "Current Image: " + (processCodeDebug.GetCurrentImageIndex() + 1);
            }
        }
        else
        {
            // Hide debug text and show normal text
            debugText.gameObject.SetActive(false);
            normalText.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (isDebugMode && processCodeDebug != null)
        {
            // Continuously update the debug text with the current image index
            debugText.text = "Current Image: " + (processCodeDebug.GetCurrentImageIndex() + 1);
        }
    }


}

public interface IProcessCodeDebug
{
    int GetCurrentImageIndex(); // Function to get the current image index
}
