# sample.ChatApp

## Overview

- sampleとしてチャットアプリが同梱されているので、クライアントをコンソールアプリへ移植して動かしてみる
- Unityはまだ使いたくなかった

▼ ソリューション構成
```
sample.ChatApp
┣ sample.ChatApp.Serverソリューション
┃　　┣ sample.ChatApp.Serverプロジェクト
┃　　┗ sample.ChatApp.Sharedプロジェクト（共有ライブラリ）
┃
┗ sample.ChatApp.Clientソリューション
　　┗ sample.ChatApp.Clientプロジェクト
　　┗ sample.ChatApp.Sharedプロジェクト
       ･･･これはサーバ側に設置したSharedを既存のプロジェクトとして追加している ※なんかダサいので構造は要検討
```

## Environment

- Windows 11 Home
- Visual Studio 2022 (Community)
- フレームワーク(全部共通)･･･.NET7.0

MagicOnion(2023/10/16現在の最新安定版)
|package|version|対象プロジェクト|メモなど|
|---|---|---|---|
|Grpc.AspNetCore|2.57.0|Server側||
|MagicOnion.Server|5.1.8|Server側||
|MagicOnion.Abstractions|5.1.8|Shared側||
|MagicOnion.Client|5.1.8|Client側||

## Guide of sample

- サーバとクライアントで共有するソース（Shared）へインタフェースを記述して、これをサーバ／クライアントでそれぞれimplementsする流れだった
- sampleのクライアントはUnity用だったのでコンソールアプリ用に少々コードをいぢった


## 実行方法

◎ サーバの起動
1. ソリューションをビルドしてサーバを起動

◎ クライアントの起動
1. ソリューションをビルドしてexeを作成する
2. コマンドラインから `sample.ChatApp.Client.exe member_X` など、起動引数にチャット上での名前を指定して起動する
    -  ソリューションからDebug実行する場合は "member_A" を指定しています
3. 接続出来たメンバー同士でストリーミングによるチャットが可能です

### チャットコマンド

- `exit`
    - 切断してアプリを終了します
- `report`
    - Service通信を使用して情報を送信します
- `exception`
    - Service通信を使用してサーバ側でExceptionを発生します。送信したクライアントは切断されます。

## その他

- コンソールに色々ログを出力しています。


