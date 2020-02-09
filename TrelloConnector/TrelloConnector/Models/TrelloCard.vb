Imports System
Imports System.Diagnostics
Imports KornSW.TrelloConnector
Imports TrelloConnector

<DebuggerDisplay("Card {Id} ('{Name}')")>
Friend Class LoadedTrelloCard
  Inherits TrelloCardContent
  Implements IPersistentTrelloCard

  Public Property Id As String
  <DebuggerBrowsable(DebuggerBrowsableState.Never)>
  Private ReadOnly Property IPersistentTrelloCard_Id As String Implements IPersistentTrelloCard.Id
    Get
      Return Me.Id
    End Get
  End Property
  <DebuggerBrowsable(DebuggerBrowsableState.Never)>
  Public Property Closed As Boolean
  Private ReadOnly Property IsArchived As Boolean Implements IPersistentTrelloCard.IsArchived
    Get
      Return Me.Closed
    End Get
  End Property

  Public Property IdList As String
  <DebuggerBrowsable(DebuggerBrowsableState.Never)>
  Private ReadOnly Property IPersistentTrelloList_IdList As String Implements IPersistentTrelloCard.IdList
    Get
      Return Me.IdList
    End Get
  End Property

  Public Property IdBoard As String
  <DebuggerBrowsable(DebuggerBrowsableState.Never)>
  Private ReadOnly Property IPersistentTrelloList_IdBoard As String Implements IPersistentTrelloCard.IdBoard
    Get
      Return Me.IdBoard
    End Get
  End Property

  Public Property DateLastActivity As Date

  <DebuggerBrowsable(DebuggerBrowsableState.Never)>
  Public ReadOnly Property IPersistentTrelloList_DateLastActivity As Date Implements IPersistentTrelloCard.DateLastActivity
    Get
      Return DateLastActivity
    End Get
  End Property

  Public Property IdLabels As String() = {}
  <DebuggerBrowsable(DebuggerBrowsableState.Never)>
  Private ReadOnly Property IPersistentTrelloList_IdLabels As String() Implements IPersistentTrelloCard.IdLabels
    Get
      Return Me.IdLabels
    End Get
  End Property

End Class

Public Interface IPersistentTrelloCard
  Inherits ITrelloCardContent

  ReadOnly Property Id As String
  ReadOnly Property IsArchived As Boolean
  ReadOnly Property IdList As String
  ReadOnly Property IdBoard As String
  ReadOnly Property DateLastActivity As DateTime
  ReadOnly Property IdLabels As String()

End Interface

Public Interface ITrelloCardContent

  Property Name As String
  Property Desc As String
  Property Due As Date?
  Property DueComplete As Boolean
  Property Pos As Integer

End Interface

Public Class TrelloCardContent
  Implements ITrelloCardContent

  Public Property Desc As String Implements ITrelloCardContent.Desc
  Public Property Due As Date? Implements ITrelloCardContent.Due
  Public Property DueComplete As Boolean Implements ITrelloCardContent.DueComplete
  Public Property Name As String Implements ITrelloCardContent.Name
  Public Property Pos As Integer Implements ITrelloCardContent.Pos

End Class
