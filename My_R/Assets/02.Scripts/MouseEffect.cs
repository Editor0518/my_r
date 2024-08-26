using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseEffect : MonoBehaviour
{
    RectTransform transform_cursor;
    Image sprRender;
    public List<Sprite> sprites;
    public AudioClip mouseClick;
    [Range(0.01f, 0.1f)] public float waitTime = 0.03f;

    private void Start()
    {
        sprRender = GetComponent<Image>();
        transform_cursor = GetComponent<RectTransform>();
    }

    void PlaySpriteEffect()
    {
        transform_cursor.localScale = new Vector3(Random.Range(0, 2) == 0 ? 1 : -1, 1f, 1f);
        StopCoroutine("ChangeSpr");
        StartCoroutine("ChangeSpr");
        //SoundManager.instance.PlaySound(mouseClick);
    }


    IEnumerator ChangeSpr()
    {
        WaitForSecondsRealtime wait = new(waitTime);
        for (int i = 0; i < sprites.Count; i++)
        {
            sprRender.sprite = sprites[i];

            yield return wait;
        }
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        transform_cursor.position = mousePos;
        if (Input.GetMouseButtonDown(0))
        {

            PlaySpriteEffect();
        }
    }
}
