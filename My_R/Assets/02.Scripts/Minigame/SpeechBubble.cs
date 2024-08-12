using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    public Image[] circles = new Image[12];
    public RectTransform circleBlur;

    private void Awake()
    {
        if (circles[0] == null)
        {
            for (int i = 0; i < circles.Length; i++)
            {
                transform.GetChild(i).GetComponent<Image>();
            }

        }
    }

    public void SetSpeech(string speech)
    {
        for (int i = 0; i < speech.Length; i++)
        {
            if (i >= circles.Length) break;
            switch (speech[i])
            {
                case '1':
                    circles[i].color = Color.yellow;
                    circles[i].rectTransform.sizeDelta = new Vector2(circles[i].rectTransform.sizeDelta.y, circles[i].rectTransform.sizeDelta.y);
                    continue;
                case '0':
                    circles[i].rectTransform.sizeDelta = new Vector2(circles[i].rectTransform.sizeDelta.x + circles[i].rectTransform.sizeDelta.y, circles[i].rectTransform.sizeDelta.y);
                    break;
                case '8':
                    circles[i].color = Color.red;
                    break;
                case '9':
                    circles[i].color = Color.blue;
                    break;
            }
            circles[i].gameObject.SetActive(true);
        }
        if (speech.Length < circles.Length)
        {
            for (int i = speech.Length; i < circles.Length; i++)
            {
                circles[i].gameObject.SetActive(false);
            }
        }
    }
    public void SucessBubble(int index)
    {

        circles[index].color = circles[index].color * Color.gray;
    }
    public void FailedBubble(int index)
    {
        circles[index].color = circles[index].color * Color.black;
    }

    public void PauseBubble(int lastPressedIndex, string speech)
    {
        circleBlur.sizeDelta = new Vector2(120 * (circles.Length - lastPressedIndex - 1), circleBlur.sizeDelta.y);
        circleBlur.gameObject.SetActive(true);
    }
    public void RestartBubble()
    {
        circleBlur.gameObject.SetActive(false);
    }

}
