﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.Base
{
    public class PrefabObjectPool<T> : IObjectPool<T> where T : MonoBehaviour
    {
        public readonly List<T> pool = new List<T>();

        private readonly Transform parent;
        private readonly T prefab;

        public PrefabObjectPool(T prefab, Transform parent)
        {
            this.prefab = prefab;
            this.parent = parent;
            this.pool = new List<T>();
        }

        public IEnumerable<T> Free => pool.Where(x => x != null && !x.gameObject.activeSelf);
        public IEnumerable<T> InUse => pool.Where(x => x != null && x.gameObject.activeSelf);

        public T Next()
        {
            foreach (T candidate in Free)
            {
                candidate.gameObject.SetActive(true);

                return candidate;
            }

            return ExtendPool();
        }

        public void Release(T released) => released.gameObject.SetActive(false);

        public void Cull()
        {
            foreach (T clutter in Free.ToList())
            {
                pool.Remove(clutter);
#if UNITY_EDITOR
                Object.DestroyImmediate(clutter.gameObject);
#else
                Object.Destroy(clutter.gameObject);
#endif
            }
        }

        public void ReleaseAll()
        {
            foreach (T element in InUse)
                Release(element);
        }

        private T ExtendPool()
        {
            T[] newThings = new T[pool.Count + 1];

            for (int i = 0; i < newThings.Length; i++)
            {
                newThings[i] = Object.Instantiate(prefab, parent);
                newThings[i].gameObject.SetActive(false);
            }

            pool.AddRange(newThings);

            newThings[0].gameObject.SetActive(true);

            return newThings[0];
        }
    }

    public interface IObjectPool<T>
    {
        T Next();
        void Release(T released);
    }
}