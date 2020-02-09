
Namespace FluentAccess

  Public Interface IListAccessContext

    'Identity
    ReadOnly Property Id As String
    Function Exists(Optional includeArchived As Boolean = False) As Boolean
    Function IsArchived() As Boolean

    'Editing
    Function Load() As IPersistentTrelloList
    Sub LoadInto(target As ITrelloListContent)
    Sub SaveContentFrom(contentSource As ITrelloListContent)

    'Archive
    Sub Archive()

    'Move
    Sub MoveTo(targetBoard As IBoardAccessContext)
    Sub MoveTo(targetBoardId As String)
    Sub MoveTo(targetBoard As IPersistentTrelloBoard)

    'Principal/Dependent-Navigation
    ReadOnly Property Board() As IBoardAccessContext

    ReadOnly Property Cards As ICardContainerAccessContext

  End Interface

End Namespace
