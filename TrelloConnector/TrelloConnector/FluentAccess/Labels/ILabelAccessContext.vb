
Namespace FluentAccess

  Public Interface ILabelAccessContext

    'Identity
    ReadOnly Property Id As String
    Function Exists(Optional includeArchived As Boolean = False) As Boolean
    Function IsArchived() As Boolean

    'Editing
    Function Load() As IPersistentTrelloLabel
    Sub LoadInto(target As ITrelloLabelContent)
    Sub SaveContentFrom(contentSource As ITrelloLabelContent)

    'Move
    Sub MoveTo(targetBoard As IBoardAccessContext)
    Sub MoveTo(targetBoardId As String)
    Sub MoveTo(targetBoard As IPersistentTrelloBoard)

    'Archive
    Sub Archive()

    'Principal/Dependent-Navigation
    ReadOnly Property Board() As IBoardAccessContext

    'Referrer-Navigation -> Cards
    Function AssignedCards() As ICardsAccessContext

  End Interface

End Namespace
