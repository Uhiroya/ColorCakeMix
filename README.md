# MyTemplete_Unity2022.2.5.f1

## 概要  

.gitignoreの細かい設定。(エディタ設定の除外、AssetStoreToolsの除外等)

UnityからVS経由でコードを作成したときにShift-JISにならないように.editorconfigを追加してます。[参考](https://noracle.jp/unity_script_encode_issue/)

## ルール

#### 個人的な作業はブランチやフォルダを分けて行う。

(例)
  
ブランチ : WorkSpace/[自分の名前]

フォルダ : Asset/WorkSpace/[自分の名前]

#### コーディング規約
  
基本的にUnityC#で利用されているメジャーなものを採用します。

細かい注意個所としては

- constはPascalCase
- privateフィールド , [SerializeField] は先頭に"_"をつける
- メソッド名は動詞から
- イベントを起動させるメソッド(サブジェクト)には Onを先頭につける

よくわからない方は、下記URL記事を参考にすると良いと思います。

https://anderson02.com/cs/cs-rules/cs-rules01/

↑ C#のコーディングルールが詳しく書いています。 #01 ~ #12 をとりあえず読むのがおすすめです。

https://blog.unity.com/ja/engine-platform/clean-up-your-code-how-to-create-your-own-c-code-style

↑  「命名規則」 項目の
> ~~変数（m_）、定数（k_）、静的変数（s_）に接頭辞をつける~~

部分以外を参考にすると良いと思います。

## 開発環境  

Unity 2022.2.5.f1

## 導入ライブラリ

- UniTask
- UniRx
- DoTween



