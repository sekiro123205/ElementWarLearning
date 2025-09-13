using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerController : SingleMonoBase<PlayerController>
{

    public PlayerModel currentPlayerModel; //当前所操控的角色模型
    private Transform cameraTransform; //相机的Transform

    [Tooltip("正常视角相机")]
    public CinemachineFreeLook freeLookCamera;
    [Tooltip("瞄准视角相机")]
    public CinemachineFreeLook aimingCamera;


    #region 角色控制
    private MyInputSystem input; //输入系统
    [HideInInspector]
    public Vector2 moveInput; //移动输入
    [HideInInspector]
    public bool isSprint; //冲刺输入
    [HideInInspector]
    public bool isAiming; //瞄准输入
    [HideInInspector]
    public bool isJumping; //跳跃输入
    #endregion

    #region 瞄准相关
    [Tooltip("瞄准目标")]
    public Transform aimTarget;
    [Tooltip("射线检测最大距离")]
    public float maxRayDistance = 10000f;
    [Tooltip("射线检测层")]
    public LayerMask aimLayerMask = ~0;
    #endregion

    [Tooltip("旋转速度")]
    public float rotationSpeed;

    [HideInInspector]
    public Vector3 localMovement; //本地空间下玩家移动方向
    [HideInInspector]
    public Vector3 worldMovement; //世界空间下玩家移动方向

    protected override void Awake()
    {
        base.Awake();
        input = new MyInputSystem();
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ExitAim();

        rotationSpeed = 300.0f;
    }

    // Update is called once per frame
    void Update()
    {
        #region 更新玩家输入
        moveInput = input.Player.Move.ReadValue<Vector2>().normalized;
        isSprint = input.Player.IsSprint.IsPressed();
        isAiming = input.Player.IsAiming.IsPressed();
        isJumping = input.Player.IsJumping.IsPressed();
        #endregion

        #region 计算玩家移动方向
        //获取相机的方向向量
        Vector3 cameraForwardProjection = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        //计算世界空间下的方向向量
        worldMovement = cameraForwardProjection * moveInput.y + cameraTransform.right * moveInput.x;
        //将世界空间下的方向向量转换为本地空间下的方向向量
        localMovement = currentPlayerModel.transform.InverseTransformVector(worldMovement);
        #endregion
    }

    /// <summary>
    /// 进入瞄准
    /// </summary>
    public void EnterAim()
    {
        //同步瞄准相机和自由相机的旋转角度
        aimingCamera.m_XAxis.Value = freeLookCamera.m_XAxis.Value;
        aimingCamera.m_YAxis.Value = freeLookCamera.m_YAxis.Value;

        //启动瞄准相关约束
        currentPlayerModel.rightHandAimConstraint.weight = 1;
        currentPlayerModel.bodyAimConstraint.weight = 1;
        currentPlayerModel.rightHandConstraint.weight = 0;
        
        //设置相机优先级
        freeLookCamera.Priority = 0;
        aimingCamera.Priority = 100;
    }

    /// <summary>
    /// 退出瞄准
    /// </summary>
    public void ExitAim()
    {
        //同步瞄准相机和自由相机的旋转角度
        freeLookCamera.m_XAxis.Value = aimingCamera.m_XAxis.Value;
        freeLookCamera.m_YAxis.Value = aimingCamera.m_YAxis.Value;

        //关闭瞄准相关约束
        currentPlayerModel.rightHandAimConstraint.weight = 0;
        currentPlayerModel.bodyAimConstraint.weight = 0;
        currentPlayerModel.rightHandConstraint.weight = 1;

        //设置相机优先级
        freeLookCamera.Priority = 100;
        aimingCamera.Priority = 0;
    }
    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
