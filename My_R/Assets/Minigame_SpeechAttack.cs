using UnityEngine;

public class Minigame_SpeechAttack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("��ȸ -1");
            Minigame_SpeechManager.life--;
            //collision.gameObject.SetActive(false);//temp
        }
    }
}