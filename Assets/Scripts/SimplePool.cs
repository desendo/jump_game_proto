using UnityEngine;
using System.Collections.Generic;

public static class SimplePool
{

    const int DEFAULT_POOL_SIZE = 30;

    class Pool
    {

        int nextId = 1;
        Stack<GameObject> inactive;
        GameObject prefab;
        public Pool(GameObject prefab, int initialQty)
        {
            this.prefab = prefab;

            inactive = new Stack<GameObject>(initialQty);
        }

        public GameObject Spawn(Vector3 pos, Quaternion rot, Transform parent = null)
        {
            GameObject obj;
            if (inactive.Count == 0)
            {
                obj = Object.Instantiate(prefab, pos, rot, parent);

                obj.name = prefab.name + " (" + (nextId++) + ")";

                obj.AddComponent<PoolMember>().myPool = this;
            }
            else
            {
                obj = inactive.Pop();

                if (obj == null)
                {
                    return Spawn(pos, rot, parent);
                }
            }

            obj.transform.position = pos;
            obj.transform.rotation = rot;
            obj.SetActive(true);
            return obj;

        }

        public void Despawn(GameObject obj)
        {
            obj.SetActive(false);

            inactive.Push(obj);
        }

    }

    class PoolMember : MonoBehaviour
    {
        public Pool myPool;

    }

    static Dictionary<GameObject, Pool> pools;

    static void Init(GameObject prefab = null, int qty = DEFAULT_POOL_SIZE)
    {
        if (pools == null)
        {
            pools = new Dictionary<GameObject, Pool>();
        }
        if (prefab != null && pools.ContainsKey(prefab) == false)
        {
            pools[prefab] = new Pool(prefab, qty);
        }
    }

    static public void Preload(GameObject prefab, int count = 1, Transform parent = null)
    {
        Init(prefab, count);

        GameObject[] obs = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            obs[i] = Spawn(prefab, Vector3.zero, Quaternion.identity, parent);
        }

        for (int i = 0; i < count; i++)
        {
            Despawn(obs[i]);
        }
    }


    static public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        Init(prefab);

        return pools[prefab].Spawn(position, rotation, parent);
    }
    public static GameObject Spawn(GameObject prefab)
    {
        return Spawn(prefab, Vector3.zero, Quaternion.identity, null);
    }
    public static GameObject Spawn(GameObject prefab,Transform parent)
    {
        return Spawn(prefab, Vector3.zero, Quaternion.identity, parent);
    }
    static public void Despawn(GameObject obj)
    {
        PoolMember pm = obj.GetComponent<PoolMember>();
        if (pm == null)
        {
            GameObject.Destroy(obj);
        }
        else
        {
            pm.myPool.Despawn(obj);
        }
    }


}

