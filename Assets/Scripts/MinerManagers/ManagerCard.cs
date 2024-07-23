using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManagerCard : MonoBehaviour
{
    [SerializeField] private Image _managerIcon;
    [SerializeField] private Image _boostIcon;

    [SerializeField] private TextMeshProUGUI _managerName;
    [SerializeField] private TextMeshProUGUI _managerLevel;
    [SerializeField] private TextMeshProUGUI _boostEffect;
    [SerializeField] private TextMeshProUGUI _boostEffectDescription;

    public Manager Manager { get; set; }
    public MineManager MineManager;
    public BaseManagerLocation ManagerLocation;

    private void Start()
    {
        
    }

    public void SetUpManagerCard(Manager manager)
    {
        Manager = manager;
        _managerIcon.sprite = manager._managerIcon;
        _boostIcon.sprite = manager._boostIcon;
        _managerName.text = manager._managerName;
        _managerLevel.text = manager.ManagerLevel.ToString();
        _managerLevel.color = manager._levelColor;
        _boostEffect.text = manager.BoostType.ToString();
        _boostEffectDescription.text = manager._boostDescription;
    }
    public void AssignManager()
    {
        ManagerLocation = ManagersController.Instance.CurrentManagerLocation;
        ManagersController.Instance.AssignManagerCard(this);
        SetManagerInfoToManagerLocation();
    }
    private void SetManagerInfoToManagerLocation()
    {
        if(ManagerLocation.Manager == null)
        {
            ManagerLocation.Manager = Manager;
            ManagersController.Instance.UpdateAssignManagerInfo(ManagerLocation);
            gameObject.SetActive(false);
        }
    }

}
