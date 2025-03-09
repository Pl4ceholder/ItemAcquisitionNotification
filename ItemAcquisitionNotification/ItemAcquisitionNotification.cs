using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Localization;

namespace ItemAcquisitionNotification
{
    [ApiVersion(2, 1)]
    public class ItemAcquisitionNotification : TerrariaPlugin
    {
        public override string Author => "Ranga";
        public override string Description => "特定アイテムの入手時にメッセージ流す奴";
        public override string Name => "ItemAcquisitionNotification";
        public override Version Version => Assembly.GetExecutingAssembly().GetName().Version;

        private List<string> alreadyObtainedPlayerName = new List<string>();

        private static readonly string CONFIG_PATH = Path.GetFullPath(Path.Combine(TShock.SavePath, "ItemAcquisitionNotification.json"));
        public Config Config { get; private set; }

        private DateTime LastTimelyRun = DateTime.UtcNow;


        public ItemAcquisitionNotification(Main game)
            : base(game)
        {
            Instance = this;
        }

        public static ItemAcquisitionNotification Instance { get; private set; }

        public class PlayerData
        {
            public Player realPlayer;
            public TSPlayer tsPlayer;

            public void Update()
            {
            }
        }
        public override void Initialize()
        {
            ServerApi.Hooks.GameUpdate.Register(this, OnGameUpdate);
            ServerApi.Hooks.GameInitialize.Register(this, OnInitialize);

            LoadFromConfig();
        }
        private void OnInitialize(EventArgs e)
        {
            Commands.ChatCommands.Add(new Command(new List<string> { Permissions.godmodeother }, clearStatusReset, "reset"));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.GameUpdate.Deregister(this, OnGameUpdate);
                ServerApi.Hooks.GameInitialize.Deregister(this, OnInitialize);
            }

            base.Dispose(disposing);
        }

        internal void OnGameUpdate(EventArgs args)
        {
            if ((DateTime.UtcNow - LastTimelyRun).TotalSeconds >= 1)
            {
                OnSecondlyUpdate(args);
            }
        }
        internal void OnSecondlyUpdate(EventArgs args)
        {
            foreach (TSPlayer player in TShock.Players)
            {
                if (player == null || !player.Active)
                {
                    continue;
                }
                if (!Main.ServerSideCharacter || (Main.ServerSideCharacter && player.IsLoggedIn))
                {

                    // Inventory item checks
                    foreach (Item item in player.TPlayer.inventory)
                    {
                        if (ItemCheck(EnglishLanguage.GetItemNameById(item.type), player))
                        {
                            SendCorrectiveMessage(player, item.Name);
                        }
                    }
                }
            }
            LastTimelyRun = DateTime.UtcNow;
        }
        private void SendCorrectiveMessage(TSPlayer player, string itemName)
        {
            string formattedMessage = Config.notificationMessage.Replace("{itemName}", itemName).Replace("{playerName}", player.Name);
            TSPlayer.All.SendInfoMessage(formattedMessage);
        }

        public bool ItemCheck(string name, TSPlayer ply)
        {

            if (name != null && name.Equals(Config.targetItemName) && !alreadyObtainedPlayerName.Contains(ply.Name))
            {
                alreadyObtainedPlayerName.Add(ply.Name);
                return true;
            }
            return false;
        }

        private void clearStatusReset(CommandArgs args)
        {
            alreadyObtainedPlayerName.Clear();
        }

        public void LoadFromConfig()
        {
            string path = CONFIG_PATH;


            if (File.Exists(path))
            {
                using (var sr = new StreamReader(path))
                {
                    Config = JsonConvert.DeserializeObject<Config>(sr.ReadToEnd());
                }
            }
            else
            {
                Config = new Config();
                using (var sw = new StreamWriter(path))
                {
                    sw.Write(JsonConvert.SerializeObject(Config, Formatting.Indented));
                }
            }
        }
    }
}
