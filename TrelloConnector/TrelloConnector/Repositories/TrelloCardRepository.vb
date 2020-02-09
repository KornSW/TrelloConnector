Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq

Namespace Repositories

  Friend NotInheritable Class TrelloCardRepository
    Inherits RepositoryBase

    Friend Sub New(api As TrelloApi)
      MyBase.New(api)
    End Sub

#Region " GetIds & Exists "

    Public Function GetAllIds(includeArchived As Boolean) As String()
      Dim filter = Me.GetFilterExpression(includeArchived)
      Return Me.ExecuteGet(Of IdContainer())($"cards", filter).Ids
    End Function

    Public Function CardExists(cardId As String, includeArchived As Boolean) As Boolean
      Dim filter = Me.GetFilterExpression(includeArchived)
      Return Me.ExecuteGet(Of IdContainer)($"cards/{cardId}").Exists(includeArchived)
    End Function

#End Region

#Region " Load & Save "

    Public Function LoadCardById(cardId As String) As IPersistentTrelloCard
      Return Me.ExecuteGet(Of LoadedTrelloCard)($"cards/{cardId}")
    End Function

    Public Sub LoadCardByIdInto(cardId As String, target As ITrelloCardContent)
      Dim b = Me.LoadCardById(cardId)
      b.CopyContentTo(target)
      If (TypeOf target Is LoadedTrelloCard) Then
        With DirectCast(target, LoadedTrelloCard)
          .Id = b.Id
          .Closed = b.IsArchived
          .IdBoard = b.IdBoard
          .IdList = b.IdList
        End With
      End If
    End Sub

    Public Function LoadCardsByListId(parentListId As String, includeArchived As Boolean) As IPersistentTrelloCard()
      Dim filter = Me.GetFilterExpression(includeArchived)
      Return Me.ExecuteGet(Of LoadedTrelloCard())($"list/{parentListId}/cards", filter)
    End Function

    Public Function CreateNewCard(parentListId As String, contentSource As ITrelloCardContent) As String
      Return Me.ExecutePost(Of IdContainer, ITrelloCardContent)("cards", contentSource, $"idList={parentListId}&keepFromSource=all").Id
    End Function

    Public Sub SaveCardContent(cardId As String, contentSource As ITrelloCardContent)
      Me.ExecutePut(Of LoadedTrelloCard, ITrelloCardContent)($"cards/{cardId}", contentSource)
    End Sub

#End Region

#Region " Archive & Move "

    Public Function IsCardArchived(cardId As String) As Boolean
      Dim c = Me.ExecuteGet(Of IdContainer)($"cards/{cardId}")
      Return (c.IsLoaded() AndAlso c.Closed)
    End Function

    Public Sub ArchiveCardsById(ParamArray cardIds() As String)
      For Each id In cardIds
        Me.ExecutePut(Of Object, Object)($"cards/{id}/closed", Nothing, "value=true")
      Next
    End Sub

    Public Sub UnarchiveCardsById(ParamArray cardIds() As String)
      For Each id In cardIds
        Me.ExecutePut(Of Object, Object)($"cards/{id}/closed", Nothing, "value=false")
      Next
    End Sub

    Public Sub MoveCardsToList(listId As String, ParamArray cardIds() As String)
      For Each id In cardIds
        Me.ExecutePut(Of Object, Object)($"cards/{id}/idList", Nothing, $"value={listId}")
      Next
    End Sub

#End Region

#Region " Principal-Relation (Lists) "

    Public Function GetCardIdsByList(parentListId As String, includeArchived As Boolean) As String()
      Dim filter = Me.GetFilterExpression(includeArchived)
      Return Me.ExecuteGet(Of IdContainer())($"list/{parentListId}/cards", filter).Ids
    End Function

    Public Function GetCardIdsByBoard(parentBoardId As String, includeArchived As Boolean) As String()
      Dim filter = Me.GetFilterExpression(includeArchived)
      Return Me.ExecuteGet(Of IdContainer())($"board/{parentBoardId}/cards", filter).Ids
    End Function

#End Region

#Region " Assignment (Members) "

    Public Function GetCardIdsByAssignedMember(assignedMemberId As String, includeArchived As Boolean) As String()
      Dim filter = Me.GetFilterExpression(includeArchived)
      Return Me.ExecuteGet(Of IdContainer())($"members/{assignedMemberId}/cards", filter).Ids
    End Function

    Public Sub AssignCardToMember(cardId As String, memberId As String)
      Me.ExecutePost(Of Object)($"cards/{cardId}/idMembers", Nothing, $"value={memberId}")
    End Sub

    Public Sub UnassignCardFromMember(cardId As String, assignedMemberId As String)
      Me.ExecuteDelete($"cards/{cardId}/idMembers/{assignedMemberId}")
    End Sub

#End Region

#Region " Assignment (Labels) "

    Public Function GetCardIdsByAssigendLabel(assignedLabelId As String, includeArchived As Boolean) As String()

      Dim label = Me.Api.LabelRepository.LoadLabelById(assignedLabelId)
      Dim filter = Me.GetFilterExpression(includeArchived)
      Dim cards = Me.ExecuteGet(Of IdContainerWithIdLabels())($"boards/{label.IdBoard}/cards", filter)

      Dim assignedCardIds As New List(Of String)

      For Each card In cards
        If (card.IdLabels.Contains(assignedLabelId)) Then
          assignedCardIds.Add(card.Id)
        End If
      Next

      Return assignedCardIds.ToArray()
    End Function

    Public Sub AssignCardToLabel(cardId As String, labelId As String)
      Me.Api.LabelRepository.AssignLabelToCard(labelId, cardId)
    End Sub

    Public Sub UnassignCardFromLabel(cardId As String, assignedLabelId As String)
      Me.Api.LabelRepository.UnassignLabelFromCard(assignedLabelId, cardId)
    End Sub

#End Region

  End Class

End Namespace
