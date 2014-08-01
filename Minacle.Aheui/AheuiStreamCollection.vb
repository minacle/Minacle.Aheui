Public Class AheuiStreamCollection
  Implements IAheuiStorageCollection(Of Stream)

  Public Sub Add(token As FinalNames) Implements ICollection(Of FinalNames).Add
    collection.Add(token, Stream.Null)
  End Sub

  Public Sub Add(ParamArray tokens As FinalNames()) Implements IAheuiStorageCollection(Of Stream).Add
    For Each token In tokens
      collection.Add(token, Stream.Null)
    Next
  End Sub

  Public Sub Clear() Implements ICollection(Of FinalNames).Clear
    collection.Clear()
  End Sub

  Public Function Contains(token As FinalNames) As Boolean Implements ICollection(Of FinalNames).Contains
    Return collection.Contains(token)
  End Function

  Public Sub CopyTo(array() As FinalNames, arrayIndex As Integer) Implements ICollection(Of FinalNames).CopyTo
    collection.Keys.CopyTo(array, arrayIndex)
  End Sub

  Public ReadOnly Property Count As Integer Implements ICollection(Of FinalNames).Count
    Get
      Return collection.Count
    End Get
  End Property

  Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of FinalNames).IsReadOnly
    Get
      Return collection.IsReadOnly
    End Get
  End Property

  Default Public ReadOnly Property Item(token As FinalNames) As Stream Implements IAheuiStorageCollection(Of Stream).Item
    Get
      Return collection.Item(token)
    End Get
  End Property

  Public Function Remove(token As FinalNames) As Boolean Implements ICollection(Of FinalNames).Remove
    If IsReadOnly Then Throw New NotSupportedException
    Try
      collection.Remove(token)
    Catch
      Return False
    End Try
    Return True
  End Function

  Public Function Remove(ParamArray tokens As FinalNames()) As Boolean Implements IAheuiStorageCollection(Of Stream).Remove
    If IsReadOnly Then Throw New NotSupportedException
    Try
      For Each token In tokens
        collection.Remove(token)
      Next
    Catch
      Return False
    End Try
    Return True
  End Function

  Public Shadows Iterator Function GetEnumerator() As IEnumerator(Of FinalNames) Implements IEnumerable(Of FinalNames).GetEnumerator
    For Each i In collection.Keys
      Yield i
    Next
  End Function

  Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
    Return GetEnumerator()
  End Function

  Private collection As New Hashtable
End Class
