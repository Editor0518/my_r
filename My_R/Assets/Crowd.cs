using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowd : MonoBehaviour
{
    public SpriteRenderer crowdSpr;
    public CrowdType crowdType;

    public int supportPercent = 20;
    public int doubtPercent = 80;
    public int hostilePercent = 0;

    public enum CrowdType
    {
        Support,
        Doubt,
        Hostile
    }
    private void Start()
    {
        crowdSpr = GetComponent<SpriteRenderer>();
    }

    public void RandomResetValue()
    {
        int ran = Random.Range(0, 100);
        //Support ∫Ò¿≤: 20%, Doubt 80%, hostile: 0%
        if (ran < supportPercent)
        {
            ChangeCrowdType(CrowdType.Support);
        }
        else if (ran < supportPercent + doubtPercent)
        {
            ChangeCrowdType(CrowdType.Doubt);
        }
        else
        {
            ChangeCrowdType(CrowdType.Hostile);
        }
    }

    public void ChangeCrowdType(CrowdType type)
    {
        crowdType = type;
        switch (crowdType)
        {
            case CrowdType.Support:
                crowdSpr.color = Color.green;
                break;
            case CrowdType.Doubt:
                crowdSpr.color = Color.yellow;
                break;
            case CrowdType.Hostile:
                crowdSpr.color = Color.red;
                break;
        }
    }
}
