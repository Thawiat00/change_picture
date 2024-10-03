using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlTalkState : IDialogState
{
    private int currentIndex = 0;

    public void DisplayMessage(DialogManager manager)
    {
        if (currentIndex < manager.CurrentStateData.girl_talk.Length)
        {
            string message = manager.CurrentStateData.girl_talk[currentIndex];
            switch (message)
            {
                case "[waiting_untill(boy_talk)]":
                    manager.SetState(new BoyTalkState());
                    manager.DisplayCurrentMessage();
                    break;
                case "[continue boy_talk]":
                    // Skip this message if boy's talk is finished
                    NextMessage(manager);
                    break;
                default:
                    manager.SetDialogText(message);
                    break;
            }
        }
    }

    public void NextMessage(DialogManager manager)
    {
        currentIndex++;
        if (currentIndex >= manager.CurrentStateData.girl_talk.Length)
        {
            manager.MoveToNextState();
        }
        else
        {
            DisplayMessage(manager);
        }
    }
}
