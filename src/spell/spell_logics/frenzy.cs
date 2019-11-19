using System;
using Godot;
namespace Game.Ability
{
    public class frenzy : Spell
    {
        private Tuple<float, float> amounts;
        public override float Cast()
        {
            amounts = new Tuple<float, float>(
                caster.animSpeed * 0.25f,
                caster.weaponSpeed * 0.25f
            );
            caster.animSpeed += amounts.Item1;
            caster.weaponSpeed += amounts.Item2;
            SetCount(5);
            SetTime(6.0f);
            caster.SetSpell(this,
                (loaded) ? GetDuration() - GetNode<Timer>("timer").GetWaitTime() : 0.0f);
            // TODO: need to connect timeout timer to effect, look at GD code
            return base.Cast();
        }
        public override void _OnTimerTimeout()
        {
            caster.TakeDamage(5, false, caster, Game.Misc.Other.CombatText.TextType.HIT);
            count--;
            if (count > 0)
            {
                GetNode<Timer>("timer").Start();
            }
            else
            {
                caster.animSpeed -= amounts.Item1;
                caster.weaponSpeed -= amounts.Item2;
                UnMake();
            }
        }
    }
}