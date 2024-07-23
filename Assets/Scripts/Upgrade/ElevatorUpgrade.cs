using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorUpgrade : BaseUpgrade
{
    protected override void RunUpgrade()
    {
        //GetStats(_elevator.Miner);
        _elevator.Miner.CollectCapacity *= (int) collectCapacityMultiplier;
        _elevator.Miner.CollectPerSecond *= collectPerSecondMultiplier;

        if(CurrentLevel % 5 == 0)
        {
            _elevator.Miner.MoveSpeed *= moveSpeedMultiplier;

        }
        //SetStats(_elevator.Miner);
    }
    //private void SetStats(ElevatorMiner miner)
    //{
    //    PlayerPrefs.SetInt("CollectCapacity", miner.CollectCapacity);
    //    PlayerPrefs.SetFloat("CollectPerSecond", miner.CollectPerSecond);
    //    PlayerPrefs.SetFloat("MoveSpeed", miner.MoveSpeed);
    //}
    //public void GetStats(ElevatorMiner miner)
    //{
    //    miner.CollectCapacity = PlayerPrefs.GetInt("CollectCapacity", miner.CollectCapacity);
    //    miner.CollectPerSecond = PlayerPrefs.GetFloat("CollectPerSecond", miner.CollectPerSecond);
    //    miner.MoveSpeed = PlayerPrefs.GetFloat("MoveSpeed", miner.MoveSpeed);
    //}
}
