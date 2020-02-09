Imports System
Imports System.Collections.Generic
Imports System.Linq

Namespace Repositories

  Friend NotInheritable Class TrelloMemberRepository
    Inherits RepositoryBase

    Friend Sub New(api As TrelloApi)
      MyBase.New(api)
    End Sub

#Region " GetIds & Exists "

    Public Function GetCurrentMemberId() As String
      Return Me.ExecuteGet(Of IdContainer)($"members/me").Id
    End Function

    Public Function MemberExists(memberId As String, includeArchived As Boolean) As Boolean
      Dim filter = Me.GetFilterExpression(includeArchived)
      Return Me.ExecuteGet(Of IdContainer)($"members/{memberId}", filter).Exists(includeArchived)
    End Function

#End Region

#Region " Load & Save "

    Public Function LoadMe() As IPersistentTrelloMember
      Dim currentMemberId = Me.GetCurrentMemberId()
      Return Me.LoadMemberById(currentMemberId)
    End Function

    Public Function LoadMemberById(memberId As String) As IPersistentTrelloMember
      Return Me.ExecuteGet(Of LoadedTrelloMember)($"members/{memberId}")
    End Function

    Public Sub LoadMemberByIdInto(memberId As String, target As ITrelloMemberContent)
      Dim b = Me.LoadMemberById(memberId)
      b.CopyContentTo(target)
      If (TypeOf target Is LoadedTrelloMember) Then
        With DirectCast(target, LoadedTrelloMember)
          .Id = b.Id
          .Closed = b.IsArchived
        End With
      End If
    End Sub

    Public Sub SaveMemberContent(memberId As String, contentSource As ITrelloMemberContent)
      Me.ExecutePut(Of LoadedTrelloMember, ITrelloMemberContent)($"members/{memberId}", contentSource)
    End Sub

#End Region

#Region " Assignment (Boards) "

    Public Function GetMemberIdsByAssignedBoard(assignedBoardId As String, includeArchived As Boolean) As String()
      Dim filter = Me.GetFilterExpression(includeArchived)
      Return Me.ExecuteGet(Of IdContainer())($"boards/{assignedBoardId}/members", filter).Ids
    End Function

    Public Sub AssignMemberToBoard(memberId As String, boardId As String)
      Me.Api.BoardRepository.AssignBoardToMember(boardId, memberId)
    End Sub

    Public Sub UnassignMemberFromBoard(memberId As String, assignedBoardId As String)
      Me.Api.BoardRepository.UnassignBoardFromMember(assignedBoardId, memberId)
    End Sub

#End Region

#Region " Assignment (Cards) "

    Public Function GetMemberIdsByAssignedCard(assignedCardId As String, includeArchived As Boolean) As String()
      Dim filter = Me.GetFilterExpression(includeArchived)
      Return Me.ExecuteGet(Of IdContainer())($"cards/{assignedCardId}/members", filter).Ids
    End Function

    Public Sub AssignMemberToCard(memberId As String, cardId As String)
      Me.Api.CardRepository.AssignCardToMember(cardId, memberId)
    End Sub

    Public Sub UnassignMemberFromCard(memberId As String, assignedCardId As String)
      Me.Api.CardRepository.UnassignCardFromMember(assignedCardId, memberId)
    End Sub

#End Region

  End Class

End Namespace
