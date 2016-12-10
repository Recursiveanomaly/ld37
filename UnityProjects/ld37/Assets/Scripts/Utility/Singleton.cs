using UnityEngine;
using System.Linq;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogError("Only one instance of " + typeof(T) +
               " is allowed in the scene.");
        }

        instance = this as T;
    }

    /**
       Returns the instance of this singleton.
    */
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null)
                {
                    // this will find inactive game objects
                    instance = (T)Resources.FindObjectsOfTypeAll(typeof(T)).FirstOrDefault();

                    if (instance == null)
                    {
                        Debug.LogError("An instance of " + typeof(T) +
                           " is needed in the scene, but there is none.");
                    }
                }
            }

            return instance;
        }
    }
}

//http://csharpindepth.com/Articles/General/Singleton.aspx
class Singleton<T> where T : new()
{
    //private Singleton()
    //{
    //}

    public static T Instance { get { return Nested.instance; } }

    private class Nested
    {
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Nested()
        {
        }

        internal static readonly T instance = new T();
    }
}