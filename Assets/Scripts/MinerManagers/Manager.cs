using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ManagerLevel
{
    Junior,
    Senior, 
    Executive
}
public enum BoostType
{
    Movement,
    Loading
}

[CreateAssetMenu]
public class Manager : ScriptableObject
{
    [Header("Manager Info")]
    public ManagerLevel ManagerLevel;
    public Color _levelColor;
    public Sprite _managerIcon;
    public string _managerName;

    [Header("Boost Info")]
    public BoostType BoostType;
    public Sprite _boostIcon;
    public float _boostDuration;
    public float _boostValue;
    public string _boostDescription;


}
