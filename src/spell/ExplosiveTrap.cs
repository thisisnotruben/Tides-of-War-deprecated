using Game.Actor;
using Game.Database;
using Game.MissileDEP;
namespace Game.Ability
{
	public class ExplosiveTrap : SpellProto
	{
		public ExplosiveTrap(Character character, string worldName) : base(character, worldName) { }

		public override void Start()
		{
			base.Start();

			LandMineDB.LandMineData landMineData = LandMineDB.GetLandMineData(worldName);

			LandMine landMine = (LandMine)LandMine.scene.Instance();
			landMine.Init(character, landMineData.maxDamage, landMineData.maxDamage,
				landMineData.armDelaySec, landMineData.timeToDetSec);
		}
	}
}