using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Minigame_SpeechManager : MonoBehaviour
{
    /// <summary>
    /// ����Ŭ����
    ///�Ҳ� ������ ����
    ///���� Ŭ����&���� ����
    ///���� ��ȯ Ŭ���� ����
    ///���� ����&��ų Ŭ���� ���� 
    ///�ð� �� ������ ����(������� ���� ������ ������
    /// </summary>

    void Start()
    {
        audioLength = audioSource.clip.length;

        StartCoroutine(Timer());
    }

    [Header("Dialogue")]
    public string thisMinigameName;
    public int branchWhenSucess;
    public GameObject successPanel;
    public GameObject failPanel;

    [Space]
    public Slider fireSlider;
    public TMP_Text timerText;
    float timer = 0f;
    float audioLength;
    public int life = 3;
    public bool isResolved = false;
    public TMP_Text lifeText;

    public static bool isPlaying = true;

    public AudioSource audioSource;
    public int fires = 0;
    public TMP_Text fireText;


    private void OnEnable()
    {
        isPlaying = true;
        //if (DialogueManager.instance != null) DialogueManager.instance.soundManager.EndBGM();
    }

    private void Update()
    {
        if (!isPlaying) return;
        lifeText.text = "Life: " + life;
        fireText.text = "Fire: " + fires;

        if (life <= 0)
        {
            Debug.Log("���ӿ���");
            lifeText.text = "Life: 0   GAME OVER!!!!";
            OnFail();
        }
        else if (fires >= 3)
        {
            Debug.Log("���� Ŭ����");
            fireText.text = "Fire: " + fires + "GAME CLEAR!!!!";
            OnSuccess();
        }

    }

    IEnumerator Timer()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        timer = audioLength;


        while (timer >= 0)
        {
            timer -= 0.1f;
            timerText.text = "Time: " + timer.ToString("N1");
            //timerFill.fillAmount = timer / audioLength;
            //float t = 1 - (timer / audioLength);
            //timerFill.color = Color.Lerp(Color.green, Color.yellow, Mathf.Clamp01(t * 2));
            // if (t > 0.5f)
            //{
            // timerFill.color = Color.Lerp(Color.yellow, Color.red, Mathf.Clamp01((t - 0.5f) * 2));
            //}
            yield return wait;
        }
        OnFail();
    }

    public void GaugeChange(float value)
    {
        float max = fireSlider.value;
        max += value;
        fireSlider.value += value;

        if (fireSlider.value >= fireSlider.maxValue)
        {
            fires++;
            fireSlider.value = max - fireSlider.maxValue;
        }
    }

    void EndMinigame()
    {
        isPlaying = false;
        StopAllCoroutines();
        audioSource.Stop();
    }

    public void OnSuccess()
    {
        EndMinigame();
        successPanel.SetActive(true);
    }

    public void OnFail()
    {
        EndMinigame();
        failPanel.SetActive(true);
    }

    public void WhenSuccessClick()
    {
        //DialogueManager.instance.ChangeCurrentBlock(branchWhenSucess);
        Destroy(gameObject);
    }

    public void WhenFailRetryClick()
    {
        //DialogueManager.instance.StartMinigame(thisMinigameName);
        Destroy(gameObject);
    }

    public void DecreaseLife()
    {
        if (isResolved) return;
        life--;
    }
}
