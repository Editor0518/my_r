using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_Tile : MonoBehaviour
{
    public int tileIndex;
    public Minigame_Crowd mCrowd;
    public Transform tileTrans;
    public Transform attack;
    public GameObject attackPrev;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!Minigame_SpeechManager.isPlaying) return;
        if (collision.CompareTag("Player"))
        {
            mCrowd.CrowdGaugeChange(tileIndex, 0.05f);
        }
    }
}
