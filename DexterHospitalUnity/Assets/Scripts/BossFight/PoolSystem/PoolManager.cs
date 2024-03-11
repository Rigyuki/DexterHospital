using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] Pool[] playerProjectilePools;

    [SerializeField] Pool[] enemyProjectilePools;
    static Dictionary<GameObject, Pool> dictionary; //static 静态函数中所有的参数都是静态的

    private void Start()
    {
        dictionary = new Dictionary<GameObject, Pool>();
        Initialized(playerProjectilePools);
        Initialized(enemyProjectilePools);

    }

#if UNITY_EDITOR
    /// <summary>
    /// OnDestroy()从编译器停止游戏运行时自动调用
    /// </summary>
    private void OnDestroy()
    {
        CheckPoolSize(playerProjectilePools);
        CheckPoolSize(enemyProjectilePools);
    }
#endif
    /// <summary>
    /// check pool real time size 检测对象池实际大小
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
    /// 初始化所有对象
    /// </summary>
    void Initialized(Pool[] pools)
    {
        foreach (var pool in pools)
        {
#if UNITY_EDITOR
            //Conditional Compilation Preprocessor Directives条件编译预处理
            //只会在unity editor里面被编译然后运行
            if (dictionary.ContainsKey(pool.Prefab))
            {
                Debug.LogError("Same prefab in multiple pool" + pool.Prefab.name);
                //如果pool里面已经存在这个key了，就continue跳出这个循环
                continue;
            }
            dictionary.Add(pool.Prefab, pool); //给字典添加一个固定的键值组合
#endif
            //为了不生成在hierarchy里面,谁挂载这个脚本生成在谁下面
            Transform poolParent = new GameObject("Pool: " + pool.Prefab.name).transform;

            poolParent.parent = transform;
            pool.Initialize(poolParent);

        }
    }

    /// <summary>
    /// <para> Return a specified <param name="prefab"></paramref>gameObject in the pool.</para>
    /// static 更方便的被其他脚本调用
    /// </summary>
    /// <para> Specified gameObject prefab.指定的游戏对象预制体</para>
    /// <para> Prepared gameObject in the pool.对象池中预备好的游戏对象</para>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            //if字典里面没有这个关键字，返回空
            Debug.LogError("Pool Manager could NOT find prefab" + prefab.name);

            return null;
        }
#endif
        return dictionary[prefab].preparedObject();//取池中预备好的对象
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
            //if字典里面没有这个关键字，返回空
            Debug.LogError("Pool Manager could NOT find prefab" + prefab.name);

            return null;
        }
#endif
        return dictionary[prefab].preparedObject(position);//取池中预备好的对象
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
            //if字典里面没有这个关键字，返回空
            Debug.LogError("Pool Manager could NOT find prefab" + prefab.name);

            return null;
        }
#endif
        return dictionary[prefab].preparedObject(position, rotation);//取池中预备好的对象
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
            //if字典里面没有这个关键字，返回空
            Debug.LogError("Pool Manager could NOT find prefab" + prefab.name);

            return null;
        }
#endif
        return dictionary[prefab].preparedObject(position, rotation, localScale);//取池中预备好的对象
    }
}
