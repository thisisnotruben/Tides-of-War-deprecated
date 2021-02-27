using System;
using Godot;
using GC = Godot.Collections;
namespace Game.Database
{
	public class UnitDB : AbstractDB<UnitDB.UnitData>
	{
		public class UnitData
		{
			public readonly string name, img;
			public readonly bool enemy;
			public readonly int level;
			public readonly string dialogue;
			public readonly Vector2[] path;
			public readonly Vector2 spawnPos;

			public UnitData(string name, string img, bool enemy,
			int level, string dialogue, Vector2[] path, Vector2 spawnPos)
			{
				this.name = name;
				this.img = img;
				this.enemy = enemy;
				this.level = level;
				this.dialogue = dialogue;
				this.path = path;
				this.spawnPos = spawnPos;
			}
		}

		public override void LoadData(string path)
		{
			data.Clear();

			GC.Dictionary dict, rawDict = LoadJson(path);
			GC.Array spawnPos, rawPath;
			int i;
			Vector2[] unitPath;

			foreach (string itemName in rawDict.Keys)
			{
				dict = (GC.Dictionary)rawDict[itemName];

				spawnPos = (GC.Array)dict[nameof(UnitData.spawnPos)];

				rawPath = (GC.Array)(dict[nameof(UnitData.path)]);
				unitPath = new Vector2[rawPath.Count];
				i = 0;
				foreach (GC.Array vectorNode in rawPath)
				{
					unitPath[i++] = new Vector2((float)vectorNode[0], (float)vectorNode[1]);
				}

				data.Add(itemName, new UnitData(
					name: (string)dict[nameof(UnitData.name)],
					img: (string)dict[nameof(UnitData.img)],
					enemy: (bool)dict[nameof(UnitData.enemy)],
					level: (int)(Single)dict[nameof(UnitData.level)],
					dialogue: (string)dict[nameof(UnitData.dialogue)],
					path: unitPath,
					spawnPos: new Vector2((float)spawnPos[0], (float)spawnPos[1])
				));
			}
		}
	}
}