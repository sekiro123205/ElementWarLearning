using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimingState : PlayerStateBase
{
    #region 动画器相关参数
    private int aimingXHash;
    private int aimingYHash;
    private float aimingX = 0;
    private float aimingY = 0;
    private float trasitionSpeed = 5;
    #endregion

    public override void Init(IStateMechainOwner owner)
    {
        base.Init(owner);
        aimingXHash = Animator.StringToHash("AimingX");
        aimingYHash = Animator.StringToHash("AimingY");
    }

    public override void Enter()
    {
        base.Enter();
        playerModel.PlayStateAnimation("Aiming");
        if(IsBeControl())
        {
            UpdateAimingTarget();
            playerController.EnterAim();
        }
    }

    public override void Update()
    {
        base.Update();

        if(IsBeControl())
        {
            //让模型旋转至相机方向
            playerModel.transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);

            UpdateAimingTarget();

            #region 待机监听
            if(!playerController.isAiming)
            {
                playerModel.SwitchState(PlayerState.Idle);
            }
            #endregion

            #region 处理移动输入
            aimingX = Mathf.Lerp(aimingX, playerController.moveInput.x, trasitionSpeed * Time.deltaTime);
            aimingY = Mathf.Lerp(aimingY, playerController.moveInput.y, trasitionSpeed * Time.deltaTime);
            playerModel.animator.SetFloat(aimingXHash, aimingX);
            playerModel.animator.SetFloat(aimingYHash, aimingY);
            #endregion
        }
    }

    public override void Exit()
    {
        base.Exit();
        if(IsBeControl())
        {
            playerController.ExitAim();
        }
    }

    /// <summary>
    /// 从屏幕中心发射一条中心，确认瞄准位置
    /// </summary>
    private void UpdateAimingTarget()
    {
        // 发射射线
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        //如果射线击中了物体
        if(Physics.Raycast(ray, out hit, playerController.maxRayDistance, playerController.aimLayerMask))
        {
            //更新瞄准目标的位置
            playerController.aimTarget.position = hit.point;
        }
        else
        {
            playerController.aimTarget.position = ray.origin + ray.direction * playerController.maxRayDistance;
        }
    }
}
