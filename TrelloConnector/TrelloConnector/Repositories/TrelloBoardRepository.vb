Imports System
Imports System.Collections.Generic
Imports System.Linq

Namespace Repositories

  Friend NotInheritable Class TrelloBoardRepository
    Inherits RepositoryBase

    Friend Sub New(api As TrelloApi)
      MyBase.New(api)
    End Sub

#Region " GetIds & Exists "

    Public Function GetAllIds(includeArchived As Boolean) As String()
      Dim filter = Me.GetFilterExpression(includeArchived)
      Return Me.ExecuteGet(Of IdContainer())($"boards", filter).Ids
    End Function

    Public Function BoardExists(boardId As String, includeArchived As Boolean) As Boolean
      Dim filter = Me.GetFilterExpression(includeArchived)
      Return Me.ExecuteGet(Of IdContainer)($"boards/{boardId}", filter).Exists(includeArchived)
    End Function

#End Region

#Region " Load & Save "

    Public Function LoadBoardById(boardId As String) As IPersistentTrelloBoard
      Return Me.ExecuteGet(Of LoadedTrelloBoard)($"boards/{boardId}")
    End Function

    Public Sub LoadBoardByIdInto(boardId As String, target As ITrelloBoardContent)
      Dim b = Me.LoadBoardById(boardId)
      b.CopyContentTo(target)
      If (TypeOf target Is LoadedTrelloBoard) Then
        With DirectCast(target, LoadedTrelloBoard)
          .Id = b.Id
          .Closed = b.IsArchived
        End With
      End If
    End Sub

    Public Function CreateNewBoard(contentSource As ITrelloBoardContent) As String
      Return Me.ExecutePost(Of IdContainer, ITrelloBoardContent)("boards", contentSource).Id
    End Function

    Public Sub SaveBoardContent(boardId As String, contentSource As ITrelloBoardContent)
      Me.ExecutePut(Of LoadedTrelloBoard, ITrelloBoardContent)($"boards/{boardId}", contentSource)
    End Sub

#End Region

#Region " Archive "

    Public Function IsBoardArchived(boardId As String) As Boolean
      Dim c = Me.ExecuteGet(Of IdContainer)($"boards/{boardId}")
      Return (c.IsLoaded() AndAlso c.Closed)
    End Function

    Public Sub ArchiveBoardsById(ParamArray boardIds() As String)
      For Each id In boardIds
        Me.ExecutePut(Of Object, Object)($"boards/{id}/closed", Nothing, "value=true")
      Next
    End Sub

    Public Sub UnarchiveBoardsById(ParamArray boardIds() As String)
      For Each id In boardIds
        Me.ExecutePut(Of Object, Object)($"boards/{id}/closed", Nothing, "value=false")
      Next
    End Sub

#End Region

#Region " Principal-Relation (Lists) "

    Public Function GetBoardIdContainingList(listId As String) As String
      Return Me.ExecuteGet(Of IdContainerWithIdBoard)($"lists/{listId}").IdBoard
    End Function

#End Region

#Region " Principal-Relation (Labels) "

    Public Function GetBoardIdContainingLabel(labelId As String) As String
      Return Me.ExecuteGet(Of IdContainerWithIdBoard)($"labels/{labelId}").IdBoard
    End Function

#End Region

#Region " Assignment (Members) "

    Public Function GetBoardIdsByAssignedMember(assignedMemberId As String, includeArchived As Boolean) As String()
      Dim filter = Me.GetFilterExpression(includeArchived)
      Return Me.ExecuteGet(Of IdContainer())($"members/{assignedMemberId}/boards", filter).ids
    End Function

    Public Sub AssignBoardToMember(boardId As String, memberId As String)
      Me.ExecutePost(Of Object)($"members/{memberId}/idBoards", Nothing, $"value={boardId}")
    End Sub

    Public Sub UnassignBoardFromMember(boardId As String, assignedMemberId As String)
      Me.ExecuteDelete($"members/{assignedMemberId}/idBoards/{boardId}")
    End Sub

#End Region

  End Class

End Namespace
