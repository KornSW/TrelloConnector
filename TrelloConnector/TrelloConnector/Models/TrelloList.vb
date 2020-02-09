Imports System
Imports System.Diagnostics
Imports KornSW.TrelloConnector

<DebuggerDisplay("List {Id} ('{Name}')")>
Friend Class LoadedTrelloList
  Inherits TrelloListContent
  Implements IPersistentTrelloList

  Public Property Id As String
  <DebuggerBrowsable(DebuggerBrowsableState.Never)>
  Private ReadOnly Property IPersistentTrelloList_Id As String Implements IPersistentTrelloList.Id
    Get
      Return Me.Id
    End Get
  End Property

  <DebuggerBrowsable(DebuggerBrowsableState.Never)>
  Public Property Closed As Boolean
  Private ReadOnly Property IsArchived As Boolean Implements IPersistentTrelloList.IsArchived
    Get
      Return Me.Closed
    End Get
  End Property

  Public Property IdBoard As String
  <DebuggerBrowsable(DebuggerBrowsableState.Never)>
  Private ReadOnly Property IPersistentTrelloList_IdBoard As String Implements IPersistentTrelloList.IdBoard
    Get
      Return Me.IdBoard
    End Get
  End Property

End Class

Public Interface IPersistentTrelloList
  Inherits ITrelloListContent

  ReadOnly Property Id As String
  ReadOnly Property IsArchived As Boolean
  ReadOnly Property IdBoard As String

End Interface

Public Interface ITrelloListContent

  Property Name As String

End Interface

Public Class TrelloListContent
  Implements ITrelloListContent

  Public Property Name As String Implements ITrelloListContent.Name

End Class
