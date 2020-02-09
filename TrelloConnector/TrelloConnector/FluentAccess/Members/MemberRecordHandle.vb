Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq

Namespace FluentAccess

  <DebuggerDisplay("Member {Id}")>
  Friend Class MemberRecordHandle
    Implements IMemberAccessContext

    Private _Api As TrelloApi
    Private _MemberId As String

    Public Sub New(api As TrelloApi, memberId As String)
      _Api = api
      _MemberId = memberId
    End Sub

    Public ReadOnly Property Id As String Implements IMemberAccessContext.Id
      Get
        Return _MemberId
      End Get
    End Property

#Region " Load / Update "

    Public Function Load() As IPersistentTrelloMember Implements IMemberAccessContext.Load
      Return _Api.MemberRepository.LoadMemberById(_MemberId)
    End Function

    Public Sub LoadInto(target As ITrelloMemberContent) Implements IMemberAccessContext.LoadInto
      _Api.MemberRepository.LoadMemberByIdInto(_MemberId, target)
    End Sub

    Public Sub SaveContentFrom(contentSource As ITrelloMemberContent) Implements IMemberAccessContext.SaveContentFrom
      _Api.MemberRepository.SaveMemberContent(_MemberId, contentSource)
    End Sub

#End Region

#Region " Exists "

    Public Function Exists(Optional includeArchived As Boolean = False) As Boolean Implements IMemberAccessContext.Exists
      Return _Api.MemberRepository.MemberExists(_MemberId, includeArchived)
    End Function

#End Region

#Region " Referrer-Navigation -> Cards "

    Public Function AssignedCards() As ICardsAccessContext Implements IMemberAccessContext.AssignedCards
      Return New CardQuery(_Api, Function(includeArchived) _Api.CardRepository.GetCardIdsByAssignedMember(_MemberId, includeArchived))
    End Function

#End Region

#Region " Referrer-Navigation -> Opened Board "

    Public Function AssignedBoards() As IBoardsAccessContext Implements IMemberAccessContext.AssignedBoards
      Return New BoardQuery(_Api, Function(includeArchived) _Api.BoardRepository.GetBoardIdsByAssignedMember(_MemberId, includeArchived))
    End Function

    Public Function IsAssignedToBoard(board As IBoardAccessContext) As Boolean Implements IMemberAccessContext.IsAssignedToBoard
      Return Me.AssignedBoards.Contains(board)
    End Function

    Public Function IsAssignedToBoard(boardId As String) As Boolean Implements IMemberAccessContext.IsAssignedToBoard
      Return Me.AssignedBoards.Contains(boardId)
    End Function

    Public Function IsAssignedToBoard(board As IPersistentTrelloBoard) As Boolean Implements IMemberAccessContext.IsAssignedToBoard
      Return Me.AssignedBoards.Contains(board)
    End Function

    Public Sub AssignBoard(board As IBoardAccessContext) Implements IMemberAccessContext.AssignBoard
      _Api.BoardRepository.AssignBoardToMember(board.Id, _MemberId)
    End Sub

    Public Sub AssignBoard(boardId As String) Implements IMemberAccessContext.AssignBoard
      _Api.BoardRepository.AssignBoardToMember(boardId, _MemberId)
    End Sub

    Public Sub AssignBoard(board As IPersistentTrelloBoard) Implements IMemberAccessContext.AssignBoard
      _Api.BoardRepository.AssignBoardToMember(board.Id, _MemberId)
    End Sub

    Public Sub UnassignBoard(board As IBoardAccessContext) Implements IMemberAccessContext.UnassignBoard
      _Api.BoardRepository.UnassignBoardFromMember(board.Id, _MemberId)
    End Sub

    Public Sub UnassignBoard(boardId As String) Implements IMemberAccessContext.UnassignBoard
      _Api.BoardRepository.UnassignBoardFromMember(boardId, _MemberId)
    End Sub

    Public Sub UnassignBoard(board As IPersistentTrelloBoard) Implements IMemberAccessContext.UnassignBoard
      _Api.BoardRepository.UnassignBoardFromMember(board.Id, _MemberId)
    End Sub

#End Region

  End Class

End Namespace
