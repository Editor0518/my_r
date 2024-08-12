using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    public List<string> variables;

    /// <summary> @@@@���̺� �ε� ���̵�@@@@
    /// ���̺� ������ 0������ �����Ѵ�.
    /// ���̺� ������ Saves��� Ű�� ����ȴ�.
    /// Saves �ȿ��� ������ �̹����� ����Ʈ �迭��, ��� ������ ������ ����� ��ϵǾ� �ִ�. (�÷� ��Ʈ ���� ���� ����.)
    /// 1�� ���̺� ������ ���̺�� sv1$�� �����ϰ� ;�� ������.
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
        Debug.Log("(SaveManager) CollectedItem: " + PlayerPrefs.GetString("CollectedItem", ""));
        Debug.Log("(SaveManager) Gender: " + PlayerPrefs.GetInt("Gender", -1));
        Debug.Log("(SaveManager) MyName: " + PlayerPrefs.GetString("MyName", "������"));

    }



    public void OpenSavedGame(int load_index)
    {
        variables = LoadData(load_index).Split(',').ToList();

        OpenGame();
    }

    public string LoadData(int index)
    {
        string data = PlayerPrefs.GetString("Saves", "");
        if (!data.Contains("sv" + index + "$")) return "";//����� �����Ͱ� ����

        string[] str = data.Split(';');
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i].Contains("sv" + index + "$"))
            {
                return str[i];
            }
        }
        return "";

    }
    public void SaveData(int index)
    {
        string data = "";
        for (int i = 0; i < variables.Count; i++)
        {
            data += variables[i];
        }
        string qry = PlayerPrefs.GetString("GameData", "") + "sv" + index + "$" + data + ";";
        PlayerPrefs.SetString("GameData", qry);
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
