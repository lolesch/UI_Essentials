using System.Collections.Generic;
using UnityEngine;

namespace Creation
{
    public class PrefabObjectPool<T> : IObjectPool<T> where T : MonoBehaviour
    {
        public readonly List<T> pool = new List<T>();
        private readonly T prefab;
        private readonly Transform parent;

        public PrefabObjectPool(T prefab, Transform parent, int initialSize = 0)
        {
            this.pool = new List<T>(initialSize);
            this.prefab = prefab;
            this.parent = parent;
        }

        public List<T> Active
        {
            get
            {
                var list = new List<T>();

                foreach (var active in pool)
                    if (active != null && active.gameObject.activeSelf)
                        list.Add(active);

                return list;
            }
        }

        public List<T> Inactive
        {
            get
            {
                var list = new List<T>();
                foreach (var inactive in pool)
                    if (inactive != null && !inactive.gameObject.activeSelf)
                        list.Add(inactive);

                return list;
            }
        }

        public T Next()
        {
            foreach (T candidate in Inactive)
            {
                candidate.gameObject.SetActive(true);

                return candidate;
            }

            return ExtendPool();

            T ExtendPool()
            {
                T[] extension = new T[pool.Count + 1];

                for (int i = 0; i < extension.Length; i++)
                {
                    extension[i] = Object.Instantiate(prefab, parent);
                    extension[i].gameObject.SetActive(false);
                }

                pool.AddRange(extension);

                extension[0].gameObject.SetActive(true);

                return extension[0];
            }
        }

        public void Release(T released) => released.gameObject.SetActive(false);

        public void ReleaseAll()
        {
            for (int i = Active.Count; i-- > 0;)
                Release(Active[i]);
        }

        public void Cull()
        {
            for (int i = Inactive.Count; i-- > 0;)
            {
                T clutter = Inactive[i];

                pool.Remove(clutter);
#if UNITY_EDITOR
                Object.DestroyImmediate(clutter.gameObject);
#else
                Object.Destroy(clutter.gameObject);
#endif
            }
        }
    }

    public interface IObjectPool<T>
    {
        T Next();
        void Release(T released);
    }
}