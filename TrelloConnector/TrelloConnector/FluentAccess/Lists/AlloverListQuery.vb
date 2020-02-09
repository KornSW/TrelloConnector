Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq

Namespace FluentAccess

  Friend Class AlloverListQuery
    Inherits ListQuery
    Implements IAlloverListAccess

    Public Sub New(api As TrelloApi)
      MyBase.New(api, Function(includeArchived) api.ListRepository.GetAllIds(includeArchived))
    End Sub

    Public Sub Save(changedItem As IPersistentTrelloList) Implements IAlloverListAccess.Save
      Me.Api.Lists(changedItem).SaveContentFrom(changedItem)
    End Sub

#Region " OfBoard "

    Public Function OfBoard(board As IPersistentTrelloBoard) As IListContainerAccessContext Implements IAlloverListAccess.OfBoard
      Return Me.Api.Boards(board.Id).Lists
    End Function

    Public Function OfBoard(boardId As String) As IListContainerAccessContext Implements IAlloverListAccess.OfBoard
      Return Me.Api.Boards(boardId).Lists
    End Function

    Public Function OfBoard(board As IBoardAccessContext) As IListContainerAccessContext Implements IAlloverListAccess.OfBoard
      Return board.Lists
    End Function

#End Region

  End Class

End Namespace
