
Namespace FluentAccess

  Public Interface IBoardContainerAccessContext
    Inherits IBoardsAccessContext

    Function CreateNew(name As String) As IBoardAccessContext
    Function CreateNew(contentSource As ITrelloBoardContent) As IBoardAccessContext

  End Interface

End Namespace
