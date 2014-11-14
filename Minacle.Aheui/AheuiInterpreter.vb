Public NotInheritable Class AheuiInterpreter

  Private Shared ReadOnly hangulBase As Integer = &HAC00
  Private Shared ReadOnly hangulLength As Integer = &H2BA4
  Private Shared ReadOnly hangulInitialsCount As Integer = 19
  Private Shared ReadOnly hangulMedialsCount As Integer = 21
  Private Shared ReadOnly hangulFinalsCount As Integer = 28
  Private Shared ReadOnly hangulFinalsStrokesCount As Integer() = New Integer() {0, 2, 4, 4, 2, 5, 5, 3, 5, 7, 9, 9, 7, 9, 9, 8, 4, 4, 6, 2, 4, 1, 3, 4, 3, 4, 4, 3}

  Private _In As TextReader = Console.In
  Private _Out As TextWriter = Console.Out
  Private _Stacks As New AheuiStackCollection
  Private _Queues As New AheuiQueueCollection
  Private _Streams As New AheuiStreamCollection
  Private _Cursor(1) As Integer
  Private _CodeSize(1) As Integer
  Private _CodeMap As String()
  Private _Direction As MedialNames = MedialNames.U
  Private _DirectionHistory As New Stack(Of MedialNames)
  Private _MoveHistory As New Stack(Of Integer())
  Private _IsFinished As Boolean
  Private _Ticks As Long
  Private _ExitCode As Integer
  Private _LastWrittenValue As Integer
  Private _ValueWritten As Boolean

  Private Property Commands As New Dictionary(Of InitialNames, AheuiCommand)

  Public Property SelectedStorageToken As FinalNames
  Public Property IsClosingOldInWhenReplace As Boolean = True
  Public Property IsClosingOldOutWhenReplace As Boolean = True

  Public Property [In] As TextReader
    Get
      Return _In
    End Get
    Set(value As TextReader)
      If IsClosingOldInWhenReplace AndAlso Not _In.Equals(Console.In) Then
        Try
          _In.Close()
        Catch
        End Try
      End If
      _In = value
    End Set
  End Property

  Public Property Out As TextWriter
    Get
      Return _Out
    End Get
    Set(value As TextWriter)
      If IsClosingOldOutWhenReplace AndAlso Not _Out.Equals(Console.Out) Then
        Try
          _Out.Close()
        Catch
        End Try
      End If
      _Out = value
    End Set
  End Property

  Public ReadOnly Property Stacks As AheuiStackCollection
    Get
      Return _Stacks
    End Get
  End Property

  Public ReadOnly Property Queues As AheuiQueueCollection
    Get
      Return _Queues
    End Get
  End Property

  Public ReadOnly Property Streams As AheuiStreamCollection
    Get
      Return _Streams
    End Get
  End Property

  Public ReadOnly Property Codes As String
    Get
      Return String.Join(Environment.NewLine, _CodeMap)
    End Get
  End Property

  Public Property X As Integer
    Get
      Return _Cursor(0)
    End Get
    Set(value As Integer)
      If _CodeMap Is Nothing Then Exit Property
      Dim max = _CodeSize(0)
      Do While value < 0
        value += max
      Loop
      Do While value > max
        value -= max
      Loop
      _Cursor(0) = value
      _MoveHistory.Push(_Cursor)
    End Set
  End Property

  Public Property Y As Integer
    Get
      Return _Cursor(1)
    End Get
    Set(value As Integer)
      If _CodeMap Is Nothing Then Exit Property
      Dim max = _CodeSize(1)
      Do While value < 0
        value += max
      Loop
      Do While value > max
        value -= max
      Loop
      _Cursor(1) = value
      _MoveHistory.Push(_Cursor)
    End Set
  End Property

  Public ReadOnly Property IsFinished As Boolean
    Get
      Return _IsFinished
    End Get
  End Property

  Public ReadOnly Property Ticks As Long
    Get
      Return _Ticks
    End Get
  End Property

  Public ReadOnly Property ExitCode As Integer
    Get
      Return _ExitCode
    End Get
  End Property

  <EditorBrowsable(EditorBrowsableState.Advanced)>
  Public Property LastWrittenValue As Integer
    Get
      Return _LastWrittenValue
    End Get
    Set(value As Integer)
      _LastWrittenValue = value
      _ValueWritten = True
    End Set
  End Property

  <EditorBrowsable(EditorBrowsableState.Advanced)>
  Public ReadOnly Property ValueWritten As Boolean
    Get
      Return _ValueWritten
    End Get
  End Property

  Public Sub New()
    RegisterCommand(InitialNames.Nieun, New AheuiDivideCommand(Me))
    RegisterCommand(InitialNames.Tikeut, New AheuiAddCommand(Me))
    RegisterCommand(InitialNames.SsangTikeut, New AheuiMultiplyCommand(Me))
    RegisterCommand(InitialNames.Rieul, New AheuiModuloCommand(Me))
    RegisterCommand(InitialNames.Mieum, New AheuiPopCommand(Me))
    RegisterCommand(InitialNames.Pieup, New AheuiPushCommand(Me))
    RegisterCommand(InitialNames.SsangPieup, New AheuiDuplicateCommand(Me))
    RegisterCommand(InitialNames.Sios, New AheuiSelectCommand(Me))
    RegisterCommand(InitialNames.SsangSios, New AheuiMoveCommand(Me))
    RegisterCommand(InitialNames.Ieung, New AheuiPassCommand(Me))
    RegisterCommand(InitialNames.Cieuc, New AheuiCompareCommand(Me))
    RegisterCommand(InitialNames.Chieuch, New AheuiEvaluateCommand(Me))
    RegisterCommand(InitialNames.Thieuth, New AheuiSubtractCommand(Me))
    RegisterCommand(InitialNames.Phieuph, New AheuiSwapCommand(Me))
    RegisterCommand(InitialNames.Hieuh, New AheuiEndCommand(Me))
    Stacks.Add(0, FinalNames.Kiyeok, FinalNames.SsangKiyeok, FinalNames.KiyeokSios, FinalNames.Nieun, FinalNames.NieunCieuc, FinalNames.NieunHieuh, FinalNames.Tikeut, FinalNames.Rieul, FinalNames.RieulKiyeok, FinalNames.RieulMieum, FinalNames.RieulPieup, FinalNames.RieulSios, FinalNames.RieulThieuth, FinalNames.RieulPhieuph, FinalNames.RieulHieuh, FinalNames.Mieum, FinalNames.Pieup, FinalNames.PieupSios, FinalNames.Sios, FinalNames.SsangSios, FinalNames.Cieuc, FinalNames.Chieuch, FinalNames.Khieukh, FinalNames.Thieuth, FinalNames.Phieuph)
    Queues.Add(FinalNames.Ieung)
    Streams.Add(FinalNames.Hieuh)
  End Sub

  Public Sub New(text As String)
    Me.New()
    [In] = New StringReader(text)
    ReadToEnd()
  End Sub

  Public Sub New(stream As Stream)
    Me.New()
    [In] = New StreamReader(stream)
    ReadToEnd()
  End Sub

  Public Sub New(reader As TextReader)
    Me.New()
    [In] = reader
    ReadToEnd()
  End Sub

  Public Sub RegisterCommand(token As InitialNames, command As AheuiCommand)
    Commands.Add(token, command)
  End Sub

  Public Sub UnregisterCommand(token As InitialNames)
    Commands.Remove(token)
  End Sub

  Public Sub ReadToEnd()
    With [In]
      Do Until .Peek < 0
        Dim line = .ReadLine
        _CodeSize(1) += 1
        If _CodeMap Is Nothing Then
          _CodeMap = Array.CreateInstance(GetType(String), 1)
        Else
          Array.Resize(_CodeMap, _CodeMap.Length + 1)
        End If
        If _CodeSize(0) < line.Length Then _CodeSize(0) = line.Length
        _CodeMap(_CodeMap.Length - 1) = line
      Loop
    End With
  End Sub

  Public Sub Reset()
    _Cursor(0) = 0
    _Cursor(1) = 0
    _Direction = MedialNames.U
    _DirectionHistory = New Stack(Of MedialNames)
    _MoveHistory = New Stack(Of Integer())
    For Each token In Stacks
      Stacks(token).Clear()
    Next
    For Each token In Queues
      Queues(token).Clear()
    Next
    _IsFinished = False
  End Sub

  Public Sub Run()
    Do Until IsFinished
      [Step]()
    Loop
  End Sub

  Public Sub [Step]()
    If Ticks > 0 Then Move()
    Dim l = _CodeMap(Y)
    Dim v As Integer
    If l.Length > X Then
      v = AscW(l(X))
    Else
      v = 0
    End If
    If v >= hangulBase AndAlso v <= hangulBase + hangulLength Then
      v -= hangulBase
      Dim i = v \ (hangulMedialsCount * hangulFinalsCount)
      v = v Mod (hangulMedialsCount * hangulFinalsCount)
      Dim m = v \ hangulFinalsCount
      Dim f = v Mod hangulFinalsCount
      If Commands.Keys.Contains(i) Then
        Commands(i).Invoke(i, m, f)
      End If
      _Direction = m
    End If
    _Ticks += 1
  End Sub

  Private Sub Move()
    Select Case _Direction
      Case MedialNames.A
        X += 1
      Case MedialNames.Ya
        X += 2
      Case MedialNames.Eo
        X -= 1
      Case MedialNames.Yeo
        X -= 2
      Case MedialNames.O
        Y -= 1
      Case MedialNames.Yo
        Y -= 2
      Case MedialNames.U
        Y += 1
      Case MedialNames.Yu
        Y += 2
      Case MedialNames.Eu
        Select Case _DirectionHistory.LastOrDefault
          Case MedialNames.A
            X += 1
          Case MedialNames.Ya
            X += 2
          Case MedialNames.Eo
            X -= 1
          Case MedialNames.Yeo
            X -= 2
          Case Else
            If _MoveHistory.Count > 0 Then
              _Cursor = _MoveHistory.Pop
            Else
              _Cursor(0) = 0
              _Cursor(1) = 0
            End If
        End Select
      Case MedialNames.Yi
        If _MoveHistory.Count > 0 Then
          _Cursor = _MoveHistory.Pop
        Else
          _Cursor(0) = 0
          _Cursor(1) = 0
        End If
      Case MedialNames.I
        Select Case _DirectionHistory.LastOrDefault
          Case MedialNames.O
            Y -= 1
          Case MedialNames.Yo
            Y -= 2
          Case MedialNames.U
            Y += 1
          Case MedialNames.Yu
            Y += 2
          Case Else
            If _MoveHistory.Count > 0 Then
              _Cursor = _MoveHistory.Pop
            Else
              _Cursor(0) = 0
              _Cursor(1) = 0
            End If
        End Select
      Case Else
        If _DirectionHistory.Count > 0 Then
          _Direction = _DirectionHistory.Pop
        Else
          _Direction = MedialNames.U
        End If
        Move()
        Exit Sub
    End Select
    _DirectionHistory.Push(_Direction)
  End Sub

  Public Sub ReverseDirection()
    Select Case _Direction
      Case MedialNames.A
        _Direction = MedialNames.Eo
      Case MedialNames.Ya
        _Direction = MedialNames.Yeo
      Case MedialNames.Eo
        _Direction = MedialNames.A
      Case MedialNames.Yeo
        _Direction = MedialNames.Ya
      Case MedialNames.O
        _Direction = MedialNames.U
      Case MedialNames.Yo
        _Direction = MedialNames.Yu
      Case MedialNames.U
        _Direction = MedialNames.O
      Case MedialNames.Yu
        _Direction = MedialNames.Yo
    End Select
  End Sub

  Public Function GetStorage(token As FinalNames) As Object
    If Stacks.Contains(token) Then
      Return Stacks(token)
    ElseIf Queues.Contains(token) Then
      Return Queues(token)
    ElseIf Streams.Contains(token) Then
      Return Streams(token)
    End If
    Return Nothing
  End Function

  <EditorBrowsable(EditorBrowsableState.Advanced)>
  Public Function GetStorage(token As FinalNames, ByRef resultType As Type) As Object
    If Stacks.Contains(token) Then
      resultType = GetType(Stack(Of Integer))
      Return Stacks(token)
    ElseIf Queues.Contains(token) Then
      resultType = GetType(Queue(Of Integer))
      Return Queues(token)
    ElseIf Streams.Contains(token) Then
      resultType = GetType(Stream)
      Return Streams(token)
    End If
    resultType = Nothing
    Return Nothing
  End Function

  Friend Sub SetAsFinished(exitCode As Integer)
    _ExitCode = exitCode
    _IsFinished = True
  End Sub

  Public Shared Function GetValue(f As FinalNames) As Integer
    Return hangulFinalsStrokesCount(f)
  End Function

  <EditorBrowsable(EditorBrowsableState.Advanced)>
  Public Shared Function GetReversedMedial(medial As MedialNames) As MedialNames
    Select Case medial
      Case MedialNames.A
        Return MedialNames.Eo
      Case MedialNames.Ya
        Return MedialNames.Yeo
      Case MedialNames.Eo
        Return MedialNames.A
      Case MedialNames.Yeo
        Return MedialNames.Ya
      Case MedialNames.O
        Return MedialNames.U
      Case MedialNames.Yo
        Return MedialNames.Yu
      Case MedialNames.U
        Return MedialNames.O
      Case MedialNames.Yu
        Return MedialNames.Yo
    End Select
    Return medial
  End Function
End Class
