Imports System
Imports System.Collections.Generic
Imports System.Linq

Namespace FluentAccess

  Friend Class CardQuery
    Implements ICardsAccessContext

    Private _Api As TrelloApi
    Private _IdGetter As Func(Of Boolean, String())
    Private _OriginListId As String = Nothing

    Public Sub New(api As TrelloApi, idGetter As Func(Of Boolean, String()), Optional originListId As String = Nothing)
      _Api = api
      _IdGetter = idGetter
      _OriginListId = originListId
    End Sub

    Protected ReadOnly Property Api As TrelloApi
      Get
        Return _Api
      End Get
    End Property

    Public Function GetIds(Optional includeArchived As Boolean = False) As String() Implements ICardsAccessContext.GetIds
      Return _IdGetter.Invoke(includeArchived)
    End Function

    Public Function GetAll(Optional includeArchived As Boolean = False) As ICardAccessContext() Implements ICardsAccessContext.GetAll
      Return _IdGetter.Invoke(includeArchived).Select(Function(id) New CardRecordHandle(_Api, id, _OriginListId)).ToArray()
    End Function

#Region " Contains "

    Public Function Contains(cardId As String) As Boolean Implements ICardsAccessContext.Contains
      Return Me.GetIds(True).Contains(cardId)
    End Function

    Public Function Contains(card As IPersistentTrelloCard) As Boolean Implements ICardsAccessContext.Contains
      Return Me.Contains(card.Id)
    End Function

    Public Function Contains(card As ICardAccessContext) As Boolean Implements ICardsAccessContext.Contains
      Return Me.Contains(card.Id)
    End Function

#End Region

#Region " Position based Picking "

    Public Function First(Optional includeArchived As Boolean = False) As ICardAccessContext Implements ICardsAccessContext.First
      Return Me.AtIndex(0, includeArchived)
    End Function

    Public Function AtIndex(index As Integer, Optional includeArchived As Boolean = False) As ICardAccessContext Implements ICardsAccessContext.AtIndex
      Dim allIds = Me.GetIds()
      If (allIds.Length <= index) Then
        Return Nothing
      End If
      Return New CardRecordHandle(_Api, allIds(index), _OriginListId)
    End Function

    Public Function Last(Optional includeArchived As Boolean = False) As ICardAccessContext Implements ICardsAccessContext.Last
      Dim allIds = Me.GetIds()
      Dim lastIndex = allIds.Count() - 1
      If (lastIndex < 0) Then
        Return Nothing
      End If
      Return New CardRecordHandle(_Api, allIds(lastIndex), _OriginListId)
    End Function

#End Region

#Region " Load "

    Public Function LoadAll(Optional includeArchived As Boolean = False) As IPersistentTrelloCard() Implements ICardsAccessContext.LoadAll
      Dim cards As New List(Of IPersistentTrelloCard)
      For Each card In Me.GetAll(includeArchived)
        cards.Add(card.Load())
      Next
      Return cards.ToArray()
    End Function

    Public Function LoadAll(startIndex As Integer, maxCount As Integer, Optional includeArchived As Boolean = False) As IPersistentTrelloCard() Implements ICardsAccessContext.LoadAll
      Dim cards As New List(Of IPersistentTrelloCard)
      For i As Integer = startIndex To (startIndex + (maxCount - 1))
        Dim card = Me.AtIndex(i, includeArchived)
        If (card IsNot Nothing) Then
          cards.Add(card.Load())
        End If
      Next
      Return cards.ToArray()
    End Function

#End Region

#Region " Archive "

    Public Sub ArchiveAll() Implements ICardsAccessContext.ArchiveAll
      _Api.CardRepository.ArchiveCardsById(Me.GetIds())
    End Sub

#End Region

#Region " Move "

    Public Sub MoveAllTo(targetList As IListAccessContext) Implements ICardsAccessContext.MoveAllTo
      _Api.CardRepository.MoveCardsToList(targetList.Id, Me.GetIds())
    End Sub

    Public Sub MoveAllTo(targetListId As String) Implements ICardsAccessContext.MoveAllTo
      _Api.CardRepository.MoveCardsToList(targetListId, Me.GetIds())
    End Sub

    Public Sub MoveAllTo(targetList As IPersistentTrelloList) Implements ICardsAccessContext.MoveAllTo
      _Api.CardRepository.MoveCardsToList(targetList.Id, Me.GetIds())
    End Sub

#End Region

  End Class

End Namespace
