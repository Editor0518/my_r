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
    public SoundHolder soundHolder;

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
    public AudioSource ambAudio;
    [Space]
    public AudioSource uiAudio;

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

    public void PlaySound(AudioClip clip)
    {
        PlaySound(clip, "");
    }
    
    public void PlaySound(string seName)
    {
        AudioClip clip = soundHolder.FindSE(seName);
        //Debug.Log("PlaySE : " + name + " (" + clip.name + ")");
        PlaySound(clip, "");
    }

    public void PlaySound(AudioClip clip, string subtitle)
    {
        soundAudio.clip = clip;
        soundAudio.Stop();
        soundAudio.Play();
        //[  ] SE ��� ��, seSubtitle ���
        if(seSubTxt!=null)seSubTxt.text = "[ " + subtitle + " ]";
    }

    public void PlayUISound(AudioClip clip)
    {
        uiAudio.PlayOneShot(clip);
    }

    public void PlayBGM(string name)
    {
        AudioClip clip = soundHolder.FindBGM(name);
        //Debug.Log("PlayBGM : " + name + " (" + clip.name + ")");
        PlayBGM(clip, "", true);
    }

    public void PlayBGM(AudioClip clip, string subtitle, bool isLoop)
    {
        if (clip == null) return;
        if (clip.Equals(bgmAudio.clip)) return;
        StartCoroutine(CoroutinePlayBGM(clip));

        //[ �� ] BGM ��� ��, bgmSubtitle ���
        if(bgmSubTxt!=null)bgmSubTxt.text = "[ �� " + subtitle + " ]";
        currentBgmSub = subtitle;
    }
    IEnumerator CoroutinePlayBGM(AudioClip clip)
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        if (bgmAudio.isPlaying)
        {
            float minus = bgmVolume / 10;
            while (bgmAudio.volume > 0)
            {
                bgmAudio.volume -= minus;
                yield return wait;
            }
            bgmAudio.Stop();
            bgmAudio.volume = 0;
            bgmAudio.clip = null;
        }

        bgmAudio.clip = clip;

        float plus = bgmVolume / 10;
        bgmAudio.Play();
        while (bgmAudio.volume < bgmVolume)
        {
            bgmAudio.volume += plus;
            yield return wait;
        }
        bgmAudio.volume = bgmVolume;
        yield return null;
    }

    public void PlayAmbience(AudioClip clip, string subtitle)
    {
        if (clip == null) return;
        if (clip.Equals(bgmAudio.clip)) return;
        ambAudio.Stop();
        ambAudio.clip = clip;
        ambAudio.Play();
        if(ambSubTxt!=null) ambSubTxt.text = "[ " + subtitle + "-��� ]";
        
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
        if (bgmAudio.isPlaying)
            bgmAudio.Stop();

        if (bgmSubTxt != null && currentBgmSub != null)
        {
            currentBgmSub = currentBgmSub.Replace("����", "����");
            currentBgmSub = currentBgmSub.Replace("�ٽ� ����", "����");
            currentBgmSub = currentBgmSub.Replace("����", "����");

            //[ �� ]
            bgmSubTxt.text = "[ �� " + currentBgmSub + " ]";
            currentBgmSub = "";
        }
    }

    public void PauseBGM()
    {
        bgmAudio.Pause();

        currentBgmSub = currentBgmSub.Replace("����", "����");
        currentBgmSub = currentBgmSub.Replace("-���", "����");
        currentBgmSub = currentBgmSub.Replace("�ٽ� ����", "����");

        //[ �� ]
       if(bgmSubTxt!=null) bgmSubTxt.text = "[ �� " + currentBgmSub + " ]";
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
