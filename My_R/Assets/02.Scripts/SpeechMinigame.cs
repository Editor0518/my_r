using System.Collections;
using TMPro;
using UnityEngine;

public class SpeechMinigame : MonoBehaviour
{
    public TMP_Text scoreTxt;
    public TMP_Text timeTxt;
    public RectTransform button;
    int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Game());
    }

    IEnumerator Game()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        score = 0;
        int time = 60;
        while (time > 0)
        {

            timeTxt.text = "Time: " + time;
            scoreTxt.text = "Score: " + score;
            RandomSpace();
            //if (time % 1 == 0) RandomSpace();
            //else button.gameObject.SetActive(false);
            yield return wait;
            time--;
        }
    }

    public void RandomSpace()
    {
        button.gameObject.SetActive(true);
        button.anchoredPosition = new Vector2(Random.Range(-300, 300), Random.Range(-300, 300));

    }

    public void OnClickedButton()
    {
        button.gameObject.SetActive(false);
        score++;

    }
}
