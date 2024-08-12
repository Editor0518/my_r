using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceSelect : MonoBehaviour
{
    public TMP_Text choiceText;
    public AudioClip clip;

    public void SetChoice(string sentence, AudioClip clip)
    {
        choiceText.text = sentence;
        this.clip = clip;
        this.GetComponent<Button>().interactable = false;
        this.GetComponent<Button>().interactable = true;
    }

    public void PlaySound()
    {
        SoundManager.instance.PlaySound(clip, "");
    }

}
