
Namespace FluentAccess

  Public Interface IAlloverBoardAccess
    Inherits IBoardContainerAccessContext

    Sub Save(changedItem As IPersistentTrelloBoard)

    Function OfCurrentMember() As IBoardsAccessContext

    Function OfMember(member As IMemberAccessContext) As IBoardsAccessContext

    Function OfMember(memberId As String) As IBoardsAccessContext

    Function OfMember(member As IPersistentTrelloMember) As IBoardsAccessContext

  End Interface

End Namespace
