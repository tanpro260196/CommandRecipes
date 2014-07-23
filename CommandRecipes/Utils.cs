﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TShockAPI;
using TShockAPI.DB;

namespace CommandRecipes
{
	public class Utils
	{
		public static List<RecPlayer> GetPlayerList(string name)
		{
			foreach (RecPlayer player in CmdRec.RPlayers)
			{
				if (player.name.ToLower().Contains(name.ToLower()))
				{
					return new List<RecPlayer>() { player };
				}
			}
			return new List<RecPlayer>();
		}

		public static RecPlayer GetPlayer(int index)
		{
			foreach (RecPlayer player in CmdRec.RPlayers)
				if (player.Index == index)
					return player;

			return null;
		}

		public static List<string> ListIngredients(List<RecItem> actIngs)
		{
			List<string> lActIngs = new List<string>();
			foreach (RecItem item in actIngs)
			{
				lActIngs.Add(FormatItem((Item)item));
			}
			return lActIngs;
		}

		public static bool CheckIfInRegion(TSPlayer plr, List<string> region)
		{
			if (region.Contains(""))
				return true;

			int count = 0;
			foreach (string reg in region)
			{
				Region r = TShock.Regions.GetRegionByName(reg);

				int minX = r.Area.X;
				int minY = r.Area.Y;
				int maxX = r.Area.X + r.Area.Width;
				int maxY = r.Area.Y + r.Area.Height;
				if (plr.TileX < minX || plr.TileY < minY || plr.TileX > maxX || plr.TileY > maxY)
					count++;
			}
			if (count == region.Count)
				return false;
			return true;
		}

		public static bool CheckIfInRegion2(TSPlayer plr, List<string> region)
		{
			if (region.Contains(""))
				return true;

			Region r;
			foreach (var reg in region)
			{
				r = TShock.Regions.GetRegionByName(reg);
				if (r != null && r.InArea((int)plr.X, (int)plr.Y))
					return true;
			}
			return false;
		}

		#region SetUpConfig
		public static void SetUpConfig()
		{
			try
			{
				if (!Directory.Exists(CmdRec.configDir))
					Directory.CreateDirectory(CmdRec.configDir);

				if (File.Exists(CmdRec.configPath))
					CmdRec.config = RecConfig.Read(CmdRec.configPath);
				else
					CmdRec.config.Write(CmdRec.configPath);

				foreach (Recipe rec in CmdRec.config.Recipes)
				{
					if (!CmdRec.recs.Contains(rec.name.ToLower()))
						CmdRec.recs.Add(rec.name.ToLower());
					rec.categories.ForEach((item) =>
					{
						CmdRec.cats.Add(item.ToLower());
					});
				}
			}
			catch (Exception ex)
			{
				// Why were you using this instead of Log.ConsoleError?
				//Console.ForegroundColor = ConsoleColor.Red;
				//Console.WriteLine("Error in recConfig.json!");
				//Console.ResetColor();
				Log.ConsoleError("Error in recConfig.json!");
				Log.ConsoleError(ex.ToString());
			}
		}
		#endregion

		#region GetPrefixById
		// Required until everyone gets their TShock updated
		public static string GetPrefixById(int id)
		{
			return id < 1 || id > 83 ? "" : Lang.prefix[id] ?? "";
		}
		#endregion

		#region FormatItem
		// Though it would be an interesting addition
		public static string FormatItem(Item item, int stacks = 0)
		{
			string prefix = GetPrefixById(item.prefix);
			if (prefix != "")
			{
				prefix += " ";
			}
			return String.Format("{0} {1}{2}(s)",
				(stacks == 0) ? Math.Abs(item.stack) : stacks,
				prefix,
				item.name);
		}
		#endregion

		#region AddToPrefixes(old)
		//public static void AddToPrefixes()
		//{
		//	#region Prefixes
		//	CmdRec.prefixes.Add(1, "Large");
		//	CmdRec.prefixes.Add(2, "Massive");
		//	CmdRec.prefixes.Add(3, "Dangerous");
		//	CmdRec.prefixes.Add(4, "Savage");
		//	CmdRec.prefixes.Add(5, "Sharp");
		//	CmdRec.prefixes.Add(6, "Pointy");
		//	CmdRec.prefixes.Add(7, "Tiny");
		//	CmdRec.prefixes.Add(8, "Terrible");
		//	CmdRec.prefixes.Add(9, "Small");
		//	CmdRec.prefixes.Add(10, "Dull");
		//	CmdRec.prefixes.Add(11, "Unhappy");
		//	CmdRec.prefixes.Add(12, "Bulky");
		//	CmdRec.prefixes.Add(13, "Shameful");
		//	CmdRec.prefixes.Add(14, "Heavy");
		//	CmdRec.prefixes.Add(15, "Light");
		//	CmdRec.prefixes.Add(16, "Sighted");
		//	CmdRec.prefixes.Add(17, "Rapid");
		//	CmdRec.prefixes.Add(18, "Hasty");
		//	CmdRec.prefixes.Add(19, "Intimidating");
		//	CmdRec.prefixes.Add(20, "Deadly");
		//	CmdRec.prefixes.Add(21, "Staunch");
		//	CmdRec.prefixes.Add(22, "Awful");
		//	CmdRec.prefixes.Add(23, "Lethargic");
		//	CmdRec.prefixes.Add(24, "Awkward");
		//	CmdRec.prefixes.Add(25, "Powerful");
		//	CmdRec.prefixes.Add(26, "Mystic");
		//	CmdRec.prefixes.Add(27, "Adept");
		//	CmdRec.prefixes.Add(28, "Masterful");
		//	CmdRec.prefixes.Add(29, "Inept");
		//	CmdRec.prefixes.Add(30, "Ignorant");
		//	CmdRec.prefixes.Add(31, "Deranged");
		//	CmdRec.prefixes.Add(32, "Intense");
		//	CmdRec.prefixes.Add(33, "Taboo");
		//	CmdRec.prefixes.Add(34, "Celestial");
		//	CmdRec.prefixes.Add(35, "Furious");
		//	CmdRec.prefixes.Add(36, "Keen");
		//	CmdRec.prefixes.Add(37, "Superior");
		//	CmdRec.prefixes.Add(38, "Forceful");
		//	CmdRec.prefixes.Add(39, "Broken");
		//	CmdRec.prefixes.Add(40, "Damaged");
		//	CmdRec.prefixes.Add(41, "Shoddy");
		//	CmdRec.prefixes.Add(42, "Quick");
		//	CmdRec.prefixes.Add(43, "Deadly");
		//	CmdRec.prefixes.Add(44, "Agile");
		//	CmdRec.prefixes.Add(45, "Nimble");
		//	CmdRec.prefixes.Add(46, "Murderous");
		//	CmdRec.prefixes.Add(47, "Slow");
		//	CmdRec.prefixes.Add(48, "Sluggish");
		//	CmdRec.prefixes.Add(49, "Lazy");
		//	CmdRec.prefixes.Add(50, "Annoying");
		//	CmdRec.prefixes.Add(51, "Nasty");
		//	CmdRec.prefixes.Add(52, "Manic");
		//	CmdRec.prefixes.Add(53, "Hurtful");
		//	CmdRec.prefixes.Add(54, "Strong");
		//	CmdRec.prefixes.Add(55, "Unpleasant");
		//	CmdRec.prefixes.Add(56, "Weak");
		//	CmdRec.prefixes.Add(57, "Ruthless");
		//	CmdRec.prefixes.Add(58, "Frenzying");
		//	CmdRec.prefixes.Add(59, "Godly");
		//	CmdRec.prefixes.Add(60, "Demonic");
		//	CmdRec.prefixes.Add(61, "Zealous");
		//	CmdRec.prefixes.Add(62, "Hard");
		//	CmdRec.prefixes.Add(63, "Guarding");
		//	CmdRec.prefixes.Add(64, "Armored");
		//	CmdRec.prefixes.Add(65, "Warding");
		//	CmdRec.prefixes.Add(66, "Arcane");
		//	CmdRec.prefixes.Add(67, "Precise");
		//	CmdRec.prefixes.Add(68, "Lucky");
		//	CmdRec.prefixes.Add(69, "Jagged");
		//	CmdRec.prefixes.Add(70, "Spiked");
		//	CmdRec.prefixes.Add(71, "Angry");
		//	CmdRec.prefixes.Add(72, "Menacing");
		//	CmdRec.prefixes.Add(73, "Brisk");
		//	CmdRec.prefixes.Add(74, "Fleeting");
		//	CmdRec.prefixes.Add(75, "Hasty");
		//	CmdRec.prefixes.Add(76, "Quick");
		//	CmdRec.prefixes.Add(77, "Wild");
		//	CmdRec.prefixes.Add(78, "Rash");
		//	CmdRec.prefixes.Add(79, "Intrepid");
		//	CmdRec.prefixes.Add(80, "Violent");
		//	CmdRec.prefixes.Add(81, "Legendary");
		//	CmdRec.prefixes.Add(82, "Unreal");
		//	CmdRec.prefixes.Add(83, "Mythical");
		//	#endregion
		//}
		#endregion
	}
}
