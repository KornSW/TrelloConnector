
Imports System

Namespace FluentAccess

  Public Interface IListContainerAccessContext
    Inherits IListsAccessContext

    Function CreateNew(name As String) As IListAccessContext
    Function CreateNew(contentSource As ITrelloListContent) As IListAccessContext

  End Interface

End Namespace
