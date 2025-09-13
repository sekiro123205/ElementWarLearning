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
            #region ��������Ƿ�������
            if (playerController.moveInput.magnitude != 0)
            {
                playerModel.SwitchState(PlayerState.Move);
            }
            #endregion

            #region ��������Ƿ�����Ծ��
            if (playerController.isJumping)
            {
                SwitchToHover();
            }
            #endregion

            // #region ��������Ƿ�����׼��
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
