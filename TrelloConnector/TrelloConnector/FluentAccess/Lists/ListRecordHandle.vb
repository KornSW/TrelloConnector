Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq

Namespace FluentAccess

  <DebuggerDisplay("List {Id}")>
  Friend Class ListRecordHandle
    Implements IListAccessContext

    Private _Api As TrelloApi
    Private _ListId As String
    Private _BoardId As String = Nothing

    Public Sub New(api As TrelloApi, listId As String, Optional alreadyDiscoveredBoardId As String = Nothing)
      _Api = api
      _ListId = listId
      _BoardId = alreadyDiscoveredBoardId
    End Sub

    Public ReadOnly Property Id As String Implements IListAccessContext.Id
      Get
        Return _ListId
      End Get
    End Property

#Region " Exisits & Archive "

    Public Function Exists(Optional includeArchived As Boolean = False) As Boolean Implements IListAccessContext.Exists
      Return _Api.ListRepository.ListExists(_ListId, includeArchived)
    End Function

    Public Function IsArchived() As Boolean Implements IListAccessContext.IsArchived
      Return _Api.ListRepository.IsListArchived(_ListId)
    End Function

    Public Sub Archive() Implements IListAccessContext.Archive
      _Api.ListRepository.ArchiveListsById(_ListId)
    End Sub

#End Region

#Region " Load / Update "

    Public Function Load() As IPersistentTrelloList Implements IListAccessContext.Load
      Return _Api.ListRepository.LoadListById(_ListId)
    End Function

    Public Sub LoadInto(target As ITrelloListContent) Implements IListAccessContext.LoadInto
      _Api.ListRepository.LoadListByIdInto(_ListId, target)
    End Sub

    Public Sub SaveContentFrom(contentSource As ITrelloListContent) Implements IListAccessContext.SaveContentFrom
      _Api.ListRepository.SaveListContent(_ListId, contentSource)
    End Sub

#End Region

#Region " Principal/Dependent-Navigation "

    Public ReadOnly Property Board As IBoardAccessContext Implements IListAccessContext.Board
      Get
        If (_BoardId Is Nothing) Then
          _BoardId = _Api.BoardRepository.GetBoardIdContainingList(_ListId)
        End If
        Return New BoardRecordHandle(_Api, _BoardId)
      End Get
    End Property

    Public ReadOnly Property Cards As ICardContainerAccessContext Implements IListAccessContext.Cards
      Get
        Return _Api.CardStorePerListId(_ListId)
      End Get
    End Property

#End Region

#Region " Move "

    Public Sub MoveTo(targetBoard As IBoardAccessContext) Implements IListAccessContext.MoveTo
      _Api.ListRepository.MoveListsToBoard(targetBoard.Id, _ListId)
    End Sub

    Public Sub MoveTo(targetBoardId As String) Implements IListAccessContext.MoveTo
      _Api.ListRepository.MoveListsToBoard(targetBoardId, _ListId)
    End Sub

    Public Sub MoveTo(targetBoard As IPersistentTrelloBoard) Implements IListAccessContext.MoveTo
      _Api.ListRepository.MoveListsToBoard(targetBoard.Id, _ListId)
    End Sub

#End Region

  End Class

End Namespace
