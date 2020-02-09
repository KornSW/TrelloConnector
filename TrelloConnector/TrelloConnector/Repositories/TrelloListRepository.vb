Imports System
Imports System.Collections.Generic
Imports System.Linq

Namespace Repositories

  Friend NotInheritable Class TrelloListRepository
    Inherits RepositoryBase

    Friend Sub New(api As TrelloApi)
      MyBase.New(api)
    End Sub

#Region " GetIds & Exists "

    Public Function GetAllIds(includeArchived As Boolean) As String()
      Dim filter = Me.GetFilterExpression(includeArchived)
      Return Me.ExecuteGet(Of IdContainer())($"lists", filter).Ids
    End Function

    Public Function ListExists(listId As String, includeArchived As Boolean) As Boolean
      Dim filter = Me.GetFilterExpression(includeArchived)
      Return Me.ExecuteGet(Of IdContainer)($"lists/{listId}", filter).Exists(includeArchived)
    End Function

    Public Function FilterListIdsByName(listIds() As String, name As String) As String()
      Dim result As New List(Of String)

      For Each id In listIds
        Dim l = Me.ExecuteGet(Of IdContainerWithName)($"lists/{id}")
        If (l.Name = name) Then
          result.Add(l.Id)
        End If
      Next

      Return result.ToArray()
    End Function

#End Region

#Region " Load & Save "

    Public Function LoadListById(listId As String) As IPersistentTrelloList
      Return Me.ExecuteGet(Of LoadedTrelloList)($"lists/{listId}")
    End Function

    Public Sub LoadListByIdInto(listId As String, target As ITrelloListContent)
      Dim b = Me.LoadListById(listId)
      b.CopyContentTo(target)
      If (TypeOf target Is LoadedTrelloList) Then
        With DirectCast(target, LoadedTrelloList)
          .Id = b.Id
          .Closed = b.IsArchived
          .IdBoard = b.IdBoard
        End With
      End If
    End Sub

    Public Function CreateNewList(parentBoardId As String, contentSource As ITrelloListContent) As String
      Return Me.ExecutePost(Of IdContainer, ITrelloListContent)("lists", contentSource, $"idBoard={parentBoardId}&keepFromSource=all").Id
    End Function

    Public Sub SaveListContent(listId As String, contentSource As ITrelloListContent)
      Me.ExecutePut(Of LoadedTrelloList, ITrelloListContent)($"lists/{listId}", contentSource)
    End Sub

#End Region

#Region " Archive & Move "

    Public Function IsListArchived(listId As String) As Boolean
      Dim c = Me.ExecuteGet(Of IdContainer)($"lists/{listId}")
      Return (c.IsLoaded() AndAlso c.Closed)
    End Function

    Public Sub ArchiveListsById(ParamArray listIds() As String)
      For Each id In listIds
        Me.ExecutePut(Of Object, Object)($"lists/{id}/closed", Nothing, "value=true")
      Next
    End Sub

    Public Sub UnarchiveListsById(ParamArray listIds() As String)
      For Each id In listIds
        Me.ExecutePut(Of Object, Object)($"lists/{id}/closed", Nothing, "value=false")
      Next
    End Sub

    Public Sub MoveListsToBoard(boardId As String, ParamArray listIds() As String)
      For Each id In listIds
        Me.ExecutePut(Of Object, Object)($"lists/{id}/idBoard", Nothing, $"value={boardId}")
      Next
    End Sub

#End Region

#Region " Principal-Relation (Board) "

    Public Function GetListIdsByBoard(parentBoardId As String, includeArchived As Boolean) As String()
      Dim filter = Me.GetFilterExpression(includeArchived)
      Return Me.ExecuteGet(Of IdContainer())($"board/{parentBoardId}/lists", filter).Ids
    End Function

#End Region

#Region " Principal-Relation (Cards) "

    Public Function GetListIdContainingCard(cardId As String) As String
      Return Me.ExecuteGet(Of IdContainerWithIdList)($"cards/{cardId}").IdList
    End Function

#End Region

  End Class

End Namespace
