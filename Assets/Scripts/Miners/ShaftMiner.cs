using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class ShaftMiner : BaseMiner
{
    public Shaft CurrentShaft { get; set; }

    private int miningAnimationParametor = Animator.StringToHash("Mining");
    private int walkingAnimationParametor = Animator.StringToHash("Walking");


    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private AnimationReferenceAsset attack;
    [SerializeField] private AnimationReferenceAsset walk;
    [SerializeField] private string currentAnimation;
    [SerializeField] private string previosState;
    [SerializeField] private string currentState;


    private void Start()
    {
        currentState = "walk";
        SetCharacterState(currentState);
    }

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
        if (state.Equals("attack"))
        {
            SetAnimation(attack, true, 2f);
        }

        else if (state.Equals("walk"))
        {
            SetAnimation(walk, true, 1f);
        }
        currentState = state;
    }

    public override void MoveMiner(Vector3 newPosition)
    {
        base.MoveMiner(newPosition);
        _animator.SetTrigger(walkingAnimationParametor);
        SetCharacterState("walk");
    }
    protected override void CollectedGold()
    {
        float collectTime = CollectCapacity / CollectPerSecond;
        SetCharacterState("attack");
        _animator.SetTrigger(miningAnimationParametor);
        OnLoading?.Invoke(this, collectTime);
        StartCoroutine(IECollect(CollectCapacity,collectTime));
    }
    protected override IEnumerator IECollect(int collectedGold, float collectTime)
    {
        yield return new WaitForSeconds(collectTime);

        CurrentGold = collectedGold;
        ChangeGoal();
        RotateMiner(-1);
      
        MoveMiner(CurrentShaft.DepositLocation.position);
    }
    protected override void DepositGold()
    {
        CurrentShaft.CurrentDeposit.DepositGold(CurrentGold);

        CurrentGold = 0;
        ChangeGoal();
        RotateMiner(1);
        MoveMiner(CurrentShaft.MiningLocation.position);
    }
}
