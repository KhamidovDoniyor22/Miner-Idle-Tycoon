using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;

public class WarehouseUI : MonoBehaviour
{
    public static Action<WareHouse, WarehouseUpgrade> OnUpgradeRequest;
    [SerializeField] private TextMeshProUGUI globalGoldTMP;

    private WarehouseUpgrade _warehouseUpgrade;
    private WareHouse _wareHouse;

    private void Start()
    {
        _warehouseUpgrade = GetComponent<WarehouseUpgrade>();
        _wareHouse = GetComponent<WareHouse>();
    }

    private void Update()
    {
        globalGoldTMP.text = GoldManager.Instance.CurrentGold.ToString();
    }
    public void UpgradeRequest()
    {
        OnUpgradeRequest?.Invoke(_wareHouse, _warehouseUpgrade);
    }
}
