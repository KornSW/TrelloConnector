
Imports System

Namespace FluentAccess

  Public Interface IAlloverListAccess
    Inherits IListsAccessContext

    Sub Save(changedItem As IPersistentTrelloList)

    Function OfBoard(board As IBoardAccessContext) As IListContainerAccessContext

    Function OfBoard(boardId As String) As IListContainerAccessContext

    Function OfBoard(board As IPersistentTrelloBoard) As IListContainerAccessContext

  End Interface

End Namespace
