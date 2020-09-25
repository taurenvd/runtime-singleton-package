using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector; 
#endif

public abstract class ScriptableSingleton<T> : ScriptableObject where T: ScriptableSingleton<T>
{
    protected static T m_instance;

	protected static string LoadPath => "Prefs/Single/ScriptableSingletons";
	protected static string FullPath => $"{LoadPath}/{_type_name}";
	static string _type_name => typeof(T).Name;

	#region Properties
	public static T Instance => m_instance ?? Create();
	#endregion

	/// <summary>
	/// 
	/// </summary>
	/// <param name="resources_subpath">Additional subfolder name in Resources/ScriptableSingletons/. Must ends with "/"</param>
	/// <returns></returns>
	protected static T Create()
	{
		m_instance = Resources.Load<T>(FullPath);

		if (!m_instance)
		{
			Debug.LogError($"<b>{_type_name}.{nameof(Create)}</b> Instance not found at path: <color=red>Resources/{FullPath}</color>");
		}

		return m_instance;
	}

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			if (!Application.isPlaying)
			{
				Destroy(this);
			}

			Debug.LogError($"<b>{_type_name}.Awake</b> Duplicates are not allowed!");
		}
		else
		{
			//Debug.Log($"<b>{TypeName}.Awake</b> Initialization finished!");

			m_instance = (T)this;
		}
	}

	public override string ToString() => $"<b>Instance<<color=green>{_type_name}</b></color>: {m_instance.name}>";

#if UNITY_EDITOR
#if ODIN_INSPECTOR
	[Button,PropertySpace(50)]
#elif !ODIN_INSPECTOR
	[ContextMenu]
#endif
	void CheckInstanceInEditor()
	{
		Debug.Log($"<b>{_type_name}.CheckInstanceInEditor</b> Instance: {(Instance != null? Instance.name : "<color=red>Null</color>")}");
		UnityEditor.EditorGUIUtility.PingObject(Instance);
	} 
#endif
}