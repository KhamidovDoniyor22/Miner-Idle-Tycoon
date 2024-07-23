using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShaftManagerLocation : BaseManagerLocation
{
    public static Action<Shaft, ShaftManagerLocation> OnBoost;

    private Shaft _shaft;

    private void Start()
    {
        _shaft = GetComponent<Shaft>();
    }
    public override void RunBoost()
    {
        OnBoost?.Invoke(_shaft, this);
    }
}
