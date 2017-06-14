# 忘れがちな設定

## Releaseビルド設定

> [ ] Visual Studio ホスティング プロセスを有効にする（O）

appname.vshost.exeの生成抑制

> ビルドの詳細設定 -> デバッグ情報 : none

pdbファイルの生成抑制

Dllのxmlはインテリセンス用でいらない

## フォルダ構成

exeから分離しても参照するには

app.config
```C#
<configuration>
<startup><supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.6" /></startup>
  <runtime>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <probing privatePath="dll" />
```
