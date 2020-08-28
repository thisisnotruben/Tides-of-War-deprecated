using System;
using Game.Actor;
using Godot;
namespace Game.Ability
{
	public class slow : Spell
	{
		private Tuple<Character, float, float> values;
		public override float Cast()
		{
			values = new Tuple<Character, float, float>(
				caster.target,
				caster.target.stats.animSpeed.value * 0.5f,
				caster.target.stats.weaponSpeed.value * 0.5f
			);
			// values.Item1.animSpeed -= values.Item2;TODO
			// values.Item1.weaponSpeed -= values.Item3;TODO
			SetTime(10.0f);
			return base.Cast();
		}
		public override void _OnTimerTimeout()
		{
			// values.Item1.animSpeed += values.Item2;TODO
			// values.Item1.weaponSpeed += values.Item3;TODO
			UnMake();
		}
	}
}