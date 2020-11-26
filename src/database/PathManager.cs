namespace Game.Database
{
	public static class PathManager
	{
		public static readonly string[] randdomSndDirs = new string[] { "combat", "spell" };
		public const string
			dataExt = "json",
			sceneExt = "tscn",
			importExt = "import",
			areaEffect = "res://data/areaEffect.json",
			collNav = "res://data/collNav.json",
			image = "res://data/image.json",
			item = "res://data/item.json",
			landMine = "res://data/landMine.json",
			modifier = "res://data/modifier.json",
			use = "res://data/use.json",
			spell = "res://data/spell.json",
			missileSpell = "res://data/missileSpell.json",
			spellEffectDir = "res://src/spell/visual/",
			questDir = "res://data/quest/",
			sndDir = "res://asset/snd/",
			startScene = "res://src/menu_ui/startMenu/StartMenuView.tscn",
			unitDataTemplate = "res://data/{0}/{0}.json",
			contentDataTemplate = "res://data/{0}/{0}_content.json",
			sceneMapPath = "res://src/map/{0}.tscn",
			savePath = "user://data/save/{0}.json",
			assetMissileSpellPath = "res://asset/img/missile-spell/{0}.tres";
	}
}