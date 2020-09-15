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
            Debug.Log($"<b>Singleton<${nameof(T)}>.CreateInstance</b> <color=red>Failed to find on scene or load from path({pref_path}).</color>. Create Instance on scene or specify correct path to prefab in Resources");
        }

        return result;

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
            Debug.Log($"<b>Singleton<{nameof(T)}>.Awake</b> <color=red>Already has Instance({m_instance.name}), destroing duplicate {name}</color>");

            Destroy(gameObject);
        }
    }

    protected virtual void Reset() 
    {
        name = typeof(T).Name;
        transform.position = Vector3.zero;
    }
}
