Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq

Namespace FluentAccess

  Friend Class BoardStore
    Implements IAlloverBoardAccess

    Private _Api As TrelloApi
    Private _BaseQuery As BoardQuery

    Public Sub New(api As TrelloApi)
      _Api = api
      _BaseQuery = New BoardQuery(_Api, Function(includeArchived) _Api.BoardRepository.GetAllIds(includeArchived))
    End Sub

#Region " ROOT "

    Public Sub Save(changedItem As IPersistentTrelloBoard) Implements IAlloverBoardAccess.Save
      _Api.Boards(changedItem).SaveContentFrom(changedItem)
    End Sub

#Region " OfMember "

    Public Function OfCurrentMember() As IBoardsAccessContext Implements IAlloverBoardAccess.OfCurrentMember
      Return _Api.Members.Current.AssignedBoards
    End Function

    Public Function OfMember(member As IMemberAccessContext) As IBoardsAccessContext Implements IAlloverBoardAccess.OfMember
      Return member.AssignedBoards
    End Function

    Public Function OfMember(memberId As String) As IBoardsAccessContext Implements IAlloverBoardAccess.OfMember
      Return _Api.Members(memberId).AssignedBoards
    End Function

    Public Function OfMember(member As IPersistentTrelloMember) As IBoardsAccessContext Implements IAlloverBoardAccess.OfMember
      Return _Api.Members(member).AssignedBoards
    End Function

#End Region

#End Region

#Region " CreateNew (only on store) "

    Public Function CreateNew(name As String) As IBoardAccessContext Implements IBoardContainerAccessContext.CreateNew
      Dim newContent As New TrelloBoardContent With {
        .Name = name
      }
      Return Me.CreateNew(newContent)
    End Function

    Public Function CreateNew(contentSource As ITrelloBoardContent) As IBoardAccessContext Implements IBoardContainerAccessContext.CreateNew
      Dim newlyCreatedId As String = _Api.BoardRepository.CreateNewBoard(contentSource)
      Return _Api.Boards(newlyCreatedId)
    End Function

#End Region

#Region " Access (proxy to BaseQuery) "

    Public Function GetAll(Optional includeArchived As Boolean = False) As IBoardAccessContext() Implements IBoardsAccessContext.GetAll
      Return _BaseQuery.GetAll(includeArchived)
    End Function

    Public Function GetIds(Optional includeArchived As Boolean = False) As String() Implements IBoardsAccessContext.GetIds
      Return _BaseQuery.GetIds(includeArchived)
    End Function

    Public Function LoadAll(Optional includeArchived As Boolean = False) As IPersistentTrelloBoard() Implements IBoardsAccessContext.LoadAll
      Return _BaseQuery.LoadAll(includeArchived)
    End Function

    Public Function Contains(board As IPersistentTrelloBoard) As Boolean Implements IBoardsAccessContext.Contains
      Return _BaseQuery.Contains(board)
    End Function

    Public Function Contains(boardId As String) As Boolean Implements IBoardsAccessContext.Contains
      Return _BaseQuery.Contains(boardId)
    End Function

    Public Function Contains(board As IBoardAccessContext) As Boolean Implements IBoardsAccessContext.Contains
      Return _BaseQuery.Contains(board)
    End Function

#End Region

  End Class

End Namespace
