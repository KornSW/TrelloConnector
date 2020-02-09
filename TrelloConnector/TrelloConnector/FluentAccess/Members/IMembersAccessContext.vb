
Namespace FluentAccess

  Public Interface IMembersAccessContext

    Function GetIds(Optional includeArchived As Boolean = False) As String()
    Function GetAll(Optional includeArchived As Boolean = False) As IMemberAccessContext()
    Function LoadAll(Optional includeArchived As Boolean = False) As IPersistentTrelloMember()

    Function Contains(member As IMemberAccessContext) As Boolean
    Function Contains(memberId As String) As Boolean
    Function Contains(member As IPersistentTrelloMember) As Boolean

  End Interface

End Namespace
