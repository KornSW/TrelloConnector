Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq

Namespace FluentAccess

  Friend Class LabelQuery
    Implements ILabelsAccessContext

    Private _Api As TrelloApi
    Private _IdGetter As Func(Of Boolean, String())
    Private _OriginBoardId As String = Nothing

    Public Sub New(api As TrelloApi, idGetter As Func(Of Boolean, String()), Optional originBoardId As String = Nothing)
      _Api = api
      _IdGetter = idGetter
      _OriginBoardId = originBoardId
    End Sub

    Protected ReadOnly Property Api As TrelloApi
      Get
        Return _Api
      End Get
    End Property

    Public Function GetIds(Optional includeArchived As Boolean = False) As String() Implements ILabelsAccessContext.GetIds
      Return _IdGetter.Invoke(includeArchived)
    End Function

    Public Function GetAll(Optional includeArchived As Boolean = False) As ILabelAccessContext() Implements ILabelsAccessContext.GetAll
      Return _IdGetter.Invoke(includeArchived).Select(Function(id) New LabelRecordHandle(_Api, id, _OriginBoardId)).ToArray()
    End Function

#Region " Contains "

    Public Function Contains(labelId As String) As Boolean Implements ILabelsAccessContext.Contains
      Return Me.GetIds(True).Contains(labelId)
    End Function

    Public Function Contains(label As IPersistentTrelloLabel) As Boolean Implements ILabelsAccessContext.Contains
      Return Me.Contains(label.Id)
    End Function

    Public Function Contains(label As ILabelAccessContext) As Boolean Implements ILabelsAccessContext.Contains
      Return Me.Contains(label.Id)
    End Function

#End Region

#Region " Position based Picking "

    Public Function First(Optional includeArchived As Boolean = False) As ILabelAccessContext Implements ILabelsAccessContext.First
      Return Me.AtIndex(0, includeArchived)
    End Function

    Public Function AtIndex(index As Integer, Optional includeArchived As Boolean = False) As ILabelAccessContext Implements ILabelsAccessContext.AtIndex
      Dim allIds = Me.GetIds()
      If (allIds.Length <= index) Then
        Return Nothing
      End If
      Return New LabelRecordHandle(_Api, allIds(index), _OriginBoardId)
    End Function

    Public Function Last(Optional includeArchived As Boolean = False) As ILabelAccessContext Implements ILabelsAccessContext.Last
      Dim allIds = Me.GetIds()
      Dim lastIndex = allIds.Count() - 1
      If (lastIndex < 0) Then
        Return Nothing
      End If
      Return New LabelRecordHandle(_Api, allIds(lastIndex), _OriginBoardId)
    End Function

#End Region

#Region " Load "

    Public Function LoadAll(Optional includeArchived As Boolean = False) As IPersistentTrelloLabel() Implements ILabelsAccessContext.LoadAll
      Dim labels As New List(Of IPersistentTrelloLabel)
      For Each label In Me.GetAll(includeArchived)
        labels.Add(label.Load())
      Next
      Return labels.ToArray()
    End Function

    Public Function LoadAll(startIndex As Integer, maxCount As Integer, Optional includeArchived As Boolean = False) As IPersistentTrelloLabel() Implements ILabelsAccessContext.LoadAll
      Dim labels As New List(Of IPersistentTrelloLabel)
      For i As Integer = startIndex To (startIndex + (maxCount - 1))
        Dim label = Me.AtIndex(i, includeArchived)
        If (label IsNot Nothing) Then
          labels.Add(label.Load())
        End If
      Next
      Return labels.ToArray()
    End Function

#End Region

#Region " Query-Pipe (Filter) "

    Public Function WithName(labelName As String) As ILabelsAccessContext Implements ILabelsAccessContext.WithName

      Return New LabelQuery(
        _Api,
        Function(includeArchived)
          Dim myIds = Me.GetIds(includeArchived)
          Dim filteredIds = _Api.LabelRepository.FilterLabelIdsByName(myIds, labelName)
          Return filteredIds
        End Function
      )

    End Function

#End Region

  End Class

End Namespace
