using UnityEngine;

public class ButtonEffect : MonoBehaviour
{
    public AudioClip uiPointerSE;
    public AudioClip uiClickSE;
    public void OnPointerEnterSound()
    {
        if (uiPointerSE != null) SoundManager.instance.PlayUISound(uiPointerSE);
    }

    public void OnClickSound()
    {
        if (uiClickSE != null) SoundManager.instance.PlayUISound(uiClickSE);
    }
}
