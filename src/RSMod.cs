using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;

// The title of your mod, as displayed in menus
[assembly: AssemblyTitle("Random Stuff Mod DEV")]

// The author of the mod
[assembly: AssemblyCompany("Antikore")]

// The description of the mod
[assembly: AssemblyDescription("Adds a bunch of random stuff to Duck Game")]

// The mod's version
[assembly: AssemblyVersion("1.0")]

namespace DuckGame.RSModDEV
{
    public class RSMod : Mod
    {
        internal static string AssemblyName { get; private set; }

        // The mod's priority; this property controls the load order of the mod.
        public override Priority priority
		{
			get { return base.priority; }
		}

		// This function is run before all mods are finished loading.
		protected override void OnPreInitialize()
		{
            Music.songs.Add("TurretOrchestra", OggSong.Load(GetPath("turretOrchestra.ogg"), false));    
            base.OnPreInitialize();
		}

		// This function is run after all mods are loaded.
		protected override void OnPostInitialize()
		{
            NetMessageAdder.UpdateNetmessageTypes();
            base.OnPostInitialize();
            this.AddTeams();
            //this.CreateRSLevelPlaylist();
        }

        /*
        private void CreateRSLevelPlaylist()
        {
            string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "DuckGame\\Levels\\Random Stuff\\");
            if (!Directory.Exists(text))
            {
                Directory.CreateDirectory(text);
            }
            IList<string> list = new List<string>();
            foreach (string text2 in Directory.GetFiles(Mod.GetPath<RSMod>("levels")))
            {
                string text3 = RSMod.AssemblyName + "\\content\\levels\\";
                string text4 = text2.Replace('/', '\\');
                string text5 = text + text4.Substring(text4.IndexOf(text3) + text3.Length);
                if (!File.Exists(text5) || !this.GetMD5Hash(File.ReadAllBytes(text4)).SequenceEqual(this.GetMD5Hash(File.ReadAllBytes(text5))))
                {
                    File.Copy(text4, text5, true);
                }   
                list.Add(text5);
            }
            string text6 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "DuckGame\\Levels\\RS Levels.play");
            XElement xelement = new XElement("playlist");
            foreach (string text7 in list)
            {
                XElement content = new XElement("element", text7.Replace('\\', '/'));
                xelement.Add(content);
            }
            XDocument xdocument = new XDocument();
            xdocument.Add(xelement);
            string text8 = xdocument.ToString();
            if (string.IsNullOrWhiteSpace(text8))
            {
                throw new Exception("Blank XML (" + text6 + ")");
            }
            if (!File.Exists(text6) || !File.ReadAllText(text6).Equals(text8))
            {
                File.WriteAllText(text6, text8);
                this.SaveCloudFile(text6, "Levels/RS Levels.play");
            }
        }*/

        private void SaveCloudFile(string path, string localPath)
        {
            if (MonoMain.disableCloud || MonoMain.cloudNoSave || !Steam.IsInitialized())
            {
                return;
            }
            byte[] array = File.ReadAllBytes(path);
            Steam.FileWrite(localPath, array, array.Length);
        }

        private byte[] GetMD5Hash(byte[] sourceBytes)
        {
            return new MD5CryptoServiceProvider().ComputeHash(sourceBytes);
        }

        private void AddTeams()
        {
            Teams.core.teams.Add(new Team("Fez", Mod.GetPath<RSMod>("Hats\\fez"), false, false, default(Vec2)));
            Teams.core.teams.Add(new Team("Portraits", Mod.GetPath<RSMod>("Hats\\portraits"), false, false, default(Vec2)));
            Teams.core.teams.Add(new Team("Afros", Mod.GetPath<RSMod>("Hats\\afros"), false, false, default(Vec2)));
            Teams.core.teams.Add(new Team("Canis", Mod.GetPath<RSMod>("Hats\\canis"), false, false, default(Vec2)));
            Teams.core.teams.Add(new Team("Bones", Mod.GetPath<RSMod>("Hats\\bones"), false, false, default(Vec2)));
            Teams.core.teams.Add(new Team("Maths", Mod.GetPath<RSMod>("Hats\\maths"), false, false, default(Vec2)));
            Teams.core.teams.Add(new Team("Vikings", Mod.GetPath<RSMod>("Hats\\vikings"), false, false, default(Vec2)));
            Teams.core.teams.Add(new Team("80s", Mod.GetPath<RSMod>("Hats\\80s"), false, false, default(Vec2)));
            Teams.core.teams.Add(new Team("Bonsai", Mod.GetPath<RSMod>("Hats\\bonsai"), false, false, default(Vec2)));
            Teams.core.teams.Add(new Team("Elders", Mod.GetPath<RSMod>("Hats\\elders"), false, false, default(Vec2)));
            Teams.core.teams.Add(new Team("Seaducks", Mod.GetPath<RSMod>("Hats\\seaducks"), false, false, default(Vec2)));
            Teams.core.teams.Add(new Team("Mallardator", Mod.GetPath<RSMod>("Hats\\mallardator"), false, false, default(Vec2)));
            Teams.core.teams.Add(new Team("Steves", Mod.GetPath<RSMod>("Hats\\steves"), false, false, default(Vec2)));
            Teams.core.teams.Add(new Team("Orients", Mod.GetPath<RSMod>("Hats\\orients"), false, false, default(Vec2)));
            Teams.core.teams.Add(new Team("Cops", Mod.GetPath<RSMod>("Hats\\cops"), false, false, default(Vec2)));
            Teams.core.teams.Add(new Team("Marges", Mod.GetPath<RSMod>("Hats\\marges"), false, false, default(Vec2)));
            Teams.core.teams.Add(new Team("VLCs", Mod.GetPath<RSMod>("Hats\\vlcs"), false, false, default(Vec2)));
            Teams.core.teams.Add(new Team("3Ds", Mod.GetPath<RSMod>("Hats\\3ds"), false, false, default(Vec2)));
            Teams.core.teams.Add(new Team("Chefs", Mod.GetPath<RSMod>("Hats\\chefs"), false, false, default(Vec2)));
        }
    }
}
