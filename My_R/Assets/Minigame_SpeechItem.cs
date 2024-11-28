using UnityEngine;

public class Minigame_SpeechItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("±‚»∏ +1");
            Minigame_SpeechManager.life++;
            // collision.gameObject.SetActive(false);//temp
            this.gameObject.SetActive(false);
        }
    }
}
