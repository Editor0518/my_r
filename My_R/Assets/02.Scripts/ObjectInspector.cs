using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CarInfo
{
    public int name;
    public float speed;
}

[Serializable
    ]
public class MonsterInfo
{
    public string name;
    public int power;
    public int hp;
    public int mp;
}


public enum ObjectType
{
    CAR,
    MONSTER
}

public class ObjectInspector : MonoBehaviour
{
    // ---------------------------------------------------
    // HideInInspector 어트리뷰트를 사용해서 해당 필드를 일단 숨겨줍니다.
    // 추 후 에디터 코드에서 해당 필드를 노출시켜 줄 예정입니다.
    // ---------------------------------------------------
    [HideInInspector] [SerializeField] ObjectType _objectType;
    [HideInInspector] [SerializeField] private CarInfo _carInfo;
    [HideInInspector] [SerializeField] private List<CarInfo> _carInfolist;
    [HideInInspector] [SerializeField] private MonsterInfo _monsterInfo;
}