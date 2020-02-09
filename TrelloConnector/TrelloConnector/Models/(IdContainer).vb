
Friend Class IdContainer

  Public Property Id As String

  Public Property Closed As Boolean

End Class

Friend Class IdContainerWithName
  Inherits IdContainer

  Public Property Name As String

End Class

Friend Class IdContainerWithIdBoards
  Inherits IdContainer

  Public Property IdBoards As String()

End Class

Friend Class IdContainerWithIdBoard
  Inherits IdContainer

  Public Property IdBoard As String

End Class

Friend Class IdContainerWithIdCards
  Inherits IdContainer

  Public Property IdCards As String()

End Class

Friend Class IdContainerWithIdList
  Inherits IdContainer

  Public Property IdList As String

End Class


Friend Class IdContainerWithIdLists
  Inherits IdContainer

  Public Property IdLists As String()

End Class

Friend Class IdContainerWithIdLabels
  Inherits IdContainer

  Public Property IdLabels As String()

End Class
