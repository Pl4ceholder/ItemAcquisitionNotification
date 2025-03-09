# 概要
特定のアイテムを拾った際、チャット欄に特定のメッセージを表示する
# 使い方
ServerPlugin内に[ItemAcquisitionNotification.dll](https://github.com/Pl4ceholder/ItemAcquisitionNotification/releases/download/v0.1/ItemAcquisitionNotification.dll)をコピー
ItemAcquisitionNotificationConfig.jsonでテキスト及び対象アイテムの変更
# Config
```json5
{
  "対象アイテム名": "Copper Pickaxe",
  "必要スタック数": 1,
  "通知メッセージ": "{playerName}が{itemName}を入手しました！"
}
```
# Command
| コマンド        | 説明               |
|----------------|--------------------|
| /reset | 全プレイヤーの獲得状況のリセット |
# Dynamic Text
| Dynamic Text        | 説明               |
|----------------|--------------------|
| {itemName} | 条件となるアイテム名 |
| {playerName} | 入手したプレイヤー名 |
