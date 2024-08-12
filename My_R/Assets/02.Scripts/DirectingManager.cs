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

    [Header("Fade")]
    public Animator fadeAnim;
    public Image fadeImg;

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
            case "goleft"://go left
                if (next.Contains("left")) StandingGoLeft(0, false);
                if (next.Contains("center")) StandingGoLeft(1, false);
                if (next.Contains("right")) StandingGoLeft(2, false);
                break;
            case "goleftlittle"://go left little
                if (next.Contains("left")) StandingGoLeft(0, true);
                if (next.Contains("center")) StandingGoLeft(1, true);
                if (next.Contains("right")) StandingGoLeft(2, true);
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
                if (next.Contains("left")) StandingGoRight(0, false);
                if (next.Contains("center")) StandingGoRight(1, false);
                if (next.Contains("right")) StandingGoRight(2, false);
                break;
            case "gorightlittle"://go right little
                if (next.Contains("left")) StandingGoRight(0, true);
                if (next.Contains("center")) StandingGoRight(1, true);
                if (next.Contains("right")) StandingGoRight(2, true);
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
                break;
        }
    }

    public void BloodSplash()
    {
        bloodSplashAnim.gameObject.SetActive(true);
        SoundManager.instance.PlaySound(bloodSplashSound, "피 튀기는 소리");
    }

    #region 스탠딩 관련 함수

    public void StandingClear(int index)
    {

        stands[index].SetTrigger("clear");
        Vector3 scale = stands[index].transform.parent.localScale;
        stands[index].transform.parent.localScale = new Vector3(scale.y, scale.y, scale.z);
    }
    public void StandingGoRight(int index, bool isLittle)
    {
        if (isLittle)
            stands[index].SetTrigger("go_right_little");
        else stands[index].SetTrigger("go_right");
    }

    public void StandingGoBackRight(int index, bool isLittle)
    {
        if (isLittle)
            stands[index].SetTrigger("goback_right_little");
        else stands[index].SetTrigger("goback_right");
    }

    public void StandingGoLeft(int index, bool isLittle)
    {
        if (isLittle)
            stands[index].SetTrigger("go_left_little");
        else stands[index].SetTrigger("go_left");
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
    public void ScreenBlack()
    {
        fadeImg.gameObject.SetActive(false);
        fadeImg.color = Color.black;
        fadeImg.gameObject.SetActive(true);
    }

    public void ScreenWhite()
    {
        fadeImg.gameObject.SetActive(false);
        fadeImg.color = Color.white;
        fadeImg.gameObject.SetActive(true);
    }

    public void ScreenClear()
    {
        fadeImg.gameObject.SetActive(false);
        fadeImg.color = Color.clear;
        fadeImg.gameObject.SetActive(true);
    }
    public void FadeInBlack()
    {
        fadeImg.color = Color.black;
        FadeIn();
    }

    public void FadeInWhite()
    {
        fadeImg.color = Color.white;
        FadeIn();
    }

    void FadeIn()
    {
        fadeImg.gameObject.SetActive(true);
        fadeAnim.SetTrigger("FadeIn");
    }

    public void FadeOutBlack()
    {
        fadeImg.color = Color.black;
        FadeOut();
    }

    public void FadeOutWhite()
    {
        fadeImg.color = Color.white;
        FadeOut();
    }

    void FadeOut()
    {
        fadeImg.gameObject.SetActive(true);
        fadeAnim.SetTrigger("FadeOut");
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

    private void Start()
    {
        if (miniCutAnim.gameObject.activeInHierarchy) MiniCutDisable();
        camAnim = cam.GetComponent<Animator>();
    }

}
