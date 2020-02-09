Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Runtime.CompilerServices

<DebuggerDisplay("Board {Id} ('{Name}')")>
Friend Class LoadedTrelloBoard
  Inherits TrelloBoardContent
  Implements IPersistentTrelloBoard

  Public Property Id As String
  <DebuggerBrowsable(DebuggerBrowsableState.Never)>
  Private ReadOnly Property IPersistentTrelloBoard_Id As String Implements IPersistentTrelloBoard.Id
    Get
      Return Me.Id
    End Get
  End Property

  <DebuggerBrowsable(DebuggerBrowsableState.Never)>
  Public Property Closed As Boolean
  Private ReadOnly Property IsArchived As Boolean Implements IPersistentTrelloBoard.IsArchived
    Get
      Return Me.Closed
    End Get
  End Property

End Class

Public Interface IPersistentTrelloBoard
  Inherits ITrelloBoardContent

  ReadOnly Property Id As String

  ReadOnly Property IsArchived As Boolean

End Interface

Public Interface ITrelloBoardContent

  Property Name As String

End Interface

Public Class TrelloBoardContent
  Implements ITrelloBoardContent

  Public Property Name As String Implements ITrelloBoardContent.Name

End Class
