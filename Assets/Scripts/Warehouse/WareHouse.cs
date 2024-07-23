using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WareHouse : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject _wareHouseMinerPrefab;

    [Header("Extras")]
    [SerializeField] private Deposit elevatorDeposit;
    [SerializeField] private Transform elevatorLocation;
    [SerializeField] private Transform warehouseDepositLoaction;

    [SerializeField] private List<WarehouseMiner> miners;

    public List<WarehouseMiner> Miners => miners;

    private void Start()
    {
        miners = new List<WarehouseMiner>();
        AddMiner();
    }
    public void AddMiner()
    {
        GameObject newMiner = Instantiate(_wareHouseMinerPrefab, warehouseDepositLoaction.position, Quaternion.identity);
        WarehouseMiner miner = newMiner.GetComponent<WarehouseMiner>();
        miner.ElevatorDeposit = elevatorDeposit;
        miner.ElevatorDepositLocation = elevatorLocation;
        miner.WareHouseLocation = warehouseDepositLoaction;

        miners.Add(miner);

    }
    private void WareHouseBoost(WareHouseManagerLocation wareHouseManager)
    {
        switch (wareHouseManager.Manager.BoostType)
        {
            case BoostType.Movement:
                foreach(WarehouseMiner miner in Miners)
                {
                    ManagersController.Instance.RunMovementBoost(miner, wareHouseManager.Manager._boostDuration, wareHouseManager.Manager._boostValue);
                }
                break;
            case BoostType.Loading:
                foreach (WarehouseMiner miner in Miners)
                {
                    ManagersController.Instance.RunLoadingBoost(miner, wareHouseManager.Manager._boostDuration, wareHouseManager.Manager._boostValue);
                }
                break;
        }
    }
    private void OnEnable()
    {
        WareHouseManagerLocation.OnBoost += WareHouseBoost;
    }
    private void OnDestroy()
    {
        WareHouseManagerLocation.OnBoost -= WareHouseBoost;
    }
}
