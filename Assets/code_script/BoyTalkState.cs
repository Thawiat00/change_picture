using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyTalkState : IDialogState
{
    private int currentIndex = 0;

    public void DisplayMessage(DialogManager manager)
    {
        if (currentIndex < manager.CurrentStateData.boy_talk.Length)
        {
            string message = manager.CurrentStateData.boy_talk[currentIndex];
            if (message == "[waiting_untill(girl_talk)]")
            {
                manager.SetState(new GirlTalkState());
                manager.DisplayCurrentMessage();
            }
            else
            {
                manager.SetDialogText(message);
            }
        }
    }

    public void NextMessage(DialogManager manager)
    {
        currentIndex++;
        if (currentIndex >= manager.CurrentStateData.boy_talk.Length)
        {
            manager.SetState(new GirlTalkState());
            manager.DisplayCurrentMessage();
        }
        else
        {
            DisplayMessage(manager);
        }
    }
}