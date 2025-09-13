using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家状态基类
/// </summary>
public class PlayerStateBase : StateBase
{
    protected PlayerController playerController;
    protected PlayerModel playerModel;//当前状态的角色模型


    public override void Init(IStateMechainOwner owner)
    {
        playerController = PlayerController.INSTANCE;
        playerModel = (PlayerModel)owner;
    }

    public override void Destroy()
    {
        
    }

    public override void Enter()
    {
        MonoManager.INSTANCE.AddUpdateAction(Update);
    }

    public override void Exit()
    {
        MonoManager.INSTANCE.RemoveUpdateAction(Update);
    }

    public override void Update()
    {
        #region 重力相关
        if(!playerModel.cc.isGrounded)
        {
            playerModel.verticalVelocity += playerModel.gravity * Time.deltaTime; //施加重力
            //由于isGround判断不精确，导致角色有时候在地上他也判断滞空，所以这里手动判断一下
            // if(playerModel.verticalVelocity < -1.0f)
            // {
            //     playerModel.SwitchState(PlayerState.Hover);
            // }
            if(playerModel.IsHover())
            {
                playerModel.SwitchState(PlayerState.Hover);
            }
        }
        else 
        {
            playerModel.verticalVelocity = playerModel.gravity * Time.deltaTime; //重置垂直速度，不能设为0，否则会穿模
        }
        #endregion

        #region 监听玩家是否按下瞄准键
        if (playerController.isAiming)
        {
            playerModel.SwitchState(PlayerState.Aiming);
        }
         #endregion
    }

    /// <summary>
    /// 是否被玩家控制
    /// </summary>
    /// <returns></returns>
    public bool IsBeControl()
    {
        return playerModel == playerController.currentPlayerModel;
    }
    /// <summary>
    /// 切换到跳跃状态
    /// </summary>
    public void SwitchToHover()
    {
        //计算跳跃力度
        playerModel.verticalVelocity = Mathf.Sqrt(-2 * playerModel.gravity * playerModel.jumpHeight);
        //切换到跳跃状态
        playerModel.SwitchState(PlayerState.Hover);
    }
}
