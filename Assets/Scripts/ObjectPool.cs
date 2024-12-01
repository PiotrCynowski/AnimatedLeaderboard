using System.Collections.Generic;
using UnityEngine;

namespace ObjectPoolSpawn
{
    public class ObjectPool<T> where T : Component
    {
        readonly Queue<T> pool = new();
        readonly T prefab;
        readonly Transform parent;

        public ObjectPool(T prefab, Transform parent, int initialSize = 10)
        {
            this.prefab = prefab;
            this.parent = parent;

            for (int i = 0; i < initialSize; i++)
            {
                T instance = CreateNewInstance();
                instance.gameObject.SetActive(false);
                pool.Enqueue(instance);
            }
        }

        public T Get()
        {
            if (pool.Count > 0)
            {
                T instance = pool.Dequeue();
                instance.gameObject.SetActive(true);
                instance.transform.SetAsLastSibling();
                return instance;
            }
            return CreateNewInstance();
        }

        public void ReturnToPool(T instance)
        {
            instance.gameObject.SetActive(false);
            pool.Enqueue(instance);
        }

        T CreateNewInstance()
        {
            T instance = GameObject.Instantiate(prefab, parent);
            return instance;
        }
    }
}