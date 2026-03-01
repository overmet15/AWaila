using Allumeria;
using Allumeria.Rendering;
using Allumeria.UI;
using Allumeria.UI.Text;
using Ignitron.Loader;
using System.Drawing;

namespace AWaila;

public class AWailaEntry : IModEntrypoint
{
	public void Main(ModBox box)
	{
		Config.InitFile();
		Init();
	}

	public static void Init()
	{
		_ = InitLoop();
	}

	private static async Task InitLoop()
	{
		try
		{
			while (!Game.inGame) await Task.Delay(100);
			
			Game.menu_HUD.panel_main.RegisterNode(new WailaNode("Waila"));
		}
		catch (Exception ex)
		{
			Logger.Error(ex.ToString());
		}
	}
}

public class WailaNode : UINode
{
	private string display = string.Empty;
	private string displayMin = string.Empty;

	private int xDraw;
	private int yDraw;

	public WailaNode(string publicName) : base(publicName)
	{
		w = 100;
		h = 32;
		x = Config.PanelX;
		y = Config.PanelY;
	}

	public override void Update()
	{
		if (Game.clientState.player == null) // here cuz planned adding support for entities in future (maybe, maybe not)
		{
			show = false;
			return;
		}
		if (Game.clientState.player.lookingAtBlock != null && Game.clientState.player.lookingAtBlock.intID != 9)
		{
			display = Game.clientState.player.lookingAtBlock.item.translatedName;
			displayMin = $"{ Game.clientState.player.lookingAtBlock.strID} #{ Game.clientState.player.lookingAtBlock.intID}";

			if (Game.clientState.player.lookingAtBlock.isCrop) displayMin += " (crop)";
			if (Game.clientState.player.lookingAtBlock.isGrass) displayMin += " (grass)";
			if (Game.clientState.player.lookingAtBlock.isFluid) displayMin += " (fluid)";

			xDraw = Game.clientState.player.lookingAtBlock.item.textureX;
			yDraw = Game.clientState.player.lookingAtBlock.item.textureY;
		}
		else
		{
			show = false;
			return;
		}
		
		w = 40 + (int)MathF.Max(TextRenderer.GetTextWidth(display, TextFont.main), TextRenderer.GetTextWidth(displayMin, TextFont.tiny));
		show = true;
	}

	public override void Render()
	{
		if (!show) return;

		TextureBatcher.batcher.Start(Drawing.uiTexture);
		TextureBatcher.batcher.AddNineSlice(new Rectangle(x, y, w, h), new Rectangle(512, 0, 16, 16), 256, 5, UIManager.scale, TextureBatcher.colorWhite);

		TextureBatcher.batcher.Finalise();
		TextureBatcher.batcher.DrawBatch();
		
		TextureBatcher.batcher.Start(Drawing.itemTexture);
		TextureBatcher.batcher.AddQuadScaled(x + 8, y + 8, 16, 16, xDraw, yDraw, 16, 16, UIManager.scale, TextureBatcher.colorWhite);
		TextureBatcher.batcher.Finalise();
		TextureBatcher.batcher.DrawBatch();

		TextRenderer.DrawText(display, x + 30, y + 8, UIManager.scale, TextFont.main, applyScaleToPos: true);
		TextRenderer.DrawText(displayMin, x + 30, y + 16, UIManager.scale, TextFont.tiny, applyScaleToPos: true);
	}
}
