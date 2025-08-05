using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Minigame_MovieChoice : MonoBehaviour
{
    public GameObject thisWhole;

    [System.Serializable]
    public class Movie
    {
        public string title;
        public string cmd;
        public Image poster;
        public string genre;
        public string director;
        [Range(0, 5)] public int rate;
        [TextArea] public string description;
    }

    public List<Movie> movies = new List<Movie>();
    public int currentMovieIndex = 0;
    public int nextBlock;
    public string cmdVarName = "ch2movie";

    public RectTransform selectContentRect;
    float selectContentRectRight = -715;

    [Header("Movie Info")]
    public GameObject infoWhole;
    public TMP_Text titleTxt;
    public TMP_Text genreTxt;
    public TMP_Text directorTxt;
    public TMP_Text rateTxt;
    public RectTransform contentRect;
    public TMP_Text descriptionTxt;
    public Image posterImg;

    bool isInfoOpen = false;

    public void MoveMovieInfo(int direction)
    {
        currentMovieIndex += direction;
        if (currentMovieIndex < 0) currentMovieIndex = movies.Count - 1;
        else if (currentMovieIndex >= movies.Count) currentMovieIndex = 0;
        OpenMovieInfo(currentMovieIndex);
    }
    public void OpenMovieInfo(int index)
    {
        currentMovieIndex = index;
        titleTxt.text = movies[index].title;
        genreTxt.text = "�帣 | " + movies[index].genre;
        directorTxt.text = "���� | " + movies[index].director;
        rateTxt.text = "���� | ";
        for (int i = 0; i < 5; i++)
        {
            if (i < movies[index].rate) rateTxt.text += "��";
            else rateTxt.text += "��";
        }
        descriptionTxt.text = movies[index].description;
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, descriptionTxt.text.Length * 1.8f);
        contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, 0);
        posterImg.sprite = movies[index].poster.sprite;
        infoWhole.SetActive(true);
        isInfoOpen = true;
    }

    public void CloseMovieInfo()
    {
        infoWhole.SetActive(false);
        isInfoOpen = false;
    }

    public void ChooseThisMovie()
    {
        PlayerPrefs.SetString(cmdVarName, movies[currentMovieIndex].cmd);
        //DialogueManager.instance.ChangeCurrentBlock(nextBlock);
        DialogueMaster.canClickToNext = true;
        DialogueMaster.isNoNext = false;
        Destroy(thisWhole.transform.parent.gameObject);
    }

    public void MovePosterView(int direction)
    {

        selectContentRect.anchoredPosition = new Vector2(direction * selectContentRectRight, selectContentRect.anchoredPosition.y);
    }

    private void Update()
    {
        if (isInfoOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) CloseMovieInfo();
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) MoveMovieInfo(-1);
            else if (Input.GetKeyDown(KeyCode.RightArrow)) MoveMovieInfo(1);
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) ChooseThisMovie();
        }
    }

}
