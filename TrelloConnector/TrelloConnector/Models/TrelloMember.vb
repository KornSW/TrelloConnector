Imports System
Imports System.Diagnostics
Imports KornSW.TrelloConnector

<DebuggerDisplay("Member {Id} ('{Name}')")>
Friend Class LoadedTrelloMember
  Inherits TrelloMemberContent
  Implements IPersistentTrelloMember

  Public Property Id As String

  <DebuggerBrowsable(DebuggerBrowsableState.Never)>
  Private ReadOnly Property IPersistentTrelloMemberContent_Id As String Implements IPersistentTrelloMember.Id
    Get
      Return Me.Id
    End Get
  End Property

  <DebuggerBrowsable(DebuggerBrowsableState.Never)>
  Public Property Closed As Boolean
  Private ReadOnly Property IsArchived As Boolean Implements IPersistentTrelloMember.IsArchived
    Get
      Return Me.Closed
    End Get
  End Property

End Class

Public Interface IPersistentTrelloMember
  Inherits ITrelloMemberContent

  ReadOnly Property Id As String

  ReadOnly Property IsArchived As Boolean

End Interface

Public Interface ITrelloMemberContent

  Property FullName As String
  Property Initials As String

End Interface

Public Class TrelloMemberContent
  Implements ITrelloMemberContent

  Public Property FullName As String Implements ITrelloMemberContent.FullName

  Public Property Initials As String Implements ITrelloMemberContent.Initials

End Class
