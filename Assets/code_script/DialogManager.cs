using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI dialogText; // If using TextMeshPro, replace with TMP_Text
    public Image Image_BG; // UI Image component for displaying the background image
    private DialogData dialogData; // To store the parsed JSON data
    private int currentIndex = 0; // Index for the current message being displayed
    private bool isBoyTalk = false; // Flag to track if we are currently displaying boy talk

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
        if (dialogData != null) // Check if dialog data is loaded
        {
            string messageToDisplay = ""; // Initialize messageToDisplay

            // Load the background image based on the bg_img field
            string bgImagePath = dialogData.state1.bg_img; // Get the background image path
            if (bgImagePath == "Outside_1") // Check if it matches the specific value
            {
                // Load the new image (make sure the image is in Resources folder)
                Sprite bgSprite = Resources.Load<Sprite>("Outside_1"); // Load the image without the file extension
                if (bgSprite != null)
                {
                    Image_BG.sprite = bgSprite; // Set the loaded sprite to the Image component
                }
                else
                {
                    Debug.LogError("Failed to load background image: Outside_1.png");
                }
            }
            else
            {
                // Handle other background image cases or a default image if needed
                Debug.LogWarning("No matching background image found. Using default image.");
                // Optionally load a default image
                Sprite defaultBgSprite = Resources.Load<Sprite>("DefaultBackground"); // Change to your default background
                Image_BG.sprite = defaultBgSprite; // Set the default sprite to the Image component
            }

            if (isBoyTalk) // Check if we are currently displaying boy talk
            {
                string[] boyMessages = dialogData.state1.boy_talk; // Access the boy_talk array directly

                if (boyMessages != null && boyMessages.Length > 0) // Check if boy_talk exists and has messages
                {
                    if (currentIndex < boyMessages.Length) // Ensure the index is within the array range
                    {
                        messageToDisplay = boyMessages[currentIndex];

                        // Check for the specific command to switch to girl talk
                        if (messageToDisplay.Contains("[waiting_untill(girl_talk)]"))
                        {
                            isBoyTalk = false; // Switch back to girl talk
                            currentIndex = FindContinueMessageIndex(dialogData.state1.girl_talk); // Get index after [continue boy_talk]
                            if (currentIndex < dialogData.state1.girl_talk.Length)
                            {
                                messageToDisplay = dialogData.state1.girl_talk[currentIndex];
                            }
                            else
                            {
                                messageToDisplay = ""; // No message to display if index is out of range
                            }
                        }
                    }
                    else
                    {
                        isBoyTalk = false; // Reset to girl talk if no more boy messages
                        currentIndex = 0; // Reset index for girl talk
                        if (dialogData.state1.girl_talk.Length > 0)
                        {
                            messageToDisplay = dialogData.state1.girl_talk[0]; // Display the first girl message
                        }
                        else
                        {
                            messageToDisplay = ""; // No message to display if girl_talk is empty
                        }
                    }
                }
                else // If boy messages are null or empty
                {
                    isBoyTalk = false; // Switch back to girl talk
                    currentIndex = 0; // Reset index
                    if (dialogData.state1.girl_talk.Length > 0)
                    {
                        messageToDisplay = dialogData.state1.girl_talk[0]; // Display the first girl message
                    }
                    else
                    {
                        messageToDisplay = ""; // No message to display if girl_talk is empty
                    }
                }
            }
            else // Display girl talk
            {
                string[] girlMessages = dialogData.state1.girl_talk; // Access the girl_talk array directly

                if (girlMessages != null && girlMessages.Length > 0) // Check if girl_talk exists and has messages
                {
                    if (currentIndex < girlMessages.Length) // Ensure the index is within the array range
                    {
                        messageToDisplay = girlMessages[currentIndex]; // Get the current girl message

                        // Check for the specific command to switch to boy talk
                        if (messageToDisplay.Contains("[waiting_untill(boy_talk)]"))
                        {
                            isBoyTalk = true; // Switch to boy talk
                            currentIndex = 0; // Reset index for boy talk
                            messageToDisplay = dialogData.state1.boy_talk.Length > 0 ? dialogData.state1.boy_talk[0] : ""; // Display the first boy message or empty
                        }
                    }
                    else // If index is out of range for girl messages
                    {
                        Debug.LogError("Current index for girl talk is out of range. Resetting to zero.");
                        currentIndex = 0; // Reset the index
                        messageToDisplay = girlMessages[currentIndex]; // Display the first girl message
                    }
                }
                else // If girl messages are null or empty
                {
                    Debug.LogError("Girl talk messages are empty or null.");
                    messageToDisplay = ""; // Set message to empty
                }
            }

            dialogText.text = messageToDisplay; // Display the message
        }
        else
        {
            Debug.LogError("Dialog data is null when trying to display a message.");
            dialogText.text = ""; // Set message to empty if data is null
        }
    }


    // Function to find the index after [continue boy_talk] in girl_talk
    private int FindContinueMessageIndex(string[] girlMessages)
    {
        for (int i = 0; i < girlMessages.Length; i++)
        {
            if (girlMessages[i].Contains("[continue boy_talk]"))
            {
                return i + 1; // Return index after [continue boy_talk]
            }
        }
        return 0; // Return 0 if [continue boy_talk] is not found
    }

    // Function to be called when moving to the next message (can be called from InputHandler)
    public void ShowNextMessage()
    {
        if (dialogData != null) // Check if dialog data is loaded
        {
            currentIndex++; // Move to the next message
            DisplayCurrentMessage(); // Update the displayed message
        }
        else
        {
            Debug.LogError("Dialog data is null when trying to show the next message.");
        }
    }
}

// Class to match the JSON structure for deserialization
[System.Serializable]
public class DialogData
{
    public StateData state1; // Update to have multiple states if needed
}

// Updated to match the new JSON structure
[System.Serializable]
public class StateData
{
    public string[] girl_talk; // Array to hold girl_talk messages
    public string[] boy_talk;  // Array to hold boy_talk messages
    public string bg_img;      // Background image path
    public string transparent_img; // Transparent image path
}
