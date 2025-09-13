using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//����ģʽ�޶���, T������
public class SingleMonoBase<T> : MonoBehaviour where T : SingleMonoBase<T>
{
    public static T INSTANCE; //ʵ��
    protected virtual void Awake()
    {
        if (INSTANCE != null)
        {
            Debug.LogError(name + "�����ϵ���ģʽ��");
        }
        INSTANCE = (T)this;
    }

    protected virtual void OnDestroy()
    {
        INSTANCE = null;
    }
}
