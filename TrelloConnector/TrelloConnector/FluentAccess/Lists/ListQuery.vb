Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq

Namespace FluentAccess

  Friend Class ListQuery
    Implements IListsAccessContext

    Private _Api As TrelloApi
    Private _IdGetter As Func(Of Boolean, String())
    Private _OriginBoardId As String = Nothing

    Public Sub New(api As TrelloApi, idGetter As Func(Of Boolean, String()), Optional originBoardId As String = Nothing)
      _Api = api
      _IdGetter = idGetter
      _OriginBoardId = originBoardId
    End Sub

    Protected ReadOnly Property Api As TrelloApi
      Get
        Return _Api
      End Get
    End Property

    Public Function GetIds(Optional includeArchived As Boolean = False) As String() Implements IListsAccessContext.GetIds
      Return _IdGetter.Invoke(includeArchived)
    End Function

    Public Function GetAll(Optional includeArchived As Boolean = False) As IListAccessContext() Implements IListsAccessContext.GetAll
      Return _IdGetter.Invoke(includeArchived).Select(Function(id) New ListRecordHandle(_Api, id, _OriginBoardId)).ToArray()
    End Function

#Region " Contains "

    Public Function Contains(listId As String) As Boolean Implements IListsAccessContext.Contains
      Return Me.GetIds(True).Contains(listId)
    End Function

    Public Function Contains(list As IPersistentTrelloList) As Boolean Implements IListsAccessContext.Contains
      Return Me.Contains(list.Id)
    End Function

    Public Function Contains(list As IListAccessContext) As Boolean Implements IListsAccessContext.Contains
      Return Me.Contains(list.Id)
    End Function

#End Region

#Region " Position based Picking "

    Public Function First(Optional includeArchived As Boolean = False) As IListAccessContext Implements IListsAccessContext.First
      Return Me.AtIndex(0, includeArchived)
    End Function

    Public Function AtIndex(index As Integer, Optional includeArchived As Boolean = False) As IListAccessContext Implements IListsAccessContext.AtIndex
      Dim allIds = Me.GetIds()
      If (allIds.Length <= index) Then
        Return Nothing
      End If
      Return New ListRecordHandle(_Api, allIds(index), _OriginBoardId)
    End Function

    Public Function Last(Optional includeArchived As Boolean = False) As IListAccessContext Implements IListsAccessContext.Last
      Dim allIds = Me.GetIds()
      Dim lastIndex = allIds.Count() - 1
      If (lastIndex < 0) Then
        Return Nothing
      End If
      Return New ListRecordHandle(_Api, allIds(lastIndex), _OriginBoardId)
    End Function

#End Region

#Region " Load "

    Public Function LoadAll(Optional includeArchived As Boolean = False) As IPersistentTrelloList() Implements IListsAccessContext.LoadAll
      Dim lists As New List(Of IPersistentTrelloList)
      For Each list In Me.GetAll(includeArchived)
        lists.Add(list.Load())
      Next
      Return lists.ToArray()
    End Function

    Public Function LoadAll(startIndex As Integer, maxCount As Integer, Optional includeArchived As Boolean = False) As IPersistentTrelloList() Implements IListsAccessContext.LoadAll
      Dim lists As New List(Of IPersistentTrelloList)
      For i As Integer = startIndex To (startIndex + (maxCount - 1))
        Dim list = Me.AtIndex(i, includeArchived)
        If (list IsNot Nothing) Then
          lists.Add(list.Load())
        End If
      Next
      Return lists.ToArray()
    End Function

#End Region

#Region " Move "

    Public Sub MoveAllTo(targetBoard As IBoardAccessContext) Implements IListsAccessContext.MoveAllTo
      _Api.ListRepository.MoveListsToBoard(targetBoard.Id, Me.GetIds())
    End Sub

    Public Sub MoveAllTo(targetBoardId As String) Implements IListsAccessContext.MoveAllTo
      _Api.ListRepository.MoveListsToBoard(targetBoardId, Me.GetIds())
    End Sub

    Public Sub MoveAllTo(targetBoard As IPersistentTrelloBoard) Implements IListsAccessContext.MoveAllTo
      _Api.ListRepository.MoveListsToBoard(targetBoard.Id, Me.GetIds())
    End Sub

#End Region

#Region " Query-Pipe (Filter) "

    Public Function WithName(listName As String) As IListsAccessContext Implements IListsAccessContext.WithName

      Return New ListQuery(
        _Api,
        Function(includeArchived)
          Dim myIds = Me.GetIds(includeArchived)
          Dim filteredIds = _Api.ListRepository.FilterListIdsByName(myIds, listName)
          Return filteredIds
        End Function
      )

    End Function

#End Region

  End Class

End Namespace
