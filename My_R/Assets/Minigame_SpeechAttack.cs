using UnityEngine;

public class Minigame_SpeechAttack : MonoBehaviour
{
    public Minigame_SpeechManager ms;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Minigame_SpeechManager.isPlaying) return;
        if (collision.gameObject.tag == "Player")
        {
            ms.DecreaseLife();

        }
    }
}
