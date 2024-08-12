using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// ������ ���� �ڸ� ���� ���̵�
    /// https://m.blog.naver.com/bluebell2018/221319730428
    /// https://m.blog.naver.com/bluebell2018/221321787597
    /// https://newsroom.daewoong.co.kr/archives/13336
    /// </summary>

    public static SoundManager instance;

    [Header("Barrier Free Subtitle")]
    string currentBgmSub;
    public TMP_Text bgmSubTxt;
    public TMP_Text ambSubTxt;
    public TMP_Text seSubTxt;



    private void Awake()
    {
        instance = this;
    }

    public List<AudioClip> voiceClips;
    public AudioSource typingAudio;
    public AudioSource soundAudio;
    public AudioSource bgmAudio;

    float bgmVolume = 0.4f;//pref���� ��������

    public void PlayVoice(int index)
    {
        //typingAudio.PlayOneShot(voiceClips[index]);
        /*typingAudio.clip = voiceClips[index];
        typingAudio.Stop();
        typingAudio.Play();*/
    }

    public void StopSound()
    {
        soundAudio.Stop();
    }


    public void PlaySound(AudioClip clip, string subtitle)
    {
        soundAudio.clip = clip;
        soundAudio.Stop();
        soundAudio.Play();
        //[  ] SE ��� ��, seSubtitle ���
        seSubTxt.text = "[ " + subtitle + " ]";
    }

    public void PlayBGM(AudioClip clip, string subtitle)
    {
        if (clip == null) return;
        if (clip.Equals(bgmAudio.clip)) return;
        StartCoroutine(CoroutinePlayBGM(clip));

        //[ �� ] BGM ��� ��, bgmSubtitle ���
        bgmSubTxt.text = "[ �� " + subtitle + " ]";
        currentBgmSub = subtitle;
    }
    IEnumerator CoroutinePlayBGM(AudioClip clip)
    {
        if (bgmAudio.isPlaying)
        {
            EndBGM();
            yield return new WaitForSeconds(1f);
        }

        bgmAudio.clip = clip;

        float plus = bgmVolume / 10;
        //  bgmAudio.volume = 0;
        bgmAudio.Play();
        /* while (bgmAudio.volume < bgmVolume)
         {
             bgmAudio.volume += plus;
             yield return new WaitForSeconds(0.1f);
         }*/
        bgmAudio.volume = bgmVolume;
        yield return null;
    }

    public void PlayAmbience(AudioClip clip, string subtitle)
    {
        ambSubTxt.text = "[ " + subtitle + "-��� ]";
    }

    public void EndBGM()
    {
        StartCoroutine(CoroutineEndBGM());
    }
    IEnumerator CoroutineEndBGM()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        float minus = bgmVolume / 10;
        while (bgmAudio.volume > 0)
        {
            bgmAudio.volume -= minus;
            yield return wait;
        }
        bgmAudio.Stop();
        bgmAudio.volume = bgmVolume;
        bgmAudio.clip = null;
    }

    public void StopBGM()
    {

        bgmAudio.Stop();
        currentBgmSub = currentBgmSub.Replace("����", "����");
        currentBgmSub = currentBgmSub.Replace("�ٽ� ����", "����");
        currentBgmSub = currentBgmSub.Replace("����", "����");

        //[ �� ]
        bgmSubTxt.text = "[ �� " + currentBgmSub + " ]";
        currentBgmSub = "";
    }

    public void PauseBGM()
    {
        bgmAudio.Pause();

        currentBgmSub = currentBgmSub.Replace("����", "����");
        currentBgmSub = currentBgmSub.Replace("-���", "����");
        currentBgmSub = currentBgmSub.Replace("�ٽ� ����", "����");

        //[ �� ]
        bgmSubTxt.text = "[ �� " + currentBgmSub + " ]";
    }

    public void StopAmbience()
    {
        //ambSubTxt.text = "[ " + subtitle + " ���� ]";
    }


    public void ReplayBGM()
    {
        bgmAudio.Play();
    }
}
