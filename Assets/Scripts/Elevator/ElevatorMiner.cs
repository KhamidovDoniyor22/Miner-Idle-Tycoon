using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorMiner : BaseMiner
{
    [SerializeField] private Elevator _elevator;

    private int _currentShaftIndex = -1;
    private Deposit _currentDeposit;

    private bool _isPressed = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (!_isPressed)
            {
                MoveToNextLocation();
                _isPressed = true;
        }
        }

    }
    public void ElevatorButton()
    {
        if(!_isPressed)
        {
            MoveToNextLocation();
            _isPressed = true;
        }
    }
    public void MoveToNextLocation()
    {
        _currentShaftIndex++;

        Shaft currentShaft = ShaftManager.Instance.Shafts[_currentShaftIndex];
        Vector2 nextPos = currentShaft.DepositLocation.position;
        Vector2 fixedPos = new Vector2(transform.position.x, nextPos.y);

        _currentDeposit = currentShaft.CurrentDeposit;
        MoveMiner(fixedPos);
    }

    protected override void CollectedGold()
    {
        if(!_currentDeposit.CanCollectGold() && _currentDeposit != null && _currentShaftIndex == ShaftManager.Instance.Shafts.Count - 1)
        {
            _currentShaftIndex = -1;
            ChangeGoal();
            Vector3 elevatorDepositPos = new Vector3(transform.position.x, _elevator.DepositLocation.position.y);
            MoveMiner(elevatorDepositPos);
            return;
        }
        int amountToCollect = _currentDeposit.CollectGold(this);
        float collectTime = amountToCollect / CollectPerSecond;
        OnLoading?.Invoke(this, collectTime);
        StartCoroutine(IECollect(amountToCollect, collectTime));

    }
    protected override IEnumerator IECollect(int collectedGold, float collectTime)
    {
        yield return new WaitForSeconds(collectTime);

        if(CurrentGold > 0 && CurrentGold < CollectCapacity)
        {
            CurrentGold += collectedGold;
        }
        else
        {
        CurrentGold = collectedGold;
        }

        _currentDeposit.RemoveGold(collectedGold);
        yield return new WaitForSeconds(0.5f);

        if(CurrentGold == CollectCapacity || _currentShaftIndex == ShaftManager.Instance.Shafts.Count - 1)
        {
        _currentShaftIndex = -1;
        ChangeGoal();
        Vector3 elevatorDepositPos = new Vector3(transform.position.x, _elevator.DepositLocation.position.y);
        MoveMiner(elevatorDepositPos);

        }
        else
        {
            MoveToNextLocation();
        }
    }
    protected override void DepositGold()
    {
        if(CurrentGold <= 0)
        {
            _currentShaftIndex = -1;
            ChangeGoal();
            MoveToNextLocation();
            return;
        }
        float depositTime = CurrentGold / CollectPerSecond;
        OnLoading.Invoke(this, depositTime);
        StartCoroutine(IEDeposit(CurrentGold, depositTime));
    }
    protected override IEnumerator IEDeposit(int goldCollected, float depositTime)
    {
        yield return new WaitForSeconds(depositTime);

        _elevator.ElevatorDeposit.DepositGold(CurrentGold);
        CurrentGold = 0;
        _currentShaftIndex = -1;

        //Update gold and move next location

        ChangeGoal();
        MoveToNextLocation();
    }
    private void ElevatorBoost(ElevatorManagerLocation elevatorManager)
    {
        switch(elevatorManager.Manager.BoostType)
        {
            case BoostType.Movement:
                ManagersController.Instance.RunMovementBoost(this, elevatorManager.Manager._boostDuration, elevatorManager.Manager._boostValue);
                break;
            case BoostType.Loading:
                ManagersController.Instance.RunLoadingBoost(this, elevatorManager.Manager._boostDuration, elevatorManager.Manager._boostValue);
                break;
        }
    }
    private void OnEnable()
    {
        ElevatorManagerLocation.OnBoost += ElevatorBoost;
    }
    private void OnDisable()
    {
        ElevatorManagerLocation.OnBoost -= ElevatorBoost;
    }
}
