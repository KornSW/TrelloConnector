
Namespace FluentAccess

  Public Interface IBoardsAccessContext

    Function GetIds(Optional includeArchived As Boolean = False) As String()
    Function GetAll(Optional includeArchived As Boolean = False) As IBoardAccessContext()
    Function LoadAll(Optional includeArchived As Boolean = False) As IPersistentTrelloBoard()

    Function Contains(board As IBoardAccessContext) As Boolean
    Function Contains(boardId As String) As Boolean
    Function Contains(board As IPersistentTrelloBoard) As Boolean

  End Interface

End Namespace
