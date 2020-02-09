Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq

Namespace FluentAccess

  Friend Class AlloverLabelQuery
    Inherits LabelQuery
    Implements IAlloverLabelAccess

    Public Sub New(api As TrelloApi)
      MyBase.New(api, Function(includeArchived) api.ListRepository.GetAllIds(includeArchived))
    End Sub

    Public Sub Save(changedItem As IPersistentTrelloLabel) Implements IAlloverLabelAccess.Save
      Me.Api.Labels(changedItem.Id).SaveContentFrom(changedItem)
    End Sub

#Region " OfBoard "

    Public Function OfBoard(board As IBoardAccessContext) As ILabelContainerAccessContext Implements IAlloverLabelAccess.OfBoard
      Return board.Labels
    End Function

    Public Function OfBoard(board As IPersistentTrelloBoard) As ILabelContainerAccessContext Implements IAlloverLabelAccess.OfBoard
      Return Me.Api.Boards(board.Id).Labels
    End Function

    Public Function OfBoard(boardId As String) As ILabelContainerAccessContext Implements IAlloverLabelAccess.OfBoard
      Return Me.Api.Boards(boardId).Labels
    End Function

#End Region

#Region " OfCard "

    Public Function OfCard(card As IPersistentTrelloCard) As ILabelsAccessContext Implements IAlloverLabelAccess.OfCard
      Return Me.Api.Cards(card).AssignedLabels()
    End Function

    Public Function OfCard(cardId As String) As ILabelsAccessContext Implements IAlloverLabelAccess.OfCard
      Return Me.Api.Cards(cardId).AssignedLabels()
    End Function

    Public Function OfCard(card As ICardAccessContext) As ILabelsAccessContext Implements IAlloverLabelAccess.OfCard
      Return card.AssignedLabels()
    End Function

#End Region

  End Class

End Namespace
