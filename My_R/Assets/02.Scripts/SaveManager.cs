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


    public int[] currentStory = new int[4];//현재 진행중인 스토리
    public List<string> variables;

    /// <summary> @@@@세이브 로드 가이드@@@@
    /// 세이브 슬롯은 0번부터 시작한다.
    /// 세이브 슬롯은 Saves라는 키에 저장된다.
    /// Saves 안에는 저장한 이미지의 바이트 배열과, 모든 선택지 선택의 결과가 기록되어 있다. (플롯 차트 만들 때도 쓴다.)
    /// 1번 세이브 슬롯의 세이브는 sv1$로 시작하고 ;로 끝난다.
    /// 저장한 날짜, 챕터는 챕터1, 소챕터2, 브랜치5, 페이지3인 경우 sv1$날짜$챕터1,소챕터2,브랜치5,페이지3$변수들;로 저장된다.
    /// sv1$YYYY.MM.DD. HH:MM$1,2,5,3$변수들;로 저장된다.
    /// 그 안의 변수들은 , 로 구분한다.
    /// 변수 값은 varName=value, 로 저장되어 있으므로 =로 split하면 된다.
    /// 
    /// 
    ///--------------
    /// 세이브 슬롯 외 저장하는 정보
    /// *다음 항목들은 다음과 같은 키 이름으로 저장된다.
    /// 이름(앙졸라스): MyName
    /// 긴머/짧머: Gender
    /// *설정 탭은 Settings라는 키에 저장되며, 구분은 , 이다.
    /// - BGM 볼륨: BGMVolume
    /// - 효과음 볼륨: SFXVolume
    /// - 엠비언스 볼륨: AmbienceVolume
    /// - CW 봤는지: CWSeen
    /// - 그랑테르 수염 모드: RsBeard 
    /// - 다이얼로그 텍스트 크기 조절: DialogTextSize
    /// - 다이얼로그 텍스트 타이핑 속도: DialogTextSpeed
    /// - 오토 딜레이 초: AutoClickDelay
    /// - 플레이 언어: Language
    /// 
    /// *다음 항목들은 다음과 같은 키 이름으로 저장된다.
    /// 수집한 아이템(도감): CollectedItem
    /// 엔딩 수집 현황: Endings
    /// 획득한 도전과제: Challenges
    /// 본 스토리들: Viewed
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
        //디버깅용
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
        // Debug.Log("(SaveManager) MyName: " + PlayerPrefs.GetString("MyName", "앙졸라스"));

    }



    public void OpenSavedGame(int load_index)
    {
        //variables = LoadData(load_index).Split(',').ToList();

        OpenGame();
    }

    public void LoadData(int index)
    {
        string data = saveData; //PlayerPrefs.GetString("Saves", "");
        if (!data.Contains("sv" + index + "$")) return;//저장된 데이터가 없음

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
        /// Saves 안에는 저장한 이미지의 바이트 배열과, 모든 선택지 선택의 결과가 기록되어 있다. (플롯 차트 만들 때도 쓴다.)
        /// 1번 세이브 슬롯의 세이브는 sv1$로 시작하고 ;로 끝난다.
        /// 저장한 날짜, 챕터는 챕터1, 소챕터2, 브랜치5, 페이지3인 경우 sv1$날짜$챕터1,소챕터2,브랜치5,페이지3$변수들;로 저장된다.
        /// sv1$YYYY.MM.DD. HH:MM$1,2,5,3$변수들;로 저장된다.
        /// 그 안의 변수들은 , 로 구분한다.
        /// 변수 값은 varName=value, 로 저장되어 있으므로 =로 split하면 된다.
        /// 


        string[] str = sv.Split('$');//str[0] = 슬롯명(안씀), str[1]=날짜(안씀), str[2]=스토리진행상황, str[3]=변수들
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
        {//중복 삭제
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
        if (name.Replace(" ", "").Equals("")) name = "앙졸라스";
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
        return PlayerPrefs.GetString("MyName", "앙졸라스");
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
