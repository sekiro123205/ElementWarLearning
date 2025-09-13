using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����������
/// </summary>
public class MonoManager : SingleMonoBase<MonoManager>
{
    private Action updateAction;

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="task">�¼�</param>
    public void AddUpdateAction(Action task)
    {
        updateAction += task;
    }
    
    /// <summary>
    /// �Ƴ�����
    /// </summary>
    /// <param name="task">�¼�</param>
    public void RemoveUpdateAction(Action task)
    {
        updateAction -= task;
    }

    // Update is called once per frame
    void Update()
    {
        updateAction?.Invoke();
    }
}
