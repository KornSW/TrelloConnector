Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq

Namespace FluentAccess

  Friend Class CardStore
    Implements ICardContainerAccessContext

    Private _Api As TrelloApi
    Private _ParentListId As String
    Private _BaseQuery As CardQuery

    Public Sub New(api As TrelloApi, parentListId As String)
      _Api = api
      _ParentListId = parentListId
      _BaseQuery = New CardQuery(_Api, Function(includeArchived) _Api.CardRepository.GetCardIdsByList(parentListId, includeArchived), _ParentListId)
    End Sub

    Public ReadOnly Property ParentListId As String
      Get
        Return _ParentListId
      End Get
    End Property

#Region " CreateNew (only on store) "

    Public Function CreateNew(name As String) As ICardAccessContext Implements ICardContainerAccessContext.CreateNew
      Dim newContent As New TrelloCardContent With {
        .Name = name
      }
      Return Me.CreateNew(newContent)
    End Function

    Public Function CreateNew(contentSource As ITrelloCardContent) As ICardAccessContext Implements ICardContainerAccessContext.CreateNew
      Dim newlyCreatedId As String = _Api.CardRepository.CreateNewCard(_ParentListId, contentSource)
      Return _Api.Cards(newlyCreatedId)
    End Function

#End Region

#Region " Access (proxy to BaseQuery) "

    Public Function GetAll(Optional includeArchived As Boolean = False) As ICardAccessContext() Implements ICardsAccessContext.GetAll
      Return _BaseQuery.GetAll(includeArchived)
    End Function

    Public Function GetIds(Optional includeArchived As Boolean = False) As String() Implements ICardsAccessContext.GetIds
      Return _BaseQuery.GetIds(includeArchived)
    End Function

    Public Function LoadAll(Optional includeArchived As Boolean = False) As IPersistentTrelloCard() Implements ICardsAccessContext.LoadAll
      'direct load - fot this common usecase we want for more performance!
      Return _Api.CardRepository.LoadCardsByListId(_ParentListId, includeArchived)
    End Function

    Public Function Contains(card As IPersistentTrelloCard) As Boolean Implements ICardsAccessContext.Contains
      Return _BaseQuery.Contains(card)
    End Function

    Public Function Contains(cardId As String) As Boolean Implements ICardsAccessContext.Contains
      Return _BaseQuery.Contains(cardId)
    End Function

    Public Function Contains(card As ICardAccessContext) As Boolean Implements ICardsAccessContext.Contains
      Return _BaseQuery.Contains(card)
    End Function

    Public Function First(Optional includeArchived As Boolean = False) As ICardAccessContext Implements ICardsAccessContext.First
      Return _BaseQuery.First(includeArchived)
    End Function

    Public Function AtIndex(index As Integer, Optional includeArchived As Boolean = False) As ICardAccessContext Implements ICardsAccessContext.AtIndex
      Return _BaseQuery.AtIndex(index, includeArchived)
    End Function

    Public Function Last(Optional includeArchived As Boolean = False) As ICardAccessContext Implements ICardsAccessContext.Last
      Return _BaseQuery.Last(includeArchived)
    End Function

    Public Function LoadAll(startIndex As Integer, maxCount As Integer, Optional includeArchived As Boolean = False) As IPersistentTrelloCard() Implements ICardsAccessContext.LoadAll
      Return _BaseQuery.LoadAll(startIndex, maxCount, includeArchived)
    End Function

    Public Sub MoveAllTo(targetList As IListAccessContext) Implements ICardsAccessContext.MoveAllTo
      _BaseQuery.MoveAllTo(targetList)
    End Sub

    Public Sub MoveAllTo(targetListId As String) Implements ICardsAccessContext.MoveAllTo
      _BaseQuery.MoveAllTo(targetListId)
    End Sub

    Public Sub MoveAllTo(targetList As IPersistentTrelloList) Implements ICardsAccessContext.MoveAllTo
      _BaseQuery.MoveAllTo(targetList)
    End Sub

    Public Sub ArchiveAll() Implements ICardsAccessContext.ArchiveAll
      _BaseQuery.ArchiveAll()
    End Sub

#End Region

  End Class

End Namespace
