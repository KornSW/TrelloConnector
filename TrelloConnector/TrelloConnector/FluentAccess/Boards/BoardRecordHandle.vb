Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq

Namespace FluentAccess

  <DebuggerDisplay("Board {Id}")>
  Friend Class BoardRecordHandle
    Implements IBoardAccessContext

    Private _Api As TrelloApi
    Private _BoardId As String

    Public Sub New(api As TrelloApi, boardId As String)
      _Api = api
      _BoardId = boardId
    End Sub

    Public ReadOnly Property Id As String Implements IBoardAccessContext.Id
      Get
        Return _BoardId
      End Get
    End Property

#Region " Exisits & Archive "

    Public Function Exists(Optional includeArchived As Boolean = False) As Boolean Implements IBoardAccessContext.Exists
      Return _Api.BoardRepository.BoardExists(_BoardId, includeArchived)
    End Function

    Public Function IsArchived() As Boolean Implements IBoardAccessContext.IsArchived
      Return _Api.BoardRepository.IsBoardArchived(_BoardId)
    End Function

    Public Sub Archive() Implements IBoardAccessContext.Archive
      _Api.BoardRepository.ArchiveBoardsById(_BoardId)
    End Sub

#End Region

#Region " Load / Update "

    Public Function Load() As IPersistentTrelloBoard Implements IBoardAccessContext.Load
      Return _Api.BoardRepository.LoadBoardById(_BoardId)
    End Function

    Public Sub LoadInto(target As ITrelloBoardContent) Implements IBoardAccessContext.LoadInto
      _Api.BoardRepository.LoadBoardByIdInto(_BoardId, target)
    End Sub

    Public Sub SaveContentFrom(contentSource As ITrelloBoardContent) Implements IBoardAccessContext.SaveContentFrom
      _Api.BoardRepository.SaveBoardContent(_BoardId, contentSource)
    End Sub

#End Region

#Region " Principal/Dependent-Navigation "

    Public ReadOnly Property Lists As IListContainerAccessContext Implements IBoardAccessContext.Lists
      Get
        Return _Api.ListStorePerBoardId(_BoardId)
      End Get
    End Property

    Public ReadOnly Property Labels As ILabelContainerAccessContext Implements IBoardAccessContext.Labels
      Get
        Return _Api.LabelStorePerBoardId(_BoardId)
      End Get
    End Property

    Public ReadOnly Property Cards As ICardsAccessContext Implements IBoardAccessContext.Cards
      Get
        Return New CardQuery(_Api, Function(includeArchived) _Api.CardRepository.GetCardIdsByBoard(_BoardId, includeArchived))
      End Get
    End Property

#End Region

#Region " Referrer-Navigation -> Opened by Member "

    Public Function AccessingMembers() As IMembersAccessContext Implements IBoardAccessContext.AccessingMembers
      Return New MemberQuery(_Api, Function(includeArchived) _Api.MemberRepository.GetMemberIdsByAssignedBoard(_BoardId, includeArchived))
    End Function

#End Region

  End Class

End Namespace
