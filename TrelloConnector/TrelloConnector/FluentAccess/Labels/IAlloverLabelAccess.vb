
Namespace FluentAccess

  Public Interface IAlloverLabelAccess
    Inherits ILabelsAccessContext

    Sub Save(changedItem As IPersistentTrelloLabel)

    Function OfBoard(board As IBoardAccessContext) As ILabelContainerAccessContext

    Function OfBoard(boardId As String) As ILabelContainerAccessContext

    Function OfBoard(board As IPersistentTrelloBoard) As ILabelContainerAccessContext

    Function OfCard(card As ICardAccessContext) As ILabelsAccessContext

    Function OfCard(cardId As String) As ILabelsAccessContext

    Function OfCard(card As IPersistentTrelloCard) As ILabelsAccessContext

  End Interface

End Namespace
