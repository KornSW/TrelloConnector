
Namespace FluentAccess

  Public Interface IAlloverMemberAccess

    ReadOnly Property Current() As IMemberAccessContext

    Sub Save(changedItem As IPersistentTrelloMember)

    Function OfCard(card As ICardAccessContext) As IMembersAccessContext

    Function OfCard(cardId As String) As IMembersAccessContext

    Function OfCard(card As IPersistentTrelloCard) As IMembersAccessContext

    Function OfBoard(board As IBoardAccessContext) As IMembersAccessContext

    Function OfBoard(boardId As String) As IMembersAccessContext

    Function OfBoard(board As IPersistentTrelloBoard) As IMembersAccessContext

  End Interface

End Namespace
