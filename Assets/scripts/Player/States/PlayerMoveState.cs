using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移动状态
/// </summary>
public class PlayerMoveState : PlayerStateBase
{
    #region 动画器相关参数
    private int moveBlendHash;
    private float moveBlend; //混合参数
    private float runThreshold = 0; //奔跑阈值
    private float sprintThreshold = 1; //冲刺阈值
    private float transitionSpeed = 5; //过渡速度
    #endregion

    public override void Init(IStateMechainOwner owner)
    {
        base.Init(owner);
        moveBlendHash = Animator.StringToHash("MoveBlend");
    }

    public override void Enter()
    {
        base.Enter();
        playerModel.PlayStateAnimation("Move");
    }

    public override void Update()
    {
        base.Update();
        if(IsBeControl())
        {
            #region 监听玩家是否按下跳跃键
            if (playerController.isJumping)
            {
                SwitchToHover();
                return;
            }
            #endregion

            #region 待机状态监听
            if(playerController.moveInput.magnitude == 0)
            {
                playerModel.SwitchState(PlayerState.Idle);
                return;
            }
            #endregion

            

            #region 处理移动速度
            if(playerController.isSprint)
            {
                moveBlend = Mathf.Lerp(moveBlend, sprintThreshold, transitionSpeed * Time.deltaTime);
            }
            else
            {
                moveBlend = Mathf.Lerp(moveBlend, runThreshold, transitionSpeed * Time.deltaTime);
            }
            playerModel.animator.SetFloat(moveBlendHash, moveBlend);
            #endregion

            #region 处理移动方向
            //计算本地空间移动方向与模型正前方像的夹角
            float rad = Mathf.Atan2(playerController.localMovement.x, playerController.localMovement.z);
            //旋转到对应方向
            playerModel.transform.Rotate(0, rad * Time.deltaTime * playerController.rotationSpeed, 0);
            #endregion
        }
    }
}
