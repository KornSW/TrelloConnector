Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq

Namespace FluentAccess

  Friend Class MemberStore
    Implements IAlloverMemberAccess

    Private _Api As TrelloApi
    Private _CurrentMember As MemberRecordHandle = Nothing

    Public ReadOnly Property Current As IMemberAccessContext Implements IAlloverMemberAccess.Current
      Get
        If (_CurrentMember Is Nothing) Then
          _CurrentMember = New MemberRecordHandle(_Api, _Api.MemberRepository.GetCurrentMemberId)
        End If
        Return _CurrentMember
      End Get
    End Property

    Public Sub New(api As TrelloApi)
      _Api = api
    End Sub

    Public Sub Save(changedItem As IPersistentTrelloMember) Implements IAlloverMemberAccess.Save
      _Api.Members(changedItem).SaveContentFrom(changedItem)
    End Sub

#Region " OfCard "

    Public Function OfCard(card As ICardAccessContext) As IMembersAccessContext Implements IAlloverMemberAccess.OfCard
      Return card.AssignedMembers()
    End Function

    Public Function OfCard(cardId As String) As IMembersAccessContext Implements IAlloverMemberAccess.OfCard
      Return _Api.Cards(cardId).AssignedMembers()
    End Function

    Public Function OfCard(card As IPersistentTrelloCard) As IMembersAccessContext Implements IAlloverMemberAccess.OfCard
      Return _Api.Cards(card).AssignedMembers()
    End Function

#End Region

#Region " OfBoard "

    Public Function OfBoard(board As IBoardAccessContext) As IMembersAccessContext Implements IAlloverMemberAccess.OfBoard
      Return board.AccessingMembers()
    End Function

    Public Function OfBoard(boardId As String) As IMembersAccessContext Implements IAlloverMemberAccess.OfBoard
      Return _Api.Boards(boardId).AccessingMembers()
    End Function

    Public Function OfBoard(board As IPersistentTrelloBoard) As IMembersAccessContext Implements IAlloverMemberAccess.OfBoard
      Return _Api.Boards(board).AccessingMembers()
    End Function

#End Region

  End Class

End Namespace
