Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq

Namespace FluentAccess

  Friend Class ListStore
    Implements IListContainerAccessContext

    Private _Api As TrelloApi
    Private _ParentBoardId As String
    Private _BaseQuery As ListQuery

    Public Sub New(api As TrelloApi, parentBoardId As String)
      _Api = api
      _ParentBoardId = parentBoardId
      _BaseQuery = New ListQuery(_Api, Function(includeArchived) _Api.ListRepository.GetListIdsByBoard(parentBoardId, includeArchived), _ParentBoardId)
    End Sub

    Public ReadOnly Property ParentBoardId As String
      Get
        Return _ParentBoardId
      End Get
    End Property

#Region " CreateNew (only on store) "

    Public Function CreateNew(name As String) As IListAccessContext Implements IListContainerAccessContext.CreateNew
      Dim newContent As New TrelloListContent With {
        .Name = name
      }
      Return Me.CreateNew(newContent)
    End Function

    Public Function CreateNew(contentSource As ITrelloListContent) As IListAccessContext Implements IListContainerAccessContext.CreateNew
      Dim newlyCreatedId As String = _Api.ListRepository.CreateNewList(_ParentBoardId, contentSource)
      Return _Api.Lists(newlyCreatedId)
    End Function

#End Region

#Region " Access (proxy to BaseQuery) "

    Public Function GetIds(Optional includeArchived As Boolean = False) As String() Implements IListsAccessContext.GetIds
      Return _BaseQuery.GetIds(includeArchived)
    End Function

    Public Function GetAll(Optional includeArchived As Boolean = False) As IListAccessContext() Implements IListsAccessContext.GetAll
      Return _BaseQuery.GetAll(includeArchived)
    End Function

    Public Function LoadAll(Optional includeArchived As Boolean = False) As IPersistentTrelloList() Implements IListsAccessContext.LoadAll
      Return _BaseQuery.LoadAll(includeArchived)
    End Function

    Public Function Contains(list As IPersistentTrelloList) As Boolean Implements IListsAccessContext.Contains
      Return _BaseQuery.Contains(list)
    End Function

    Public Function Contains(listId As String) As Boolean Implements IListsAccessContext.Contains
      Return _BaseQuery.Contains(listId)
    End Function

    Public Function Contains(list As IListAccessContext) As Boolean Implements IListsAccessContext.Contains
      Return _BaseQuery.Contains(list)
    End Function

    Public Function First(Optional includeArchived As Boolean = False) As IListAccessContext Implements IListsAccessContext.First
      Return _BaseQuery.First(includeArchived)
    End Function

    Public Function AtIndex(index As Integer, Optional includeArchived As Boolean = False) As IListAccessContext Implements IListsAccessContext.AtIndex
      Return _BaseQuery.AtIndex(index, includeArchived)
    End Function

    Public Function Last(Optional includeArchived As Boolean = False) As IListAccessContext Implements IListsAccessContext.Last
      Return _BaseQuery.Last(includeArchived)
    End Function

    Public Function LoadAll(startIndex As Integer, maxCount As Integer, Optional includeArchived As Boolean = False) As IPersistentTrelloList() Implements IListsAccessContext.LoadAll
      Return _BaseQuery.LoadAll(startIndex, maxCount, includeArchived)
    End Function

    Public Function WithName(listName As String) As IListsAccessContext Implements IListsAccessContext.WithName
      Return _BaseQuery.WithName(listName)
    End Function

    Public Sub MoveAllTo(targetBoard As IBoardAccessContext) Implements IListsAccessContext.MoveAllTo
      _BaseQuery.MoveAllTo(targetBoard)
    End Sub

    Public Sub MoveAllTo(targetBoardId As String) Implements IListsAccessContext.MoveAllTo
      _BaseQuery.MoveAllTo(targetBoardId)
    End Sub

    Public Sub MoveAllTo(targetBoard As IPersistentTrelloBoard) Implements IListsAccessContext.MoveAllTo
      _BaseQuery.MoveAllTo(targetBoard)
    End Sub

#End Region

  End Class

End Namespace
