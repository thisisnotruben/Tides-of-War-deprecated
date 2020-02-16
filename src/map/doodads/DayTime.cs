using Game.Light;
using Godot;
namespace Game.Map.Doodads
{
    public class DayTime : Timer, ISaveable
    {
        private const float LENGTH_OF_DAY = 210.0f;
        private bool dayLight = true;

        public void _OnTimerTimeout()
        {
            AnimationPlayer anim = GetNode<AnimationPlayer>("anim");
            string animName = "sun_up_down";
            if (dayLight)
            {
                anim.Play(animName);
            }
            else
            {
                anim.PlayBackwards(animName);
            }
            dayLight = !dayLight;
        }
        public void _OnAnimFinished(string animName)
        {
            WaitTime = LENGTH_OF_DAY;
            Start();
        }
        public void TriggerLights()
        {
            foreach (GameLight light in GameLight.GetLights())
            {   
                light.SetIntensity(!dayLight);
            }
        }
        public void SetSaveData(Godot.Collections.Dictionary data)
        {
            GD.Print("TODO: Save Not Implemented");
        }
        public Godot.Collections.Dictionary GetSaveData()
        {
            Godot.Collections.Dictionary saveData = new Godot.Collections.Dictionary();
            GD.Print("TODO: Save Not Implemented");
            return saveData;
        }
    }
}