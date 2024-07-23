using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineManager : MonoBehaviour
{
    [SerializeField] private Image _boostImage;
    [SerializeField] private GameObject _boostButton;
    [SerializeField] private BaseManagerLocation _location;
    public BaseManagerLocation Loaction => _location;
    public Image BoostImage { get; set; }
    public GameObject BoostButton { get; set; }
    public ShaftManagerLocation ShaftManagerLocation { get; set; }

    public void SetupManager(BaseManagerLocation managerLocation)
    {
        BoostButton = _boostButton;
         BoostImage = _boostImage;
        _location = managerLocation;
        ShaftManagerLocation = managerLocation as ShaftManagerLocation;
    }
    public void RunBoost()
    {
        ShaftManagerLocation.RunBoost();
        
    }
}