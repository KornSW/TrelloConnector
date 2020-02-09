Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq

Namespace FluentAccess

  Friend Class BoardQuery
    Implements IBoardsAccessContext

    Private _Api As TrelloApi
    Private _IdGetter As Func(Of Boolean, String())

    Public Sub New(api As TrelloApi, idGetter As Func(Of Boolean, String()))
      _Api = api
      _IdGetter = idGetter
    End Sub

    Protected ReadOnly Property Api As TrelloApi
      Get
        Return _Api
      End Get
    End Property

    Public Function GetIds(Optional includeArchived As Boolean = False) As String() Implements IBoardsAccessContext.GetIds
      Return _IdGetter.Invoke(includeArchived)
    End Function

    Public Function GetAll(Optional includeArchived As Boolean = False) As IBoardAccessContext() Implements IBoardsAccessContext.GetAll
      Return _IdGetter.Invoke(includeArchived).Select(Function(id) New BoardRecordHandle(_Api, id)).ToArray()
    End Function

#Region " Contains "

    Public Function Contains(boardId As String) As Boolean Implements IBoardsAccessContext.Contains
      Return Me.GetIds(True).Contains(boardId)
    End Function

    Public Function Contains(board As IPersistentTrelloBoard) As Boolean Implements IBoardsAccessContext.Contains
      Return Me.Contains(board.Id)
    End Function

    Public Function Contains(board As IBoardAccessContext) As Boolean Implements IBoardsAccessContext.Contains
      Return Me.Contains(board.Id)
    End Function

#End Region

#Region " Load "

    Public Function LoadAll(Optional includeArchived As Boolean = False) As IPersistentTrelloBoard() Implements IBoardsAccessContext.LoadAll
      Dim boards As New List(Of IPersistentTrelloBoard)
      For Each board In Me.GetAll(includeArchived)
        boards.Add(board.Load())
      Next
      Return boards.ToArray()
    End Function

#End Region

  End Class

End Namespace
