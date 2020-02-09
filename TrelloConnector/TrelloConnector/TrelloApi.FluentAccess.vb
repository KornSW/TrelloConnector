Imports System
Imports System.Collections.Generic
Imports TrelloConnector.FluentAccess

Partial Class TrelloApi

#Region " Boards "

  Private _BoardStore As New BoardStore(Me)
  Public ReadOnly Property Boards() As IAlloverBoardAccess
    Get
      Return _BoardStore
    End Get
  End Property

  Public ReadOnly Property Boards(boardId As String) As IBoardAccessContext
    Get
      Return New BoardRecordHandle(Me, boardId)
    End Get
  End Property

  Public ReadOnly Property Boards(board As IPersistentTrelloBoard) As IBoardAccessContext
    Get
      Return New BoardRecordHandle(Me, board.Id)
    End Get
  End Property

#End Region

#Region " Lists "

  'Singleton
  Private _ListStorePerBoardId As New Dictionary(Of String, ListStore)
  Friend Function ListStorePerBoardId(boardId As String) As ListStore
    SyncLock _ListStorePerBoardId
      If (_ListStorePerBoardId.ContainsKey(boardId)) Then
        Return _ListStorePerBoardId(boardId)
      Else
        Dim newInstance As New ListStore(Me, boardId)
        _ListStorePerBoardId.Add(boardId, newInstance)
        Return newInstance
      End If
    End SyncLock
  End Function

  Private _ListsAllover As New AlloverListQuery(Me)
  Public ReadOnly Property Lists() As IAlloverListAccess
    Get
      Return _ListsAllover
    End Get
  End Property

  Public ReadOnly Property Lists(listId As String) As IListAccessContext
    Get
      Return New ListRecordHandle(Me, listId)
    End Get
  End Property

  Public ReadOnly Property Lists(list As IPersistentTrelloList) As IListAccessContext
    Get
      Return New ListRecordHandle(Me, list.Id)
    End Get
  End Property

#End Region

#Region " Cards "

  'Singleton
  Private _CardStorePerListId As New Dictionary(Of String, CardStore)
  Friend Function CardStorePerListId(listId As String) As CardStore
    SyncLock _CardStorePerListId
      If (_CardStorePerListId.ContainsKey(listId)) Then
        Return _CardStorePerListId(listId)
      Else
        Dim newInstance As New CardStore(Me, listId)
        _CardStorePerListId.Add(listId, newInstance)
        Return newInstance
      End If
    End SyncLock
  End Function

  Private _CardsAllover As New AlloverCardQuery(Me)
  Public ReadOnly Property Cards() As IAlloverCardAccess
    Get
      Return _CardsAllover
    End Get
  End Property

  Public ReadOnly Property Cards(cardId As String) As ICardAccessContext
    Get
      Return New CardRecordHandle(Me, cardId)
    End Get
  End Property

  Public ReadOnly Property Cards(card As IPersistentTrelloCard) As ICardAccessContext
    Get
      Return New CardRecordHandle(Me, card.Id)
    End Get
  End Property

#End Region

#Region " Labels "

  'Singleton
  Private _LabelStorePerBoardId As New Dictionary(Of String, LabelStore)
  Friend Function LabelStorePerBoardId(boardId As String) As LabelStore
    SyncLock _LabelStorePerBoardId
      If (_LabelStorePerBoardId.ContainsKey(boardId)) Then
        Return _LabelStorePerBoardId(boardId)
      Else
        Dim newInstance As New LabelStore(Me, boardId)
        _LabelStorePerBoardId.Add(boardId, newInstance)
        Return newInstance
      End If
    End SyncLock
  End Function

  Private _LabelsAllover As New AlloverLabelQuery(Me)
  Public ReadOnly Property Labels() As IAlloverLabelAccess
    Get
      Return _LabelsAllover
    End Get
  End Property

  Public ReadOnly Property Labels(labelId As String) As ILabelAccessContext
    Get
      Return New LabelRecordHandle(Me, labelId)
    End Get
  End Property

  Public ReadOnly Property Labels(label As IPersistentTrelloLabel) As ILabelAccessContext
    Get
      Return New LabelRecordHandle(Me, label.Id)
    End Get
  End Property

#End Region

#Region " Members "

  Private _MemberStore As New MemberStore(Me)
  Public ReadOnly Property Members() As IAlloverMemberAccess
    Get
      Return _MemberStore
    End Get
  End Property

  Public ReadOnly Property Members(memberId As String) As IMemberAccessContext
    Get
      Return New MemberRecordHandle(Me, memberId)
    End Get
  End Property

  Public ReadOnly Property Members(member As IPersistentTrelloMember) As IMemberAccessContext
    Get
      Return New MemberRecordHandle(Me, member.Id)
    End Get
  End Property

#End Region

End Class
