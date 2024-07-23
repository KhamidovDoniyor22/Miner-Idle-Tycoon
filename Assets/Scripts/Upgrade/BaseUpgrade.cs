using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BaseUpgrade : MonoBehaviour
{
    public static Action<BaseUpgrade, int> OnUpgrade; 


    [Header("Upgrades")]
    [SerializeField] protected float collectCapacityMultiplier = 2f;
    [SerializeField] protected float collectPerSecondMultiplier = 2f;
    [SerializeField] protected float moveSpeedMultiplier = 1.25f;

    [Header("Cost")]
    [SerializeField] private float initialUpgradecost = 200f;
    [SerializeField] private float upgardeCostMultiplier = 2f;

    public int CurrentLevel { get; set; }
    public float UpgradeCost { get; set; }

    public float CollectCapacityMultiplier => collectCapacityMultiplier;
    public float CollectPerSecondMultiplier => collectPerSecondMultiplier;
    public float MoveSpeedMultiplier => moveSpeedMultiplier;
    public float UpgradeCostMultiplier => upgardeCostMultiplier;
    public Elevator Elevator => _elevator;

    protected Shaft _shaft;
    protected Elevator _elevator;
    protected WareHouse _wareHouse;

    private void Start()
    {
        _shaft = GetComponent<Shaft>();
        _elevator = GetComponent<Elevator>();
        _wareHouse = GetComponent<WareHouse>();

        CurrentLevel = 1;
        UpgradeCost = initialUpgradecost;
    }
    public virtual void Upgrade(int upgradeAmount)
    {
        if(upgradeAmount > 0 && GoldManager.Instance.CurrentGold >= (int)UpgradeCost)
        {
            for(int i = 0; i < upgradeAmount; i++)
            {
                UpgradeSuccess();
                UpdateUpgradeValues();
                RunUpgrade();
            }
        }
    }
    protected virtual void UpgradeSuccess()
    {
        GoldManager.Instance.RemoveGold((int)UpgradeCost);
        CurrentLevel++;
        OnUpgrade?.Invoke(this, CurrentLevel);
    }
    protected virtual void UpdateUpgradeValues()
    {
        UpgradeCost *= upgardeCostMultiplier;
    }
    protected virtual void RunUpgrade()
    {

    }
}
