using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//状态基类
public abstract class StateBase
{
    //状态初始化
    public abstract void Init(IStateMechainOwner owner);

    //进入状态
    public abstract void Enter();

    //退出状态
    public abstract void Exit();

    //销毁
    public abstract void Destroy();

    //更新状态
    public abstract void Update();
}
