using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SaveSlot;

public class Minigame_Crowd : MonoBehaviour
{
    /// <summary>
    /// 군중 오브젝트
    ///- Crowd : 일정 영역으로 다가옴.게이지 관리.특성(지지/의심/적대) 변수 있
    ///별도의 군중 소환 클래스
    ///랜덤시간마다 랜덤 위치에 군중 소환.군중을 리스트의 리스트로 관리함
    ///앙졸라스의 위치 변경시마다 함수 계속 call됨 군중클래스가 갖고 있음
    /// </summary>



    [System.Serializable]
    public class CrowdHolder
    {
        public string posName;
        public Transform holderPos;
        public GameObject gaugeObject;
        public Image gaugeFill;
        public List<Crowd> cwdList;
    }

    public Minigame_SpeechManager sm;
    public Crowd crowdIns;
    public float crowdBoost = 1f;


    [Header("Crowd")]
    public List<CrowdHolder> crowdHolders;
    public int currentPlayerIndex;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IECrowdSpawn());
        GaugeSetting();
    }

    void GaugeSetting()
    {
        for (int i = 0; i < crowdHolders.Count; i++)
        {
            crowdHolders[i].gaugeObject.SetActive(crowdHolders[i].cwdList.Count > 0);
        }
    }

    IEnumerator IECrowdSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            int ranSpot = Random.Range(0, crowdHolders.Count);
            Crowd crowd = Instantiate(crowdIns, new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-4.5f, 4.5f), 0), Quaternion.identity);
            crowd.RandomResetValue();

            crowd.transform.SetParent(crowdHolders[ranSpot].holderPos);
            crowdHolders[ranSpot].cwdList.Add(crowd);

            //0, 2
            float direction = ranSpot % 2 == 0 ? -1 : 1;
            crowd.transform.localPosition = new Vector3((crowdHolders[ranSpot].cwdList.Count - 1) * 0.5f * direction, 0);

            if (crowdHolders[ranSpot].cwdList.Count == 1)
            {
                CrowdGaugeReset(ranSpot);
            }
            GaugeSetting();
        }
    }


    public void CrowdGaugeChange(int spot, float value, bool isIgnoreType = false)
    {
        if (crowdHolders[spot].cwdList.Count == 0) return;

        //군중의 타입에 따라 달라짐
        switch (crowdHolders[spot].cwdList[0].crowdType)
        {
            case Crowd.CrowdType.Support:
                value = value * crowdBoost;
                break;
            case Crowd.CrowdType.Doubt:
                value = value * (isIgnoreType ? 1f : 0.25f) * crowdBoost;
                break;
            case Crowd.CrowdType.Hostile:
                value = -value * 0.5f;
                break;

        }
        crowdHolders[spot].gaugeFill.fillAmount += value;

        if (crowdHolders[spot].cwdList[0].crowdType.Equals(Crowd.CrowdType.Hostile))
        {
            if (crowdHolders[spot].gaugeFill.fillAmount <= 0f) RemoveOneCrowd(spot);

        }
        else if (crowdHolders[spot].gaugeFill.fillAmount >= 1f)
        {
            sm.GaugeChange(15);
            RemoveOneCrowd(spot);

        }
    }

    public void CrowdGaugeReset(int spot)
    {
        if (crowdHolders[spot].cwdList.Count == 0) return;
        if (crowdHolders[spot].cwdList[0].crowdType.Equals(Crowd.CrowdType.Hostile))
            crowdHolders[spot].gaugeFill.fillAmount = 1;
        else
            crowdHolders[spot].gaugeFill.fillAmount = 0;
    }


    public void RemoveOneCrowd(int spot)
    {
        if (crowdHolders[spot].cwdList.Count == 0) return;  // ✅ 리스트가 비었으면 실행 안 함

        Crowd crowd = crowdHolders[spot].cwdList[0];
        crowdHolders[spot].cwdList.RemoveAt(0);
        Destroy(crowd.gameObject);

        float direction = spot % 2 == 0 ? 1 : -1;
        for (int i = 0; i < crowdHolders[spot].cwdList.Count; i++)
        {
            crowdHolders[spot].cwdList[i].transform.position += new Vector3(0.5f * direction, 0, 0);
        }
        CrowdGaugeReset(spot);  // ✅ 게이지 초기화 추가
        GaugeSetting();
    }

    public void RemoveHostileInLine()
    {
        for (int i = 0; i < crowdHolders.Count; i++)
        {
            if (crowdHolders[i].cwdList.Count == 0) continue;
            if (crowdHolders[i].cwdList[0].crowdType.Equals(Crowd.CrowdType.Hostile))
            {
                RemoveOneCrowd(i);
            }
        }
    }

    public void ChangeDoubt2Support()
    {
        for (int i = 0; i < crowdHolders.Count; i++)
        {
            if (crowdHolders[i].cwdList.Count == 0) continue;
            if (crowdHolders[i].cwdList[0].crowdType.Equals(Crowd.CrowdType.Doubt))
            {
                crowdHolders[i].cwdList[0].ChangeCrowdType(Crowd.CrowdType.Support);
            }
            if (crowdHolders[i].cwdList.Count == 1) continue;
            if (crowdHolders[i].cwdList[1].crowdType.Equals(Crowd.CrowdType.Doubt))
            {
                crowdHolders[i].cwdList[1].ChangeCrowdType(Crowd.CrowdType.Support);
            }

        }
    }

}
