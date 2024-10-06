

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    public Image Image_BG;
    private DialogData dialogData;
    private IDialogState currentState;
    public StateData CurrentStateData { get; private set; }
    private int currentStateIndex = 0;
    public bool isDataModified = false;  // dynamic flag to check if data is modified

    // ???????????????????????????????? Inspector
    [Header("Add New Dialog")]
    public string newGirlTalk;  // ???????????????????????
    public string newBoyTalk;   // ???????????????????????

    void Start()
    {
        LoadDialogData();
        if (dialogData != null && dialogData.states.Count > 0)
        {
            CurrentStateData = dialogData.states[currentStateIndex];
            SetState(new GirlTalkState());
            SetBackgroundImage();
            DisplayCurrentMessage();
        }
        else
        {
            Debug.LogError("Dialog data not loaded or invalid.");
        }
    }

    public void SetCurrentStateData(StateData newStateData)
    {
        CurrentStateData = newStateData;
    }

    void LoadDialogData()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("dialog");
        if (jsonText != null)
        {
            dialogData = JsonUtility.FromJson<DialogData>(jsonText.text);
        }
        else
        {
            Debug.LogError("dialog.json not found in Resources.");
        }
    }

    // ????????????? dialog ??????? Inspector

    // Save updated data to JSON
    public void AddNewDialogFromInspector()
    {
        // Check if either girl talk or boy talk is provided
        if (!string.IsNullOrEmpty(newGirlTalk) || !string.IsNullOrEmpty(newBoyTalk))
        {
            // Create updated lists for girl talk and boy talk
            List<string> updatedGirlTalk = new List<string>(CurrentStateData.girl_talk);
            List<string> updatedBoyTalk = new List<string>(CurrentStateData.boy_talk);

            // Add new girl talk if provided
            if (!string.IsNullOrEmpty(newGirlTalk))
            {
                updatedGirlTalk.Add(newGirlTalk);
            }

            // Add new boy talk if provided
            if (!string.IsNullOrEmpty(newBoyTalk))
            {
                updatedBoyTalk.Add(newBoyTalk);
            }

            // Update CurrentStateData
            CurrentStateData.girl_talk = updatedGirlTalk.ToArray();
            CurrentStateData.boy_talk = updatedBoyTalk.ToArray();

            // Set modified flag
            isDataModified = true;

            // Save updated dialog data to JSON
            SaveDialogData();
            Debug.Log("New dialog added successfully.");
        }
        else
        {
            Debug.LogWarning("New dialog talks cannot be empty.");
        }
    }



    // Save updated data to JSON
    public void SaveDialogData()
    {
        if (isDataModified)
        {
            string json = JsonUtility.ToJson(dialogData, true);
            File.WriteAllText(Path.Combine(Application.dataPath, "Resources/dialog.json"), json);
            Debug.Log("Dialog data saved.");
            isDataModified = false;  // Reset the modified flag
        }
        else
        {
            Debug.Log("No changes to save.");
        }
    }

    public void SetState(IDialogState newState)
    {
        currentState = newState;
    }

    public void DisplayCurrentMessage()
    {
        currentState.DisplayMessage(this);
    }

    public void ShowNextMessage()
    {
        currentState.NextMessage(this);
    }

    public void SetDialogText(string message)
    {
        dialogText.text = message;
    }

    public void MoveToNextState()
    {
        currentStateIndex++;
        if (currentStateIndex < dialogData.states.Count)
        {
            CurrentStateData = dialogData.states[currentStateIndex];
            SetState(new GirlTalkState());
            SetBackgroundImage();
            DisplayCurrentMessage();
        }
        else
        {
            Debug.Log("Dialog completed.");
        }
    }

    private void SetBackgroundImage()
    {
        if (Image_BG != null)
        {
            string bgImagePath = CurrentStateData.bg_img;
            Sprite bgSprite = Resources.Load<Sprite>(bgImagePath);
            if (bgSprite != null)
            {
                Image_BG.sprite = bgSprite;
            }
            else
            {
                Debug.LogError($"Failed to load background image: {bgImagePath}");
                Sprite defaultBgSprite = Resources.Load<Sprite>("DefaultBackground");
                Image_BG.sprite = defaultBgSprite; // Load default background if not found
            }
        }
    }

    public void ModifyDataCheck()
    {
        // Example usage of dynamic data
        if (dialogData.dynamic_data.isModified)
        {
            Debug.Log("Data has been modified by " + dialogData.dynamic_data.lastModifiedBy);
        }
        else
        {
            Debug.Log("No modifications detected.");
        }
    }
}

[System.Serializable]
public class DialogData
{
    public List<StateData> states = new List<StateData>();
    public DynamicData dynamic_data = new DynamicData();  // added to store dynamic information
}

[System.Serializable]
public class DynamicData
{
    public bool isModified;
    public string lastModifiedBy;
}

[System.Serializable]
public class StateData
{
    public string[] girl_talk;
    public string[] boy_talk;
    public string bg_img;
    public string transparent_img;
}

public interface IDialogState
{
    void DisplayMessage(DialogManager manager);
    void NextMessage(DialogManager manager);
}


