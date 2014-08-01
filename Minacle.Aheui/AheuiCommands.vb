<EditorBrowsable(EditorBrowsableState.Advanced)>
Public MustInherit Class AheuiCommand

  Protected Property Interpreter As AheuiInterpreter

  Public Sub New(interpreter As AheuiInterpreter)
    Me.Interpreter = interpreter
  End Sub

  Public MustOverride Sub Invoke(ByRef initial As InitialNames, ByRef medial As MedialNames, ByRef final As FinalNames)
End Class

<EditorBrowsable(EditorBrowsableState.Advanced)>
Public NotInheritable Class AheuiPassCommand
  Inherits AheuiCommand

  Public Sub New(interpreter As AheuiInterpreter)
    MyBase.New(interpreter)
  End Sub

  Public Overrides Sub Invoke(ByRef initial As InitialNames, ByRef medial As MedialNames, ByRef final As FinalNames)
  End Sub
End Class

<EditorBrowsable(EditorBrowsableState.Advanced)>
Public NotInheritable Class AheuiEndCommand
  Inherits AheuiCommand

  Public Sub New(interpreter As AheuiInterpreter)
    MyBase.New(interpreter)
  End Sub

  Public Overrides Sub Invoke(ByRef initial As InitialNames, ByRef medial As MedialNames, ByRef final As FinalNames)
    With Interpreter
      Dim v As Integer
      If .Stacks.Contains(.SelectedStorageToken) Then
        Dim s = .Stacks(.SelectedStorageToken)
        If s.Count >= 1 Then v = s.Pop
      ElseIf .Queues.Contains(.SelectedStorageToken) Then
        Dim q = .Queues(.SelectedStorageToken)
        If q.Count >= 1 Then v = q.Dequeue
      ElseIf .Streams.Contains(.SelectedStorageToken) Then
        Dim s = .Streams(.SelectedStorageToken)
        Using r As New BinaryReader(s, Encoding.UTF8, True)
          v = r.ReadInt32
        End Using
      End If
      .SetAsFinished(v)
    End With
  End Sub
End Class

<EditorBrowsable(EditorBrowsableState.Advanced)>
Public NotInheritable Class AheuiAddCommand
  Inherits AheuiCommand

  Public Sub New(interpreter As AheuiInterpreter)
    MyBase.New(interpreter)
  End Sub

  Public Overrides Sub Invoke(ByRef initial As InitialNames, ByRef medial As MedialNames, ByRef final As FinalNames)
    With Interpreter
      If .Stacks.Contains(.SelectedStorageToken) Then
        Dim s = .Stacks(.SelectedStorageToken)
        If s.Count < 2 Then
          medial = AheuiInterpreter.GetReversedMedial(medial)
          Exit Sub
        End If
        s.Push(s.Pop + s.Pop)
      ElseIf .Queues.Contains(.SelectedStorageToken) Then
        Dim q = .Queues(.SelectedStorageToken)
        If q.Count < 2 Then
          medial = AheuiInterpreter.GetReversedMedial(medial)
          Exit Sub
        End If
        q.Enqueue(q.Dequeue + q.Dequeue)
      ElseIf .Streams.Contains(.SelectedStorageToken) Then
        Dim s = .Streams(.SelectedStorageToken)
        Dim a, b, c As Integer
        Using r As New BinaryReader(s, Encoding.UTF8, True)
          b = r.ReadInt32
          a = r.ReadInt32
        End Using
        Using w As New BinaryWriter(s, Encoding.UTF8, True)
          c = a + b
          w.Write(c)
          .LastWrittenValue = c
        End Using
      End If
    End With
  End Sub
End Class

<EditorBrowsable(EditorBrowsableState.Advanced)>
Public NotInheritable Class AheuiMultiplyCommand
  Inherits AheuiCommand

  Public Sub New(interpreter As AheuiInterpreter)
    MyBase.New(interpreter)
  End Sub

  Public Overrides Sub Invoke(ByRef initial As InitialNames, ByRef medial As MedialNames, ByRef final As FinalNames)
    With Interpreter
      If .Stacks.Contains(.SelectedStorageToken) Then
        Dim s = .Stacks(.SelectedStorageToken)
        If s.Count < 2 Then
          medial = AheuiInterpreter.GetReversedMedial(medial)
          Exit Sub
        End If
        s.Push(s.Pop * s.Pop)
      ElseIf .Queues.Contains(.SelectedStorageToken) Then
        Dim q = .Queues(.SelectedStorageToken)
        If q.Count < 2 Then
          medial = AheuiInterpreter.GetReversedMedial(medial)
          Exit Sub
        End If
        q.Enqueue(q.Dequeue * q.Dequeue)
      ElseIf .Streams.Contains(.SelectedStorageToken) Then
        Dim s = .Streams(.SelectedStorageToken)
        Dim a, b As Integer
        Using r As New BinaryReader(s, Encoding.UTF8, True)
          a = r.ReadInt32
          b = r.ReadInt32
        End Using
        Using w As New BinaryWriter(s, Encoding.UTF8, True)
          Dim c = a * b
          w.Write(c)
          .LastWrittenValue = c
        End Using
      End If
    End With
  End Sub
End Class

<EditorBrowsable(EditorBrowsableState.Advanced)>
Public NotInheritable Class AheuiSubtractCommand
  Inherits AheuiCommand

  Public Sub New(interpreter As AheuiInterpreter)
    MyBase.New(interpreter)
  End Sub

  Public Overrides Sub Invoke(ByRef initial As InitialNames, ByRef medial As MedialNames, ByRef final As FinalNames)
    With Interpreter
      If .Stacks.Contains(.SelectedStorageToken) Then
        Dim s = .Stacks(.SelectedStorageToken)
        If s.Count < 2 Then
          medial = AheuiInterpreter.GetReversedMedial(medial)
          Exit Sub
        End If
        Dim b = s.Pop
        Dim a = s.Pop
        s.Push(a - b)
      ElseIf .Queues.Contains(.SelectedStorageToken) Then
        Dim q = .Queues(.SelectedStorageToken)
        If q.Count < 2 Then
          medial = AheuiInterpreter.GetReversedMedial(medial)
          Exit Sub
        End If
        Dim b = q.Dequeue
        Dim a = q.Dequeue
        q.Enqueue(a - b)
      ElseIf .Streams.Contains(.SelectedStorageToken) Then
        Dim s = .Streams(.SelectedStorageToken)
        Dim a, b As Integer
        Using r As New BinaryReader(s, Encoding.UTF8, True)
          b = r.ReadInt32
          a = r.ReadInt32
        End Using
        Using w As New BinaryWriter(s, Encoding.UTF8, True)
          Dim c = a - b
          w.Write(c)
          .LastWrittenValue = c
        End Using
      End If
    End With
  End Sub
End Class

<EditorBrowsable(EditorBrowsableState.Advanced)>
Public NotInheritable Class AheuiDivideCommand
  Inherits AheuiCommand

  Public Sub New(interpreter As AheuiInterpreter)
    MyBase.New(interpreter)
  End Sub

  Public Overrides Sub Invoke(ByRef initial As InitialNames, ByRef medial As MedialNames, ByRef final As FinalNames)
    With Interpreter
      If .Stacks.Contains(.SelectedStorageToken) Then
        Dim s = .Stacks(.SelectedStorageToken)
        If s.Count < 2 Then
          medial = AheuiInterpreter.GetReversedMedial(medial)
          Exit Sub
        End If
        Dim b = s.Pop
        Dim a = s.Pop
        s.Push(Convert.ToInt32(a / b))
      ElseIf .Queues.Contains(.SelectedStorageToken) Then
        Dim q = .Queues(.SelectedStorageToken)
        If q.Count < 2 Then
          medial = AheuiInterpreter.GetReversedMedial(medial)
          Exit Sub
        End If
        Dim b = q.Dequeue
        Dim a = q.Dequeue
        q.Enqueue(Convert.ToInt32(a / b))
      ElseIf .Streams.Contains(.SelectedStorageToken) Then
        Dim s = .Streams(.SelectedStorageToken)
        Dim a, b As Integer
        Using r As New BinaryReader(s, Encoding.UTF8, True)
          b = r.ReadInt32
          a = r.ReadInt32
        End Using
        Using w As New BinaryWriter(s, Encoding.UTF8, True)
          Dim c = Convert.ToInt32(a / b)
          w.Write(c)
          .LastWrittenValue = c
        End Using
      End If
    End With
  End Sub
End Class

<EditorBrowsable(EditorBrowsableState.Advanced)>
Public NotInheritable Class AheuiModuloCommand
  Inherits AheuiCommand

  Public Sub New(interpreter As AheuiInterpreter)
    MyBase.New(interpreter)
  End Sub

  Public Overrides Sub Invoke(ByRef initial As InitialNames, ByRef medial As MedialNames, ByRef final As FinalNames)
    With Interpreter
      If .Stacks.Contains(.SelectedStorageToken) Then
        Dim s = .Stacks(.SelectedStorageToken)
        If s.Count < 2 Then
          medial = AheuiInterpreter.GetReversedMedial(medial)
          Exit Sub
        End If
        Dim b = s.Pop
        Dim a = s.Pop
        s.Push(Convert.ToInt32(a Mod b))
      ElseIf .Queues.Contains(.SelectedStorageToken) Then
        Dim q = .Queues(.SelectedStorageToken)
        If q.Count < 2 Then
          medial = AheuiInterpreter.GetReversedMedial(medial)
          Exit Sub
        End If
        Dim b = q.Dequeue
        Dim a = q.Dequeue
        q.Enqueue(Convert.ToInt32(a Mod b))
      ElseIf .Streams.Contains(.SelectedStorageToken) Then
        Dim s = .Streams(.SelectedStorageToken)
        Dim a, b As Integer
        Using r As New BinaryReader(s, Encoding.UTF8, True)
          b = r.ReadInt32
          a = r.ReadInt32
        End Using
        Using w As New BinaryWriter(s, Encoding.UTF8, True)
          Dim c = Convert.ToInt32(a Mod b)
          w.Write(c)
          .LastWrittenValue = c
        End Using
      End If
    End With
  End Sub
End Class

<EditorBrowsable(EditorBrowsableState.Advanced)>
Public NotInheritable Class AheuiPopCommand
  Inherits AheuiCommand

  Public Sub New(interpreter As AheuiInterpreter)
    MyBase.New(interpreter)
  End Sub

  Public Overrides Sub Invoke(ByRef initial As InitialNames, ByRef medial As MedialNames, ByRef final As FinalNames)
    With Interpreter
      Dim v As Integer = Nothing
      Dim e As Boolean = False
      If .Stacks.Contains(.SelectedStorageToken) Then
        Dim s = .Stacks(.SelectedStorageToken)
        If s.Count < 1 Then
          medial = AheuiInterpreter.GetReversedMedial(medial)
          Exit Sub
        End If
        v = s.Pop
        e = True
      ElseIf .Queues.Contains(.SelectedStorageToken) Then
        Dim q = .Queues(.SelectedStorageToken)
        If q.Count < 1 Then
          medial = AheuiInterpreter.GetReversedMedial(medial)
          Exit Sub
        End If
        v = q.Dequeue
        e = True
      ElseIf .Streams.Contains(.SelectedStorageToken) Then
        Dim s = .Streams(.SelectedStorageToken)
        Using r As New BinaryReader(s, Encoding.UTF8, True)
          v = r.ReadInt32
        End Using
        e = True
      End If
      If e Then
        Select Case final
          Case FinalNames.Ieung
            .Out.Write(v)
          Case FinalNames.Hieuh
            .Out.Write(ChrW(v))
        End Select
      End If
    End With
  End Sub
End Class

<EditorBrowsable(EditorBrowsableState.Advanced)>
Public NotInheritable Class AheuiPushCommand
  Inherits AheuiCommand

  Public Sub New(interpreter As AheuiInterpreter)
    MyBase.New(interpreter)
  End Sub

  Public Overrides Sub Invoke(ByRef initial As InitialNames, ByRef medial As MedialNames, ByRef final As FinalNames)
    With Interpreter
      If .Stacks.Contains(.SelectedStorageToken) Then
        Dim s = .Stacks(.SelectedStorageToken)
        s.Push(AheuiInterpreter.GetValue(final))
      ElseIf .Queues.Contains(.SelectedStorageToken) Then
        Dim q = .Queues(.SelectedStorageToken)
        q.Enqueue(AheuiInterpreter.GetValue(final))
      ElseIf .Streams.Contains(.SelectedStorageToken) Then
        Dim s = .Streams(.SelectedStorageToken)
        Using w As New BinaryWriter(s, Encoding.UTF8, True)
          Dim v = AheuiInterpreter.GetValue(final)
          w.Write(v)
          .LastWrittenValue = v
        End Using
      End If
      If final = FinalNames.Ieung OrElse final = FinalNames.Hieuh Then
        Do Until .In.Peek > 0
          Thread.Sleep(0)
        Loop
        Select Case final
          Case FinalNames.Ieung
            Dim v = .In.ReadLine
            Dim r As Integer
            If Integer.TryParse(v, r) Then
              .Out.Write(r)
            Else
              .Out.Write(0)
            End If
          Case FinalNames.Hieuh
            Dim v = .In.Read
            .Out.Write(ChrW(v))
        End Select
      End If
    End With
  End Sub
End Class

<EditorBrowsable(EditorBrowsableState.Advanced)>
Public NotInheritable Class AheuiDuplicateCommand
  Inherits AheuiCommand

  Public Sub New(interpreter As AheuiInterpreter)
    MyBase.New(interpreter)
  End Sub

  Public Overrides Sub Invoke(ByRef initial As InitialNames, ByRef medial As MedialNames, ByRef final As FinalNames)
    With Interpreter
      If .Stacks.Contains(.SelectedStorageToken) Then
        Dim s = .Stacks(.SelectedStorageToken)
        If s.Count < 1 Then
          medial = AheuiInterpreter.GetReversedMedial(medial)
          Exit Sub
        End If
        s.Push(s.Peek)
      ElseIf .Queues.Contains(.SelectedStorageToken) Then
        Dim q = .Queues(.SelectedStorageToken)
        If q.Count < 1 Then
          medial = AheuiInterpreter.GetReversedMedial(medial)
          Exit Sub
        End If
        q.Enqueue(q.Peek)
      ElseIf .Streams.Contains(.SelectedStorageToken) Then
        Dim s = .Streams(.SelectedStorageToken)
        Using w As New BinaryWriter(s, Encoding.UTF8, True)
          w.Write(.LastWrittenValue)
        End Using
      End If
    End With
  End Sub
End Class

<EditorBrowsable(EditorBrowsableState.Advanced)>
Public NotInheritable Class AheuiSwapCommand
  Inherits AheuiCommand

  Public Sub New(interpreter As AheuiInterpreter)
    MyBase.New(interpreter)
  End Sub

  Public Overrides Sub Invoke(ByRef initial As InitialNames, ByRef medial As MedialNames, ByRef final As FinalNames)
    With Interpreter
      If .Stacks.Contains(.SelectedStorageToken) Then
        Dim s = .Stacks(.SelectedStorageToken)
        If s.Count < 2 Then
          medial = AheuiInterpreter.GetReversedMedial(medial)
          Exit Sub
        End If
        Dim a, b As Integer
        a = s.Pop
        b = s.Pop
        s.Push(a)
        s.Push(b)
      ElseIf .Queues.Contains(.SelectedStorageToken) Then
        Dim q = .Queues(.SelectedStorageToken)
        If q.Count < 2 Then
          medial = AheuiInterpreter.GetReversedMedial(medial)
          Exit Sub
        End If
        Dim a, b As Integer
        b = q.Dequeue
        a = q.Dequeue
        q.Enqueue(a)
        q.Enqueue(b)
      End If
    End With
  End Sub
End Class

<EditorBrowsable(EditorBrowsableState.Advanced)>
Public NotInheritable Class AheuiSelectCommand
  Inherits AheuiCommand

  Public Sub New(interpreter As AheuiInterpreter)
    MyBase.New(interpreter)
  End Sub

  Public Overrides Sub Invoke(ByRef initial As InitialNames, ByRef medial As MedialNames, ByRef final As FinalNames)
    With Interpreter
      .SelectedStorageToken = final
    End With
  End Sub
End Class

<EditorBrowsable(EditorBrowsableState.Advanced)>
Public NotInheritable Class AheuiMoveCommand
  Inherits AheuiCommand

  Public Sub New(interpreter As AheuiInterpreter)
    MyBase.New(interpreter)
  End Sub

  Public Overrides Sub Invoke(ByRef initial As InitialNames, ByRef medial As MedialNames, ByRef final As FinalNames)
    With Interpreter
      Dim v As Integer = Nothing
      Dim e As Boolean = False
      If .Stacks.Contains(.SelectedStorageToken) Then
        Dim s = .Stacks(.SelectedStorageToken)
        If s.Count < 1 Then
          medial = AheuiInterpreter.GetReversedMedial(medial)
          Exit Sub
        End If
        v = s.Pop
        e = True
      ElseIf .Queues.Contains(.SelectedStorageToken) Then
        Dim q = .Queues(.SelectedStorageToken)
        If q.Count < 1 Then
          medial = AheuiInterpreter.GetReversedMedial(medial)
          Exit Sub
        End If
        v = q.Dequeue
        e = True
      ElseIf .Streams.Contains(.SelectedStorageToken) Then
        Dim s = .Streams(.SelectedStorageToken)
        Using r As New BinaryReader(s, Encoding.UTF8, True)
          v = r.ReadInt32
        End Using
        e = True
      End If
      If e Then
        If .Stacks.Contains(final) Then
          Dim s = .Stacks(final)
          s.Push(v)
        ElseIf .Queues.Contains(final) Then
          Dim q = .Queues(final)
          q.Enqueue(v)
        ElseIf .Streams.Contains(final) Then
          Dim s = .Streams(final)
          Using w As New BinaryWriter(s, Encoding.UTF8, True)
            w.Write(v)
          End Using
        End If
      End If
    End With
  End Sub
End Class

<EditorBrowsable(EditorBrowsableState.Advanced)>
Public NotInheritable Class AheuiCompareCommand
  Inherits AheuiCommand

  Public Sub New(interpreter As AheuiInterpreter)
    MyBase.New(interpreter)
  End Sub

  Public Overrides Sub Invoke(ByRef initial As InitialNames, ByRef medial As MedialNames, ByRef final As FinalNames)
    With Interpreter
      If .Stacks.Contains(.SelectedStorageToken) Then
        Dim s = .Stacks(.SelectedStorageToken)
        If s.Count < 2 Then
          medial = AheuiInterpreter.GetReversedMedial(medial)
          Exit Sub
        End If
        Dim a = s.Pop
        Dim b = s.Pop
        s.Push(If(a <= b, 1, 0))
      ElseIf .Queues.Contains(.SelectedStorageToken) Then
        Dim q = .Queues(.SelectedStorageToken)
        If q.Count < 2 Then
          medial = AheuiInterpreter.GetReversedMedial(medial)
          Exit Sub
        End If
        Dim a = q.Dequeue
        Dim b = q.Dequeue
        q.Enqueue(If(a <= b, 1, 0))
      ElseIf .Streams.Contains(.SelectedStorageToken) Then
        Dim s = .Streams(.SelectedStorageToken)
        Dim a, b, c As Integer
        Using r As New BinaryReader(s, Encoding.UTF8, True)
          a = r.ReadInt32
          b = r.ReadInt32
        End Using
        Using w As New BinaryWriter(s, Encoding.UTF8, True)
          c = If(a <= b, 1, 0)
          w.Write(c)
          .LastWrittenValue = c
        End Using
      End If
    End With
  End Sub
End Class

<EditorBrowsable(EditorBrowsableState.Advanced)>
Public NotInheritable Class AheuiEvaluateCommand
  Inherits AheuiCommand

  Public Sub New(interpreter As AheuiInterpreter)
    MyBase.New(interpreter)
  End Sub

  Public Overrides Sub Invoke(ByRef initial As InitialNames, ByRef medial As MedialNames, ByRef final As FinalNames)
    With Interpreter
      Dim v As Integer = Nothing
      If .Stacks.Contains(.SelectedStorageToken) Then
        Dim s = .Stacks(.SelectedStorageToken)
        If s.Count < 1 Then
          v = 0
        End If
        v = s.Pop
      ElseIf .Queues.Contains(.SelectedStorageToken) Then
        Dim q = .Queues(.SelectedStorageToken)
        If q.Count < 1 Then
          v = 0
        End If
        v = q.Dequeue
      ElseIf .Streams.Contains(.SelectedStorageToken) Then
        Dim s = .Streams(.SelectedStorageToken)
        Using r As New BinaryReader(s, Encoding.UTF8, True)
          v = r.ReadInt32
        End Using
      End If
      If v = 0 Then
        medial = AheuiInterpreter.GetReversedMedial(medial)
      End If
    End With
  End Sub
End Class
