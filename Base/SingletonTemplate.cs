//@author: qiuyukun
//2018/9/25 17:04:37


using System.Collections;
using System.Collections.Generic;


/// <summary>
/// 单例模板
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SingletonTemplate<T> where T: new()
{
    public static T inst
    {
        get
        {
            if (_inst == null) _inst = new T();
            return _inst;
        }
    }
    protected static T _inst;
}
