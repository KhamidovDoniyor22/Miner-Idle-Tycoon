using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WareHouseManagerLocation : BaseManagerLocation
{
    public static Action<WareHouseManagerLocation> OnBoost;

    public override void RunBoost()
    {
        OnBoost?.Invoke(this);
    }
}
