Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq

Namespace FluentAccess

  <DebuggerDisplay("Label {Id}")>
  Friend Class LabelRecordHandle
    Implements ILabelAccessContext

    Private _Api As TrelloApi
    Private _LabelId As String
    Private _BoardId As String = Nothing

    Public Sub New(api As TrelloApi, labelId As String, Optional alreadyDiscoveredBoardId As String = Nothing)
      _Api = api
      _LabelId = labelId
      _BoardId = alreadyDiscoveredBoardId
    End Sub

    Public ReadOnly Property Id As String Implements ILabelAccessContext.Id
      Get
        Return _LabelId
      End Get
    End Property

#Region " Exisits & Archive "

    Public Function Exists(Optional includeArchived As Boolean = False) As Boolean Implements ILabelAccessContext.Exists
      Return _Api.LabelRepository.LabelExists(_LabelId, includeArchived)
    End Function

    Public Function IsArchived() As Boolean Implements ILabelAccessContext.IsArchived
      Return _Api.LabelRepository.IsLabelArchived(_LabelId)
    End Function

    Public Sub Archive() Implements ILabelAccessContext.Archive
      _Api.LabelRepository.ArchiveLabelsById(_LabelId)
    End Sub

#End Region

#Region " Load / Update "

    Public Function Load() As IPersistentTrelloLabel Implements ILabelAccessContext.Load
      Return _Api.LabelRepository.LoadLabelById(_LabelId)
    End Function

    Public Sub LoadInto(target As ITrelloLabelContent) Implements ILabelAccessContext.LoadInto
      _Api.LabelRepository.LoadLabelByIdInto(_LabelId, target)
    End Sub

    Public Sub SaveContentFrom(contentSource As ITrelloLabelContent) Implements ILabelAccessContext.SaveContentFrom
      _Api.LabelRepository.SaveLabelContent(_LabelId, contentSource)
    End Sub

#End Region

#Region " Principal/Dependent-Navigation "

    Public ReadOnly Property Board As IBoardAccessContext Implements ILabelAccessContext.Board
      Get
        If (_BoardId Is Nothing) Then
          _BoardId = _Api.BoardRepository.GetBoardIdContainingLabel(_LabelId)
        End If
        Return New BoardRecordHandle(_Api, _BoardId)
      End Get
    End Property

#End Region

#Region " Referrer-Navigation -> Cards "

    Public Function AssignedCards() As ICardsAccessContext Implements ILabelAccessContext.AssignedCards
      Return New CardQuery(_Api, Function(includeArchived) _Api.CardRepository.GetCardIdsByAssigendLabel(_LabelId, includeArchived))
    End Function

#End Region

#Region " Move "

    Public Sub MoveTo(targetBoard As IBoardAccessContext) Implements ILabelAccessContext.MoveTo
      _Api.LabelRepository.MoveLabelsToBoard(targetBoard.Id, _LabelId)
    End Sub

    Public Sub MoveTo(targetBoardId As String) Implements ILabelAccessContext.MoveTo
      _Api.LabelRepository.MoveLabelsToBoard(targetBoardId, _LabelId)
    End Sub

    Public Sub MoveTo(targetBoard As IPersistentTrelloBoard) Implements ILabelAccessContext.MoveTo
      _Api.LabelRepository.MoveLabelsToBoard(targetBoard.Id, _LabelId)
    End Sub

#End Region

  End Class

End Namespace
