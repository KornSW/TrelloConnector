Imports System
Imports System.Collections.Generic
Imports System.Linq

Namespace Repositories

  Friend NotInheritable Class TrelloLabelRepository
    Inherits RepositoryBase

    Friend Sub New(api As TrelloApi)
      MyBase.New(api)
    End Sub

#Region " GetIds & Exists "

    Public Function GetAllIds(includeArchived As Boolean) As String()
      Dim filter = Me.GetFilterExpression(includeArchived)
      Return Me.ExecuteGet(Of IdContainer())($"labels", filter).Ids
    End Function

    Public Function LabelExists(labelId As String, includeArchived As Boolean) As Boolean
      Dim filter = Me.GetFilterExpression(includeArchived)
      Return Me.ExecuteGet(Of IdContainer)($"labels/{labelId}", filter).Exists(includeArchived)
    End Function

    Public Function FilterLabelIdsByName(labelIds() As String, name As String) As String()
      Dim result As New List(Of String)

      For Each id In labelIds
        Dim l = Me.ExecuteGet(Of IdContainerWithName)($"labels/{id}")
        If (l.name = name) Then
          result.Add(l.id)
        End If
      Next

      Return result.ToArray()
    End Function

#End Region

#Region " Load & Save "

    Public Function LoadLabelById(labelId As String) As IPersistentTrelloLabel
      Return Me.ExecuteGet(Of LoadedTrelloLabel)($"labels/{labelId}")
    End Function

    Public Sub LoadLabelByIdInto(labelId As String, target As ITrelloLabelContent)
      Dim b = Me.LoadLabelById(labelId)
      b.CopyContentTo(target)
      If (TypeOf target Is LoadedTrelloLabel) Then
        With DirectCast(target, LoadedTrelloLabel)
          .Id = b.Id
          .Closed = b.IsArchived
          .IdBoard = b.IdBoard
        End With
      End If
    End Sub

    Public Function CreateNewLabel(parentBoardId As String, contentSource As ITrelloLabelContent) As String
      Return Me.ExecutePost(Of IdContainer, ITrelloLabelContent)("labels", contentSource, $"idBoard={parentBoardId}&keepFromSource=all").Id
    End Function

    Public Sub SaveLabelContent(labelId As String, contentSource As ITrelloLabelContent)
      Me.ExecutePut(Of LoadedTrelloLabel, ITrelloLabelContent)($"labels/{labelId}", contentSource)
    End Sub

#End Region

#Region " Archive & Move "

    Public Function IsLabelArchived(labelId As String) As Boolean
      Dim c = Me.ExecuteGet(Of IdContainer)($"labels/{labelId}")
      Return (c.IsLoaded() AndAlso c.Closed)
    End Function

    Public Sub ArchiveLabelsById(ParamArray labelIds() As String)
      For Each id In labelIds
        Me.ExecutePut(Of Object, Object)($"labels/{id}/closed", Nothing, "value=true")
      Next
    End Sub

    Public Sub UnarchiveLabelsById(ParamArray labelIds() As String)
      For Each id In labelIds
        Me.ExecutePut(Of Object, Object)($"labels/{id}/closed", Nothing, "value=false")
      Next
    End Sub

    Public Sub MoveLabelsToBoard(boardId As String, ParamArray labelIds() As String)
      For Each id In labelIds
        Me.ExecutePut(Of Object, Object)($"labeld/{id}/idBoard", Nothing, $"value={boardId}")
      Next
    End Sub

#End Region

#Region " Principal-Relation (Board) "

    Public Function GetLabelIdsByBoard(parentBoardId As String, includeArchived As Boolean) As String()
      Dim filter = Me.GetFilterExpression(includeArchived)
      Return Me.ExecuteGet(Of IdContainer())($"board/{parentBoardId}/labels", filter).Ids
    End Function

#End Region

#Region " Assignment (Cards) "

    Public Function GetLabelIdsByAssignedCard(assignedCardId As String, includeArchived As Boolean) As String()
      Return Me.ExecuteGet(Of IdContainerWithIdLabels)($"cards/{assignedCardId}").IdLabels
    End Function

    Public Sub AssignLabelToCard(labelId As String, cardId As String)
      Dim existing = Me.GetLabelIdsByAssignedCard(cardId, True)
      If (Not existing.Contains(labelId)) Then
        Me.ExecutePost(Of Object)($"cards/{cardId}/idLabels", Nothing, $"value={labelId}")
      End If
    End Sub

    Public Sub UnassignLabelFromCard(labelId As String, assignedCardId As String)
      Dim existing = Me.GetLabelIdsByAssignedCard(assignedCardId, False)
      If (existing.Contains(labelId)) Then
        Me.ExecuteDelete($"cards/{assignedCardId}/idLabels/{labelId}")
      End If
    End Sub

#End Region

  End Class

End Namespace
