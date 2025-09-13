using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IStateMechainOwner { } //״̬�������ı��
//��ɫ״̬��
public class StateMachine
{
    private StateBase currentState; // ��ǰ״̬
    private IStateMechainOwner owner; // ״̬������
    private Dictionary<Type, StateBase> stateDic = new Dictionary<Type, StateBase>();

    public StateMachine(IStateMechainOwner owner)
    {
        this.owner = owner;
    }

    //���붯��״̬
    public void EnterState<T>() where T : StateBase, new()
    {
        //��ֹ�ظ�����ͬһ״̬
        if (currentState!= null && currentState.GetType() == typeof(T)) return;

        if(currentState!= null)
            currentState.Exit();
        currentState = LoadState<T>();
        currentState.Enter();
    }

    //ȡ��״̬�� T��״̬�࣬ ����״̬ʵ��
    private StateBase LoadState<T>() where T : StateBase, new()
    {
        Type stateType = typeof(T); //��ȡ״̬����

        //���״̬�ֵ���û�и�״̬���򴴽�
        if(!stateDic.TryGetValue(stateType, out StateBase state))
        {
            state = new T();
            state.Init(owner);
            stateDic.Add(stateType, state); //����״̬��ӵ�״̬�ֵ�
        }
        return state;
    }

    public void Stop()
    {
        if(currentState!= null)
            currentState.Exit();
        foreach(var state in stateDic.Values)
        {
            state.Destroy();
        }
        stateDic.Clear();
    }
}
