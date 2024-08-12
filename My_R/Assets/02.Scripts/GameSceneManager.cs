using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;
    public int chapter=-1;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    public void LoadScene(int index) {
        Debug.Log("�� �̵�! " + index);
        SceneManager.LoadScene(index);
    }

    public void LoadScene(string sceneName) {
        Debug.Log("�� �̵�! " + sceneName);
        SceneManager.LoadScene(sceneName);
    }



}
