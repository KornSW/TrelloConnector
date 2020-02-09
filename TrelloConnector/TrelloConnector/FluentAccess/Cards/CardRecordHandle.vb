Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq

Namespace FluentAccess

  <DebuggerDisplay("Card {Id}")>
  Friend Class CardRecordHandle
    Implements ICardAccessContext

    Private _Api As TrelloApi
    Private _CardId As String
    Private _ListId As String = Nothing

    Public Sub New(api As TrelloApi, cardId As String, Optional alreadyDiscoveredListId As String = Nothing)
      _Api = api
      _CardId = cardId
      _ListId = alreadyDiscoveredListId
    End Sub

    Public ReadOnly Property Id As String Implements ICardAccessContext.Id
      Get
        Return _CardId
      End Get
    End Property

#Region " Exisits & Archive "

    Public Function Exists(Optional includeArchived As Boolean = False) As Boolean Implements ICardAccessContext.Exists
      Return _Api.CardRepository.CardExists(_CardId, includeArchived)
    End Function

    Public Function IsArchived() As Boolean Implements ICardAccessContext.IsArchived
      Return _Api.CardRepository.IsCardArchived(_CardId)
    End Function

    Public Sub Archive() Implements ICardAccessContext.Archive
      _Api.CardRepository.ArchiveCardsById(_CardId)
    End Sub

    Public Sub Unarchive() Implements ICardAccessContext.Unarchive
      _Api.CardRepository.UnarchiveCardsById(_CardId)
    End Sub

#End Region

#Region " Load / Update "

    Public Function Load() As IPersistentTrelloCard Implements ICardAccessContext.Load
      Return _Api.CardRepository.LoadCardById(_CardId)
    End Function

    Public Sub LoadInto(target As ITrelloCardContent) Implements ICardAccessContext.LoadInto
      _Api.CardRepository.LoadCardByIdInto(_CardId, target)
    End Sub

    Public Sub SaveContentFrom(contentSource As ITrelloCardContent) Implements ICardAccessContext.SaveContentFrom
      _Api.CardRepository.SaveCardContent(_CardId, contentSource)
    End Sub

#End Region

#Region " Principal/Dependent-Navigation "

    Public ReadOnly Property List As IListAccessContext Implements ICardAccessContext.List
      Get
        If (_ListId Is Nothing) Then
          _ListId = _Api.ListRepository.GetListIdContainingCard(_CardId)
        End If
        Return New ListRecordHandle(_Api, _ListId)
      End Get
    End Property

    Public ReadOnly Property Board As IBoardAccessContext Implements ICardAccessContext.Board
      Get
        Return Me.List.Board
      End Get
    End Property

#End Region

#Region " Referrer-Navigation -> Labels "

    Public Function AssignedLabels() As ILabelsAccessContext Implements ICardAccessContext.AssignedLabels
      Return New LabelQuery(_Api, Function(includeArchived) _Api.LabelRepository.GetLabelIdsByAssignedCard(_CardId, includeArchived))
    End Function

    Public Function IsAssignedToLabel(label As ILabelAccessContext) As Boolean Implements ICardAccessContext.IsAssignedToLabel
      Return Me.AssignedLabels().Contains(label)
    End Function

    Public Function IsAssignedToLabel(labelId As String) As Boolean Implements ICardAccessContext.IsAssignedToLabel
      Return Me.AssignedLabels().Contains(labelId)
    End Function

    Public Function IsAssignedToLabel(label As IPersistentTrelloLabel) As Boolean Implements ICardAccessContext.IsAssignedToLabel
      Return Me.AssignedLabels().Contains(label)
    End Function

    Public Sub AssignLabel(label As ILabelAccessContext) Implements ICardAccessContext.AssignLabel
      _Api.LabelRepository.AssignLabelToCard(label.Id, _CardId)
    End Sub

    Public Sub AssignLabel(labelId As String) Implements ICardAccessContext.AssignLabel
      _Api.LabelRepository.AssignLabelToCard(labelId, _CardId)
    End Sub

    Public Sub AssignLabel(label As IPersistentTrelloLabel) Implements ICardAccessContext.AssignLabel
      _Api.LabelRepository.AssignLabelToCard(label.Id, _CardId)
    End Sub

    Public Sub UnassignLabel(label As ILabelAccessContext) Implements ICardAccessContext.UnassignLabel
      _Api.LabelRepository.UnassignLabelFromCard(label.Id, _CardId)
    End Sub

    Public Sub UnassignLabel(labelId As String) Implements ICardAccessContext.UnassignLabel
      _Api.LabelRepository.UnassignLabelFromCard(labelId, _CardId)
    End Sub

    Public Sub UnassignLabel(label As IPersistentTrelloLabel) Implements ICardAccessContext.UnassignLabel
      _Api.LabelRepository.UnassignLabelFromCard(label.Id, _CardId)
    End Sub

#End Region

#Region " Referrer-Navigation -> Members "

    Public Function AssignedMembers() As IMembersAccessContext Implements ICardAccessContext.AssignedMembers
      Return New MemberQuery(_Api, Function(includeArchived) _Api.MemberRepository.GetMemberIdsByAssignedCard(_CardId, includeArchived))
    End Function

    Public Function IsAssignedToMember(member As IMemberAccessContext) As Boolean Implements ICardAccessContext.IsAssignedToMember
      Return Me.AssignedMembers().Contains(member)
    End Function

    Public Function IsAssignedToMember(memberId As String) As Boolean Implements ICardAccessContext.IsAssignedToMember
      Return Me.AssignedMembers().Contains(memberId)
    End Function

    Public Function IsAssignedToMember(member As IPersistentTrelloMember) As Boolean Implements ICardAccessContext.IsAssignedToMember
      Return Me.AssignedMembers().Contains(member)
    End Function

    Public Sub AssignMember(member As IMemberAccessContext) Implements ICardAccessContext.AssignMember
      _Api.MemberRepository.AssignMemberToCard(member.Id, _CardId)
    End Sub

    Public Sub AssignMember(memberId As String) Implements ICardAccessContext.AssignMember
      _Api.MemberRepository.AssignMemberToCard(memberId, _CardId)
    End Sub

    Public Sub AssignMember(member As IPersistentTrelloMember) Implements ICardAccessContext.AssignMember
      _Api.MemberRepository.AssignMemberToCard(member.Id, _CardId)
    End Sub

    Public Sub UnassignMember(member As IMemberAccessContext) Implements ICardAccessContext.UnassignMember
      _Api.MemberRepository.UnassignMemberFromCard(member.Id, _CardId)
    End Sub

    Public Sub UnassignMember(memberId As String) Implements ICardAccessContext.UnassignMember
      _Api.MemberRepository.UnassignMemberFromCard(memberId, _CardId)
    End Sub

    Public Sub UnassignMember(member As IPersistentTrelloMember) Implements ICardAccessContext.UnassignMember
      _Api.MemberRepository.UnassignMemberFromCard(member.Id, _CardId)
    End Sub


#End Region

#Region " Move "

    Public Sub MoveTo(targetList As IListAccessContext) Implements ICardAccessContext.MoveTo
      _Api.CardRepository.MoveCardsToList(targetList.Id, _CardId)
    End Sub

    Public Sub MoveTo(targetListId As String) Implements ICardAccessContext.MoveTo
      _Api.CardRepository.MoveCardsToList(targetListId, _CardId)
    End Sub

    Public Sub MoveTo(targetList As IPersistentTrelloList) Implements ICardAccessContext.MoveTo
      _Api.CardRepository.MoveCardsToList(targetList.Id, _CardId)
    End Sub

#End Region

  End Class

End Namespace
