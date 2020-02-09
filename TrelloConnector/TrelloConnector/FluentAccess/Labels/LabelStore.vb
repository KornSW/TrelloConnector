Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq

Namespace FluentAccess

  Friend Class LabelStore
    Implements ILabelContainerAccessContext

    Private _Api As TrelloApi
    Private _ParentBoardId As String
    Private _BaseQuery As LabelQuery

    Public Sub New(api As TrelloApi, parentBoardId As String)
      _Api = api
      _ParentBoardId = parentBoardId
      _BaseQuery = New LabelQuery(_Api, Function(includeArchived) _Api.LabelRepository.GetLabelIdsByBoard(parentBoardId, includeArchived), _ParentBoardId)
    End Sub

    Public ReadOnly Property ParentBoardId As String
      Get
        Return _ParentBoardId
      End Get
    End Property

#Region " CreateNew (only on store) "

    Public Function CreateNew(name As String, Optional colorKey As String = TrelloLabelColors.None) As ILabelAccessContext Implements ILabelContainerAccessContext.CreateNew
      Dim newContent As New TrelloLabelContent With {
        .Name = name,
        .Color = colorKey
      }
      Return Me.CreateNew(newContent)
    End Function

    Public Function CreateNew(contentSource As ITrelloLabelContent) As ILabelAccessContext Implements ILabelContainerAccessContext.CreateNew
      Dim newlyCreatedId As String = _Api.LabelRepository.CreateNewLabel(_ParentBoardId, contentSource)
      Return _Api.Labels(newlyCreatedId)
    End Function

#End Region

#Region " Access (proxy to BaseQuery) "

    Public Function GetAll(Optional includeArchived As Boolean = False) As ILabelAccessContext() Implements ILabelsAccessContext.GetAll
      Return _BaseQuery.GetAll(includeArchived)
    End Function

    Public Function GetIds(Optional includeArchived As Boolean = False) As String() Implements ILabelsAccessContext.GetIds
      Return _BaseQuery.GetIds(includeArchived)
    End Function

    Public Function LoadAll(Optional includeArchived As Boolean = False) As IPersistentTrelloLabel() Implements ILabelsAccessContext.LoadAll
      Return _BaseQuery.LoadAll(includeArchived)
    End Function

    Public Function Contains(label As IPersistentTrelloLabel) As Boolean Implements ILabelsAccessContext.Contains
      Return _BaseQuery.Contains(label)
    End Function

    Public Function Contains(labelId As String) As Boolean Implements ILabelsAccessContext.Contains
      Return _BaseQuery.Contains(labelId)
    End Function

    Public Function Contains(label As ILabelAccessContext) As Boolean Implements ILabelsAccessContext.Contains
      Return _BaseQuery.Contains(label)
    End Function

    Public Function First(Optional includeArchived As Boolean = False) As ILabelAccessContext Implements ILabelsAccessContext.First
      Return _BaseQuery.First(includeArchived)
    End Function

    Public Function AtIndex(index As Integer, Optional includeArchived As Boolean = False) As ILabelAccessContext Implements ILabelsAccessContext.AtIndex
      Return _BaseQuery.AtIndex(index, includeArchived)
    End Function

    Public Function Last(Optional includeArchived As Boolean = False) As ILabelAccessContext Implements ILabelsAccessContext.Last
      Return _BaseQuery.Last(includeArchived)
    End Function

    Public Function LoadAll(startIndex As Integer, maxCount As Integer, Optional includeArchived As Boolean = False) As IPersistentTrelloLabel() Implements ILabelsAccessContext.LoadAll
      Return _BaseQuery.LoadAll(startIndex, maxCount, includeArchived)
    End Function

    Public Function WithName(labelName As String) As ILabelsAccessContext Implements ILabelsAccessContext.WithName
      Return _BaseQuery.WithName(labelName)
    End Function

#End Region

  End Class

End Namespace
