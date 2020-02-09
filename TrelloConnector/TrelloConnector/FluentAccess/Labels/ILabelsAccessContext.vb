
Namespace FluentAccess

  Public Interface ILabelsAccessContext

    Function GetIds(Optional includeArchived As Boolean = False) As String()
    Function GetAll(Optional includeArchived As Boolean = False) As ILabelAccessContext()
    Function LoadAll(Optional includeArchived As Boolean = False) As IPersistentTrelloLabel()
    Function LoadAll(startIndex As Integer, maxCount As Integer, Optional includeArchived As Boolean = False) As IPersistentTrelloLabel()

    Function Contains(label As ILabelAccessContext) As Boolean
    Function Contains(labelId As String) As Boolean
    Function Contains(label As IPersistentTrelloLabel) As Boolean

    Function First(Optional includeArchived As Boolean = False) As ILabelAccessContext
    ''' <summary> access to an non existent index returns nothing</summary>
    Function AtIndex(index As Integer, Optional includeArchived As Boolean = False) As ILabelAccessContext
    Function Last(Optional includeArchived As Boolean = False) As ILabelAccessContext

    'Filtering
    Function WithName(labelName As String) As ILabelsAccessContext

  End Interface

End Namespace
