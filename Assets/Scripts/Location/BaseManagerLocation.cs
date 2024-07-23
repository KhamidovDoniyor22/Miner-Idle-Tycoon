using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseManagerLocation : MonoBehaviour
{
    [SerializeField] public string locationTitle;
    [SerializeField] private Image managerBoostIcon;
    public string LoactionTitle => locationTitle;
    public Manager Manager { get; set; }
    public MineManager MineManager { get; set; }

    public virtual void RunBoost()
    {
        
    }
    public void UpdateBoostIcon()
    {
        if(managerBoostIcon != null)
        {
            managerBoostIcon.sprite = Manager._boostIcon;
            
        }
        else
        {
            MineManager.BoostImage.sprite = Manager._boostIcon;
            
        }
    }
}
