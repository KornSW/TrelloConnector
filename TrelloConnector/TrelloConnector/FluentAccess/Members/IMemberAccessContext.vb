
Namespace FluentAccess

  Public Interface IMemberAccessContext

    'Identity
    ReadOnly Property Id As String
    Function Exists(Optional includeArchived As Boolean = False) As Boolean

    'Editing
    Function Load() As IPersistentTrelloMember
    Sub LoadInto(target As ITrelloMemberContent)
    Sub SaveContentFrom(contentSource As ITrelloMemberContent)

    'Referrer-Navigation -> Cards
    Function AssignedCards() As ICardsAccessContext

    'Referrer-Navigation -> Boards (open)

    Function AssignedBoards() As IBoardsAccessContext

    Function IsAssignedToBoard(board As IBoardAccessContext) As Boolean
    Function IsAssignedToBoard(boardId As String) As Boolean
    Function IsAssignedToBoard(board As IPersistentTrelloBoard) As Boolean

    Sub AssignBoard(board As IBoardAccessContext)
    Sub AssignBoard(boardId As String)
    Sub AssignBoard(board As IPersistentTrelloBoard)

    Sub UnassignBoard(board As IBoardAccessContext)
    Sub UnassignBoard(boardId As String)
    Sub UnassignBoard(board As IPersistentTrelloBoard)

  End Interface

End Namespace
