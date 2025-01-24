using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VInspector;

public class PoolManager : MonoBehaviour
{
    class Pool
    {
        public GameObject original;
        public Transform Root;
        Stack<GameObject> poolStack = new Stack<GameObject>();

        public void Init(GameObject original, int count = 5, Transform rootParent = null)
        {
            this.original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";
            if (rootParent != null)
            {
                Root.SetParent(rootParent);
            }

            for (int i = 0; i < count; i++)
            {
                Push(Create());
            }
        }

        /// <summary>
        /// originalPrefab을 바탕으로 GameObject를 생성한다.
        /// </summary>
        /// <returns></returns>
        GameObject Create()
        {
            GameObject go = Instantiate(original);
            go.name = original.name;
            go.transform.SetParent(Root);
            return go;
        }

        /// <summary>
        /// GameObject를 Pool로 반환한다.
        /// </summary>
        /// <param name="go">반환할 GameObject</param>
        public void Push(GameObject go)
        {
            if (go == null) { return; }

            go.transform.SetParent(Root);
            go.gameObject.SetActive(false);
            poolStack.Push(go);
        }

        /// <summary>
        /// GameObject를 count만큼 생성하고 Pool에 넣는다.
        /// </summary>
        /// <param name="count"></param>
        public void CreateAndPush(int count = 5)
        {
            for (int i = 0; i < count; i++)
            {
                Push(Create());
            }
        }

        /// <summary>
        /// Pool에서 GameObject를 꺼낸다.
        /// </summary>
        /// <param name="parent">꺼낸 GameObject의 부모가 될 transform</param>
        /// <returns></returns>
        public GameObject Pop(Transform parent = null)
        {
            GameObject go;

            if (poolStack.Count > 0)
            {
                go = poolStack.Pop();
            }
            else
            {
                go = Create();
            }

            go.SetActive(true);

            if (parent != null)
            {
                go.transform.SetParent(parent);
            }

            return go;
        }

        public void Clear()
        {
            while (poolStack.Count > 0)
            {
                GameObject go = poolStack.Pop();
                Destroy(go);
            }
        }
    }

    #region MonoBehaviour
    private static PoolManager instance;
    public static PoolManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Init()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void Awake()
    {
        Init();
    }

    #endregion

    SerializedDictionary<string, Pool> pools = new SerializedDictionary<string, Pool>();
    [SerializeField] GameObject itemPrefab;

    /// <summary>
    /// 풀 생성을 시도한다
    /// </summary>
    /// <param name="original"></param>
    /// <param name="count"></param>
    public void TryCreatePool(GameObject original, int count = 5, Transform rootParent = null)
    {
        if (pools.ContainsKey(original.name))
        {
            DebugController.Log("이미 존재하는 풀은 생성이 불가능합니다.");
            return;
        }

        CreatePool(original, count, rootParent);
    }

    /// <summary>
    /// 풀이 진짜 생성되는 함수
    /// </summary>
    /// <param name="original"></param>
    /// <param name="count"></param>
    public void CreatePool(GameObject original, int count = 5, Transform rootParent = null)
    {
        Pool pool = new Pool();
        pool.Init(original, count, rootParent);
        pools.Add(original.name, pool);
    }

    /// <summary>
    /// 풀을 삭제한다.
    /// </summary>
    /// <param name="name"></param>
    public void DestroyPool(string name)
    {
        if (pools.ContainsKey(name))
        {
            pools[name].Clear();
        }
    }

    /// <summary>
    /// 풀에 오브젝트를 반환한다.
    /// </summary>
    /// <param name="go"></param>
    public void Push(GameObject go)
    {
        string name = go.name;

        if (!pools.ContainsKey(name))
        {
            DebugController.Log($"풀에 {go.name}을 반환하려고 했으나 해당 풀이 존재하지 않아 오브젝트를 Destroy합니다.");
            Destroy(go);
            return;
        }

        pools[name].Push(go);
    }

    /// <summary>
    /// 풀에서 오브젝트를 꺼낸다.
    /// </summary>
    /// <param name="original"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public GameObject Pop(GameObject original, Transform parent = null, Transform rootParent = null)
    {
        if (!pools.ContainsKey(original.name))      // 풀이 생성되지 않은 오브젝트를 꺼내려고 하면 풀을 생성한다.
        {
            TryCreatePool(original, rootParent: rootParent);
        }

        return pools[original.name].Pop(parent);
    }

    /// <summary>
    /// Original Prefab을 반환한다.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject GetOriginal(string name)
    {
        if (!pools.ContainsKey(name))
        {
            return null;
        }

        return pools[name].original;
    }

    /// <summary>
    /// 풀링이 적용된 오브젝트를 반환한다.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="parent"></param>
    /// <param name="rootParent"></param>
    /// <returns></returns>
    public GameObject InstantiatePoolObject(GameObject obj, Transform parent = null, Transform rootParent = null)
    {
        if (obj == null) { return null; }

        return Pop(obj, parent, rootParent);
    }

    public Item InstantiateItem(ItemData data)
    {
        if (data == null) { return null; }

        Item go = Pop(itemPrefab).GetOrAddComponent<Item>();
        go.SetItemData(data);

        return go;
    }

    public bool HasPool(string name)
    {
        return pools.ContainsKey(name);
    }
}
