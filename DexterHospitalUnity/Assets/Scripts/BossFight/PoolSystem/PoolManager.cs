using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] Pool[] playerProjectilePools;

    [SerializeField] Pool[] enemyProjectilePools;
    static Dictionary<GameObject, Pool> dictionary; //static ��̬���������еĲ������Ǿ�̬��

    private void Start()
    {
        dictionary = new Dictionary<GameObject, Pool>();
        Initialized(playerProjectilePools);
        Initialized(enemyProjectilePools);

    }

#if UNITY_EDITOR
    /// <summary>
    /// OnDestroy()�ӱ�����ֹͣ��Ϸ����ʱ�Զ�����
    /// </summary>
    private void OnDestroy()
    {
        CheckPoolSize(playerProjectilePools);
        CheckPoolSize(enemyProjectilePools);
    }
#endif
    /// <summary>
    /// check pool real time size �������ʵ�ʴ�С
    /// </summary>
    /// <param name="pool"></param>
    void CheckPoolSize(Pool[] pools)
    {
        foreach (var pool in pools)
        {
            if (pool.RuntimeSize > pool.Size)
            {
                Debug.LogWarning(string.Format("Pool: {0} has a runtime size {1} bigger than its initial size {2}",
                    pool.Prefab.name,
                    pool.RuntimeSize,
                    pool.Size));
            }
        }
    }

    /// <summary>
    /// ��ʼ�����ж���
    /// </summary>
    void Initialized(Pool[] pools)
    {
        foreach (var pool in pools)
        {
#if UNITY_EDITOR
            //Conditional Compilation Preprocessor Directives��������Ԥ����
            //ֻ����unity editor���汻����Ȼ������
            if (dictionary.ContainsKey(pool.Prefab))
            {
                Debug.LogError("Same prefab in multiple pool" + pool.Prefab.name);
                //���pool�����Ѿ��������key�ˣ���continue�������ѭ��
                continue;
            }
            dictionary.Add(pool.Prefab, pool); //���ֵ����һ���̶��ļ�ֵ���
#endif
            //Ϊ�˲�������hierarchy����,˭��������ű�������˭����
            Transform poolParent = new GameObject("Pool: " + pool.Prefab.name).transform;

            poolParent.parent = transform;
            pool.Initialize(poolParent);

        }
    }

    /// <summary>
    /// <para> Return a specified <param name="prefab"></paramref>gameObject in the pool.</para>
    /// static ������ı������ű�����
    /// </summary>
    /// <para> Specified gameObject prefab.ָ������Ϸ����Ԥ����</para>
    /// <para> Prepared gameObject in the pool.�������Ԥ���õ���Ϸ����</para>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            //if�ֵ�����û������ؼ��֣����ؿ�
            Debug.LogError("Pool Manager could NOT find prefab" + prefab.name);

            return null;
        }
#endif
        return dictionary[prefab].preparedObject();//ȡ����Ԥ���õĶ���
    }

    /// <summary>
    /// <para>Release a specified prepared gameObject in the pool at specified position </para>
    /// </summary>
    /// <param name="prefab">Specified gameObject prefab</param>
    /// <param name="position">Specified release position</param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            //if�ֵ�����û������ؼ��֣����ؿ�
            Debug.LogError("Pool Manager could NOT find prefab" + prefab.name);

            return null;
        }
#endif
        return dictionary[prefab].preparedObject(position);//ȡ����Ԥ���õĶ���
    }

    /// <summary>
    /// Release a specified prepared gameObject in the pool at specified position and rotation
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            //if�ֵ�����û������ؼ��֣����ؿ�
            Debug.LogError("Pool Manager could NOT find prefab" + prefab.name);

            return null;
        }
#endif
        return dictionary[prefab].preparedObject(position, rotation);//ȡ����Ԥ���õĶ���
    }

    /// <summary>
    /// Release a specified prepared gameObject in the pool at specified position , rotation and scale
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="localScale">Specified scale</param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            //if�ֵ�����û������ؼ��֣����ؿ�
            Debug.LogError("Pool Manager could NOT find prefab" + prefab.name);

            return null;
        }
#endif
        return dictionary[prefab].preparedObject(position, rotation, localScale);//ȡ����Ԥ���õĶ���
    }
}
