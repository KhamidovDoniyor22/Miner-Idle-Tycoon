using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseUpgrade : BaseUpgrade
{
    protected override void RunUpgrade()
    {
        if(CurrentLevel % 5 == 0)
        {
            _wareHouse.AddMiner();
        }

        foreach(WarehouseMiner miner in _wareHouse.Miners)
        {
            miner.CollectCapacity *= (int)collectCapacityMultiplier;
            miner.CollectPerSecond *= collectPerSecondMultiplier;

            if(CurrentLevel % 5 == 0)
            {
                miner.MoveSpeed *= moveSpeedMultiplier;
            }
        }
    }
}
