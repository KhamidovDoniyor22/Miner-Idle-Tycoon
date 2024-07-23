using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShaftUI : MonoBehaviour
{
    public static Action<Shaft, ShaftUpgrade> OnUpgradeRequest;

    [Header("Buttons")]
    [SerializeField] public GameObject buyNewShaftButton;
    [SerializeField] public TextMeshProUGUI _priceText;

    [SerializeField] private TextMeshProUGUI _currentGoldTMP;
    [SerializeField] private TextMeshProUGUI _currentLevelTMP;

    private Shaft _shaft;
    private ShaftUpgrade _shaftUpgrade;

    private void Start()
    {
        _shaft = GetComponent<Shaft>();
        _shaftUpgrade = GetComponent<ShaftUpgrade>();
    }
    private void Update()
    {
        _currentGoldTMP.text = _shaft.CurrentDeposit.CurrentGold.ToString();
    }
    public void BuyNewShaft()
    {
        if(GoldManager.Instance.CurrentGold >= ShaftManager.Instance.NewShaftCost)
        {
            GoldManager.Instance.RemoveGold(ShaftManager.Instance.NewShaftCost);
            ShaftManager.Instance.AddShaft();
            buyNewShaftButton.SetActive(false);
        }
    }
    public void UpgradeRequest()
    {
        OnUpgradeRequest?.Invoke(_shaft, _shaftUpgrade);
    }
    private void UpgradeShaft(BaseUpgrade upgrade, int currentLevel)
    {
        if(upgrade == _shaftUpgrade)
        {
            _currentLevelTMP.text = $"Level\n{currentLevel}"; 
        }
    }
    private void OnEnable()
    {
        ShaftUpgrade.OnUpgrade += UpgradeShaft;
    }
    private void OnDisable()
    {
        ShaftUpgrade.OnUpgrade -= UpgradeShaft;
    }

    
}
