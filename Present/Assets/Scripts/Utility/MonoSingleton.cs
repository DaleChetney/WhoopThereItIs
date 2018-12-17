using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<T>();
                //if(_instance != null)
                    //DontDestroyOnLoad(_instance.gameObject);
            }
            if(_instance == null)
            {
                Debug.LogError($"{typeof(T)} is not present in the current scene! Add it to a GameObject somewhere if it is going to be referenced.");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = (T)this;
            //DontDestroyOnLoad(_instance.gameObject);
        }
    }
}