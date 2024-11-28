using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    public string settingData;
    public string collectionData;
    public string endingData;
    public string saveData;


    public int[] currentStory = new int[4];//���� �������� ���丮
    public List<string> variables;

    /// <summary> @@@@���̺� �ε� ���̵�@@@@
    /// ���̺� ������ 0������ �����Ѵ�.
    /// ���̺� ������ Saves��� Ű�� ����ȴ�.
    /// Saves �ȿ��� ������ �̹����� ����Ʈ �迭��, ��� ������ ������ ����� ��ϵǾ� �ִ�. (�÷� ��Ʈ ���� ���� ����.)
    /// 1�� ���̺� ������ ���̺�� sv1$�� �����ϰ� ;�� ������.
    /// ������ ��¥, é�ʹ� é��1, ��é��2, �귣ġ5, ������3�� ��� sv1$��¥$é��1,��é��2,�귣ġ5,������3$������;�� ����ȴ�.
    /// sv1$YYYY.MM.DD. HH:MM$1,2,5,3$������;�� ����ȴ�.
    /// �� ���� �������� , �� �����Ѵ�.
    /// ���� ���� varName=value, �� ����Ǿ� �����Ƿ� =�� split�ϸ� �ȴ�.
    /// 
    /// 
    ///--------------
    /// ���̺� ���� �� �����ϴ� ����
    /// *���� �׸���� ������ ���� Ű �̸����� ����ȴ�.
    /// �̸�(������): MyName
    /// ���/ª��: Gender
    /// *���� ���� Settings��� Ű�� ����Ǹ�, ������ , �̴�.
    /// - BGM ����: BGMVolume
    /// - ȿ���� ����: SFXVolume
    /// - ����� ����: AmbienceVolume
    /// - CW �ô���: CWSeen
    /// - �׶��׸� ���� ���: RsBeard 
    /// - ���̾�α� �ؽ�Ʈ ũ�� ����: DialogTextSize
    /// - ���̾�α� �ؽ�Ʈ Ÿ���� �ӵ�: DialogTextSpeed
    /// - ���� ������ ��: AutoClickDelay
    /// - �÷��� ���: Language
    /// 
    /// *���� �׸���� ������ ���� Ű �̸����� ����ȴ�.
    /// ������ ������(����): CollectedItem
    /// ���� ���� ��Ȳ: Endings
    /// ȹ���� ��������: Challenges
    /// �� ���丮��: Viewed
    /// 
    /// 



    private void Awake()
    {
        SaveManager.instance = this;
        DontDestroyOnLoad(this.gameObject);

        var obj = FindObjectsOfType<SaveManager>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        //������
        // ResetAll();
    }


    public void ResetAll()
    {
        PlayerPrefs.DeleteAll();
    }

    public void OpenGame()
    {

    }

    void DebugSave()
    {
        // Debug.Log("(SaveManager) CollectedItem: " + PlayerPrefs.GetString("CollectedItem", ""));
        //  Debug.Log("(SaveManager) Gender: " + PlayerPrefs.GetInt("Gender", -1));
        // Debug.Log("(SaveManager) MyName: " + PlayerPrefs.GetString("MyName", "������"));

    }



    public void OpenSavedGame(int load_index)
    {
        //variables = LoadData(load_index).Split(',').ToList();

        OpenGame();
    }

    public void LoadData(int index)
    {
        string data = saveData; //PlayerPrefs.GetString("Saves", "");
        if (!data.Contains("sv" + index + "$")) return;//����� �����Ͱ� ����

        string[] str = data.Split(';');
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i].Contains("sv" + index + "$"))
            {
                LoadToCurrentData(str[i]);
                return;
            }
        }

    }

    void LoadToCurrentData(string sv)
    {
        /// Saves �ȿ��� ������ �̹����� ����Ʈ �迭��, ��� ������ ������ ����� ��ϵǾ� �ִ�. (�÷� ��Ʈ ���� ���� ����.)
        /// 1�� ���̺� ������ ���̺�� sv1$�� �����ϰ� ;�� ������.
        /// ������ ��¥, é�ʹ� é��1, ��é��2, �귣ġ5, ������3�� ��� sv1$��¥$é��1,��é��2,�귣ġ5,������3$������;�� ����ȴ�.
        /// sv1$YYYY.MM.DD. HH:MM$1,2,5,3$������;�� ����ȴ�.
        /// �� ���� �������� , �� �����Ѵ�.
        /// ���� ���� varName=value, �� ����Ǿ� �����Ƿ� =�� split�ϸ� �ȴ�.
        /// 


        string[] str = sv.Split('$');//str[0] = ���Ը�(�Ⱦ�), str[1]=��¥(�Ⱦ�), str[2]=���丮�����Ȳ, str[3]=������
        string[] story = str[2].Split(',');
        for (int i = 0; i < story.Length; i++)
        {
            currentStory[i] = int.Parse(story[i]);
        }
        string[] vars = str[3].Split(',');
        variables = new List<string>();
        for (int i = 0; i < vars.Length; i++)
        {
            variables.Add(vars[i]);
        }
        Debug.Log("Loaded: " + sv);
    }

    public string[] SaveData(int index)
    {
        string data = "";
        for (int i = 0; i < variables.Count; i++)
        {
            data += variables[i];
        }
        string qry = saveData;// = PlayerPrefs.GetString("GameData", "") + "sv" + index + "$" + data + ";";

        if (qry.Contains("sv" + index + "$"))
        {//�ߺ� ����
            string[] str = qry.Split(';');
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i].Contains("sv" + index + "$"))
                {
                    qry = qry.Replace(str[i] + ";", "");
                    break;
                }

            }
        }
        string[] strReturn = { DateTime.Now.ToString(("yyyy.MM.dd HH:mm")), "Ch." + currentStory[0].ToString() };
        saveData = qry + $"sv{index}${strReturn[0]}${currentStory[0]},{currentStory[1]},{currentStory[2]},{currentStory[3]}${data} ;";//PlayerPrefs.SetString("GameData", qry);
        Debug.Log("Saved: " + saveData);
        return strReturn;
    }


    #region SET
    public void SetMyName(string name)
    {
        if (name.Replace(" ", "").Equals("")) name = "������";
        PlayerPrefs.SetString("MyName", name);
    }

    public void SetGender(bool isMale)
    {
        SetGender(isMale ? 1 : 0);

    }

    public void SetGender(int gender)
    {
        PlayerPrefs.SetInt("Gender", gender);


        DebugSave();
    }
    #endregion
    public void AddCollectedItem(string itemName)
    {
        string[] str = GetCollectedItemArray();
        for (int i = 0; i < str.Length; i++)
        {
            if (str[0].Equals(itemName)) return;
        }

        PlayerPrefs.SetString("CollectedItem", (PlayerPrefs.GetString("CollectedItem", "") + itemName + ","));

        DebugSave();
    }
    #region GET
    public string[] GetCollectedItemArray()
    {
        return PlayerPrefs.GetString("CollectedItem", "").Split(';');
    }

    public string GetMyName()
    {
        return PlayerPrefs.GetString("MyName", "������");
    }

    public bool GetGenderIsMale()
    {//0(false)=long hair, 1(true)=short hair
        return GetGender().Equals(1) ? true : false;
    }
    public int GetGender()
    {//0=long hair, 1=short hair
        return PlayerPrefs.GetInt("Gender", 1);
    }

    #endregion
}
