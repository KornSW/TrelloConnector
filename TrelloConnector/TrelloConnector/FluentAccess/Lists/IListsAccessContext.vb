
Namespace FluentAccess

  Public Interface IListsAccessContext

    Function GetIds(Optional includeArchived As Boolean = False) As String()
    Function GetAll(Optional includeArchived As Boolean = False) As IListAccessContext()
    Function LoadAll(Optional includeArchived As Boolean = False) As IPersistentTrelloList()
    Function LoadAll(startIndex As Integer, maxCount As Integer, Optional includeArchived As Boolean = False) As IPersistentTrelloList()

    Function Contains(list As IListAccessContext) As Boolean
    Function Contains(listId As String) As Boolean
    Function Contains(list As IPersistentTrelloList) As Boolean

    Function First(Optional includeArchived As Boolean = False) As IListAccessContext
    ''' <summary> access to an non existent index returns nothing</summary>
    Function AtIndex(index As Integer, Optional includeArchived As Boolean = False) As IListAccessContext
    Function Last(Optional includeArchived As Boolean = False) As IListAccessContext

    'Move
    Sub MoveAllTo(targetBoard As IBoardAccessContext)
    Sub MoveAllTo(targetBoardId As String)
    Sub MoveAllTo(targetBoard As IPersistentTrelloBoard)

    'Filtering
    Function WithName(listName As String) As IListsAccessContext

  End Interface

End Namespace
