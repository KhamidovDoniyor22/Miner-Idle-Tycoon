using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManagersController : Singleton<ManagersController>
{
    [SerializeField] private ManagerCard _managerCardPrefab;
    [SerializeField] private int initialManagerCost = 100;
    [SerializeField] private int managerCostMultiplier = 3;

    [Header("AssignManagerUI")]
    [SerializeField] private Image _managerIcon;
    [SerializeField] private Image _boostIcon;
    [SerializeField] private TextMeshProUGUI _managerName; 
    [SerializeField] private TextMeshProUGUI _managerLevel;
    [SerializeField] private TextMeshProUGUI _boostEffect;
    [SerializeField] private TextMeshProUGUI _boostDescription;

    [SerializeField] private TextMeshProUGUI _managerPanelTitle;
    [SerializeField] private Transform _managersContainer;
    [SerializeField] private GameObject managerPanel;
    [SerializeField] private GameObject assignManagerPanel;
    [SerializeField] private List<Manager> managerList;

    public BaseManagerLocation CurrentManagerLocation { get; set; }

    private List<ManagerCard> _assignedManagerCards;
    private Camera _camera;

    public MineManager MineManager { get; set; }
    public int NewManagerCost { get; set; }

    private void Start()
    {
        _assignedManagerCards = new List<ManagerCard>();
        NewManagerCost = initialManagerCost;
        _camera = Camera.main;
    }
    //private void Update()
    //{
    //    if(Input.GetMouseButton(0))
    //    {
    //        if(Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo))
    //        {
    //            if(hitInfo.transform.GetComponent<MineManager>() != null)
    //            {
    //                CurrentManagerLocation = hitInfo.transform.GetComponent<MineManager>().Loaction;
    //                OpenPanel(true);
    //            }
    //        }
    //    }
    //}
    public void BoostButton()
    {
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo))
        {
            if (hitInfo.transform.GetComponent<MineManager>() != null)
            {
                CurrentManagerLocation = hitInfo.transform.GetComponent<MineManager>().Loaction;
                OpenPanel(true);
            }
        }
    }
    #region Boosts

    public void RunMovementBoost(BaseMiner miner, float duration, float value)
    {
        StartCoroutine(IEMovementBoost(miner, duration, value));
    }
    public void RunLoadingBoost(BaseMiner miner, float duration, float value)
    {
        StartCoroutine(IELoadingBoost(miner, duration, value));
    }

    private IEnumerator IEMovementBoost(BaseMiner miner, float duration, float value)
    {
        float startSpeed = miner.MoveSpeed;
        miner.MoveSpeed *= value;
        yield return new WaitForSeconds(duration);
        miner.MoveSpeed = startSpeed;
    }
    private IEnumerator IELoadingBoost(BaseMiner miner, float duration, float value)
    {
        float startValue= miner.CollectPerSecond;
        miner.CollectPerSecond *= value;
        yield return new WaitForSeconds(duration);
        miner.CollectPerSecond = startValue;
    }

    #endregion
    public void UnassignManager()
    {
        RestoreManagersCard(CurrentManagerLocation.Manager);
        CurrentManagerLocation.Manager = null;
        UpdateAssignManagerInfo(CurrentManagerLocation);
    }
    public void HireManager()
    {
        if(GoldManager.Instance.CurrentGold >= NewManagerCost)
        {
            //Creating The Card
            ManagerCard card = Instantiate(_managerCardPrefab, _managersContainer);

            //RandomManager
            int randomIndex = Random.Range(0, managerList.Count);
            Manager randomManager = managerList[randomIndex];
            card.SetUpManagerCard(randomManager);

            //managerList.RemoveAt(randomIndex);

            GoldManager.Instance.RemoveGold(NewManagerCost);
            NewManagerCost *= managerCostMultiplier;
        }
    }
    public void UpdateAssignManagerInfo(BaseManagerLocation managerLocation)
    {
        if(managerLocation.Manager != null)
        {
            _managerIcon.sprite = managerLocation.Manager._managerIcon;
            _boostIcon.sprite = managerLocation.Manager._boostIcon;
            _managerName.text = managerLocation.Manager._managerName;
            _managerLevel.text = managerLocation.Manager.ManagerLevel.ToString();
            _boostEffect.text = managerLocation.Manager._boostDuration.ToString();
            _boostDescription.text = managerLocation.Manager._boostDescription;
            managerLocation.UpdateBoostIcon();
            assignManagerPanel.SetActive(true);
        }
        else
        {
            _managerIcon.sprite = null;
            _boostIcon.sprite = null;
            _managerName.text = null;
            _managerLevel.text = null;
            _boostEffect.text = null;
            _boostDescription.text = null;
            assignManagerPanel.SetActive(false);
           
        }
    }
    public void AssignManagerCard(ManagerCard card)
    {
        _assignedManagerCards.Add(card);
    }
    public void OpenPanel(bool value)
    {
        managerPanel.SetActive(value);
        
        if(value)
        {
            _managerPanelTitle.text = CurrentManagerLocation.locationTitle;
            UpdateAssignManagerInfo(CurrentManagerLocation);
        }
    }
    private void RestoreManagersCard(Manager manager)
    {
        ManagerCard managerCard = null;
        for(int i = 0; i < _assignedManagerCards.Count; i++)
        {
            if(_assignedManagerCards[i].Manager._managerName == manager._managerName)
            {
                _assignedManagerCards[i].gameObject.SetActive(true);
                managerCard = _assignedManagerCards[i];
            }
        }

        if(managerCard != null)
        {
            _assignedManagerCards.Remove(managerCard);
        }
    }
}
