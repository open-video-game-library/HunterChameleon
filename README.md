# Hunter Chameleon (version 2.1.0)

[English version README](https://github.com/open-video-game-library/HunterChameleon/blob/main/README.EN.md)

Hunter Chameleonはカメレオンの2Dシューティングゲームです。

絶え間なくターゲットが出現するので、カメレオンの舌で撃ち落としましょう。

![Hunter Chameleonのキャプチャ画像](https://user-images.githubusercontent.com/52689532/223369450-5b382679-08e8-43c9-a6de-0350c3860375.png)


## コンテンツ

### ルール

出現するターゲット（ハエ・リンゴ）に照準を合わせて、舌で落としましょう。

カメレオンの胴体にリンゴが当たるとスコアが減ります。

### 操作方法

Hunter Chamaleonはマウス操作でプレイできます。
- マウスの移動 → 照準の移動
- 左クリック → トリガーを引く

![Hunter Chameleonの操作方法](https://user-images.githubusercontent.com/52689532/196676762-4b561a4d-eacf-43a2-9de5-26b8e95a69aa.png)


## 機能

### パラメータ調整機能

以下のパラメータをゲーム画面上で調整できます。

- 制限時間（秒）
   - デフォルト値: 30

- カメレオンの舌のスピード
   - デフォルト値: 8

- カメレオンの下と照準の色（RGBA）

   デフォルト値
   - Red: 243
   - Green: 132
   - Blue: 229
   - Alpha: 255

- 各ターゲットの出現頻度（10秒に何個か）

   デフォルト値
   - ハエ: 10
   - リンゴ: 5

- 各ターゲットの移動速度

   デフォルト値
   - ハエ: 5
   - リンゴ（重力の度合い）: 0.5

- ゲームパッドの操作感度

   - デフォルト値: 5
   対応しているゲームパッドについては,[こちら](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.5/manual/SupportedDevices.html#gamepads)をご覧ください。


### パラメータ出力機能

以下のパラメータをゲーム終了時にCSVファイルとして出力できます。

- スコア
- ヒット数
- ヒット率

### 研究利用例

1. 複数のゲームコントローラのポインティング性能の比較評価

   本ゲームを複数のゲームコントローラでプレイしてもらい、パラメータ出力機能で得たヒット率を比較して相対的な評価をおこなう
   
2. マウスで操作するシステムの体験評価

   本ゲームの基本操作方法であるマウスでプレイするシステムを体験してもらい、体験後のインタビューから定性的な評価をおこなう
   
Hunter Chameleonを用いた具体的なケーススタディは、[こちらの論文](http://id.nii.ac.jp/1001/00214482/)をご覧ください。


## 環境

MacOS, Unity 2021.3.19f1


## インストール方法

本リポジトリのデータは下記のコマンドを入力することでローカル環境にクローンできます。

```bash
git clone git@github.com:open-video-game-library/HunterChameleon.git
```


## 使い方

Hunter Chameleonが動作するために必要なデータはすべて同梱されています。

グラフィックやBGMなどのアセットを変更したい場合は、その都度インポート・置き換えをして下さい。

また、プロジェクトに含まれているアセットの中には、外部が提供している再配布が認められた素材も含まれています。それらを含んだゲームデータを公開する場合は、各アセット毎に同梱されたライセンスファイルをご確認下さい。


## ライセンス

本コンテンツは、[MITライセンス](https://github.com/open-video-game-library/HunterChameleon/blob/main/LICENSE)のもとで利用が許可されています。

再配布のため、このコードにはBGMや効果音などの音声ファイルは含まれていません。


## 引用

研究利用しやすく標準性を目指したビデオゲームの設計と開発

[論文はこちら](http://id.nii.ac.jp/1001/00212465/)

```
@inproceedings{weko_212571_1,
   author	 = "拓也,岡 and 拓也,川島 and 大智,林 and 恵太,渡邊",
   title	 = "研究利用しやすく標準性を目指したビデオゲームの設計と開発",
   booktitle	 = "エンタテインメントコンピューティングシンポジウム論文集",
   year 	 = "2021",
   volume	 = "2021",
   number	 = "",
   pages	 = "181--186",
   month	 = "aug"
}
```


## お問い合わせ

意見や要望、質問などがありましたら、[こちら](https://openvideogame.cc/contact/)からお問い合わせ下さい。
