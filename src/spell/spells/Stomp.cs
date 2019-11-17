using Godot;
using Game.Actor;
using System.Collections.Generic;

namespace Game.Spell
{
    public class Stomp : Spell
    {
        private List<Character> targetList = new List<Character>();

        public override float Cast()
        {
            if (loaded)
            {
                foreach (Character character in targetList)
                {
                    StunUnit(character, true);
                }
            }
            else
            {
                foreach (Area2D characterArea2D in GetNode<Area2D>("sight").GetOverlappingAreas())
                {
                    Character character = characterArea2D.GetOwner() as Character;
                    if (character != null && character != caster)
                    {
                        character.TakeDamage(10, false, caster, Game.Misc.Other.CombatText.TextType.HIT);
                        GD.Randomize();
                        if (GD.Randi() % 100 + 1 > 20)
                        {
                            StunUnit(character, true);
                            targetList.Add(character);
                        }
                    }
                }
            }
            SetTime(3.0f);
            return base.Cast();
        }
        public override void _OnTimerTimeout()
        {
            foreach (Character character in targetList)
            {
                StunUnit(character, false);
            }
            UnMake();
        }
    }
}