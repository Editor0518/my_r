using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,
    ISubmitHandler
{
    public bool useTextHightlightColor = true;
    public Color textHighlightColor = Color.black;
    public Color textNormalColor = Color.white;
    public AudioClip seToPlayOnEnter;
    public AudioClip seToPlayOnClick;

    private Button btn;
    public TMP_Text btnText;
    private Vector2 highlightScale;
    private Vector2 normalScale;

    private void Start()
    {
        btn = GetComponent<Button>();
        normalScale = transform.localScale;
        highlightScale = normalScale * 1.1f; // Highlight 시 크기를 10% 증가시킴

        if(btnText==null && transform.childCount > 0) btnText = transform.GetChild(0).GetComponent<TMP_Text>();
        if (btnText == null) Debug.Log(gameObject.name + " UIButtonHighlight: No TMP_Text found in child 0 objects.");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
        if(seToPlayOnClick!=null)
            SoundManager.instance.PlayUISound(seToPlayOnClick);
        Deselect();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        BtnTextHighlight();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BtnTextNormal();
    }

    // ISubmitHandler
    public void OnSubmit(BaseEventData eventData)
    {
        Deselect();
    }

    private void BtnTextHighlight()
    {
       
        if (!btn.interactable) return;
        if (btnText != null&&useTextHightlightColor) btnText.color = textHighlightColor;
        transform.localScale = highlightScale; // 버튼 크기 증가
        
        if(seToPlayOnEnter!=null)
            SoundManager.instance.PlayUISound(seToPlayOnEnter);
    }

    private void BtnTextNormal()
    {
        if (btnText != null&&useTextHightlightColor)
            btnText.color = textNormalColor;
        transform.localScale = normalScale; // 버튼 크기 원래대로
    }

    // 실제 선택 해제 처리
    private void Deselect()
    {
        // 1) 현재 선택된 GameObject를 null로 설정
        EventSystem.current.SetSelectedGameObject(null);
        BtnTextNormal();

        // 2) (선택적으로) 다른 버튼을 선택하고 싶다면
        //    EventSystem.current.SetSelectedGameObject(anotherButton.gameObject);
    }
}