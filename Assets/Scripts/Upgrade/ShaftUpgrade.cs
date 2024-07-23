using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaftUpgrade : BaseUpgrade
{
    private ShaftMiner _miner;
    protected override void RunUpgrade()
    {
        
        //if(GoldManager.Instance.CurrentGold)
        if (_shaft != null)
        {
           
            if (CurrentLevel % 5 == 0)
            {
                _shaft.CreateMiner();
            }
            //if(CurrentLevel == 5)
            //{
            //    _shaft.CreateManager();
            //}

            foreach(ShaftMiner miner in _shaft.Miners)
            {
                //GetStats(miner);

                miner.CollectCapacity *= (int)collectCapacityMultiplier;
                miner.CollectPerSecond *= collectPerSecondMultiplier;

                if(CurrentLevel % 5 == 0)
                {
                    miner.MoveSpeed *= moveSpeedMultiplier;
                }
                //SetStats(miner);
            }
        }
    }
    //private void SetStats(ShaftMiner miner)
    //{
    //    PlayerPrefs.SetInt("CollectCapacity", miner.CollectCapacity);
    //    PlayerPrefs.SetFloat("CollectPerSecond", miner.CollectPerSecond);
    //    PlayerPrefs.SetFloat("MoveSpeed", miner.MoveSpeed);
    //}
    //public void GetStats(ShaftMiner miner)
    //{
    //    miner.CollectCapacity = PlayerPrefs.GetInt("CollectCapacity", miner.CollectCapacity);
    //    miner.CollectPerSecond = PlayerPrefs.GetFloat("CollectPerSecond", miner.CollectPerSecond);
    //    miner.MoveSpeed = PlayerPrefs.GetFloat("MoveSpeed", miner.MoveSpeed);
    //}
}

