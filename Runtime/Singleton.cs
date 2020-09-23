using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected virtual bool DontDestroy => false;

    protected static string pref_path = $"Prefs/Single/{_type_name}";

    protected static T m_instance;

    static string _type_name => typeof(T).Name;

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
            else if(m_instance != T.GetComponent<T>())
            {
                Debug.Log($"<b>Singleton.set_Instance</b> <color=red>Destroy other instance</color> ({_type_name})");
                Destroy(value.gameObject);
            }
        }
    }

    public static T CreateInstance()
    {
        T result;
        //Debug.Log($"{"Singleton.CreateInstance".WrapWithTag(tag_name: "b")} Prefab{typeof(T).GetTypeName()} path: {pref_path}");
        var pref = Resources.Load<T>(pref_path);
        if (pref)
        {
            result = Instantiate(pref);
        }
        else
        {
            //Debug.Log($"<b>Singleton.Init</b> <color=red>prefab doesnt exits</color>: {pref_path}");
            result = FindObjectOfType<T>();
        }

        if (!result)
        {
            Debug.Log($"<b>Singleton<{_type_name}>.CreateInstance</b> <color=red>Failed to find Instance on scene or load from path({pref_path}).</color>. Create Instance on scene or specify correct path to prefab in Resources");
        }

        return result;

    }

    protected virtual void Awake()
    {
        //Debug.Log($"<b>Singleton.Awake</b> {nameof(T)}");
        if (m_instance == null)
        {
            Instance = GetComponent<T>();
            name = _type_name;
            if (DontDestroy)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Debug.Log($"<b>Singleton<{_type_name}>.Awake</b> <color=red>Already has Instance</color>({m_instance.name}), destroing duplicate {name}");

            Destroy(gameObject);
        }
    }

    protected virtual void Reset() 
    {
        name = _type_name;
        transform.position = Vector3.zero;
    }
}
