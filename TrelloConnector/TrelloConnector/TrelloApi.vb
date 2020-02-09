Imports System
Imports System.ComponentModel
Imports System.IO
Imports System.Linq
Imports System.Net
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Web
Imports TrelloConnector.Repositories
Imports Newtonsoft.Json

'https://developers.trello.com/docs/api-introduction

Public Class TrelloApi

  'Repositories:
  Private _Boards As TrelloBoardRepository
  Private _Cards As TrelloCardRepository
  Private _Labels As TrelloLabelRepository
  Private _Lists As TrelloListRepository
  Private _Members As TrelloMemberRepository

  Friend ReadOnly Property ApplicationKey As String
  Friend ReadOnly Property AuthToken As String
  Friend ReadOnly Property Serializer As New JsonSerializer()
  Friend ReadOnly Property WebClient As New WebClient

  Public Sub New(applicationKey As String, authToken As String)
    MyClass.New(applicationKey, authToken, False)
  End Sub

  Public Sub New(applicationKey As String, authToken As String, autoConfigureServicepointSecurity As Boolean)
    Me.WebClient.Encoding = Encoding.UTF8

    Me.ApplicationKey = applicationKey
    Me.AuthToken = authToken

    If (Not ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12) Then
      If (autoConfigureServicepointSecurity) Then
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
      Else
        Throw New Exception($"Unfortunately the trello server requires https communication via TLS 1.2! Please set 'ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12' or let the trello-connector to it by calling the constructor with argument '{NameOf(autoConfigureServicepointSecurity)}:=true'.")
      End If
    End If

    WebClient.Headers.Item("Content-Type") = "application/json"

    _Boards = New TrelloBoardRepository(Me)
    _Cards = New TrelloCardRepository(Me)
    _Labels = New TrelloLabelRepository(Me)
    _Lists = New TrelloListRepository(Me)
    _Members = New TrelloMemberRepository(Me)

  End Sub

  Friend ReadOnly Property BoardRepository As TrelloBoardRepository
    Get
      Return _Boards
    End Get
  End Property

  Friend ReadOnly Property CardRepository As TrelloCardRepository
    Get
      Return _Cards
    End Get
  End Property

  Friend ReadOnly Property LabelRepository As TrelloLabelRepository
    Get
      Return _Labels
    End Get
  End Property

  Friend ReadOnly Property ListRepository As TrelloListRepository
    Get
      Return _Lists
    End Get
  End Property

  Friend ReadOnly Property MemberRepository As TrelloMemberRepository
    Get
      Return _Members
    End Get
  End Property

End Class

