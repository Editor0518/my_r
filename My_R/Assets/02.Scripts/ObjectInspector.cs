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
    // HideInInspector ��Ʈ����Ʈ�� ����ؼ� �ش� �ʵ带 �ϴ� �����ݴϴ�.
    // �� �� ������ �ڵ忡�� �ش� �ʵ带 ������� �� �����Դϴ�.
    // ---------------------------------------------------
    [HideInInspector] [SerializeField] ObjectType _objectType;
    [HideInInspector] [SerializeField] private CarInfo _carInfo;
    [HideInInspector] [SerializeField] private List<CarInfo> _carInfolist;
    [HideInInspector] [SerializeField] private MonsterInfo _monsterInfo;
}