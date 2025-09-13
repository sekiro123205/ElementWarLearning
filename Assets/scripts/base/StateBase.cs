using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//״̬����
public abstract class StateBase
{
    //״̬��ʼ��
    public abstract void Init(IStateMechainOwner owner);

    //����״̬
    public abstract void Enter();

    //�˳�״̬
    public abstract void Exit();

    //����
    public abstract void Destroy();

    //����״̬
    public abstract void Update();
}
