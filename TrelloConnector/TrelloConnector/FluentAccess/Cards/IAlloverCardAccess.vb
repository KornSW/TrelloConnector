
Namespace FluentAccess

  Public Interface IAlloverCardAccess
    Inherits ICardsAccessContext

    Sub Save(changedItem As IPersistentTrelloCard)

    Function OfBoard(board As IBoardAccessContext) As ICardsAccessContext

    Function OfBoard(boardId As String) As ICardsAccessContext

    Function OfBoard(board As IPersistentTrelloBoard) As ICardsAccessContext


    Function OfList(list As IListAccessContext) As ICardContainerAccessContext

    Function OfList(listId As String) As ICardContainerAccessContext

    Function OfList(list As IPersistentTrelloList) As ICardContainerAccessContext


    Function OfCurrentMember() As ICardsAccessContext

    Function OfMember(member As IMemberAccessContext) As ICardsAccessContext

    Function OfMember(memberId As String) As ICardsAccessContext

    Function OfMember(member As IPersistentTrelloMember) As ICardsAccessContext


    Function WithLabel(label As ILabelAccessContext) As ICardsAccessContext

    Function WithLabel(labelId As String) As ICardsAccessContext

    Function WithLabel(label As IPersistentTrelloLabel) As ICardsAccessContext

  End Interface

End Namespace
