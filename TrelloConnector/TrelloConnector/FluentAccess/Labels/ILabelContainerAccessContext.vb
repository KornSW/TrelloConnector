
Namespace FluentAccess

  Public Interface ILabelContainerAccessContext
    Inherits ILabelsAccessContext

    Function CreateNew(name As String, Optional colorKey As String = TrelloLabelColors.None) As ILabelAccessContext
    Function CreateNew(contentSource As ITrelloLabelContent) As ILabelAccessContext

  End Interface

End Namespace
