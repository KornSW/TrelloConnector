Imports System
Imports System.ComponentModel
Imports System.Linq
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Web

Public Module ExtensionMethods

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Friend Function GetFieldExpression(extendee As Type) As String
    If (extendee.IsArray) Then
      extendee = extendee.GetElementType()
    End If
    Dim propStr As String = Nothing
    For Each prop In extendee.GetProperties()
      If (propStr Is Nothing) Then
        propStr = "fields="
      Else
        propStr = propStr + ","
      End If
      propStr = propStr + Char.ToLower(prop.Name(0)) + prop.Name.Substring(1)
    Next
    Return propStr
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Friend Function FirstToLower(extendee As String) As String
    If (Char.IsUpper(extendee(0))) Then
      extendee = Char.ToLower(extendee(0)) + extendee.Substring(1)
    End If
    Return extendee
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Friend Function Ids(extendee As IdContainer()) As String()
    Return extendee.Select(Function(c) c.Id).ToArray()
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Friend Function IsLoaded(extendee As IdContainer) As Boolean
    If (extendee Is Nothing OrElse String.IsNullOrWhiteSpace(extendee.Id)) Then
      Return False
    End If
    Return True
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Friend Function IsLoaded(extendee As LoadedTrelloBoard) As Boolean
    If (extendee Is Nothing OrElse String.IsNullOrWhiteSpace(extendee.Id)) Then
      Return False
    End If
    Return True
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Friend Function IsLoaded(extendee As LoadedTrelloCard) As Boolean
    If (extendee Is Nothing OrElse String.IsNullOrWhiteSpace(extendee.Id)) Then
      Return False
    End If
    Return True
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Friend Function IsLoaded(extendee As LoadedTrelloLabel) As Boolean
    If (extendee Is Nothing OrElse String.IsNullOrWhiteSpace(extendee.Id)) Then
      Return False
    End If
    Return True
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Friend Function IsLoaded(extendee As LoadedTrelloList) As Boolean
    If (extendee Is Nothing OrElse String.IsNullOrWhiteSpace(extendee.Id)) Then
      Return False
    End If
    Return True
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Friend Function IsLoaded(extendee As LoadedTrelloMember) As Boolean
    If (extendee Is Nothing OrElse String.IsNullOrWhiteSpace(extendee.Id)) Then
      Return False
    End If
    Return True
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Friend Function Exists(extendee As IdContainer, includeArchived As Boolean) As Boolean
    If (extendee.IsLoaded AndAlso (includeArchived OrElse extendee.Closed = False)) Then
      Return True
    End If
    Return False
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Friend Function Exists(extendee As LoadedTrelloBoard, includeArchived As Boolean) As Boolean
    If (extendee.IsLoaded AndAlso (includeArchived OrElse extendee.Closed = False)) Then
      Return True
    End If
    Return False
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Friend Function Exists(extendee As LoadedTrelloCard, includeArchived As Boolean) As Boolean
    If (extendee.IsLoaded AndAlso (includeArchived OrElse extendee.Closed = False)) Then
      Return True
    End If
    Return False
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Friend Function Exists(extendee As LoadedTrelloLabel, includeArchived As Boolean) As Boolean
    If (extendee.IsLoaded AndAlso (includeArchived OrElse extendee.Closed = False)) Then
      Return True
    End If
    Return False
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Friend Function Exists(extendee As LoadedTrelloList, includeArchived As Boolean) As Boolean
    If (extendee.IsLoaded AndAlso (includeArchived OrElse extendee.Closed = False)) Then
      Return True
    End If
    Return False
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Friend Function Exists(extendee As LoadedTrelloMember, includeArchived As Boolean) As Boolean
    If (extendee.IsLoaded AndAlso (includeArchived OrElse extendee.Closed = False)) Then
      Return True
    End If
    Return False
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub CopyContentTo(extendee As ITrelloBoardContent, target As ITrelloBoardContent)
    For Each p In GetType(ITrelloBoardContent).GetProperties()
      p.SetValue(target, p.GetValue(extendee))
    Next
  End Sub

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub CopyContentTo(extendee As ITrelloCardContent, target As ITrelloCardContent)
    For Each p In GetType(ITrelloCardContent).GetProperties()
      p.SetValue(target, p.GetValue(extendee))
    Next
  End Sub

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub CopyContentTo(extendee As ITrelloLabelContent, target As ITrelloLabelContent)
    For Each p In GetType(ITrelloLabelContent).GetProperties()
      p.SetValue(target, p.GetValue(extendee))
    Next
  End Sub

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub CopyContentTo(extendee As ITrelloListContent, target As ITrelloListContent)
    For Each p In GetType(ITrelloListContent).GetProperties()
      p.SetValue(target, p.GetValue(extendee))
    Next
  End Sub

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub CopyContentTo(extendee As ITrelloMemberContent, target As ITrelloMemberContent)
    For Each p In GetType(ITrelloMemberContent).GetProperties()
      p.SetValue(target, p.GetValue(extendee))
    Next
  End Sub

End Module
