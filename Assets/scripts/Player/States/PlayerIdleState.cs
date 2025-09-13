using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerIdleState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();
        playerModel.PlayStateAnimation("Idle");
    }

    public override void Update()
    {
        base.Update();
        if(IsBeControl())
        {
            #region 监听玩家是否有输入
            if (playerController.moveInput.magnitude != 0)
            {
                playerModel.SwitchState(PlayerState.Move);
            }
            #endregion

            #region 监听玩家是否按下跳跃键
            if (playerController.isJumping)
            {
                SwitchToHover();
            }
            #endregion

            // #region 监听玩家是否按下瞄准键
            // if (playerController.isAiming)
            // {
            //     playerModel.SwitchState(PlayerState.Aiming);
            // }
            // else 
            // {
            //     playerModel.SwitchState(PlayerState.Idle);
            // }
            // #endregion
        }
    }
}
