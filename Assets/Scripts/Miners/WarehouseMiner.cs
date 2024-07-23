using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class WarehouseMiner : BaseMiner
{
    public Deposit ElevatorDeposit { get; set; }
    public Transform ElevatorDepositLocation { get; set; }
    public Transform WareHouseLocation { get; set; }

    private readonly int _walkingNoGold = Animator.StringToHash("WalkingNoGold");
    private readonly int _walkingWithGold = Animator.StringToHash("WalkingWithGold");

    private LoadBar _loadBar;

    [Header("Animation Settings")]
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private AnimationReferenceAsset idle;
    [SerializeField] private AnimationReferenceAsset walk;
    [SerializeField] private string currentAnimation;
    [SerializeField] private string previosState;
    [SerializeField] private string currentState;

    private void Start()
    {
        _loadBar = GetComponent<LoadBar>();
        currentState = "idle";
        SetCharacterState(currentState);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            RotateMiner(-1);
            SetCharacterState("walk");
            MoveMiner(new Vector2(ElevatorDepositLocation.position.x, transform.position.y));
        }
    }
    #region SpineManagement
    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(currentAnimation))
        {
            return;
        }
        Spine.TrackEntry animationEntry = skeletonAnimation.state.SetAnimation(0, animation, loop);
        animationEntry.TimeScale = timeScale;
        animationEntry.Complete += AnimationEntry_Complete;
        currentAnimation = animation.name;
    }

    private void AnimationEntry_Complete(Spine.TrackEntry trackEntry)
    {
        if (currentState.Equals("walk"))
        {
            SetCharacterState(previosState);
        }
    }

    public void SetCharacterState(string state)
    {
        if (state.Equals("idle"))
        {
            SetAnimation(idle, true, 2f);
        }

        else if (state.Equals("walk"))
        {
            SetAnimation(walk, true, 1f);
        }
        currentState = state;
    }

    #endregion
    public void WarehouseButton()
    {
        RotateMiner(-1);

        MoveMiner(new Vector2(ElevatorDepositLocation.position.x, transform.position.y));
        SetCharacterState("walk");
    }
    public override void MoveMiner(Vector3 newPosition)
    {
        base.MoveMiner(newPosition);      
    }
    protected override void CollectedGold()
    {
        if(ElevatorDeposit.CurrentGold <= 0)
        {
            RotateMiner(1);
            ChangeGoal();
            MoveMiner(new Vector2(WareHouseLocation.position.x, transform.position.y));
            return;
        }

        SetCharacterState("idle");

        int currentGold = ElevatorDeposit.CollectGold(this);
        float collectTime = CollectCapacity / CollectPerSecond;
       
        _loadBar.BarContainer.localScale = new Vector3(-1, 1, 1);
        OnLoading?.Invoke(this, collectTime);
        StartCoroutine(IECollect(currentGold, collectTime));
    }
    protected override IEnumerator IECollect(int collectedGold, float collectTime)
    {
        if (ElevatorDeposit.CurrentGold > 0)
        {
            yield return new WaitForSeconds(collectTime);

            CurrentGold = collectedGold;
            ElevatorDeposit.RemoveGold(collectedGold);
            SetCharacterState("walk");
            
            RotateMiner(1);
            ChangeGoal();
            MoveMiner(new Vector2(WareHouseLocation.position.x, transform.position.y));
        }
    }
    protected override void DepositGold()
    {
        if(CurrentGold <= 0)
        {
            RotateMiner(-1);
            ChangeGoal();
            MoveMiner(new Vector2(ElevatorDepositLocation.position.x, transform.position.y));
            return;
        }
        SetCharacterState("idle");

        float depositTime = CurrentGold / CollectPerSecond;
        _loadBar.BarContainer.localScale = new Vector3(1, 1, 1);
        OnLoading?.Invoke(this, depositTime);
        StartCoroutine(IEDeposit(CurrentGold, depositTime));
    }

    protected override IEnumerator IEDeposit(int goldCollected, float depositTime)
    {
        yield return new WaitForSeconds(depositTime);

        GoldManager.Instance.AddGold(CurrentGold);
        CurrentGold = 0;

        RotateMiner(-1);
        ChangeGoal();
        MoveMiner(new Vector2(ElevatorDepositLocation.position.x, transform.position.y));
    }
}
