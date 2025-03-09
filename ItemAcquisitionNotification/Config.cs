using Newtonsoft.Json;

namespace ItemAcquisitionNotification
{
    [JsonObject("アイテム獲得通知設定")]
    public class Config
    {
        [JsonProperty("対象アイテム名")]
        public string targetItemName { get; private set; } = "";

        [JsonProperty("必要スタック数")]
        public int requireStacks { get; private set; } = 1;

        [JsonProperty("通知メッセージ")]
        public string notificationMessage { get; private set; } = "{playerName}が{itemName}を入手しました！";


    }
}