using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public enum PlayerState { 
    Idle, Move, Hover, Aiming
}

public class PlayerModel : MonoBehaviour, IStateMechainOwner
{
    [HideInInspector]
    public Animator animator;
    public CharacterController cc;
    private StateMachine stateMachine; //用于角色模型的状态机
    private PlayerState currentState; //当前角色的状态
 
    public MultiAimConstraint rightHandAimConstraint;//瞄准状态下的右手约束
    public TwoBoneIKConstraint rightHandConstraint;//正常状态下的右手约束
    public MultiAimConstraint bodyAimConstraint;//瞄准状态下的身体约束

    #region 垂直速度相关
    [Tooltip("重力")]
    public float gravity = -15.0f;
    [Tooltip("跳跃高度")]
    public float jumpHeight = 1.5f;
    [HideInInspector]
    public float verticalVelocity; //当前垂直方向速度
    [Tooltip("悬空高度")]
    public float hoverHeight = 0.2f; //悬空高度
    #endregion

    #region 玩家在地面时前三帧的速度
    private static readonly int CACHE_SIZE = 3;
    Vector3[] speedCache = new Vector3[CACHE_SIZE]; //动画前三个帧的速度
    private int speedCache_index = 0; //速度缓存索引
    private Vector3 averageDeltaMovement; //平均速度
    #endregion

    private void Awake()
    {
        stateMachine = new StateMachine(this);
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SwitchState(PlayerState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchState(PlayerState state)
    {
        switch(state)
        { 
            case PlayerState.Idle:
                stateMachine.EnterState<PlayerIdleState>();
                break;

            case PlayerState.Move:
                stateMachine.EnterState<PlayerMoveState>();
                break;

            case PlayerState.Hover:
                stateMachine.EnterState<PlayerHoverState>();
                break;

            case PlayerState.Aiming:
                stateMachine.EnterState<PlayerAimingState>();
                break;
        }
        currentState = state;
    }

    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="animationName">动画名称</param>
    /// <param name="transition">动画之间的过渡时间</param>
    /// <param name="layer">动画层</param>
    public void PlayStateAnimation(string animationName, float transition = 0.25f, int layer = 0)
    {
        animator.CrossFadeInFixedTime(animationName, transition, layer);
    }

    public bool IsHover()
    {
        return !Physics.Raycast(transform.position, Vector3.down, hoverHeight);
    }

    private void UpdataAverageCacheSpeed(Vector3 newSpeed)
    {
        speedCache[speedCache_index] = newSpeed;
        speedCache_index = (speedCache_index + 1) % CACHE_SIZE;
        //计算平均速度
        Vector3 sum = Vector3.zero;
        foreach(Vector3 speed in speedCache)
        {
            sum += speed;
        }
        averageDeltaMovement = sum / CACHE_SIZE;
    }

    private void OnAnimatorMove()
    {
        Vector3 palyerDeltaMovement = animator.deltaPosition; //获取动画控制器当前帧的位置信息
        if(currentState != PlayerState.Hover)
        {
            UpdataAverageCacheSpeed(animator.velocity);
        }
        else 
        {
            palyerDeltaMovement = averageDeltaMovement * Time.deltaTime;
        }
        palyerDeltaMovement.y = verticalVelocity * Time.deltaTime;
        cc.Move(palyerDeltaMovement);
    }
}
