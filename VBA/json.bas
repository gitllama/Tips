
Sub a()

Dim Json As Object
Dim MyString As String
Dim text As String

text = ""
text = ReadFile(ThisWorkbook.Path & "\a.json")


Set Json = JsonConverter.ParseJson(text)

Debug.Print JsonConverter.ConvertToJson(Json, Whitespace:=2)
Debug.Print UBound(Json.Keys)

' 順序は保証されているのかな？
' されてないといやなのでナンバリングしたほうが良いよね
Dim key As Variant
For Each key In Json.Keys
    Debug.Print key
Next key

' JSON内の同じ項目は上書き

End Sub

Public Function ReadFile(ByVal File_Target As String)
'OpenステートメントのBinaryが一番早いらしい

    Dim intFF As Long
    Dim byt_buf() As Byte
    Dim str_Uni As String
    
    intFF = FreeFile
    Open File_Target For Binary As #intFF
        ReDim byt_buf(LOF(intFF))
        Get #intFF, , byt_buf
    Close #intFF
    str_Uni = StrConv(byt_buf(), vbUnicode) 'Unicodeに変換

    ReadFile = str_Uni
    
End Function
