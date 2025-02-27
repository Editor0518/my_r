using UnityEngine;

public class Minigame_SpeechItem : MonoBehaviour
{
    public Minigame_SpeechAttackManager sam;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            sam.UseSkillItem();

            this.gameObject.SetActive(false);
        }
    }
}
