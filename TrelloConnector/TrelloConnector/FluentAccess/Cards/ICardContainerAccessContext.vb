
Namespace FluentAccess

  Public Interface ICardContainerAccessContext
    Inherits ICardsAccessContext

    Function CreateNew(name As String) As ICardAccessContext
    Function CreateNew(contentSource As ITrelloCardContent) As ICardAccessContext

  End Interface

End Namespace
