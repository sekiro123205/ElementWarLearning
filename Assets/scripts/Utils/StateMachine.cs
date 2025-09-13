using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IStateMechainOwner { } //状态机宿主的标记
//角色状态机
public class StateMachine
{
    private StateBase currentState; // 当前状态
    private IStateMechainOwner owner; // 状态机宿主
    private Dictionary<Type, StateBase> stateDic = new Dictionary<Type, StateBase>();

    public StateMachine(IStateMechainOwner owner)
    {
        this.owner = owner;
    }

    //进入动画状态
    public void EnterState<T>() where T : StateBase, new()
    {
        //防止重复进入同一状态
        if (currentState!= null && currentState.GetType() == typeof(T)) return;

        if(currentState!= null)
            currentState.Exit();
        currentState = LoadState<T>();
        currentState.Enter();
    }

    //取出状态， T是状态类， 返回状态实例
    private StateBase LoadState<T>() where T : StateBase, new()
    {
        Type stateType = typeof(T); //获取状态类型

        //如果状态字典里没有该状态，则创建
        if(!stateDic.TryGetValue(stateType, out StateBase state))
        {
            state = new T();
            state.Init(owner);
            stateDic.Add(stateType, state); //将新状态添加到状态字典
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
