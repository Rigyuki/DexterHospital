using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : new()
{
    static T _instance;

    static Singleton()
    {
        _instance = new T();
    }

    public static T Instance
    {
        get { return _instance; }
    }

}
