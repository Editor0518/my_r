//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectingManager : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public DirectingSpr dspr;
    public Animator dialogue;
    public GameObject standings;

    [Header("Cam")]
    public Camera cam;
    public Animator camAnim;

    [Header("Screen Effect Panel")]
    public Animator screenEffectAnim;
    public Image screenEffectImg;

    [Header("Standing Move")]
    public List<Animator> stands = new List<Animator>(3);

    [Header("Mini cutscene")]
    public Animator miniCutAnim;
    public Image miniCutImg;

    [Header("Blood Splash")]
    public Animator bloodSplashAnim;
    public AudioClip bloodSplashSound;

    [Header("Background")]
    public SpriteRenderer backgroundRender;
    public Material m_blur;
    public Material m_dft;

    Vector3 standingDefaultPos;
    Vector3[] dftpos = new Vector3[3];

    private void Start()
    {
        dftpos = new Vector3[3];
        for (int i = 0; i < 3; i++)
            dftpos[i] = stands[i].transform.parent.position;

        if (miniCutAnim.gameObject.activeInHierarchy) MiniCutDisable();
        camAnim = cam.GetComponent<Animator>();
        SetDefaultPos();
    }

    void SetDefaultPos()
    {//스탠딩 parent(전체) 위치 저장. 캐가 가까이 다가올거 초기화값
        standingDefaultPos = stands[0].transform.parent.position;
    }

    void BackgroundBlur()
    {
        backgroundRender.material = m_blur;
    }
    void BackgroundReset()
    {
        backgroundRender.material = m_dft;
    }


    public void RunCMD(string cmd, string next)
    {
        switch (cmd)
        {
            case "minion"://미니컷씬 온
                //minion_파일명
                AppearMiniCutscene(next);
                dialogueManager.ChangeIsMiniOn(true);

                break;
            case "minioff"://미니컷씬 오프
                DisappearMiniCutscene();
                dialogueManager.ChangeIsMiniOn(false);

                break;
            case "minichange":
                //미니컷씬 변경
                miniCutImg.sprite = dspr.FindCutSprite(next);
                break;
            case "closeup":
                CloseUp(int.Parse(next));
                break;
            case "defaultpos"://default position
                DefaultPos();
                break;
            case "fadein"://fade in
                if (next.Equals("black")) FadeInBlack();
                else if (next.Equals("white")) FadeInWhite();
                break;
            case "fadeout"://fade out
                if (next.Equals("black")) FadeOutBlack();
                else if (next.Equals("white")) FadeOutWhite();
                break;
            case "screen":
                if (next.Equals("black")) ScreenBlack();
                else if (next.Equals("white")) ScreenWhite();
                else if (next.Equals("clear")) ScreenClear();
                break;
            case "shake"://스탠딩 흔들기
                if (next.Contains("left")) StandingShake(0);
                if (next.Contains("center")) StandingShake(1);
                if (next.Contains("right")) StandingShake(2);
                break;
            case "flinch"://스탠딩 움찔
                if (next.Contains("left")) StandingFlinch(0);
                if (next.Contains("center")) StandingFlinch(1);
                if (next.Contains("right")) StandingFlinch(2);
                break;
            case "stretch":
                //스탠딩 늘리기, 하품, 기지개 등.
                if (next.Contains("left")) StandingStretch(0);
                if (next.Contains("center")) StandingStretch(1);
                if (next.Contains("right")) StandingStretch(2);
                break;
            case "sighshort":
                if (next.Contains("left")) Sigh(0, 0);
                if (next.Contains("center")) Sigh(1, 0);
                if (next.Contains("right")) Sigh(2, 0);
                break;
            case "sighlong":
                if (next.Contains("left")) Sigh(0, 1);
                if (next.Contains("center")) Sigh(1, 1);
                if (next.Contains("right")) Sigh(2, 1);
                break;
            case "goleft"://go left
                if (next.Contains("left")) StandingGoLeft(0, 2);
                if (next.Contains("center")) StandingGoLeft(1, 2);
                if (next.Contains("right")) StandingGoLeft(2, 2);
                break;
            case "goleftlittle"://go left little
                if (next.Contains("left")) StandingGoLeft(0, 0);
                if (next.Contains("center")) StandingGoLeft(1, 0);
                if (next.Contains("right")) StandingGoLeft(2, 0);
                break;
            case "goleftlittleleft":
                if (next.Contains("left")) StandingGoLeft(0, 1);
                if (next.Contains("center")) StandingGoLeft(1, 1);
                if (next.Contains("right")) StandingGoLeft(2, 1);
                break;
            case "gobackleft"://go back left
                if (next.Contains("left")) StandingGoBackLeft(0, false);
                if (next.Contains("center")) StandingGoBackLeft(1, false);
                if (next.Contains("right")) StandingGoBackLeft(2, false);
                break;
            case "gobackleftlittle"://go back left little
                if (next.Contains("left")) StandingGoBackLeft(0, true);
                if (next.Contains("center")) StandingGoBackLeft(1, true);
                if (next.Contains("right")) StandingGoBackLeft(2, true);
                break;
            case "goright"://go right
                if (next.Contains("left")) StandingGoRight(0, 2);
                if (next.Contains("center")) StandingGoRight(1, 2);
                if (next.Contains("right")) StandingGoRight(2, 2);
                break;
            case "gorightlittle"://go right little
                if (next.Contains("left")) StandingGoRight(0, 0);
                if (next.Contains("center")) StandingGoRight(1, 0);
                if (next.Contains("right")) StandingGoRight(2, 0);
                break;
            case "gorightlittleright":
                if (next.Contains("left")) StandingGoRight(0, 1);
                if (next.Contains("center")) StandingGoRight(1, 1);
                if (next.Contains("right")) StandingGoRight(2, 1);
                break;
            case "gobackright"://go back right
                if (next.Contains("left")) StandingGoBackRight(0, false);
                if (next.Contains("center")) StandingGoBackRight(1, false);
                if (next.Contains("right")) StandingGoBackRight(2, false);
                break;
            case "gobackrightlittle"://go back right little
                if (next.Contains("left")) StandingGoBackRight(0, true);
                if (next.Contains("center")) StandingGoBackRight(1, true);
                if (next.Contains("right")) StandingGoBackRight(2, true);
                break;
            case "flip":
                if (next.Contains("left")) StandingFlip(0);
                if (next.Contains("center")) StandingFlip(1);
                if (next.Contains("right")) StandingFlip(2);
                break;
            case "clearmoving"://clear moving, 스탠딩 제자리로
                if (next.Contains("left")) StandingClear(0);
                if (next.Contains("center")) StandingClear(1);
                if (next.Contains("right")) StandingClear(2);
                if (next.Contains("all")) for (int i = 0; i < 3; i++) StandingClear(i);
                break;
            case "background":
                if (next.Equals("blur")) BackgroundBlur();
                else if (next.Equals("reset")) BackgroundReset();
                break;
            case "bloodsplash":

                BloodSplash();
                break;
            case "look":
                if (next.Equals("updowndoubted")) LookCamera("look_updowndoubted", 4f);
                else if (next.Equals("up")) LookCamera("look_up", 2f);
                else if (next.Equals("down")) LookCamera("look_down", 2f);
                else if (next.Equals("leftright")) LookCamera("look_leftright", 4f);
                else if (next.Equals("updown")) LookCamera("look_updown", 2f);
                break;
            case "letterbox":
                if (next.Equals("on")) LetterBox(true);
                else if (next.Equals("off")) LetterBox(false);
                break;
            case "comeclose0":
                if (next.Contains("left")) StandingComeClose(0, 0);
                if (next.Contains("center")) StandingComeClose(1, 0);
                if (next.Contains("right")) StandingComeClose(2, 0);
                break;
            case "comeclose1":
                if (next.Contains("left")) StandingComeClose(0, 1);
                if (next.Contains("center")) StandingComeClose(1, 1);
                if (next.Contains("right")) StandingComeClose(2, 1);
                break;
            case "comeclose2":
                if (next.Contains("left")) StandingComeClose(0, 2);
                if (next.Contains("center")) StandingComeClose(1, 2);
                if (next.Contains("right")) StandingComeClose(2, 2);
                break;
        }
    }

    public void BloodSplash()
    {
        bloodSplashAnim.gameObject.SetActive(true);
        SoundManager.instance.PlaySound(bloodSplashSound, "피 튀기는 소리");
    }

    #region 스탠딩 관련 함수

    void StandingStretch(int index)
    {
        StartCoroutine(StandStretch(index));
    }

    IEnumerator StandStretch(int index)
    {
        /*
         45f 1.08
        65f 1.1
        85f 1
        */
        float size = 1f;
        Vector3 scale = stands[index].transform.localScale;
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        for (int i = 0; i < 17; i++)
        {
            stands[index].transform.parent.localScale = new Vector3(scale.x, scale.y * size, scale.z);
            if (i <= 9)
            {
                size += 0.01f;
            }
            else if (i <= 12)
            {
                size += 0.0025f;
            }
            else
            {
                size -= 0.025f;
            }
            yield return wait;
        }
        stands[index].transform.parent.localScale = new Vector3(scale.x, scale.y, scale.z);
    }

    void Sigh(int index, int level)
    {
        switch (level)
        {
            case 0:
                StartCoroutine(SighShort(index));
                break;
            case 1:
                StartCoroutine(SighLong(index));
                break;

        }
    }

    IEnumerator SighLong(int index)
    {
        /*
        0
        pos y0
        scale y1
        60
        pos y-0.4
        scale y0.95
        90
        pos y-0.4
        scale y0.95
        105
        pos y0
        scale y1
         */
        Vector3 scale = stands[index].transform.parent.localScale;
        Vector3 pos = stands[index].transform.parent.position;
        WaitForSeconds wait = new WaitForSeconds(0.05f);
        float scaleToAdd = 0, posToAdd = 0;

        for (int i = 0; i < 21; i++)
        {
            stands[index].transform.parent.position = new Vector3(pos.x, pos.y + posToAdd, pos.z);
            stands[index].transform.parent.localScale = new Vector3(scale.x, scale.y + scaleToAdd, scale.z);
            if (i <= 12)
            {
                posToAdd -= 0.03f;
                scaleToAdd -= 0.004f;
            }
            else if (i > 18)
            {
                posToAdd += 0.18f;
                scaleToAdd += 0.024f;
            }
            yield return wait;
        }
        stands[index].transform.parent.position = pos;
        stands[index].transform.parent.localScale = scale;

    }

    IEnumerator SighShort(int index)
    {
        /*
         0
        pos y0
        scale y1
        20
        pos y-0.4
        scale y0.95
        45
        pos y-0.4
        scale y0.95
        90
        pos y0
        scale y1
         */
        Vector3 scale = stands[index].transform.parent.localScale;
        Vector3 pos = stands[index].transform.parent.position;
        WaitForSeconds wait = new WaitForSeconds(0.05f);
        float scaleToAdd = 0, posToAdd = 0;
        for (int i = 0; i < 18; i++)
        {
            stands[index].transform.parent.position = new Vector3(pos.x, pos.y + posToAdd, pos.z);
            stands[index].transform.parent.localScale = new Vector3(scale.x, scale.y + scaleToAdd, scale.z);
            if (i <= 4)
            {
                posToAdd -= 0.1f;
                scaleToAdd -= 0.0125f;
            }
            else if (i > 9)
            {
                posToAdd += 0.0444f;
                scaleToAdd += 0.0055f;
            }
            yield return wait;
        }
        stands[index].transform.parent.position = pos;
        stands[index].transform.parent.localScale = scale;


    }

    public void StandingClear(int index)
    {
        //StandingComeClose(index, 0);
        stands[index].gameObject.SetActive(false);
        stands[index].gameObject.SetActive(true);
        stands[index].transform.parent.position = dftpos[index];
        //stands[index].SetTrigger("clear");
        Vector3 scale = Vector3.one;//stands[index].transform.parent.localScale;
        stands[index].transform.parent.localScale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);
    }
    public void StandingGoRight(int index, int level)
    {
        switch (level)
        {
            case 0:
                stands[index].SetTrigger("go_right_little");
                break;
            case 1:
                stands[index].SetTrigger("go_right_little_right");
                break;
            case 2:
                stands[index].SetTrigger("go_right");
                break;
        }
    }

    public void StandingGoBackRight(int index, bool isLittle)
    {
        if (isLittle)
            stands[index].SetTrigger("goback_right_little");
        else stands[index].SetTrigger("goback_right");
    }

    public void StandingGoLeft(int index, int level)
    {
        switch (level)
        {
            case 0:
                stands[index].SetTrigger("go_left_little");
                break;
            case 1:
                stands[index].SetTrigger("go_left_little_left");
                break;
            case 2:
                stands[index].SetTrigger("go_left");
                break;
        }
    }

    public void StandingGoBackLeft(int index, bool isLittle)
    {
        if (isLittle)
            stands[index].SetTrigger("goback_left_little");
        else stands[index].SetTrigger("goback_left");
    }

    public void StandingFlip(int index)
    {
        StartCoroutine(StandFlip(index));
    }

    IEnumerator StandFlip(int index)
    {
        Vector3 scale = stands[index].transform.parent.localScale;
        float elapsed = scale.x;
        float delta = 5f;

        if (scale.x > 0)
        {//1 --> -1
            while (elapsed > -scale.x)
            {
                stands[index].transform.parent.localScale = new Vector3(elapsed, scale.y, scale.z);

                elapsed -= Time.deltaTime * delta;

                yield return null;
            }
        }
        else
        {
            while (elapsed < -scale.x)
            {
                stands[index].transform.parent.localScale = new Vector3(elapsed, scale.y, scale.z);

                elapsed += Time.deltaTime * delta;

                yield return null;
            }
        }
        stands[index].transform.parent.localScale = new Vector3(-scale.x, scale.y, scale.z);
    }

    public void StandingComeClose(int index, int level)
    {
        int isMinus = stands[index].transform.parent.localScale.x > 0 ? 1 : -1;
        Vector3 dftPos = new Vector3(stands[index].transform.parent.position.x, standingDefaultPos.y, stands[index].transform.parent.position.z);
        switch (level)
        {
            case 0://reset
                stands[index].transform.parent.position = dftPos;
                stands[index].transform.parent.localScale = new Vector3(isMinus * 1, 1, 1);
                break;
            case 1://close little
                   //-0.4, 1.15
                stands[index].transform.parent.position = dftPos + new Vector3(0, -0.4f, 0);
                stands[index].transform.parent.localScale = new Vector3(isMinus * 1.15f, 1.15f, 1.15f);
                break;
            case 2://close lot
                   //-1.15, 1.4
                stands[index].transform.parent.position = dftPos + new Vector3(0, -1.15f, 0);
                stands[index].transform.parent.localScale = new Vector3(isMinus * 1.4f, 1.4f, 1.4f);
                break;
        }

    }


    public void StandingShake(int index)
    {
        StartCoroutine(StandShake(0.25f, 0.1f, index));
    }

    public void StandingFlinch(int index)
    {
        StartCoroutine(StandShake(0.05f, 0.075f, index));
    }

    IEnumerator StandShake(float dur, float mag, int index)
    {
        Vector2 originalPos = stands[index].transform.parent.localPosition;
        float elapsed = 0;

        while (elapsed < dur)
        {
            float x = Random.Range(-1, 1) * mag;
            float y = Random.Range(-1, 1.5f) * mag;

            stands[index].transform.parent.localPosition = new Vector2(originalPos.x + x, originalPos.y + y);

            elapsed += Time.deltaTime;

            yield return null;
        }
        stands[index].transform.parent.localPosition = originalPos;

    }
    #endregion
    #region 화면 관련 함수

    void LetterBox(bool isTrue)
    {
        screenEffectAnim.gameObject.SetActive(true);
        screenEffectAnim.SetBool("letterbox", isTrue);

    }
    public void ScreenBlack()
    {
        // fadeImg.gameObject.SetActive(false);

        //fadeImg.color = Color.black;
        screenEffectImg.gameObject.SetActive(true);
        screenEffectAnim.SetTrigger("Black");
    }

    public void ScreenWhite()
    {
        // fadeImg.gameObject.SetActive(false);

        //fadeImg.color = Color.white;
        screenEffectImg.gameObject.SetActive(true);
        screenEffectAnim.SetTrigger("White");
    }

    public void ScreenClear()
    {
        screenEffectImg.gameObject.SetActive(false);
        //fadeImg.color = Color.clear;
        screenEffectImg.gameObject.SetActive(true);
        screenEffectAnim.gameObject.SetActive(false);
    }
    public void FadeInBlack()
    {
        screenEffectImg.color = Color.black;
        FadeIn();
    }

    public void FadeInWhite()
    {
        screenEffectImg.color = Color.white;
        FadeIn();
    }

    void FadeIn()
    {
        screenEffectImg.gameObject.SetActive(true);
        screenEffectAnim.SetTrigger("FadeIn");
    }

    public void FadeOutBlack()
    {
        screenEffectImg.color = Color.black;
        FadeOut();
    }

    public void FadeOutWhite()
    {
        screenEffectImg.color = Color.white;
        FadeOut();
    }

    void FadeOut()
    {
        screenEffectImg.gameObject.SetActive(true);
        screenEffectAnim.SetTrigger("FadeOut");
    }

    #endregion
    #region 카메라 관련 함수
    public void CloseUp(int focusOn)
    {
        StartCoroutine(CamCloseUp());
    }
    IEnumerator CamCloseUp()
    {
        WaitForSeconds wait = new WaitForSeconds(0.01f);

        while (cam.orthographicSize > 4f)
        {
            cam.orthographicSize -= 0.1f;
            yield return wait;
        }
        cam.orthographicSize = 4f;
        yield return null;
    }

    IEnumerator CamDefaultPos()
    {
        WaitForSeconds wait = new WaitForSeconds(0.01f);

        while (cam.orthographicSize < 5f)
        {
            cam.orthographicSize += 0.1f;
            yield return wait;
        }
        cam.orthographicSize = 5f;
        yield return null;
    }
    public void DefaultPos()
    {
        StartCoroutine(CamDefaultPos());
    }

    void LookCamera(string trigger, float wait)
    {
        camAnim.SetTrigger(trigger);
        StartCoroutine(DialogueOn(wait));

    }


    IEnumerator DialogueOn(float waitTime)
    {
        dialogue.gameObject.SetActive(false);
        yield return new WaitForSeconds(waitTime);
        dialogue.gameObject.SetActive(true);
    }

    #endregion

    //when start_cmd is "minion_파일명"
    public void AppearMiniCutscene(string cutName)
    {
        if (miniCutAnim.gameObject.activeInHierarchy) MiniCutDisable();
        miniCutImg.sprite = dspr.FindCutSprite(cutName);
        if (miniCutImg.sprite == null)
        {
            Debug.Log("miniCutImg is null !!"); return;
        }
        miniCutAnim.gameObject.SetActive(true);
        DialogueOn(1);
    }
    public void DisappearMiniCutscene()
    {
        standings.SetActive(false);
        miniCutAnim.SetTrigger("OFF");
        Invoke("MiniCutDisable", 0.1f);

    }

    void MiniCutDisable()
    {
        standings.SetActive(true);
        miniCutAnim.gameObject.SetActive(false);
    }


}
