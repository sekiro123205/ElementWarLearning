using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerController : SingleMonoBase<PlayerController>
{

    public PlayerModel currentPlayerModel; //��ǰ���ٿصĽ�ɫģ��
    private Transform cameraTransform; //�����Transform

    [Tooltip("�����ӽ����")]
    public CinemachineFreeLook freeLookCamera;
    [Tooltip("��׼�ӽ����")]
    public CinemachineFreeLook aimingCamera;


    #region ��ɫ����
    private MyInputSystem input; //����ϵͳ
    [HideInInspector]
    public Vector2 moveInput; //�ƶ�����
    [HideInInspector]
    public bool isSprint; //�������
    [HideInInspector]
    public bool isAiming; //��׼����
    [HideInInspector]
    public bool isJumping; //��Ծ����
    #endregion

    #region ��׼���
    [Tooltip("��׼Ŀ��")]
    public Transform aimTarget;
    [Tooltip("���߼��������")]
    public float maxRayDistance = 10000f;
    [Tooltip("���߼���")]
    public LayerMask aimLayerMask = ~0;
    #endregion

    [Tooltip("��ת�ٶ�")]
    public float rotationSpeed;

    [HideInInspector]
    public Vector3 localMovement; //���ؿռ�������ƶ�����
    [HideInInspector]
    public Vector3 worldMovement; //����ռ�������ƶ�����

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
        #region �����������
        moveInput = input.Player.Move.ReadValue<Vector2>().normalized;
        isSprint = input.Player.IsSprint.IsPressed();
        isAiming = input.Player.IsAiming.IsPressed();
        isJumping = input.Player.IsJumping.IsPressed();
        #endregion

        #region ��������ƶ�����
        //��ȡ����ķ�������
        Vector3 cameraForwardProjection = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        //��������ռ��µķ�������
        worldMovement = cameraForwardProjection * moveInput.y + cameraTransform.right * moveInput.x;
        //������ռ��µķ�������ת��Ϊ���ؿռ��µķ�������
        localMovement = currentPlayerModel.transform.InverseTransformVector(worldMovement);
        #endregion
    }

    /// <summary>
    /// ������׼
    /// </summary>
    public void EnterAim()
    {
        //ͬ����׼����������������ת�Ƕ�
        aimingCamera.m_XAxis.Value = freeLookCamera.m_XAxis.Value;
        aimingCamera.m_YAxis.Value = freeLookCamera.m_YAxis.Value;

        //������׼���Լ��
        currentPlayerModel.rightHandAimConstraint.weight = 1;
        currentPlayerModel.bodyAimConstraint.weight = 1;
        currentPlayerModel.rightHandConstraint.weight = 0;
        
        //����������ȼ�
        freeLookCamera.Priority = 0;
        aimingCamera.Priority = 100;
    }

    /// <summary>
    /// �˳���׼
    /// </summary>
    public void ExitAim()
    {
        //ͬ����׼����������������ת�Ƕ�
        freeLookCamera.m_XAxis.Value = aimingCamera.m_XAxis.Value;
        freeLookCamera.m_YAxis.Value = aimingCamera.m_YAxis.Value;

        //�ر���׼���Լ��
        currentPlayerModel.rightHandAimConstraint.weight = 0;
        currentPlayerModel.bodyAimConstraint.weight = 0;
        currentPlayerModel.rightHandConstraint.weight = 1;

        //����������ȼ�
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
