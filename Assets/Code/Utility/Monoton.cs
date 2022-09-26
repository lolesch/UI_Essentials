using UnityEngine;

namespace Creation
{
    public abstract class Monoton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance = null;

        public static bool IsInstantiated => instance != null;

        public static T Instance
        {
            get
            {
                if (!IsInstantiated)
                {
                    T[] candidates = FindObjectsOfType<T>();

                    if (0 < candidates.Length)
                    {
                        instance = candidates[0];

                        for (int i = candidates.Length; 1 < i; i--)
                        {
                            if (candidates[i] != null && candidates[i] != instance)
                            {
#if UNITY_EDITOR
                                DestroyImmediate(candidates[i]);
#else
                                Destroy(candidates[i]);
#endif
                                Debug.LogWarning($"Destroyed obsolete instance of {typeof(T)}");
                            }
                        }
                    }
                    else if (Application.isPlaying)
                        instance = new GameObject($"Monoton_{typeof(T).Name}").AddComponent<T>();
                }

                if (Application.isPlaying)
                    DontDestroyOnLoad(instance.gameObject);

                return instance;
            }
        }
    }
}

