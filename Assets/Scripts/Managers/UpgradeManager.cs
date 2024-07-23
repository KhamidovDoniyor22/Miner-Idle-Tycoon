using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    #region Inspector
    [SerializeField] private GameObject _upgradePanel;
    [SerializeField] private TextMeshProUGUI _panelTitle;
    [SerializeField] private GameObject[] stats;
    [SerializeField] private Image _panelIcon;

    [Header("Button Colours")]
    [SerializeField] private Color _buttonDisableColor;
    [SerializeField] private Color _buttonEnableColor;

    [Header("Buttons")]
    [SerializeField] private GameObject[] _upgradeButtons;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _upgradeCost;
    [SerializeField] private TextMeshProUGUI _current_stat_one;
    [SerializeField] private TextMeshProUGUI _current_stat_two;
    [SerializeField] private TextMeshProUGUI _current_stat_three;
    [SerializeField] private TextMeshProUGUI _current_stat_four;
    
    [SerializeField] private TextMeshProUGUI stat_first_title;
    [SerializeField] private TextMeshProUGUI stat_second_title;
    [SerializeField] private TextMeshProUGUI stat_third_title;
    [SerializeField] private TextMeshProUGUI stat_forth_title;

    [Header("Upgraded")]
    [SerializeField] private TextMeshProUGUI _stat_upgrade_one;
    [SerializeField] private TextMeshProUGUI _stat_upgrade_two;
    [SerializeField] private TextMeshProUGUI _stat_upgrade_three;
    [SerializeField] private TextMeshProUGUI _stat_upgrade_four;
   
    [Header("Images")]
    [SerializeField] private Image stat_first_icon;
    [SerializeField] private Image stat_second_icon;
    [SerializeField] private Image stat_third_icon;
    [SerializeField] private Image stat_forth_icon;

    [Header("ShaftIcons")]
    [SerializeField] private Sprite _shaftMinerIcon;
    [SerializeField] private Sprite _minerIcon;
    [SerializeField] private Sprite _walkingIcon;
    [SerializeField] private Sprite _miningIcon;
    [SerializeField] private Sprite _workerCapacityIcon;

    [Header("Elevator Icon")]
    [SerializeField] private Sprite _elevatorIcon;
    [SerializeField] private Sprite _loadIcon;
    [SerializeField] private Sprite _movementIcon;
    [SerializeField] private Sprite _loadingIcon;

    [Header("Warehouse Icon")]
    [SerializeField] private Sprite _warehouseIcon;
    [SerializeField] private Sprite _transpotersIcon;
    [SerializeField] private Sprite _transportationIcon;
    [SerializeField] private Sprite _wareHouseLoadingIcon;
    [SerializeField] private Sprite _wareHouseWalkingIcon;


    #endregion

    private Shaft _selectedShaft;
    private ShaftUpgrade _selectedShaftUpgrade;
    private ElevatorUpgrade _elevatorUpgarde;
    private BaseUpgrade _currentUpgrade;
    private WareHouse _currentWareHouse;

    public int TimesToUpgrade { get; set; }
    private void Awake()
    {
        _elevatorUpgarde = FindObjectOfType<ElevatorUpgrade>();
    }
    private void Start()
    {
        ActivateButton(0);
        TimesToUpgrade = 1;
    }
    public void Upgrade()
    {
        if(GoldManager.Instance.CurrentGold >= _currentUpgrade.UpgradeCost)
        {
            _currentUpgrade.Upgrade(TimesToUpgrade);

            if(_currentUpgrade is ShaftUpgrade)
            {
                UpdateUpgradePanel(_currentUpgrade);
            }
            if(_currentUpgrade is ElevatorUpgrade)
            {
                UpdateElevatorPanel(_currentUpgrade);

            }
            if (_currentUpgrade is WarehouseUpgrade)
            {
                UpdateWareHousePanel(_currentUpgrade);
            }

        }
    }
    public void OpenUpgradePanel(bool isActive)
    {
        _upgradePanel.SetActive(isActive);
    }
   
    #region UpgradeButtons
    public void UpgradeX1()
    {
        ActivateButton(0);
        TimesToUpgrade = 1;
        _upgradeCost.text = $"{_currentUpgrade.UpgradeCost}";
    }
    public void UpgradeX10()
    {
        ActivateButton(1);
        TimesToUpgrade = CanUpgradeManyTimes(10, _currentUpgrade) ? 10 : 0;
        _upgradeCost.text = GetUpgardeCost(10, _currentUpgrade).ToString();
    }
    public void UpgradeX50()
    {
        ActivateButton(2);
        TimesToUpgrade = CanUpgradeManyTimes(50, _currentUpgrade) ? 50 : 0;
        _upgradeCost.text = GetUpgardeCost(50, _currentUpgrade).ToString();
    }
    public void UpgradeMax()
    {
        ActivateButton(3);
        TimesToUpgrade = CalculateUpgradeCount(_currentUpgrade);
        _upgradeCost.text = GetUpgardeCost(TimesToUpgrade, _currentUpgrade).ToString();
    }
    #endregion

    #region Help Methods
    public void ActivateButton(int buttonIndex)
    {
        for (int i = 0; i < _upgradeButtons.Length; i++)
        {
            _upgradeButtons[i].GetComponent<Image>().color = _buttonDisableColor;
        }
        _upgradeButtons[buttonIndex].GetComponent<Image>().color = _buttonEnableColor;
        _upgradeButtons[buttonIndex].transform.DOPunchPosition(transform.localPosition + new Vector3(0, -5f, 0), 0.5f).Play();
    }
    public bool CanUpgradeManyTimes(int upgradeAmount, BaseUpgrade upgrade)
    {
        int count = CalculateUpgradeCount(upgrade);
        if(count > upgradeAmount)
        {
            return true;
        }
        return false;
    }
    private int GetUpgardeCost(int amount, BaseUpgrade upgrade)
    {
        int cost = 0;
        int upgradeCost = (int)upgrade.UpgradeCost;

        for(int i = 0; i < amount; i++)
        {
            cost += upgradeCost;
            upgradeCost *= (int)upgrade.UpgradeCostMultiplier;
        }
        return cost;
    }
    public int CalculateUpgradeCount(BaseUpgrade upgrade)
    {
        int count = 0;
        int currentGold = GoldManager.Instance.CurrentGold;
        int upgradeCost = (int)upgrade.UpgradeCost;

        for(int i = currentGold; i >= 0; i -= upgradeCost)
        {
            count++;
            upgradeCost *= (int)upgrade.UpgradeCostMultiplier;
        }
        return count;
    }
    #endregion

    #region Update WareHousePanel
    private void UpdateWareHousePanel(BaseUpgrade upgrade)
    {
        _panelTitle.text = $"Warehouse Level {upgrade.CurrentLevel}";
        _upgradeCost.text = $"{upgrade.UpgradeCost}";
        
        //UpgradeIcons
        _panelIcon.sprite = _warehouseIcon;
        stat_first_icon.sprite = _transpotersIcon;
        stat_second_icon.sprite = _transportationIcon;
        stat_third_icon.sprite = _wareHouseLoadingIcon;
        stat_forth_icon.sprite = _wareHouseWalkingIcon;

        //UpgradeText Title_st
        stat_first_title.text = "Transporters";
        stat_second_title.text = "Transportation";
        stat_third_title.text = "Loading Speed";
        stat_forth_title.text = "Walking Speed";

        //Update Current Values
        _current_stat_one.text = $"{_currentWareHouse.Miners.Count}";
        _current_stat_two.text = $"{_currentWareHouse.Miners[0].CollectCapacity}";
        _current_stat_three.text = $"{_currentWareHouse.Miners[0].CollectPerSecond}";
        _current_stat_four.text = $"{_currentWareHouse.Miners[0].MoveSpeed}";

        //UpgradeMinersCount WareHouse
        if ((upgrade.CurrentLevel + 1) % 5 == 0)
        {
            _stat_upgrade_one.text = $"+1";
        }
        else
        {
            _stat_upgrade_one.text = $"+0";
        }

        //Update Transportation Values
        int collectCapacity = _currentWareHouse.Miners[0].CollectCapacity;
        float collectCapacityMultiplier = upgrade.CollectCapacityMultiplier;
        int collectCapacityAdded = Mathf.Abs(collectCapacity - (collectCapacity * (int)collectCapacityMultiplier));
        _stat_upgrade_two.text = $"+{collectCapacityAdded}";

        //UpdateLoading Speed Upgraded
        float currentLoadSpeed = _currentWareHouse.Miners[0].CollectPerSecond;
        float currentLoadSpeedMultiplier = upgrade.CollectPerSecondMultiplier;
        int loadSpeedAdded = (int)Mathf.Abs(currentLoadSpeed - (currentLoadSpeed * currentLoadSpeedMultiplier));
        _stat_upgrade_three.text = $"+{loadSpeedAdded}";

        //Update Move Speed Upg Warehouse
        float walkSpeed = _currentWareHouse.Miners[0].MoveSpeed;
        float walkSpeedMultiplier = upgrade.MoveSpeedMultiplier;
        int walkSpeedAdded = (int)Mathf.Abs(walkSpeed - (walkSpeed * walkSpeedMultiplier));
        if ((upgrade.CurrentLevel + 1) % 5 == 0)
        {
            _stat_upgrade_four.text = $"+{walkSpeedAdded}/s";
        }
        else
        {
            _stat_upgrade_four.text = $"+0/s";
        }
    }
    #endregion

    #region Update Elevator Panel
    public void UpdateElevatorPanel(BaseUpgrade upgrade)
    {
        ElevatorMiner miner = upgrade.Elevator.Miner;
        _panelTitle.text = $"Elevator Level {upgrade.CurrentLevel}";

        stats[3].SetActive(false);
        //Updating Stats Icons
        _panelIcon.sprite = _elevatorIcon;
        stat_first_icon.sprite = _loadIcon;
        stat_second_icon.sprite = _movementIcon;
        stat_third_icon.sprite = _loadingIcon;
        //Updating Stats Titles
        stat_first_title.text = "Load";
        stat_second_title.text = "Movement Speed";
        stat_third_title.text = "Loading Speed";

        //Update Current Stats
        _current_stat_one.text = $"{miner.CollectCapacity}";
        _current_stat_two.text = $"{miner.MoveSpeed}";
        _current_stat_three.text = $"+{miner.CollectPerSecond}";

        //Update load value upgraded
        int currentCollect = miner.CollectCapacity;
        int collectMultiplier = (int) upgrade.CollectCapacityMultiplier;
        int load = Mathf.Abs(currentCollect - (currentCollect * collectMultiplier));
        _stat_upgrade_one.text = $"+{load}";

        //Update Move Speed Upgraded
        float currentMoveSpeed = miner.MoveSpeed;
        float moveSpeedMultiplier = upgrade.MoveSpeedMultiplier;
        float moveSpeedAdded = Mathf.Abs(currentMoveSpeed - (currentMoveSpeed * moveSpeedMultiplier));

        if((upgrade.CurrentLevel + 1) % 5 == 0)
        {
            _stat_upgrade_two.text = $"+{moveSpeedAdded}/s";
        }
        //update new loading speed added
        float loadingSpeed = miner.CollectPerSecond;
        float loadingSpeedMultiplier = upgrade.CollectPerSecondMultiplier;
        float loadingAdded = Mathf.Abs(loadingSpeed - (loadingSpeed * loadingSpeedMultiplier));

        _stat_upgrade_three.text = $"+{loadingAdded}/s";
    }
    #endregion

    #region Update Upgrade Shaft Panel
    private void UpdateUpgradePanel(BaseUpgrade upgrade)
    {
        //_selectedShaftUpgrade.GetStats(_selectedShaft.Miners[0]);
        _panelTitle.text = $"MineShaft {_selectedShaft.ShaftID + 1} Level {upgrade.CurrentLevel}";

        _upgradeCost.text = $"{upgrade.UpgradeCost}";
        _current_stat_one.text = $"{_selectedShaft.Miners.Count}";
        _current_stat_two.text = $"{_selectedShaft.Miners[0].MoveSpeed}";
        _current_stat_three.text = $"{_selectedShaft.Miners[0].CollectPerSecond}";
        _current_stat_four.text = $"{_selectedShaft.Miners[0].CollectCapacity}";
        stats[3].SetActive(true);

        //Updating Stats Icons
        _panelIcon.sprite = _shaftMinerIcon;
        stat_first_icon.sprite = _minerIcon;
        stat_second_icon.sprite = _walkingIcon;
        stat_third_icon.sprite = _miningIcon;
        stat_forth_icon.sprite = _workerCapacityIcon;
        //Updating Stats Titles
        stat_first_title.text = "Miners";
        stat_second_title.text = "Walking Speed";
        stat_third_title.text = "Mining Speed";
        stat_forth_title.text = "Worker Capacity";


        //UpgradeWorkerCapacity
        int collectCapacity = _selectedShaft.Miners[0].CollectCapacity;
        float collectCapacityMultiplier = upgrade.CollectCapacityMultiplier;
        int collectCapacityAdded = Mathf.Abs(collectCapacity - (collectCapacity * (int)collectCapacityMultiplier));
        _stat_upgrade_four.text = $"+{collectCapacityAdded}";

        //Upgrade load speed
        float currentLoadSpeed = _selectedShaft.Miners[0].CollectPerSecond;
        float currentLoadSpeedMultiplier = upgrade.CollectPerSecondMultiplier;
        int loadSpeedAdded = (int)Mathf.Abs(currentLoadSpeed - (currentLoadSpeed * currentLoadSpeedMultiplier));
        _stat_upgrade_three.text = $"+{loadSpeedAdded}";


        //Upgrade move speed
        float walkSpeed = _selectedShaft.Miners[0].MoveSpeed;
        float walkSpeedMultiplier = upgrade.MoveSpeedMultiplier;
        int walkSpeedAdded = (int)Mathf.Abs(walkSpeed - (walkSpeed * walkSpeedMultiplier));
        if ((upgrade.CurrentLevel + 1) % 5 == 0)
        {
            _stat_upgrade_two.text = $"+{walkSpeedAdded}/s";
        }
        else
        {
            _stat_upgrade_two.text = $"+0/s";
        }

        //UpgradeMinerCount
        if ((upgrade.CurrentLevel + 1) % 5 == 0)
        {
            _stat_upgrade_one.text = $"+1";
        }
        else
        {
            _stat_upgrade_one.text = $"+0";
        }

    }
   
    #endregion

    #region Events
    private void ShaftUpgradeRequest(Shaft shaft, ShaftUpgrade shaftUpg)
    {
        List<Shaft> shaftList = ShaftManager.Instance.Shafts;

        for (int i = 0; i < shaftList.Count; i++)
        {
            if (shaft.ShaftID == shaftList[i].ShaftID)
            {
                _selectedShaft = shaftList[i];
                _selectedShaftUpgrade = shaftList[i].GetComponent<ShaftUpgrade>();
            }
        }
        _currentUpgrade = shaftUpg;
        OpenUpgradePanel(true);
        UpdateUpgradePanel(_selectedShaftUpgrade);
    }
    private void ElevatorUpgradeRequest(ElevatorUpgrade elevatorUpgrade)
    {
        _currentUpgrade = elevatorUpgrade;
        OpenUpgradePanel(true);
        UpdateElevatorPanel(elevatorUpgrade);
    }
    private void WareHouseUpgradeRequest(WareHouse wareHouse, WarehouseUpgrade wareHouseUpgrade)
    {
        _currentUpgrade = wareHouseUpgrade;
        _currentWareHouse = wareHouse;
        OpenUpgradePanel(true);
        UpdateWareHousePanel(wareHouseUpgrade);
    }
    private void OnEnable()
    {
        ShaftUI.OnUpgradeRequest += ShaftUpgradeRequest;
        ElevatorUI.OnUpgradeRequest += ElevatorUpgradeRequest;
        WarehouseUI.OnUpgradeRequest += WareHouseUpgradeRequest;
    }
    private void OnDisable()
    {
        ShaftUI.OnUpgradeRequest -= ShaftUpgradeRequest;
        ElevatorUI.OnUpgradeRequest -= ElevatorUpgradeRequest;
        WarehouseUI.OnUpgradeRequest -= WareHouseUpgradeRequest;
    }
    #endregion
}
