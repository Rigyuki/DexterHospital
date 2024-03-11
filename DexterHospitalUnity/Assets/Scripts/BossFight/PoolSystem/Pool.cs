using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public GameObject Prefab => prefab;
    //public GameObject Prefab { get => prefab; }

    //ȷ������ص�ʵ�����гߴ�
    public int Size => size;
    public int RuntimeSize => queue.Count;

    [SerializeField] GameObject prefab;

    [SerializeField] int size = 1;

    Queue<GameObject> queue;

    Transform parent;
    public void Initialize(Transform parent)
    {
        queue = new Queue<GameObject>();
        this.parent = parent;

        for (var i = 0; i < size; i++)
        {
            queue.Enqueue(Copy());
        }
    }

    /// <summary>
    /// ����һ����prefab
    /// </summary>
    /// <returns></returns>
    GameObject Copy()
    {
        var copy = GameObject.Instantiate(prefab, parent);
        copy.SetActive(false);
        return copy;
    }

    /// <summary>
    /// ���ö��󣬽��ж��е���Ӻͳ���
    /// </summary>
    /// <returns></returns>
    GameObject AvailableObject()
    {
        GameObject availableObject = null;
        if (queue.Count > 0 && !queue.Peek().activeSelf)
        {
            availableObject = queue.Dequeue();
        }
        else
        {
            availableObject = Copy();
        }
        //return queue.Count > 0 ? queue.Dequeue() : Copy();

        queue.Enqueue(availableObject);
        return availableObject;
    }

    /// <summary>
    /// �����׼��
    /// </summary>
    /// <returns></returns>
    public GameObject preparedObject()
    {
        GameObject preparedobject = AvailableObject();

        preparedobject.SetActive(true);

        return preparedobject;
    }

    public GameObject preparedObject(Vector3 position)
    {
        GameObject preparedobject = AvailableObject();

        preparedobject.SetActive(true);
        preparedobject.transform.position = position;

        return preparedobject;
    }

    public GameObject preparedObject(Vector3 position, Quaternion rotation)
    {
        GameObject preparedobject = AvailableObject();

        preparedobject.SetActive(true);
        preparedobject.transform.position = position;
        preparedobject.transform.rotation = rotation;

        return preparedobject;
    }

    public GameObject preparedObject(Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        GameObject preparedobject = AvailableObject();

        preparedobject.SetActive(true);
        preparedobject.transform.position = position;
        preparedobject.transform.rotation = rotation;
        preparedobject.transform.localScale = localScale;

        return preparedobject;
    }
}
