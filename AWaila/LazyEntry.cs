namespace AWaila;

public class LazyEntry : Allumeria.IExternalLoader
{
	public void Init()
	{
		Config.InitFile();
		AWailaEntry.Init();
	}
}
