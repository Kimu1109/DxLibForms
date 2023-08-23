'ファイル名:DxLibForms
'制作K-Studio
'開発K-Studio
'メンバーBroccoli
'バージョン:Ver1.0.0.1
'ダウンロード:https://github.com/Kimu1109/DxLibForms

'使えるコントロール一覧
'・TextBox
'・Label
'・Button
'・ListBox
'・TrackBar
'・ProgressBar
'・SpinButton
'・GraphScroll
'・VideoBox
'です。バグ等あれば教えてほしいです。

Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports DxLibDLL.DX
Public Module DxlibProcessCheck
    Public posx As Integer
    Public posy As Integer
    Public input As Integer
    Public key(256) As Byte
    ''' <summary>
    ''' ウインドウのメッセージを処理したり、入力されているキーを取得したり、マウスの状態を取得したり、
    ''' DxLibFormsにおいて、ちらつき防止の為の処理です。
    ''' </summary>
    Public Sub IfEnd()
        GetMousePoint(posx, posy)
        input = GetMouseInput()
        GetHitKeyStateAll(key)
        ScreenFlip()
        If ProcessMessage() = -1 Then
            DxLib_End()
            End
        End If
    End Sub
End Module
''' <summary>
''' DxlibFormsの位置に関する定数
''' </summary>
Public Module DxlibAlign

    Public Const Left As Integer = 0
    Public Const Center As Integer = 1
    Public Const Right As Integer = 2

End Module
''' <summary>
''' DxLibFormsのGraphのサイズに関する定数
''' </summary>
Public Module DxlibSize

    Public Const Keep As Integer = 0
    Public Const Zoom As Integer = 1

End Module
''' <summary>
''' DxLibFormsの位置に関するクラス
''' </summary>
Public Class Point

    Property X As Integer
    Property Y As Integer

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="x_int">X座標の値</param>
    ''' <param name="y_int">Y座標の値</param>
    Public Sub New(x_int As Integer, y_int As Integer)
        X = x_int
        Y = y_int
    End Sub

End Class
''' <summary>
''' DxlibFormsの上下のボタンのフォーム
''' </summary>
Public Class DxLibSpinButton

    ''' <summary>
    ''' 上のボタンがマウスで押されたら起きるイベント
    ''' </summary>
    ''' <param name="sender">このDxLibSpinButtonのオブジェクト。</param>
    ''' <param name="e">マウスの押されたボタン</param>
    Public Event UpButtonMouseInput(sender As Object, e As Integer)
    ''' <summary>
    ''' 上のボタンにマウスが中に入ったときに起きるイベント
    ''' </summary>
    ''' <param name="sender">このDxLibSpinButtonのオブジェクト。</param>
    ''' <param name="e">マウスが入ってきたときの座標</param>
    Public Event UpButtonInArea(sender As Object, e As Point)

    ''' <summary>
    ''' 下のボタンがマウスで押されたら起きるイベント
    ''' </summary>
    ''' <param name="sender">このDxLibSpinButtonのオブジェクト。</param>
    ''' <param name="e">マウスの押されたボタン</param>
    Public Event DownButtonMouseInput(sender As Object, e As Integer)
    ''' <summary>
    ''' 下のボタンにマウスが中に入ったときに起きるイベント
    ''' </summary>
    ''' <param name="sender">このDxLibSpinButtonのオブジェクト。</param>
    ''' <param name="e">マウスが入ってきたときの座標</param>
    Public Event DownButtonInArea(sender As Object, e As Point)

    ''' <summary>
    ''' 上か下のボタンがマウスで押されたときに起きるイベント
    ''' </summary>
    ''' <param name="sender">このDxLibSpinButtonのオブジェクト。</param>
    ''' <param name="up">Trueのときは上のボタンが押された。Falseのときは下のボタンが押された。</param>
    ''' <param name="e">マウスの押されたボタン</param>
    Public Event BothButtonMouseInput(sender As Object, up As Boolean, e As Integer)
    ''' <summary>
    ''' 上か下のボタンにマウスが中に入ったときに起きるイベント
    ''' </summary>
    ''' <param name="sender">このDxLibSpinButtonのオブジェクト。</param>
    ''' <param name="up">Trueのときは上のボタンに入ってきた。Falseのときは下のボタンに入ってきた。</param>
    ''' <param name="e">マウスが入ってきたときの座標</param>
    Public Event BothButtonInArea(sender As Object, up As Boolean, e As Point)

    Public UpButtonMouseInput_var As Boolean
    Public DownButtonMouseInput_var As Boolean
    Public BothButtonMouseInput_var As Boolean
    Public UpButtonInArea_var As Boolean
    Public DownButtonInArea_var As Boolean
    Public BothButtonInArea_var As Boolean

    Const White As UInteger = 4294967295
    Const Gray As UInteger = 4286611584

    Property X As Integer
    Property Y As Integer
    Property Width As Integer
    Property Height As Integer
    Property GraphSize As Integer
    Property BackGroundColor As UInteger
    Property BorderColor As UInteger
    Property Value As Double
    Property Interval As Double
    Property UpGraph As Integer
    Property DownGraph As Integer
    Property Trans As Integer
    Property Visible As Boolean

    Public ButtonInterval As Integer = 2

    Dim UpBoolean As Boolean
    Dim DownBoolean As Boolean

    Dim UpArea As Boolean
    Dim DownArea As Boolean

    ''' <summary>
    ''' 新規オブジェクト
    ''' </summary>
    ''' <param name="X_int"></param>
    ''' <param name="Y_int"></param>
    ''' <param name="Width_int"></param>
    ''' <param name="Height_int"></param>
    ''' <param name="GraphSize_siz"></param>
    ''' <param name="BackGroundColor_uint"></param>
    ''' <param name="BorderColor_uint"></param>
    ''' <param name="Value_dbl"></param>
    ''' <param name="Interval_dbl"></param>
    ''' <param name="UpGraph_int"></param>
    ''' <param name="DownGraph_int"></param>
    ''' <param name="Trans_int"></param>
    ''' <param name="Visible_bool"></param>
    Public Sub New(Optional X_int As Integer = 0,
                   Optional Y_int As Integer = 0,
                   Optional Width_int As Integer = 30,
                   Optional Height_int As Integer = 64,
                   Optional GraphSize_siz As Integer = DxlibSize.Keep,
                   Optional BackGroundColor_uint As UInteger = White,
                   Optional BorderColor_uint As UInteger = Gray,
                   Optional Value_dbl As Double = 0,
                   Optional Interval_dbl As Double = 1,
                   Optional UpGraph_int As Integer = -1,
                   Optional DownGraph_int As Integer = -1,
                   Optional Trans_int As Integer = 0,
                   Optional Visible_bool As Boolean = False)

        Visible = Visible_bool
        X = X_int
        Y = Y_int
        Width = Width_int
        Height = Height_int
        GraphSize = GraphSize_siz
        BackGroundColor = BackGroundColor_uint
        BorderColor = BorderColor_uint
        Value = Value_dbl
        Interval = Interval_dbl
        UpGraph = UpGraph_int
        DownGraph = DownGraph_int
        Trans = Trans_int

    End Sub
    ''' <summary>
    ''' 描画します。
    ''' </summary>
    Public Sub Draw()

        If Not Visible Then Exit Sub

        DrawBox(X, Y, X + Width, Y + Height / 2 - ButtonInterval, BackGroundColor, 1)
        DrawBox(X, Y + Height / 2 + ButtonInterval, X + Width, Y + Height, BackGroundColor, 1)

        Select Case GraphSize
            Case DxlibSize.Keep
                DrawRectGraph(X, Y, 0, 0, X + Width, Y + Height / 2 - ButtonInterval, UpGraph, Trans)
                DrawRectGraph(X, Y + Height / 2 + ButtonInterval, 0, 0, X + Width, Y + Height, DownGraph, Trans)
            Case DxlibSize.Zoom
                DrawExtendGraph(X, Y, X + Width, Y + Height / 2 - ButtonInterval, UpGraph, Trans)
                DrawExtendGraph(X, Y + Height / 2 + ButtonInterval, X + Width, Y + Height, DownGraph, Trans)
        End Select

        DrawBox(X, Y, X + Width, Y + Height / 2 - ButtonInterval, BorderColor, 0)
        DrawBox(X, Y + Height / 2 + ButtonInterval, X + Width, Y + Height, BorderColor, 0)

        If posx >= X And posy >= Y And posx <= X + Width And posy <= Y + Height / 2 - ButtonInterval Then
            If input <> 0 Then
                If UpBoolean = False Then
                    Value += Interval
                    RaiseEvent UpButtonMouseInput(Me, input)
                    RaiseEvent BothButtonMouseInput(Me, True, input)
                    UpButtonMouseInput_var = True
                    BothButtonMouseInput_var = True
                End If
                UpBoolean = True
            Else
                UpBoolean = False
            End If
            If Not UpArea Then
                RaiseEvent UpButtonInArea(Me, New Point(posx, posy))
                RaiseEvent BothButtonInArea(Me, True, New Point(posx, posy))
                UpButtonInArea_var = True
                BothButtonInArea_var = True
            End If
            UpArea = True
        Else
            UpArea = False
        End If
        If posx >= X And posy >= Y + Height / 2 - ButtonInterval And posx <= X + Width And posy <= Y + Height Then
            If input <> 0 Then
                If DownBoolean = False Then
                    Value -= Interval
                    RaiseEvent DownButtonMouseInput(Me, input)
                    RaiseEvent BothButtonMouseInput(Me, False, input)
                    DownButtonMouseInput_var = True
                    BothButtonMouseInput_var = True
                End If
                DownBoolean = True
            Else
                DownBoolean = False
            End If
            If Not DownArea Then
                RaiseEvent DownButtonInArea(Me, New Point(posx, posy))
                RaiseEvent BothButtonInArea(Me, False, New Point(posx, posy))
                DownButtonInArea_var = True
                BothButtonInArea_var = True
            End If
            DownArea = True
        Else
            DownArea = False
        End If

    End Sub

End Class
''' <summary>
''' DxLibFormsのGraphをスライドショーのように表示するフォーム
''' 左端をダブルクリックで、一個前のGraph
''' 右端をダブルクリックで、一個先のGraph
''' </summary>
Public Class DxLibGraphScroll

    ''' <summary>
    ''' マウスが押されたとき
    ''' </summary>
    ''' <param name="sender">DxLibGraphScrollのオブジェクト</param>
    ''' <param name="e">マウスで押されたボタン</param>
    Public Event MouseInput(sender As Object, e As Integer)
    ''' <summary>
    ''' スクロールされたとき
    ''' </summary>
    ''' <param name="sender">DxLibGraphScrollのオブジェクト</param>
    ''' <param name="e">表示されているGraphの順番</param>
    Public Event ScrollGraph(sender As Object, e As Integer)
    ''' <summary>
    ''' マウスがエリアに入ってきたとき
    ''' </summary>
    ''' <param name="sender">DxLibGraphScrollのオブジェクト</param>
    ''' <param name="e">入ってきたときのマウスの座標</param>
    Public Event InArea(sender As Object, e As Point)

    Public MouseInput_var As Boolean
    Public ScrollGraph_var As Boolean
    Public InArea_var As Boolean

    Const White As UInteger = 4294967295
    Const Gray As UInteger = 4286611584

    Property X As Integer
    Property Y As Integer
    Property Width As Integer
    Property Height As Integer
    Property GraphList As List(Of Integer)
    Property GraphSize As Integer
    Property BackGroundColor As UInteger
    Property BorderColor As UInteger
    Property Trans As Integer
    Property AutoSize As Boolean
    Property Value As Integer
    Property Visible As Boolean

    Dim leftClick As Integer
    Dim leftClickTime As Integer
    Dim leftInput As Boolean
    Dim rightClick As Integer
    Dim rightClickTime As Integer
    Dim rightInput As Boolean

    Dim Area As Boolean
    Dim InputBuf As Boolean

    ''' <summary>
    ''' 新規オブジェクト
    ''' </summary>
    ''' <param name="GraphList_lst"></param>
    ''' <param name="X_int"></param>
    ''' <param name="Y_int"></param>
    ''' <param name="Width_int"></param>
    ''' <param name="Height_int"></param>
    ''' <param name="GraphSize_int"></param>
    ''' <param name="BackGroundColor_uint"></param>
    ''' <param name="BorderColor_uint"></param>
    ''' <param name="Trans_int"></param>
    ''' <param name="AutoSize_bool"></param>
    ''' <param name="Value_int"></param>
    Public Sub New(GraphList_lst As List(Of Integer),
                   Optional X_int As Integer = 0,
                   Optional Y_int As Integer = 0,
                   Optional Width_int As Integer = 128,
                   Optional Height_int As Integer = 64,
                   Optional GraphSize_int As Integer = DxlibSize.Keep,
                   Optional BackGroundColor_uint As UInteger = White,
                   Optional BorderColor_uint As UInteger = Gray,
                   Optional Trans_int As Integer = 0,
                   Optional AutoSize_bool As Boolean = False,
                   Optional Value_int As Integer = 0,
                   Optional Visible_bool As Boolean = True)

        Visible = Visible_bool
        GraphList = GraphList_lst
        X = X_int
        Y = Y_int
        Width = Width_int
        Height = Height_int
        GraphSize = GraphSize_int
        BackGroundColor = BackGroundColor_uint
        BorderColor = BorderColor_uint
        Trans = Trans_int
        AutoSize = AutoSize_bool
        Value = Value_int

    End Sub
    ''' <summary>
    ''' 描画する
    ''' </summary>
    Public Sub Draw()

        If Not Visible Then Exit Sub

        DrawBox(X, Y, X + Width, Y + Height, BackGroundColor, 1)

        Select Case GraphSize
            Case DxlibSize.Keep
                DrawRectGraph(X, Y, 0, 0, Width, Height, GraphList(Value), Trans)
            Case DxlibSize.Zoom
                DrawExtendGraph(X, Y, X + Width, Y + Height, GraphList(Value), Trans)
        End Select

        If posy >= Y And posy <= Y + Height Then

            If posx >= X And posx <= X + Width Then

                If input <> 0 Then
                    If InputBuf = False Then
                        RaiseEvent MouseInput(Me, input)
                        MouseInput_var = True
                    End If
                    InputBuf = True
                Else
                    InputBuf = False
                End If

                If Area = False Then
                    RaiseEvent InArea(Me, New Point(posx, posy))
                    InArea_var = True
                End If
                Area = True
            Else
                Area = False
            End If

            If posx >= X And posx <= X + Width / 4 Then
                If input <> 0 Then
                    If leftInput = False Then
                        If leftClickTime + 1 >= Microsoft.VisualBasic.Timer Then
                            leftClick += 1
                            leftClickTime = Microsoft.VisualBasic.Timer
                            If leftClick = 2 Then
                                RaiseEvent ScrollGraph(Me, Value)
                                ScrollGraph_var = True
                                If Value - 1 >= 0 Then Value -= 1
                                leftClick = 0
                            End If
                        Else
                            leftClick = 1
                            leftClickTime = Microsoft.VisualBasic.Timer
                        End If
                    End If
                    leftInput = True
                Else
                    leftInput = False
                End If
            End If
            If posx >= X + Width - (Width / 4) And posx <= X + Width Then
                If input <> 0 Then
                    If rightInput = False Then
                        If rightClickTime + 1 >= Microsoft.VisualBasic.Timer Then
                            rightClick += 1
                            rightClickTime = Microsoft.VisualBasic.Timer
                            If rightClick = 2 Then
                                RaiseEvent ScrollGraph(Me, Value)
                                If Value + 1 <= GraphList.Count - 1 Then Value += 1
                                rightClick = 0
                            End If
                        Else
                            rightClick = 1
                            rightClickTime = Microsoft.VisualBasic.Timer
                        End If
                    End If
                    rightInput = True
                Else
                    rightInput = False
                End If
            End If
        Else
            Area = False
        End If

        DrawBox(X, Y, X + Width, Y + Height, BorderColor, 0)

    End Sub

End Class
''' <summary>
''' DxLibFormsの映像を表示するフォーム
''' </summary>
Public Class DxLibVideoBox

    ''' <summary>
    ''' マウスがエリアに入ってきたとき
    ''' </summary>
    ''' <param name="sender">DxLibVideoBoxのオブジェクト</param>
    ''' <param name="e">入ってきたときのマウスの座標</param>
    Public Event InArea(sender As Object, e As Point)
    ''' <summary>
    ''' 映像をユーザーが再生状態が変わったとき
    ''' </summary>
    ''' <param name="sender">DxLibVideoBoxのオブジェクト</param>
    Public Event UserPlayType(sender As Object)
    ''' <summary>
    ''' 映像のシークバーの位置をユーザーが変えたとき
    ''' </summary>
    ''' <param name="sender">DxLibVideoBoxのオブジェクト</param>
    ''' <param name="o">変える前のシークバーのフレーム数</param>
    ''' <param name="n">変えた後のシークバーのフレーム数</param>
    Public Event UserTime(sender As Object, o As Integer, n As Integer)
    ''' <summary>
    ''' マウスのボタンが押されたとき
    ''' </summary>
    ''' <param name="sender">DxLibVideoBoxのオブジェクト</param>
    ''' <param name="e">押されたマウスのボタン</param>
    Public Event MouseInput(sender As Object, e As Integer)

    Public InArea_var As Boolean
    Public UserPlayType_var As Boolean
    Public UserTime_var As Boolean
    Public MouseInput_var As Boolean

    Const White As UInteger = 4294967295
    Const Gray As UInteger = 4286611584
    Const DarkCyan As UInteger = 4278222976

    Property X As Integer
    Property Y As Integer
    Property Width As Integer
    Property Height As Integer
    Property VideoSize As Integer
    Property VideoHandle As Integer
    Property BackGroundColor As UInteger
    Property BorderColor As UInteger
    Property Trans As Integer
    Property Play As Boolean
    Property Speed As Double
    Property Time As Integer
    Property SeekBarColor As UInteger
    Property Visible As Boolean

    Public AllTime As Double

    Dim oldFrame As Integer
    Dim newFrame As Integer

    Dim BufInput As Boolean
    Dim BufArea As Boolean
    ''' <summary>
    ''' 新規オブジェクト
    ''' </summary>
    ''' <param name="X_int"></param>
    ''' <param name="Y_int"></param>
    ''' <param name="Width_int"></param>
    ''' <param name="Height_int"></param>
    ''' <param name="VideoSize_int"></param>
    ''' <param name="VideoHandle_int"></param>
    ''' <param name="Trans_int"></param>
    ''' <param name="BackGroundColor_uint"></param>
    ''' <param name="BorderColor_uint"></param>
    ''' <param name="Play_bool"></param>
    ''' <param name="Speed_dbl"></param>
    ''' <param name="Time_int"></param>
    ''' <param name="SeekBarColor_uint"></param>
    Public Sub New(Optional X_int As Integer = 0,
                   Optional Y_int As Integer = 0,
                   Optional Width_int As Integer = 256,
                   Optional Height_int As Integer = 128,
                   Optional VideoSize_int As Integer = DxlibSize.Keep,
                   Optional VideoHandle_int As Integer = -1,
                   Optional Trans_int As Integer = 0,
                   Optional BackGroundColor_uint As UInteger = White,
                   Optional BorderColor_uint As UInteger = Gray,
                   Optional Play_bool As Boolean = False,
                   Optional Speed_dbl As Double = 1,
                   Optional Time_int As Integer = 0,
                   Optional SeekBarColor_uint As UInteger = DarkCyan,
                   Optional Visible_bool As Boolean = True)

        Visible = Visible_bool
        X = X_int
        Y = Y_int
        Width = Width_int
        Height = Height_int
        VideoSize = VideoSize_int
        VideoHandle = VideoHandle_int
        Trans = Trans_int
        BackGroundColor = BackGroundColor_uint
        BorderColor = BorderColor_uint
        SeekBarColor = SeekBarColor_uint

        Play = Play_bool
        If Play Then
            PlayMovieToGraph(VideoHandle)
        End If
        Speed = Speed_dbl
        SetPlaySpeedRateMovieToGraph(VideoHandle, Speed)
        Time = Time_int
        SeekMovieToGraph(VideoHandle, Time)

        AllTime = GetMovieTotalFrameToGraph(VideoHandle)

    End Sub
    ''' <summary>
    ''' 描画する
    ''' </summary>
    Public Sub Draw()

        If Not Visible Then Exit Sub

        DrawBox(X, Y, X + Width, Y + Height, BackGroundColor, 1)

        Select Case VideoSize
            Case DxlibSize.Keep
                DrawRectGraph(X, Y, 0, 0, Width, Height, VideoHandle, Trans)
            Case DxlibSize.Zoom
                DrawExtendGraph(X, Y, X + Width, Y + Height, VideoHandle, Trans)
        End Select
        Time = TellMovieToGraphToFrame(VideoHandle)

        DrawBox(X, Y + Height + 10, X + Width, Y + Height + 20, BackGroundColor, 1)
        DrawBox(X, Y + Height + 10, X + Width, Y + Height + 20, BorderColor, 0)

        DrawBox(X, Y + Height + 10, X + (Time / AllTime) * Width, Y + Height + 20, SeekBarColor, 1)
        DrawBox(X, Y + Height + 10, X + (Time / AllTime) * Width, Y + Height + 20, Gray, 0)

        DrawBox(X, Y, X + Width, Y + Height, BorderColor, 0)

        If input <> 0 Then
            If Not BufInput Then

                If posx >= X And posx <= X + Width And posy >= Y And posy <= Y + Height Then
                    RaiseEvent MouseInput(Me, input)
                    MouseInput_var = True
                End If

                If posx >= X - Width / 3 + Width / 2 And posx <= X + Width / 3 + Width / 2 And posy >= Y + Height / 2 - Height / 3 And posy <= Y + Height / 2 + Height / 3 Then
                    Play = Not Play
                    If Play Then
                        PlayMovieToGraph(VideoHandle)
                    Else
                        PauseMovieToGraph(VideoHandle)
                    End If
                    RaiseEvent UserPlayType(Me)
                    UserPlayType_var = True
                End If
                If posx >= X And posx <= X + Width And posy >= Y + Height + 10 And posy <= Y + Height + 20 Then

                    oldFrame = Time
                    newFrame = AllTime * ((posx - X) / Width)

                    SeekMovieToGraphToFrame(VideoHandle, newFrame)

                    RaiseEvent UserTime(Me, oldFrame, newFrame)
                    UserTime_var = True
                End If
            End If
            BufInput = True
        Else
            BufInput = False
        End If

        If posx >= X And posx <= X + Width And posy >= Y And posy <= Y + Height Then
            If BufArea = False Then
                RaiseEvent InArea(Me, New Point(posx, posy))
                InArea_var = True
            End If
            BufArea = True
        Else
            BufArea = False
        End If

    End Sub
    ''' <summary>
    ''' 映像の再生映像を再生にする。
    ''' </summary>
    Public Sub PlayVideo()
        Play = True
        PlayMovieToGraph(VideoHandle)
    End Sub
    ''' <summary>
    ''' 映像の再生状態を停止にする。
    ''' </summary>
    Public Sub PauseVideo()
        Play = False
        PauseMovieToGraph(VideoHandle)
    End Sub
    ''' <summary>
    ''' 映像の再生速度を変更する。
    ''' </summary>
    ''' <param name="rate">再生倍率(標準:1.00)</param>
    Public Sub SpeedVideo(rate As Double)
        Speed = rate
        SetPlaySpeedRateMovieToGraph(VideoHandle, Speed)
    End Sub
    ''' <summary>
    ''' 映像の再生するフレーム数を設定する。
    ''' </summary>
    ''' <param name="frame">フレーム数</param>
    Public Sub TimeVideo(frame As Integer)
        Time = frame
        SeekMovieToGraph(VideoHandle, Time)
    End Sub
End Class
''' <summary>
''' DxLibFormsのプロセスの状態を表現するためのフォーム
''' </summary>
Public Class DxLibProgressBar

    ''' <summary>
    ''' DxLibProgressBarの状態がマックスになったとき
    ''' </summary>
    ''' <param name="sender">DxLibProgressBarのオブジェクト</param>
    Public Event ValueIsMaxNum(sender As Object)
    Public ValueIsMaxNum_var As Boolean

    Const White As UInteger = 4294967295
    Const Gray As UInteger = 4286611584

    Property X As Integer
    Property Y As Integer
    Property Width As Integer
    Property Height As Integer
    Property MaxNum As Integer
    Property MinNum As Integer
    Property Value As Double
    Property BackGroundColor As UInteger
    Property BorderColor As UInteger
    Property ProgressColor As UInteger
    Property Visible As Boolean

    ''' <summary>
    ''' 新規オブジェクト
    ''' </summary>
    ''' <param name="ProgressColor_uint"></param>
    ''' <param name="X_int"></param>
    ''' <param name="Y_int"></param>
    ''' <param name="Width_int"></param>
    ''' <param name="Height_int"></param>
    ''' <param name="MaxNum_int"></param>
    ''' <param name="MinNum_int"></param>
    ''' <param name="Value_dbl"></param>
    ''' <param name="BackGroundColor_uint"></param>
    ''' <param name="BorderColor_uint"></param>
    Public Sub New(Optional ProgressColor_uint As UInteger = Gray,
                   Optional X_int As Integer = 0,
                   Optional Y_int As Integer = 0,
                   Optional Width_int As Integer = 200,
                   Optional Height_int As Integer = 30,
                   Optional MaxNum_int As Integer = 100,
                   Optional MinNum_int As Integer = 0,
                   Optional Value_dbl As Double = 0,
                   Optional BackGroundColor_uint As UInteger = White,
                   Optional BorderColor_uint As UInteger = Gray,
                   Optional Visible_bool As Boolean = True)

        Visible = Visible_bool
        X = X_int
        Y = Y_int
        Width = Width_int
        Height = Height_int
        MaxNum = MaxNum_int
        MinNum = MinNum_int
        Value = Value_dbl
        BackGroundColor = BackGroundColor_uint
        BorderColor = BorderColor_uint
        ProgressColor = ProgressColor_uint

    End Sub
    ''' <summary>
    ''' 描画する
    ''' </summary>
    Public Sub Draw()

        If Not Visible Then Exit Sub

        DrawBox(X, Y, X + Width, Y + Height, BackGroundColor, 1)
        DrawBox(X, Y, X + Value / (MaxNum - MinNum) * Width, Y + Height, ProgressColor, 1)
        DrawBox(X, Y, X + Width, Y + Height, BorderColor, 0)

        If CInt(Value) = MaxNum Then
            RaiseEvent ValueIsMaxNum(Me)
            ValueIsMaxNum_var = True
        End If

    End Sub
    ''' <summary>
    ''' 状態を割合で表現する(0～1)
    ''' </summary>
    ''' <param name="rate"></param>
    Public Sub ValueRate(rate As Double)

        Value = (MaxNum - MinNum) * rate

    End Sub
    ''' <summary>
    ''' 状態を割合分足す(0～1)
    ''' </summary>
    ''' <param name="rate"></param>
    Public Sub ValuePlusRate(rate As Double)

        Value += (MaxNum - Value) * rate

    End Sub

End Class
''' <summary>
''' DxLibFormsのトラックバーのフォーム
''' </summary>
Public Class DxLibTrackBar

    ''' <summary>
    ''' 値を変更されたときのイベント
    ''' </summary>
    ''' <param name="sender">DxLibTrackBarのオブジェクト</param>
    ''' <param name="e">値</param>
    Public Event ChangeValue(sender As Object, e As Double)

    Public ChangeValue_var As Boolean

    Const White As UInteger = 4294967295
    Const Gray As UInteger = 4286611584
    Const Black As UInteger = 4278190080
    Const DarkCyan As UInteger = 4278222976

    Property X As Integer
    Property Y As Integer
    Property Width As Integer
    Property Height As Integer
    Property MaxNum As Double
    Property MinNum As Double
    Property Value As Double
    Property Interval As Double
    Property BarColor As UInteger
    Property SelectColor As UInteger
    Property PositionColor As UInteger
    Property BackGroundColor As UInteger
    Property BackGroundGraph As Integer
    Property GraphSize As Integer
    Property BackGroundTrans As Boolean
    Property Trans As Integer
    Property Thick As Integer
    Property Visible As Boolean

    Dim Range As Double
    Dim DotInterval As Double

    Public WidthOfSelectColor As Integer = 3

    ''' <summary>
    ''' 新規オブジェクト
    ''' </summary>
    ''' <param name="X_int"></param>
    ''' <param name="Y_int"></param>
    ''' <param name="Width_int"></param>
    ''' <param name="Height_int"></param>
    ''' <param name="MaxNum_dbl"></param>
    ''' <param name="MinNum_dbl"></param>
    ''' <param name="Value_dbl"></param>
    ''' <param name="Interval_dbl"></param>
    ''' <param name="BarColor_uint"></param>
    ''' <param name="PositionColor_uint"></param>
    ''' <param name="SelectColor_uint"></param>
    ''' <param name="BackGroundColor_uint"></param>
    ''' <param name="BackGroundGraph_int"></param>
    ''' <param name="GraphSize_siz"></param>
    ''' <param name="BackGroundTrans_bool"></param>
    ''' <param name="Trans_int"></param>
    ''' <param name="Thick_int"></param>
    Public Sub New(Optional X_int As Integer = 0,
                   Optional Y_int As Integer = 0,
                   Optional Width_int As Integer = 200,
                   Optional Height_int As Integer = 30,
                   Optional MaxNum_dbl As Double = 10,
                   Optional MinNum_dbl As Double = 0,
                   Optional Value_dbl As Double = 5,
                   Optional Interval_dbl As Double = 1,
                   Optional BarColor_uint As UInteger = Black,
                   Optional PositionColor_uint As UInteger = Gray,
                   Optional SelectColor_uint As UInteger = DarkCyan,
                   Optional BackGroundColor_uint As UInteger = White,
                   Optional BackGroundGraph_int As Integer = -1,
                   Optional GraphSize_siz As Integer = DxlibSize.Keep,
                   Optional BackGroundTrans_bool As Boolean = False,
                   Optional Trans_int As Integer = 0,
                   Optional Thick_int As Integer = 3,
                   Optional Visible_bool As Boolean = True)

        Visible = Visible_bool
        X = X_int
        Y = Y_int
        Width = Width_int
        Height = Height_int
        MaxNum = MaxNum_dbl
        MinNum = MinNum_dbl
        Value = Value_dbl
        Interval = Interval_dbl
        BarColor = BarColor_uint
        PositionColor = PositionColor_uint
        BackGroundColor = BackGroundColor_uint
        BackGroundGraph = BackGroundGraph_int
        BackGroundTrans = BackGroundTrans_bool
        SelectColor = SelectColor_uint
        Trans = Trans_int
        GraphSize = GraphSize_siz
        Thick = Thick_int

    End Sub
    ''' <summary>
    ''' 描画する
    ''' </summary>
    Public Sub Draw()

        If Not Visible Then Exit Sub

        If Not BackGroundTrans Then
            DrawBox(X, Y, X + Width, Y + Height, BackGroundColor, 1)
            If BackGroundGraph <> -1 Then
                Select Case GraphSize
                    Case DxlibSize.Keep
                        DrawRectGraph(X, Y, 0, 0, Width, Height, BackGroundGraph, Trans)
                    Case DxlibSize.Zoom
                        DrawExtendGraph(X, Y, X + Width, Y + Height, BackGroundGraph, Trans)
                End Select
            End If
        End If

        DrawLine(X, Y + Height / 2, X + Width, Y + Height / 2, BarColor, Thick)
        DrawLine(X, Y, X, Y + Height, BarColor, Thick)
        DrawLine(X + Width, Y, X + Width, Y + Height, BarColor, Thick)

        Range = MaxNum - MinNum
        DotInterval = Width / (Range / Interval)

        For i = 0 To Width Step DotInterval
            DrawLine(X + i, Y + Height / 4, X + i, Y + Height - Height / 4, BarColor, Thick)
        Next

        DrawBox(X + DotInterval * (Value / Interval) - WidthOfSelectColor, Y, X + DotInterval * (Value / Interval) + WidthOfSelectColor, Y + Height, SelectColor, 1)

        If posx >= X And posx <= X + Width Then
            If posy >= Y And posy <= Y + Height Then
                If input <> 0 Then
                    For i = 0 To Width Step DotInterval
                        If i - DotInterval / 2 <= posx - X And i + DotInterval / 2 >= posx - X Then
                            If Value <> Interval * (i / DotInterval) Then
                                Value = Interval * (i / DotInterval)
                                RaiseEvent ChangeValue(Me, Value)
                                ChangeValue_var = True
                            End If
                        End If
                    Next
                End If
            End If
        End If

    End Sub

End Class
''' <summary>
''' DxLibFormsのチェックボックスのフォーム
''' </summary>
Public Class DxLibCheckBox

    ''' <summary>
    ''' ボックスの中身が変わったとき
    ''' </summary>
    ''' <param name="sender">DxLibCheckBoxのオブジェクト</param>
    ''' <param name="e">値</param>
    Public Event ChangeValue(sender As Object, e As Boolean)
    ''' <summary>
    ''' エリア内に入ってきたとき
    ''' </summary>
    ''' <param name="sender">DxLibCheckBoxのオブジェクト</param>
    ''' <param name="e">入ってきたときのマウス座標</param>
    Public Event InArea(sender As Object, e As Point)
    ''' <summary>
    ''' チェックボックスのエリアに入ってきたとき
    ''' </summary>
    ''' <param name="sender">DxLibCheckBoxのオブジェクト</param>
    ''' <param name="e">入ってきたときのマウス座標</param>
    Public Event InAreaInBox(sender As Object, e As Point)

    Public ChangeValue_var As Boolean
    Public InArea_var As Boolean
    Public InAreaInBox_var As Boolean

    Const White As UInteger = 4294967295
    Const Gray As UInteger = 4286611584
    Const Black As UInteger = 4278190080

    Property X As Integer
    Property Y As Integer
    Property Width As Integer
    Property Height As Integer
    Property Text As String
    Property GraphSize As Integer
    Property BackGroundColor As UInteger
    Property BackGroundGraph As Integer
    Property FontColor As UInteger
    Property FontHandle As Integer
    Property BorderColor As UInteger
    Property AutoSize As Boolean
    Property Trans As Integer
    Property BackGroundTrans As Boolean
    Property BoxBorderColor As UInteger
    Property CheckGraph As Integer
    Property NothingGraph As Integer
    Property Value As Boolean
    Property BoxTrans As Integer
    Property Visible As Boolean

    Dim TextArr() As String
    Dim BufInput As Integer
    Dim BufArea As Boolean
    Dim BufAreaInBox As Boolean
    ''' <summary>
    ''' 新規オブジェクト
    ''' </summary>
    ''' <param name="X_int"></param>
    ''' <param name="Y_int"></param>
    ''' <param name="Width_int"></param>
    ''' <param name="Height_int"></param>
    ''' <param name="Text_str"></param>
    ''' <param name="GraphSize_siz"></param>
    ''' <param name="BackGroundColor_uint"></param>
    ''' <param name="BackGroundGraph_int"></param>
    ''' <param name="FontColor_uint"></param>
    ''' <param name="FontHandle_int"></param>
    ''' <param name="BorderColor_uint"></param>
    ''' <param name="AutoSize_bool"></param>
    ''' <param name="BackGroundTrans_bool"></param>
    ''' <param name="Trans_int"></param>
    ''' <param name="BoxBorderColor_uint"></param>
    ''' <param name="CheckGraph_int"></param>
    ''' <param name="NothingGraph_int"></param>
    ''' <param name="Value_bool"></param>
    ''' <param name="BoxTrans_int"></param>
    Public Sub New(Optional X_int As Integer = 0,
                   Optional Y_int As Integer = 0,
                   Optional Width_int As Integer = 150,
                   Optional Height_int As Integer = 25,
                   Optional Text_str As String = "CheckBox",
                   Optional GraphSize_siz As Integer = DxlibSize.Keep,
                   Optional BackGroundColor_uint As UInteger = White,
                   Optional BackGroundGraph_int As Integer = -1,
                   Optional FontColor_uint As UInteger = Black,
                   Optional FontHandle_int As Integer = -1,
                   Optional BorderColor_uint As UInteger = Gray,
                   Optional AutoSize_bool As Boolean = False,
                   Optional BackGroundTrans_bool As Boolean = False,
                   Optional Trans_int As Integer = 0,
                   Optional BoxBorderColor_uint As UInteger = Black,
                   Optional CheckGraph_int As Integer = -1,
                   Optional NothingGraph_int As Integer = -1,
                   Optional Value_bool As Boolean = False,
                   Optional BoxTrans_int As Integer = 0,
                   Optional Visible_bool As Boolean = True)

        Visible = Visible_bool
        X = X_int
        Y = Y_int
        Width = Width_int
        Height = Height_int
        Text = Text_str
        GraphSize = GraphSize_siz
        BackGroundColor = BackGroundColor_uint
        BackGroundGraph = BackGroundGraph_int
        FontColor = FontColor_uint
        BoxBorderColor = BoxBorderColor_uint
        Value = Value_bool
        BoxTrans = BoxTrans_int

        If FontHandle_int = -1 Then
            FontHandle = CreateFontToHandle("MS UI Gothic", 24, -1)
        Else
            FontHandle = FontHandle_int
        End If

        BorderColor = BorderColor_uint
        AutoSize = AutoSize_bool
        BackGroundTrans = BackGroundTrans_bool
        Trans = Trans_int
        CheckGraph = CheckGraph_int
        NothingGraph = NothingGraph_int

    End Sub
    ''' <summary>
    ''' 描画する
    ''' </summary>
    Public Sub Draw()

        If Not Visible Then Exit Sub

        If AutoSize Then
            TextArr = Regex.Split(Text, "\r\n|\n")
            Height = GetFontSizeToHandle(FontHandle) * TextArr.Length
            Width = 0
            For i = 0 To UBound(TextArr)
                If Width < GetDrawStringWidthToHandle(TextArr(i), -1, FontHandle) Then
                    Width = GetDrawStringWidthToHandle(TextArr(i), -1, FontHandle)
                End If
            Next
            Width += Height - 5 + 5 + 2
        End If

        If Not BackGroundTrans Then
            DrawBox(X, Y, X + Width, Y + Height, BackGroundColor, 1)

            If BackGroundGraph <> -1 Then
                Select Case GraphSize
                    Case DxlibSize.Keep
                        DrawRectGraph(X, Y, 0, 0, Width, Height, BackGroundGraph, Trans)
                    Case DxlibSize.Zoom
                        DrawExtendGraph(X, Y, X + Width, Y + Height, BackGroundGraph, Trans)
                End Select
            End If

            DrawBox(X, Y, X + Width, Y + Height, BorderColor, 0)
        End If

        If Value Then
            DrawExtendGraph(X + 5, Y + 5, X + Height - 5, Y + Height - 5, CheckGraph, BoxTrans)
        Else
            DrawExtendGraph(X + 5, Y + 5, X + Height - 5, Y + Height - 5, NothingGraph, BoxTrans)
        End If

        DrawBox(X + 5, Y + 5, X + Height - 5, Y + Height - 5, BoxBorderColor, 0)

        DrawStringToHandle(X + Height - 5 + 2, Y, Text, FontColor, FontHandle)

        If posx >= X And posx <= X + Width And posy >= Y And posy <= Y + Height Then
            If Not BufArea Then
                RaiseEvent InArea(Me, New Point(posx, posy))
                InArea_var = True
            End If
            BufArea = True
        Else
            BufArea = False
        End If

        If posx >= X + 5 And posx <= X + Height - 5 And posy >= Y + 5 And posy <= Y + Height - 5 Then
            If BufInput <> input And input <> 0 Then
                Value = Not Value
                RaiseEvent ChangeValue(Me, Value)
                ChangeValue_var = True
            End If

            If Not BufAreaInBox Then
                RaiseEvent InAreaInBox(Me, New Point(posx, posy))
                InAreaInBox_var = True
            End If
            BufAreaInBox = True
        Else
            BufAreaInBox = False
        End If
        BufInput = input

    End Sub

End Class
''' <summary>
''' DxLibFormsのボタンのフォーム
''' </summary>
Public Class DxLibButton

    ''' <summary>
    ''' マウスがエリア内に入ってきたとき
    ''' </summary>
    ''' <param name="sender">DxLibButtonのオブジェクト</param>
    ''' <param name="e">入ってきたときのマウス座標</param>
    Public Event InArea(sender As Object, e As Point)
    ''' <summary>
    ''' マウスでクリックされたとき
    ''' </summary>
    ''' <param name="sender">DxLibButtonのオブジェクト</param>
    ''' <param name="e">押されたマウスのボタン</param>
    Public Event MouseInput(sender As Object, e As Integer)
    ''' <summary>
    ''' マウスのクリックが終わったとき
    ''' </summary>
    ''' <param name="sender">DxLibButtonのオブジェクト</param>
    Public Event MouseEndInput(sender As Object)

    Public InArea_var As Boolean
    Public MouseInput_var As Boolean
    Public MouseEndInput_var As Boolean

    Const White As UInteger = 4294967295
    Const Gray As UInteger = 4286611584
    Const Black As UInteger = 4278190080

    Property X As Integer
    Property Y As Integer
    Property Width As Integer
    Property Height As Integer
    Property Text As String
    Property TextAlign As Integer
    Property GraphSize As Integer
    Property BackGroundColor As UInteger
    Property BackGroundGraph As Integer
    Property FontColor As UInteger
    Property FontHandle As Integer
    Property BorderColor As UInteger
    Property AutoSize As Boolean
    Property Trans As Integer
    Property Visible As Boolean

    Dim TextArr() As String
    Dim TextWidth() As Integer
    Dim TextMaxWidth As Integer

    Dim TextHeight As Integer
    Dim SingleTextHeight As Integer

    Dim bufinput As Integer
    Dim bufarea As Boolean
    Dim bufinputbool As Boolean
    ''' <summary>
    ''' 新規オブジェクト
    ''' </summary>
    ''' <param name="X_int"></param>
    ''' <param name="Y_int"></param>
    ''' <param name="Width_int"></param>
    ''' <param name="Height_int"></param>
    ''' <param name="Text_str"></param>
    ''' <param name="TextAlign_alg"></param>
    ''' <param name="GraphSize_siz"></param>
    ''' <param name="BackGroundColor_uint"></param>
    ''' <param name="BackGroundGraph_int"></param>
    ''' <param name="FontColor_uint"></param>
    ''' <param name="FontHandle_int"></param>
    ''' <param name="BorderColor_uint"></param>
    ''' <param name="AutoSize_bool"></param>
    ''' <param name="Trans_int"></param>
    Public Sub New(Optional X_int As Integer = 0,
                   Optional Y_int As Integer = 0,
                   Optional Width_int As Integer = 100,
                   Optional Height_int As Integer = 30,
                   Optional Text_str As String = "Button",
                   Optional TextAlign_alg As Integer = DxlibAlign.Left,
                   Optional GraphSize_siz As Integer = DxlibSize.Keep,
                   Optional BackGroundColor_uint As UInteger = White,
                   Optional BackGroundGraph_int As Integer = -1,
                   Optional FontColor_uint As UInteger = Black,
                   Optional FontHandle_int As Integer = -1,
                   Optional BorderColor_uint As UInteger = Gray,
                   Optional AutoSize_bool As Boolean = False,
                   Optional Trans_int As Integer = 0,
                   Optional Visible_bool As Boolean = True)

        Visible = Visible_bool
        X = X_int
        Y = Y_int
        Width = Width_int
        Height = Height_int
        Text = Text_str
        TextAlign = TextAlign_alg
        GraphSize = GraphSize_siz
        BackGroundColor = BackGroundColor_uint
        BackGroundGraph = BackGroundGraph_int
        FontColor = FontColor_uint
        Trans = Trans_int

        If FontHandle_int = -1 Then
            FontHandle = CreateFontToHandle("MS UI Gothic", 24, -1)
        Else
            FontHandle = FontHandle_int
        End If

        BorderColor = BorderColor_uint
        AutoSize = AutoSize_bool

    End Sub
    ''' <summary>
    ''' 描画する
    ''' </summary>
    Public Sub Draw()

        If Not Visible Then Exit Sub

        TextArr = Regex.Split(Text, "\r\n|\n")
        ReDim TextWidth(UBound(TextArr))
        For i = 0 To UBound(TextArr)
            TextWidth(i) = GetDrawStringWidthToHandle(TextArr(i), -1, FontHandle)
            If TextWidth(i) > TextMaxWidth Then TextMaxWidth = TextWidth(i)
        Next
        SingleTextHeight = GetFontSizeToHandle(FontHandle)
        TextHeight = SingleTextHeight * UBound(TextArr)

        If AutoSize Then
            Height = TextHeight
            Width = TextMaxWidth
        End If

        DrawBox(X, Y, X + Width, Y + Height, BackGroundColor, 1)
        If BackGroundGraph <> -1 Then
            Select Case GraphSize
                Case DxlibSize.Keep
                    DrawRectGraph(X, Y, 0, 0, Width, Height, BackGroundGraph, Trans)
                Case DxlibSize.Zoom
                    DrawExtendGraph(X, Y, X + Width, Y + Height, BackGroundGraph, Trans)
            End Select
        End If
        DrawBox(X, Y, X + Width, Y + Height, BorderColor, 0)

        For i = 0 To UBound(TextArr)
            Select Case TextAlign
                Case DxlibAlign.Left
                    DrawStringToHandle(X, Y + SingleTextHeight * i, TextArr(i), FontColor, FontHandle)
                Case DxlibAlign.Center
                    DrawStringToHandle(X + (Width - TextWidth(i)) / 2, Y + SingleTextHeight * i, TextArr(i), FontColor, FontHandle)
                Case DxlibAlign.Right
                    DrawStringToHandle(X + Width - TextWidth(i), Y + SingleTextHeight * i, TextArr(i), FontColor, FontHandle)
            End Select
        Next

        If posx >= X And posx <= X + Width And posy >= Y And posy <= Y + Height Then

            If bufinput <> input And input <> 0 Then
                RaiseEvent MouseInput(Me, input)
                MouseInput_var = True
                bufinputbool = True
            End If

            If bufarea = False Then
                RaiseEvent InArea(Me, New Point(posx, posy))
                InArea_var = True
            End If
            bufarea = True
        Else
            bufarea = False
        End If

        If bufinputbool = True And input = 0 Then
            RaiseEvent MouseEndInput(Me)
            MouseEndInput_var = True
            bufinputbool = False
        End If

        bufinput = input

    End Sub

End Class
''' <summary>
''' DxLibFormsのリストボックスのフォーム
''' </summary>
Public Class DxLibListBox

    ''' <summary>
    ''' 値が変わったとき
    ''' </summary>
    ''' <param name="sender">DxLibListBoxのオブジェクト</param>
    ''' <param name="a">値の順番</param>
    ''' <param name="v">値</param>
    Public Event ChangeValue(sender As Object, a As Integer, v As String)
    ''' <summary>
    ''' マウスがエリア内に入ってきたとき
    ''' </summary>
    ''' <param name="sender">DxLibListBoxのオブジェクト</param>
    ''' <param name="e">入ってきたときのマウス座標</param>
    Public Event InArea(sender As Object, e As Point)
    ''' <summary>
    ''' マウスでクリックされたとき
    ''' </summary>
    ''' <param name="sender">DxLibListBoxのオブジェクト</param>
    ''' <param name="e">押されたマウスのボタン</param>
    Public Event MouseInput(sender As Object, e As Integer)
    ''' <summary>
    ''' マウスの入力が終わったとき
    ''' </summary>
    ''' <param name="sender">DxLibListBoxのオブジェクト</param>
    Public Event MouseEndInput(sender As Object)

    Public ChangeValue_var As Boolean
    Public InArea_var As Boolean
    Public MouseInput_var As Boolean
    Public MouseEndInput_var As Boolean

    Const White As UInteger = 4294967295
    Const Gray As UInteger = 4286611584
    Const Black As UInteger = 4278190080
    Const DarkCyan As UInteger = 4278222976

    Property X As Integer
    Property Y As Integer
    Property Width As Integer
    Property Height As Integer
    Property BackGroundColor As UInteger
    Property BorderColor As UInteger
    Property ScrollBar As Integer
    Property FontHandle As Integer
    Property FontColor As UInteger
    Property Value As String
    Property ValueAt As Integer
    Property AutoSize As Boolean
    Property Visible As Boolean

    Public Item As List(Of String)

    Dim SingleTextHeight As Integer

    Dim ScrollHeight As Integer
    Dim ScrollY As Integer

    Dim i As Integer

    Dim DrawFirstAt As Integer

    Dim WaitNum As Integer
    Dim BufInput As Integer
    Dim BufInArea As Boolean
    Dim bufinputbool As Boolean

    Property ScrollBarColor As UInteger
    Property ScrollBarRangeColor As UInteger
    Property SelectColor As UInteger
    ''' <summary>
    ''' 新規オブジェクト
    ''' </summary>
    ''' <param name="Item_lst"></param>
    ''' <param name="X_int"></param>
    ''' <param name="Y_int"></param>
    ''' <param name="Width_int"></param>
    ''' <param name="Height_int"></param>
    ''' <param name="BorderColor_uint"></param>
    ''' <param name="BackGroundColor_uint"></param>
    ''' <param name="FontColor_uint"></param>
    ''' <param name="ScrollBarColor_uint"></param>
    ''' <param name="ScrollBarRangeColor_uint"></param>
    ''' <param name="SelectColor_uint"></param>
    ''' <param name="FontHandle_int"></param>
    ''' <param name="Value_str"></param>
    ''' <param name="ValueAt_int"></param>
    ''' <param name="AutoSize_bool"></param>
    Public Sub New(Item_lst As List(Of String),
                   Optional X_int As Integer = 0,
                   Optional Y_int As Integer = 0,
                   Optional Width_int As Integer = 150,
                   Optional Height_int As Integer = 30,
                   Optional BorderColor_uint As UInteger = Gray,
                   Optional BackGroundColor_uint As UInteger = White,
                   Optional FontColor_uint As UInteger = Black,
                   Optional ScrollBarColor_uint As UInteger = Gray,
                   Optional ScrollBarRangeColor_uint As UInteger = White,
                   Optional SelectColor_uint As UInteger = DarkCyan,
                   Optional FontHandle_int As Integer = -1,
                   Optional Value_str As String = "",
                   Optional ValueAt_int As Integer = -1,
                   Optional AutoSize_bool As Boolean = False,
                   Optional Visible_bool As Boolean = True)

        Visible = Visible_bool
        Item = Item_lst

        X = X_int
        Y = Y_int
        Width = Width_int
        Height = Height_int

        BorderColor = BorderColor_uint
        BackGroundColor = BackGroundColor_uint
        ScrollBarColor = ScrollBarColor_uint
        ScrollBarRangeColor = ScrollBarRangeColor_uint

        FontColor = FontColor_uint

        SelectColor = SelectColor_uint

        If FontHandle_int = -1 Then
            FontHandle = CreateFontToHandle("MS UI Gothic", 24, -1)
        Else
            FontHandle = FontHandle_int
        End If

        Value = ""
        ValueAt = -1

        If Item.IndexOf(Value_str) <> -1 Then
            Value = Value_str
            ValueAt = Item.IndexOf(Value_str)
        End If
        If ValueAt_int <> -1 And Item.Count - 1 <= ValueAt_int Then
            Value = Item(ValueAt_int)
            ValueAt = ValueAt_int
        End If

        AutoSize = AutoSize_bool

    End Sub
    ''' <summary>
    ''' 描画する
    ''' </summary>
    Public Sub Draw()

        If Not Visible Then Exit Sub

        Dim ItemStr() As String = Item.ToArray()

        SingleTextHeight = GetFontSizeToHandle(FontHandle)
        If AutoSize Then
            Height = SingleTextHeight * (UBound(ItemStr) + 1)
        Else
            If Height Mod SingleTextHeight <> 0 Then
                Dim bufheight As Integer = 0
                Do While bufheight <= Height
                    bufheight += SingleTextHeight
                Loop
                Height = bufheight
            End If
        End If

        DrawBox(X, Y, X + Width, Y + Height, BackGroundColor, 1)

        For buf = 0 To UBound(ItemStr)

            i = buf + DrawFirstAt

            If i > UBound(ItemStr) Then Exit For

            If input <> 0 Then
                If posx >= X And posx <= X + Width - 10 And posy >= Y + buf * SingleTextHeight And posy <= Y + (buf + 1) * SingleTextHeight Then
                    If ValueAt <> i Then
                        ChangeValueAt(i)
                    End If
                End If
            End If

            If ValueAt = i Then
                DrawBox(X, Y + buf * SingleTextHeight, X + Width, Y + (buf + 1) * SingleTextHeight, SelectColor, 1)
            End If

            DrawStringToHandle(X, Y + buf * SingleTextHeight, ItemStr(i), FontColor, FontHandle)

            If Height <= SingleTextHeight * (buf + 1) Then
                Exit For
            End If

        Next

        ScrollY = DrawFirstAt / ItemStr.Length * Height
        ScrollHeight = (Height / SingleTextHeight) / ItemStr.Length * Height

        DrawBox(X + Width - 10, Y, X + Width, Y + Height, ScrollBarColor, 1)
        DrawBox(X + Width - 10, Y + ScrollY, X + Width, Y + ScrollY + ScrollHeight, ScrollBarRangeColor, 1)
        DrawBox(X + Width - 10, Y, X + Width, Y + Height, BorderColor, 0)
        DrawBox(X + Width - 10, Y + ScrollY, X + Width, Y + ScrollY + ScrollHeight, BorderColor, 0)

        If posx >= X + Width - 10 And posx <= X + Width + 10 Then
            If posy > Y And posy < Y + Height Then
                If input <> 0 Then
                    If (posy - Y) / Height * ItemStr.Length <= UBound(ItemStr) - (Height / SingleTextHeight) / ItemStr.Length Then
                        DrawFirstAt = (posy - Y) / Height * ItemStr.Length
                    End If
                End If
            End If
        End If

        If posx >= X And posx <= X + Width And posy >= Y And posy <= Y + Height Then

            If input <> BufInput And input <> 0 Then
                RaiseEvent MouseInput(Me, input)
                MouseInput_var = True
                bufinputbool = True
            End If

            If BufInArea = False Then
                RaiseEvent InArea(Me, New Point(posx, posy))
                InArea_var = True
            End If
            BufInArea = True
        Else
            BufInArea = False
        End If

        If bufinputbool = True And input = 0 Then
            RaiseEvent MouseEndInput(Me)
            MouseEndInput_var = True
            bufinputbool = False
        End If

        BufInput = input

        If WaitNum <= 0 Then
            If key(KEY_INPUT_UP) Then
                If ValueAt - 1 >= 0 Then
                    ChangeValueAt(ValueAt - 1)
                End If
                If DrawFirstAt > ValueAt Then
                    DrawFirstAt -= 1
                End If
                WaitNum = 10
            End If
            If key(KEY_INPUT_DOWN) Then
                If ValueAt + 1 <= UBound(ItemStr) Then
                    ChangeValueAt(ValueAt + 1)
                End If
                If DrawFirstAt + (Height / SingleTextHeight) <= ValueAt Then
                    DrawFirstAt += 1
                End If
                WaitNum = 10

            End If
        Else
            WaitNum -= 1
        End If

        DrawBox(X, Y, X + Width, Y + Height, BorderColor, 0)

    End Sub
    Private Sub ChangeValueAt(at As Integer)

        ValueAt = at
        Value = Item(at)

        RaiseEvent ChangeValue(Me, ValueAt, Value)
        ChangeValue_var = True

    End Sub
End Class
''' <summary>
''' DxLibFormsのラベルのフォーム
''' </summary>
Public Class DxLibLabel

    ''' <summary>
    ''' マウスでクリックされたとき
    ''' </summary>
    ''' <param name="sender">DxLibLabelのオブジェクト</param>
    ''' <param name="e">押されたマウスのボタン</param>
    Public Event MouseInput(sender As Object, e As Integer)
    ''' <summary>
    ''' マウスがエリア内に入ってきたとき
    ''' </summary>
    ''' <param name="sender">DxLibLabelのオブジェクト</param>
    ''' <param name="e">入ってきたときのマウス座標</param>
    Public Event InArea(sender As Object, e As Point)
    ''' <summary>
    ''' マウスの入力が終わったとき
    ''' </summary>
    ''' <param name="sender">DxLibLabelのオブジェクト</param>
    Public Event MouseEndInput(sender As Object)

    Const White As UInteger = 4294967295
    Const Gray As UInteger = 4286611584
    Const Black As UInteger = 4278190080

    Property X As Integer
    Property Y As Integer
    Property Width As Integer
    Property Height As Integer
    Property Text As String
    Property TextAlign As Integer
    Property GraphSize As Integer
    Property BackGroundColor As UInteger
    Property BackGroundGraph As Integer
    Property FontColor As UInteger
    Property BorderColor As UInteger
    Property FontHandle As Integer
    Property Trans As Integer
    Property AutoSize As Boolean
    Property BackGroundTrans As Boolean
    Property Visible As Boolean

    Dim TextWidth() As Integer
    Dim TextHeight As Integer
    Dim TextArr() As String
    Dim MostTextWidth As Integer
    Dim SingleTextHeight As Integer
    Dim BufInput As Integer
    Dim PushButton As Boolean
    Dim InBox As Boolean
    Dim bufinputbool As Boolean

    Public MouseInput_var As Boolean
    Public InArea_var As Boolean
    Public MouseEndInput_var As Boolean
    ''' <summary>
    ''' 新規オブジェクト
    ''' </summary>
    ''' <param name="X_int"></param>
    ''' <param name="Y_int"></param>
    ''' <param name="Width_int"></param>
    ''' <param name="Height_int"></param>
    ''' <param name="Text_str"></param>
    ''' <param name="TextAlign_alg"></param>
    ''' <param name="GraphSize_siz"></param>
    ''' <param name="BackGroundColor_uint"></param>
    ''' <param name="BackGroundGraph_int"></param>
    ''' <param name="FontColor_uint"></param>
    ''' <param name="BorderColor_uint"></param>
    ''' <param name="FontHandle_int"></param>
    ''' <param name="Trans_int"></param>
    ''' <param name="AutoSize_bool"></param>
    ''' <param name="BackGroundTrans_bool"></param>
    Public Sub New(Optional X_int As Integer = 0,
                   Optional Y_int As Integer = 0,
                   Optional Width_int As Integer = 100,
                   Optional Height_int As Integer = 30,
                   Optional Text_str As String = "Label",
                   Optional TextAlign_alg As Integer = 0,
                   Optional GraphSize_siz As Integer = 0,
                   Optional BackGroundColor_uint As UInteger = White,
                   Optional BackGroundGraph_int As Integer = -1,
                   Optional FontColor_uint As UInteger = Black,
                   Optional BorderColor_uint As UInteger = Gray,
                   Optional FontHandle_int As Integer = -1,
                   Optional Trans_int As Integer = 0,
                   Optional AutoSize_bool As Boolean = False,
                   Optional BackGroundTrans_bool As Boolean = False,
                   Optional Visible_bool As Boolean = True)

        Visible = Visible_bool
        If FontHandle_int = -1 Then
            FontHandle = CreateFontToHandle("MS UI Gothic", 24, -1)
        Else
            FontHandle = FontHandle_int
        End If

        X = X_int
        Y = Y_int
        Width = Width_int
        Height = Height_int
        Text = Text_str
        TextAlign = TextAlign_alg
        GraphSize = GraphSize_siz
        BackGroundColor = BackGroundColor_uint
        BackGroundGraph = BackGroundGraph_int
        FontColor = FontColor_uint
        BorderColor = BorderColor_uint
        Trans = Trans_int
        AutoSize = AutoSize_bool
        BackGroundTrans = BackGroundTrans_bool

    End Sub
    ''' <summary>
    ''' 描画する
    ''' </summary>
    Public Sub Draw()

        If Not Visible Then Exit Sub
        '文字入力
        If posx >= X And posx <= X + Width And posy >= Y And posy <= Y + Height Then

            If BufInput <> input And input <> 0 Then
                RaiseEvent MouseInput(Me, input)
                MouseInput_var = True
                bufinputbool = True
            End If

            If InBox = False Then
                RaiseEvent InArea(Me, New Point(posx, posy))
                InArea_var = True
            End If
            InBox = True
            If input <> 0 And PushButton = False Then
                PushButton = True
            End If
        Else
            InBox = False
            If input <> 0 Then
                PushButton = False
            End If
        End If

        If bufinputbool = True And input = 0 Then
            RaiseEvent MouseEndInput(Me)
            MouseEndInput_var = True
            bufinputbool = False
        End If

        BufInput = input

        'テキストの位置表示
        MostTextWidth = -1
        TextArr = Regex.Split(Text, "\r\n|\n")
        ReDim TextWidth(UBound(TextArr))
        For i = 0 To UBound(TextArr)
            TextWidth(i) = GetDrawStringWidthToHandle(TextArr(i), -1, FontHandle)
            If TextWidth(i) > MostTextWidth Then MostTextWidth = TextWidth(i)
        Next

        SingleTextHeight = GetFontSizeToHandle(FontHandle)
        TextHeight = SingleTextHeight * (UBound(Text.Split(vbCrLf)) + 1)

        '自動サイズ調整
        If AutoSize Then
            Width = MostTextWidth
            Height = TextHeight
        End If

        'BackGround表示
        If Not BackGroundTrans Then
            DrawBox(X, Y, X + Width, Y + Height, BackGroundColor, 1)
            If BackGroundGraph <> -1 Then
                Select Case GraphSize
                    Case Keep
                        DrawRectGraph(X, Y, 0, 0, Width, Height, BackGroundGraph, Trans)
                    Case Zoom
                        DrawExtendGraph(X, Y, X + Width, Y + Height, BackGroundGraph, Trans)
                End Select
            End If
            DrawBox(X, Y, X + Width, Y + Height, BorderColor, 0)
        End If

        'テキスト位置
        For i = 0 To UBound(TextArr)
            Select Case TextAlign
                Case Left
                    DrawStringToHandle(X, Y + (i * SingleTextHeight), TextArr(i), FontColor, FontHandle)
                Case Center
                    DrawStringToHandle(X + (Width - TextWidth(i)) / 2, Y + (i * SingleTextHeight), TextArr(i), FontColor, FontHandle)
                Case Right
                    DrawStringToHandle(X + Width - TextWidth(i), Y + (i * SingleTextHeight), TextArr(i), FontColor, FontHandle)
            End Select
        Next

    End Sub

End Class
''' <summary>
''' DxLibFormsのテキスト入力のフォーム
''' </summary>
Public Class DxLibTextBox

    ''' <summary>
    ''' テキストが変わったとき
    ''' </summary>
    ''' <param name="sender">DxLibTextBoxのオブジェクト</param>
    ''' <param name="e">テキスト</param>
    Public Event ChangedText(sender As Object, e As String)
    ''' <summary>
    ''' マウスでクリックされた
    ''' </summary>
    ''' <param name="sender">DxLibTextBoxのオブジェクト</param>
    ''' <param name="e">押されたマウスのボタン</param>
    Public Event MouseInput(sender As Object, e As Integer)
    ''' <summary>
    ''' マウスの入力が終わったとき
    ''' </summary>
    ''' <param name="sender">DxLibTextBoxのオブジェクト</param>
    Public Event MouseEndInput(sender As Object)
    ''' <summary>
    ''' マウスがエリア内に入ってきたとき
    ''' </summary>
    ''' <param name="sender">DxLibTextBoxのオブジェクト</param>
    ''' <param name="e">入ってきたときのマウス座標</param>
    Public Event InArea(sender As Object, e As Point)

    Public ChangeText_var As Boolean
    Public MouseInput_var As Boolean
    Public MouseEndInput_var As Boolean
    Public InArea_var As Boolean

    Const White As UInteger = 4294967295
    Const Gray As UInteger = 4286611584
    Const Black As UInteger = 4278190080

    Property X As Integer
    Property Y As Integer
    Property Width As Integer
    Property Height As Integer
    Property Text As String
    Property TextAlign As Integer
    Property GraphSize As Integer
    Property BackGroundColor As UInteger
    Property BackGroundGraph As Integer
    Property FontColor As UInteger
    Property BorderColor As UInteger
    Property FontHandle As Integer
    Property Trans As Integer
    Property AutoSize As Boolean
    Property BackGroundTrans As Boolean
    Property Visible As Boolean

    Dim TextWidth() As Integer
    Dim TextHeight As Integer
    Dim TextArr() As String
    Dim MostTextWidth As Integer
    Dim SingleTextHeight As Integer

    Dim Keys As Integer
    Dim PushButton As Boolean

    Dim InBox As Boolean
    Dim BufInput As Integer
    Dim bufinputbool As Boolean

    Dim st As StringBuilder
    ''' <summary>
    ''' 新規オブジェクト
    ''' </summary>
    ''' <param name="X_int"></param>
    ''' <param name="Y_int"></param>
    ''' <param name="Width_int"></param>
    ''' <param name="Height_int"></param>
    ''' <param name="Text_str"></param>
    ''' <param name="TextAlign_alg"></param>
    ''' <param name="GraphSize_siz"></param>
    ''' <param name="BackGroundColor_uint"></param>
    ''' <param name="BackGroundGraph_int"></param>
    ''' <param name="FontColor_uint"></param>
    ''' <param name="BorderColor_uint"></param>
    ''' <param name="FontHandle_int"></param>
    ''' <param name="Trans_int"></param>
    ''' <param name="AutoSize_bool"></param>
    ''' <param name="BackGroundTrans_bool"></param>
    Public Sub New(Optional X_int As Integer = 0,
                   Optional Y_int As Integer = 0,
                   Optional Width_int As Integer = 100,
                   Optional Height_int As Integer = 30,
                   Optional Text_str As String = "TextBox",
                   Optional TextAlign_alg As Integer = 0,
                   Optional GraphSize_siz As Integer = 0,
                   Optional BackGroundColor_uint As UInteger = White,
                   Optional BackGroundGraph_int As Integer = -1,
                   Optional FontColor_uint As UInteger = Black,
                   Optional BorderColor_uint As UInteger = Gray,
                   Optional FontHandle_int As Integer = -1,
                   Optional Trans_int As Integer = 0,
                   Optional AutoSize_bool As Boolean = False,
                   Optional BackGroundTrans_bool As Boolean = False,
                   Optional Visible_bool As Boolean = True)

        If FontHandle_int = -1 Then
            FontHandle = CreateFontToHandle("MS UI Gothic", 24, -1)
        Else
            FontHandle = FontHandle_int
        End If

        X = X_int
        Y = Y_int
        Width = Width_int
        Height = Height_int
        Text = Text_str
        TextAlign = TextAlign_alg
        GraphSize = GraphSize_siz
        BackGroundColor = BackGroundColor_uint
        BackGroundGraph = BackGroundGraph_int
        FontColor = FontColor_uint
        BorderColor = BorderColor_uint
        Trans = Trans_int
        AutoSize = AutoSize_bool
        BackGroundTrans = BackGroundTrans_bool

        Keys = MakeKeyInput(100, [FALSE], [FALSE], [FALSE])

        st = New StringBuilder
        Visible = Visible_bool

    End Sub
    ''' <summary>
    ''' テキストボックスの最大文字数を設定します。
    ''' </summary>
    ''' <param name="length"></param>
    Public Sub MaxTextLength(length As Integer)
        DeleteKeyInput(Keys)
        MakeKeyInput(length, [FALSE], [FALSE], [FALSE])
    End Sub
    ''' <summary>
    ''' 描画します。
    ''' </summary>
    Public Sub Draw()

        If Not Visible Then Exit Sub

        '文字入力
        If posx >= X And posx <= X + Width And posy >= Y And posy <= Y + Height Then

            If BufInput <> input And input <> 0 Then
                RaiseEvent MouseInput(Me, input)
                MouseInput_var = True
                bufinputbool = True
            End If

            If InBox = False Then
                RaiseEvent InArea(Me, New Point(posx, posy))
                InArea_var = True
            End If
            InBox = True
            If input <> 0 And PushButton = False Then
                PushButton = True
                SetActiveKeyInput(Keys)
                SetKeyInputString(Text, Keys)
            End If
        Else
            InBox = False
            If input <> 0 Then
                PushButton = False
            End If
        End If

        If bufinputbool = True And input = 0 Then
            bufinputbool = False
            RaiseEvent MouseEndInput(Me)
            mouseendinput_var = True
        End If

        If PushButton Then
            DrawKeyInputString(0, 0, Keys)
            GetKeyInputString(st, Keys)
            Text = st.ToString
        End If
        If CheckKeyInput(Keys) <> 0 Then
            If CheckKeyInput(Keys) = 1 Then
                Text &= vbCrLf
                SetActiveKeyInput(Keys)
                SetKeyInputString(Text, Keys)
            Else
                PushButton = False
            End If
        End If

        BufInput = input

        'テキストの位置表示
        MostTextWidth = -1
        TextArr = Regex.Split(Text, "\r\n|\n")
        ReDim TextWidth(UBound(TextArr))
        For i = 0 To UBound(TextArr)
            TextWidth(i) = GetDrawStringWidthToHandle(TextArr(i), -1, FontHandle)
            If TextWidth(i) > MostTextWidth Then MostTextWidth = TextWidth(i)
        Next

        SingleTextHeight = GetFontSizeToHandle(FontHandle)
        TextHeight = SingleTextHeight * (UBound(Text.Split(vbCrLf)) + 1)

        '自動サイズ調整
        If AutoSize Then
            Width = MostTextWidth
            Height = TextHeight
        End If

        'BackGround表示
        If Not BackGroundTrans Then
            DrawBox(X, Y, X + Width, Y + Height, BackGroundColor, 1)
            If BackGroundGraph <> -1 Then
                Select Case GraphSize
                    Case Keep
                        DrawRectGraph(X, Y, 0, 0, Width, Height, BackGroundGraph, Trans)
                    Case Zoom
                        DrawExtendGraph(X, Y, X + Width, Y + Height, BackGroundGraph, Trans)
                End Select
            End If
            DrawBox(X, Y, X + Width, Y + Height, BorderColor, 0)
        End If

        'テキスト位置
        For i = 0 To UBound(TextArr)
            Select Case TextAlign
                Case Left
                    DrawStringToHandle(X, Y + (i * SingleTextHeight), TextArr(i), FontColor, FontHandle)
                Case Center
                    DrawStringToHandle(X + (Width - TextWidth(i)) / 2, Y + (i * SingleTextHeight), TextArr(i), FontColor, FontHandle)
                Case Right
                    DrawStringToHandle(X + Width - TextWidth(i), Y + (i * SingleTextHeight), TextArr(i), FontColor, FontHandle)
            End Select
        Next

    End Sub

End Class
