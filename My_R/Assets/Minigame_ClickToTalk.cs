using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minigame_ClickToTalk : MonoBehaviour
{
    private void OnEnable()
    {
        EnableButtons(true);
    }

    public List<Button> buttons;

    public void MoveBranch(int branch)
    {
        EnableButtons(false);
        DialogueManager.instance.canClickToNext = true;
        DialogueManager.instance.isNoNext = false;
        DialogueManager.instance.ChangeCurrentBlock(branch);

    }

    void EnableButtons(bool isTrue)
    {
        foreach (Button button in buttons)
        {
            button.interactable = isTrue;
        }
    }

    public void MoveBranchFinish(int branch)
    {
        DialogueManager.instance.ChangeCurrentBlock(branch);
        DialogueManager.instance.EndMinigame();
    }

}
