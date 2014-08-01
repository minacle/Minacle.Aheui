Public Interface IAheuiStorageCollection(Of T)
  Inherits ICollection(Of FinalNames)

  Overloads Sub Add(ParamArray tokens As FinalNames())
  Overloads Function Remove(ParamArray tokens As FinalNames()) As Boolean
  ReadOnly Property Item(token As FinalNames) As T
End Interface
