# MagicOnionTest202310

## Overview

- [MagicOnion](https://github.com/Cysharp/MagicOnion) の勉強用リポジトリ

今のところ以下の形で色々見ていく予定
```
▼ ファイル構成
省略

▼ ソリューション構成
┣ MyAppServerソリューション
┃　　┣ MyAppServerプロジェクト
┃　　┗ MyAppSharedプロジェクト（共有ライブラリ）
┗ MyAppClientソリューション
　　┗ MyAppClientプロジェクト
　　┗ MyAppSharedプロジェクト･･･これはサーバ側に設置したMyAppSharedを追加している ※なんかダサいので構造は要検討
```

## Environment

- Windows 11 Home
- Visual Studio 2022 (Community)
- フレームワーク(全部共通)･･･.NET7.0

MagicOnion(2023/10/16現在の最新安定版)
|package|version|対象プロジェクト|メモなど|
|---|---|---|---|
|Grpc.AspNetCore|2.57.0|Server|MagicOnionのREADMEの指示|
|MagicOnion.Server|5.1.8|Server|MagicOnionのREADMEの指示|
|MagicOnion.Abstractions|5.1.8|Shared|MagicOnionのREADMEの指示|
|MagicOnion.Client|5.1.8|Client|MagicOnionのREADMEの指示|

## Guide of Quick Start

- [MagicOnion](https://github.com/Cysharp/MagicOnion) のREADME.mdに「クイックスタート」があり、その通りに進めれば接続の確認が出来る（このリポジトリもそれに倣っている）
- 上記リンク先にもあるが、VisualStudio上でのプロジェクトの作成方法は [learn.microsoft.comのチュートリアル](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-7.0&tabs=visual-studio) を参考にする
  - 今のところ参考にしているのはプロジェクトの作成までで、NuGet以降は見ていない
  - ggって出てくる情報は古いものが多い。初手で公式を見るべし。

### いくつかのメモ

共有ソース(README中のMyApp.Shared)について

- 本リポジトリではファイルはServer側へ設置していて、Server/Client両方から参照している。
  - 現場ではServerとClientはリポジトリ分けるしSharedも独立させるかもなぁ。。(要検討)
  - 本リポジトリの構造は動作確認用ということで。

詰まりそうな箇所

- README.md内のコードをそのままCopy&Pasteで動作するが、ServerがListenするポート番号とClientが接続するポート番号は合わせましょう。
- チュートリアルのままならサーバ側の `Properties/launchSettings.json` にURLの設定がある
  - そのjsonの `profiles.http(https).applicationUrl` の値を参照する

【git】no-ffのすすめ(詳細はggる)
```
git config --global --add merge.ff false
git config --global --add pull.ff only
```



