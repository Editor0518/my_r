using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minigame_MovieScreen : MonoBehaviour
{
    [System.Serializable]
    public class MovieScenario
    {
        public Sprite scene;
        public string directing;
        public AudioClip soundClip;
    }

    [System.Serializable]
    public class MoiveDirecting
    {
        public string movieName;
        public int startBlock;
        public int endBlock;
        public List<MovieScenario> scenario;
    }

    public Image movieScreen;
    public string cmdVarName = "ch2movie";
    public string movieName;
    int currentScenarioIndex = -1;

    public List<MoiveDirecting> movieDirecting;

    private void OnEnable()
    {
        movieName = PlayerPrefs.GetString(cmdVarName, "영화티켓고르보");
        currentScenarioIndex = movieDirecting.FindIndex(x => x.movieName == movieName);
        StartMovie();
    }

    public void StartMovie()
    {
        DialogueManager.instance.dirManager.prefab = this.gameObject;

        DialogueManager.instance.ChangeCurrentBlock(movieDirecting[currentScenarioIndex].startBlock);
    }

    public void ChangeMovieScreen(int index)
    {
        movieScreen.sprite = movieDirecting[currentScenarioIndex].scenario[index].scene;
    }

    public void CloseMovieScreen()
    {
        DialogueManager.instance.dirManager.prefab = null;
        DialogueManager.instance.ChangeCurrentBlock(movieDirecting[currentScenarioIndex].endBlock);
        this.gameObject.SetActive(false);
    }
}
