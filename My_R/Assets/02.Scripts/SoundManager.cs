using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// 베리어 프리 자막 제작 가이드
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
    [Space]
    public AudioSource uiAudio;

    float bgmVolume = 0.4f;//pref에서 가져오기

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
        //[  ] SE 재생 시, seSubtitle 출력
        seSubTxt.text = "[ " + subtitle + " ]";
    }

    public void PlayUISound(AudioClip clip)
    {
        uiAudio.PlayOneShot(clip);
    }


    public void PlayBGM(AudioClip clip, string subtitle, bool isLoop)
    {
        if (clip == null) return;
        if (clip.Equals(bgmAudio.clip)) return;
        StartCoroutine(CoroutinePlayBGM(clip));

        //[ ♪ ] BGM 재생 시, bgmSubtitle 출력
        bgmSubTxt.text = "[ ♪ " + subtitle + " ]";
        currentBgmSub = subtitle;
    }
    IEnumerator CoroutinePlayBGM(AudioClip clip)
    {
        if (bgmAudio.isPlaying)
        {
            EndBGM();
            yield return new WaitForSeconds(1.05f);
        }

        bgmAudio.clip = clip;

        float plus = bgmVolume / 10;
        bgmAudio.volume = bgmVolume;
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
        ambSubTxt.text = "[ " + subtitle + "-계속 ]";
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
        currentBgmSub = currentBgmSub.Replace("시작", "종료");
        currentBgmSub = currentBgmSub.Replace("다시 시작", "종료");
        currentBgmSub = currentBgmSub.Replace("멈춤", "종료");

        //[ ♪ ]
        bgmSubTxt.text = "[ ♪ " + currentBgmSub + " ]";
        currentBgmSub = "";
    }

    public void PauseBGM()
    {
        bgmAudio.Pause();

        currentBgmSub = currentBgmSub.Replace("시작", "멈춤");
        currentBgmSub = currentBgmSub.Replace("-계속", "멈춤");
        currentBgmSub = currentBgmSub.Replace("다시 시작", "멈춤");

        //[ ♪ ]
        bgmSubTxt.text = "[ ♪ " + currentBgmSub + " ]";
    }

    public void StopAmbience()
    {
        //ambSubTxt.text = "[ " + subtitle + " 종료 ]";
    }


    public void ReplayBGM()
    {
        bgmAudio.Play();
    }
}
