using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected virtual bool DontDestroy => false;

    protected static string pref_path = $"Prefs/Single/{typeof(T).Name}";

    protected static T m_instance;

    public static T Instance
    {
        get
        {
            return m_instance ? m_instance : (m_instance = CreateInstance());
        }
        protected set
        {
            if (!m_instance)
            {
                m_instance = value;
            }
            else
            {
                Debug.Log($"<b>Singleton.set_Instance</b> <color=red>Destroy other instance</color> ({typeof(T).Name})");
                Destroy(value.gameObject);
            }
        }
    }

    public static T CreateInstance()
    {
        //Debug.Log($"{"Singleton.CreateInstance".WrapWithTag(tag_name: "b")} Prefab{typeof(T).GetTypeName()} path: {pref_path}");
        var pref = Resources.Load<T>(pref_path);
        if (pref)
        {
            return Instantiate(pref);
        }
        else
        {
            //Debug.Log($"<b>Singleton.Init</b> <color=red>prefab doesnt exits</color>: {pref_path}");
            return FindObjectOfType<T>();
        }

    }

    protected virtual void Awake()
    {
        //Debug.Log($"<b>Singleton.Awake</b> {nameof(T)}");
        if (m_instance == null)
        {
            Instance = GetComponent<T>();
            name = typeof(T).Name;
            if (DontDestroy)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void Reset() 
    {
        name = typeof(T).Name;
        transform.position = Vector3.zero;
    }
}
