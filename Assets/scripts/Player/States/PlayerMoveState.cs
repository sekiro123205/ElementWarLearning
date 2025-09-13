using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ƶ�״̬
/// </summary>
public class PlayerMoveState : PlayerStateBase
{
    #region ��������ز���
    private int moveBlendHash;
    private float moveBlend; //��ϲ���
    private float runThreshold = 0; //������ֵ
    private float sprintThreshold = 1; //�����ֵ
    private float transitionSpeed = 5; //�����ٶ�
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
            #region ��������Ƿ�����Ծ��
            if (playerController.isJumping)
            {
                SwitchToHover();
                return;
            }
            #endregion

            #region ����״̬����
            if(playerController.moveInput.magnitude == 0)
            {
                playerModel.SwitchState(PlayerState.Idle);
                return;
            }
            #endregion

            

            #region �����ƶ��ٶ�
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

            #region �����ƶ�����
            //���㱾�ؿռ��ƶ�������ģ����ǰ����ļн�
            float rad = Mathf.Atan2(playerController.localMovement.x, playerController.localMovement.z);
            //��ת����Ӧ����
            playerModel.transform.Rotate(0, rad * Time.deltaTime * playerController.rotationSpeed, 0);
            #endregion
        }
    }
}
