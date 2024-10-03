using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    public Image Image_BG;
    private DialogData dialogData;
    private IDialogState currentState;
    public StateData CurrentStateData { get; private set; }
    private int currentStateIndex = 0;

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
            if (bgImagePath == "Outside_1")
            {
                Sprite bgSprite = Resources.Load<Sprite>("Outside_1");
                if (bgSprite != null)
                {
                    Image_BG.sprite = bgSprite;
                }
                else
                {
                    Debug.LogError("Failed to load background image: Outside_1");
                }
            }
            else
            {
                Sprite defaultBgSprite = Resources.Load<Sprite>("DefaultBackground");
                Image_BG.sprite = defaultBgSprite;
            }
        }
    }
}

[System.Serializable]
public class DialogData
{
    public List<StateData> states;
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