Imports System
Imports System.Diagnostics
Imports KornSW.TrelloConnector

<DebuggerDisplay("Label {Id} ('{Name}')")>
Friend Class LoadedTrelloLabel
  Inherits TrelloLabelContent
  Implements IPersistentTrelloLabel

  Public Property Id As String
  <DebuggerBrowsable(DebuggerBrowsableState.Never)>
  Private ReadOnly Property IPersistedTrelloLabel_Id As String Implements IPersistentTrelloLabel.Id
    Get
      Return Me.Id
    End Get
  End Property
  <DebuggerBrowsable(DebuggerBrowsableState.Never)>
  Public Property Closed As Boolean
  Private ReadOnly Property IsArchived As Boolean Implements IPersistentTrelloLabel.IsArchived
    Get
      Return Me.Closed
    End Get
  End Property

  Public Property IdBoard As String
  <DebuggerBrowsable(DebuggerBrowsableState.Never)>
  Private ReadOnly Property IPersistentTrelloList_IdBoard As String Implements IPersistentTrelloLabel.IdBoard
    Get
      Return Me.IdBoard
    End Get
  End Property

End Class

Public Interface IPersistentTrelloLabel
  Inherits ITrelloLabelContent

  ReadOnly Property Id As String
  ReadOnly Property IsArchived As Boolean
  ReadOnly Property IdBoard As String

End Interface

Public Interface ITrelloLabelContent
  Property Name As String
  Property Color As String
End Interface

Public Class TrelloLabelContent
  Implements ITrelloLabelContent

  Public Property Color As String Implements ITrelloLabelContent.Color

  Public Property Name As String Implements ITrelloLabelContent.Name

End Class
