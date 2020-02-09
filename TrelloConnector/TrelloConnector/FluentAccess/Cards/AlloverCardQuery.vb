Imports System
Imports System.Collections.Generic
Imports System.Linq

Namespace FluentAccess

  Friend Class AlloverCardQuery
    Inherits CardQuery
    Implements IAlloverCardAccess

    Public Sub New(api As TrelloApi)
      MyBase.New(api, Function(includeArchived) api.CardRepository.GetAllIds(includeArchived))
    End Sub

    Public Sub Save(changedItem As IPersistentTrelloCard) Implements IAlloverCardAccess.Save
      Me.Api.Cards(changedItem.Id).SaveContentFrom(changedItem)
    End Sub

#Region " OfMember "

    Public Function OfCurrentMember() As ICardsAccessContext Implements IAlloverCardAccess.OfCurrentMember
      Return Me.Api.Members.Current.AssignedCards
    End Function

    Public Function OfMember(member As IMemberAccessContext) As ICardsAccessContext Implements IAlloverCardAccess.OfMember
      Return member.AssignedCards()
    End Function

    Public Function OfMember(memberId As String) As ICardsAccessContext Implements IAlloverCardAccess.OfMember
      Return Me.Api.Members(memberId).AssignedCards()
    End Function

    Public Function OfMember(member As IPersistentTrelloMember) As ICardsAccessContext Implements IAlloverCardAccess.OfMember
      Return Me.Api.Members(member).AssignedCards()
    End Function

#End Region

#Region " OfBoard "

    Public Function OfBoard(board As IPersistentTrelloBoard) As ICardsAccessContext Implements IAlloverCardAccess.OfBoard
      Return Me.Api.Boards(board.Id).Cards
    End Function

    Public Function OfBoard(boardId As String) As ICardsAccessContext Implements IAlloverCardAccess.OfBoard
      Return Me.Api.Boards(boardId).Cards
    End Function

    Public Function OfBoard(board As IBoardAccessContext) As ICardsAccessContext Implements IAlloverCardAccess.OfBoard
      Return board.Cards
    End Function

#End Region

#Region " OfList "

    Public Function OfList(list As IPersistentTrelloList) As ICardContainerAccessContext Implements IAlloverCardAccess.OfList
      Return Me.Api.Lists(list.Id).Cards
    End Function

    Public Function OfList(listId As String) As ICardContainerAccessContext Implements IAlloverCardAccess.OfList
      Return Me.Api.Lists(listId).Cards
    End Function

    Public Function OfList(list As IListAccessContext) As ICardContainerAccessContext Implements IAlloverCardAccess.OfList
      Return list.Cards
    End Function

#End Region

#Region " WithLabel "

    Public Function WithLabel(label As ILabelAccessContext) As ICardsAccessContext Implements IAlloverCardAccess.WithLabel
      Return label.AssignedCards()
    End Function

    Public Function WithLabel(labelId As String) As ICardsAccessContext Implements IAlloverCardAccess.WithLabel
      Return Me.Api.Labels(labelId).AssignedCards()
    End Function

    Public Function WithLabel(label As IPersistentTrelloLabel) As ICardsAccessContext Implements IAlloverCardAccess.WithLabel
      Return Me.Api.Labels(label).AssignedCards()
    End Function

#End Region

  End Class

End Namespace
