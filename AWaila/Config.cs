using Allumeria;
using System.Reflection;
using System.Text;

namespace AWaila;

public static class Config
{
	public static int PanelX = 50;
	public static int PanelY = 120;

	public static string? OverridePath;

	public static void InitFile()
	{

		try
		{
			string? path = OverridePath ?? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			if (string.IsNullOrWhiteSpace(path))
			{
				Logger.Error("[AWaila] Cannot init custom config due to path being null or empty.");
				return;
			}

			path = Path.Combine(path, "AWaila.config");

			if (!File.Exists(path))
			{
				CreateFile(path);
				return;
			}
			
			string text = File.ReadAllText(path);

			int found = 0;
			int p;

			foreach (var v in text.Split("\n"))
			{
				var parts = v.Split("=");

				if (parts.Length != 2) continue;

				if (parts[0].Trim() == "PanelX")
				{
					if (!int.TryParse(parts[1].Trim(), out p)) continue;
					found++;
					PanelX = p;
				}
				else if (parts[0].Trim() == "PanelY")
				{
					if (!int.TryParse(parts[1].Trim(), out p)) continue;
					found++;
					PanelY = p;
				}
			}

			if (found != 2) CreateFile(path);
		}
		catch (Exception ex)
		{
			Logger.Error(ex.ToString());
		}
	}

	public static void CreateFile(string path)
	{
		try
		{
			StringBuilder builder = new();

			builder.AppendLine($"PanelX = {PanelX}");
			builder.AppendLine($"PanelY = {PanelY}");

			File.WriteAllText(path, builder.ToString());
		}
		catch (Exception ex)
		{
			Logger.Error(ex.ToString());
		}
	}
}