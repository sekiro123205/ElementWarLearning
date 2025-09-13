using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���״̬����
/// </summary>
public class PlayerStateBase : StateBase
{
    protected PlayerController playerController;
    protected PlayerModel playerModel;//��ǰ״̬�Ľ�ɫģ��


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
        #region �������
        if(!playerModel.cc.isGrounded)
        {
            playerModel.verticalVelocity += playerModel.gravity * Time.deltaTime; //ʩ������
            //����isGround�жϲ���ȷ�����½�ɫ��ʱ���ڵ�����Ҳ�ж��Ϳգ����������ֶ��ж�һ��
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
            playerModel.verticalVelocity = playerModel.gravity * Time.deltaTime; //���ô�ֱ�ٶȣ�������Ϊ0������ᴩģ
        }
        #endregion

        #region ��������Ƿ�����׼��
        if (playerController.isAiming)
        {
            playerModel.SwitchState(PlayerState.Aiming);
        }
         #endregion
    }

    /// <summary>
    /// �Ƿ���ҿ���
    /// </summary>
    /// <returns></returns>
    public bool IsBeControl()
    {
        return playerModel == playerController.currentPlayerModel;
    }
    /// <summary>
    /// �л�����Ծ״̬
    /// </summary>
    public void SwitchToHover()
    {
        //������Ծ����
        playerModel.verticalVelocity = Mathf.Sqrt(-2 * playerModel.gravity * playerModel.jumpHeight);
        //�л�����Ծ״̬
        playerModel.SwitchState(PlayerState.Hover);
    }
}
