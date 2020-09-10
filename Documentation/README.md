##Usage

Creation:

public class SomeNewSingleton : Singleton<SomeNewSingleton>
{
}

##Additionals

#Lifecycle:

public class SomeNewSingleton : Singleton<SomeNewSingleton>
{
	protected override bool DontDestroy => true/false;
}

#Instance property override using "new" keyword

public class SomeNewSingleton : Singleton<SomeNewSingleton>
{
	public static new SomeNewSingleton Instance => m_instance ?? (m_instance = CreateInstance());
}


#Prefab path override using "new" keyword

public class SomeNewSingleton : Singleton<SomeNewSingleton>
{
	protected static new string pref_path = "/123/new_path";
	public static new SomeNewSingleton Instance => m_instance ?? (m_instance = CreateInstance());
}