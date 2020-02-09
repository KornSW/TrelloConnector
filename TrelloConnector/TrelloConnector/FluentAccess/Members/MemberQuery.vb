Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq

Namespace FluentAccess

  Friend Class MemberQuery
    Implements IMembersAccessContext

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

    Public Function GetIds(Optional includeArchived As Boolean = False) As String() Implements IMembersAccessContext.GetIds
      Return _IdGetter.Invoke(includeArchived)
    End Function

    Public Function GetAll(Optional includeArchived As Boolean = False) As IMemberAccessContext() Implements IMembersAccessContext.GetAll
      Return _IdGetter.Invoke(includeArchived).Select(Function(id) New MemberRecordHandle(_Api, id)).ToArray()
    End Function

#Region " Contains "

    Public Function Contains(memberId As String) As Boolean Implements IMembersAccessContext.Contains
      Return Me.GetIds(True).Contains(memberId)
    End Function

    Public Function Contains(member As IPersistentTrelloMember) As Boolean Implements IMembersAccessContext.Contains
      Return Me.Contains(member.Id)
    End Function

    Public Function Contains(member As IMemberAccessContext) As Boolean Implements IMembersAccessContext.Contains
      Return Me.Contains(member.Id)
    End Function

#End Region

#Region " Load "

    Public Function LoadAll(Optional includeArchived As Boolean = False) As IPersistentTrelloMember() Implements IMembersAccessContext.LoadAll
      Dim members As New List(Of IPersistentTrelloMember)
      For Each member In Me.GetAll(includeArchived)
        members.Add(member.Load())
      Next
      Return members.ToArray()
    End Function

#End Region

  End Class

End Namespace
