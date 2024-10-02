using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI dialogText; // If using TextMeshPro, replace with TMP_Text
    private DialogData dialogData; // To store the parsed JSON data
    private int currentIndex = 0; // Index for the current message being displayed

    void Start()
    {
        if (dialogText != null) // Ensure UI element is assigned
        {
            LoadDialogData(); // Load the dialog data from JSON
            if (dialogData != null) // Check if dialog data is loaded
            {
                DisplayCurrentMessage(); // Show the first message initially
            }
            else
            {
                Debug.LogError("Dialog data not loaded. Check JSON format or file path.");
            }
        }
        else
        {
            Debug.LogError("DialogText UI component is not assigned.");
        }
    }

    // Function to load dialog data from the JSON file
    void LoadDialogData()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("dialog"); // Load JSON file from Resources folder
        if (jsonText != null) // Check if JSON file is found
        {
            dialogData = JsonUtility.FromJson<DialogData>(jsonText.text); // Parse the JSON data
            if (dialogData == null) // Additional check for parsing success
            {
                Debug.LogError("Failed to parse JSON data. Check the JSON structure.");
            }
        }
        else
        {
            Debug.LogError("dialog.json not found in Resources.");
        }
    }

    // Function to display the current message based on the current index
    void DisplayCurrentMessage()
    {
        if (dialogData != null)
        {
            // Convert JSON dialog data to an array of messages
            string[] messages = new string[] {
                dialogData.states.image1,
                dialogData.states.image2,
                dialogData.states.image3,
                dialogData.states.image4
            };

            // Check if messages are not null or empty
            if (messages == null || messages.Length == 0)
            {
                Debug.LogError("Dialog messages are empty or null.");
                return; // Exit the method if messages are invalid
            }

            if (currentIndex < messages.Length) // Ensure the index is within the array range
            {
                dialogText.text = messages[currentIndex]; // Display the message
            }
            else
            {
                Debug.LogError("Current index is out of range. Resetting to zero.");
                currentIndex = 0; // Reset the index to avoid out of bounds
                DisplayCurrentMessage(); // Display the first message
            }
        }
        else
        {
            Debug.LogError("Dialog data is null when trying to display a message.");
        }
    }

    // Function to be called when moving to the next message (can be called from InputHandler)
    public void ShowNextMessage()
    {
        if (dialogData != null)
        {
            // Move to the next message
            currentIndex++;
            string[] messages = new string[] {
                dialogData.states.image1,
                dialogData.states.image2,
                dialogData.states.image3,
                dialogData.states.image4
            };

            // Check if messages are not null or empty
            if (messages == null || messages.Length == 0)
            {
                Debug.LogError("Dialog messages are empty or null.");
                return; // Exit the method if messages are invalid
            }

            // If we've reached the end of the messages, loop back to the start
            if (currentIndex >= messages.Length)
            {
                currentIndex = 0; // Reset to the first message
                Debug.Log("Resetting current index to 0 after reaching the end of messages.");
            }

            DisplayCurrentMessage(); // Update the displayed message
        }
        else
        {
            Debug.LogError("Dialog data is null when trying to show the next message.");
        }
    }
}

[System.Serializable]
public class DialogData
{
    public StateData states;
}

[System.Serializable]
public class StateData
{
    public string image1; // Dialog for image 1
    public string image2; // Dialog for image 2
    public string image3; // Dialog for image 3
    public string image4; // Dialog for image 4
}
