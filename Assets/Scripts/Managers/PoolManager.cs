using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    #region Pool
    // 定义Pool类，用于管理对象池
    class Pool
    {
        // 原始的GameObject引用
        public GameObject Original { get; private set; }
 
        // 对象池的根节点
        public Transform Root { get; set; }

        // 存储空闲对象的栈
        Stack<Poolable> _poolStack = new Stack<Poolable>();
 
        // 初始化对象池
        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{Original.name}_Root";
            for (int i = 0; i < count; i++)
                Push(Create());
        }

        // 创建一个新的对象
        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();
        }

        // 将对象放回对象池
        public void Push(Poolable poolable)
        {
            if (poolable == null)
                return;

            poolable.transform.parent = Root;
            poolable.gameObject.SetActive(false);
            poolable.isUsing = false;

            _poolStack.Push(poolable);
        }

        // 从对象池中取出一个对象
        public Poolable Pop(Transform parent)
        {
            Poolable poolable = null;

            while (_poolStack.Count > 0)
            {
                poolable = _poolStack.Pop();
                if (!poolable.gameObject.activeSelf)
                    break;
            }

            if (poolable == null || poolable.gameObject.activeSelf)
                poolable = Create();
            poolable.gameObject.SetActive(true);
            if (parent == null)
                poolable.transform.parent = Managers.Scene.CurrentScene.transform;

            poolable.transform.parent = parent;
            poolable.isUsing = true;
            return poolable;
        }
    }
    #endregion
    
    // 存储所有对象池的字典
    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    Transform _root;

    // 初始化对象池管理器
    public void Init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }
   
    // 将对象放回对象池，或者在一定时间后销毁
    public void Push(Poolable poolable, float time)
    {
        string name = poolable.gameObject.name;
        if (!_pool.ContainsKey(name))
        {
            GameObject.Destroy(poolable.gameObject, time);
            return;
        }
        
        _pool[name].Push(poolable);
    }
   
    // 从对象池中获取一个对象
    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (!_pool.ContainsKey(original.name))
            CreatePool(original);
        
        return _pool[original.name].Pop(parent);
    }

    // 创建一个新的对象池
    public void CreatePool(GameObject original, int count = 5)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = _root;
        
        _pool.Add(original.name, pool);
    }
    
    // 获取对象池中的原始对象
    public GameObject GetOriginal(string name)
    {
        if (!_pool.ContainsKey(name))
            return null;
        return _pool[name].Original;
    }

    // 清除所有对象池
    public void Clear()
    {
        foreach (Transform child in _root)
        {
            GameObject.Destroy(child.gameObject);
        }
        _pool.Clear();
    }
}
