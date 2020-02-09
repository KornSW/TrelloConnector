
Namespace FluentAccess

  Public Interface ICardsAccessContext

    Function GetIds(Optional includeArchived As Boolean = False) As String()
    Function GetAll(Optional includeArchived As Boolean = False) As ICardAccessContext()
    Function LoadAll(Optional includeArchived As Boolean = False) As IPersistentTrelloCard()
    Function LoadAll(startIndex As Integer, maxCount As Integer, Optional includeArchived As Boolean = False) As IPersistentTrelloCard()

    Function Contains(card As ICardAccessContext) As Boolean
    Function Contains(cardId As String) As Boolean
    Function Contains(card As IPersistentTrelloCard) As Boolean

    Function First(Optional includeArchived As Boolean = False) As ICardAccessContext
    ''' <summary> access to an non existent index returns nothing</summary>
    Function AtIndex(index As Integer, Optional includeArchived As Boolean = False) As ICardAccessContext
    Function Last(Optional includeArchived As Boolean = False) As ICardAccessContext

    'Move
    Sub MoveAllTo(targetList As IListAccessContext)
    Sub MoveAllTo(targetListId As String)
    Sub MoveAllTo(targetList As IPersistentTrelloList)

    Sub ArchiveAll()

  End Interface

End Namespace
