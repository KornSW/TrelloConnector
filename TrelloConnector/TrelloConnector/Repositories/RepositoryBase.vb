Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Web
Imports Newtonsoft.Json

Namespace Repositories

  Friend MustInherit Class RepositoryBase

    Private _Api As TrelloApi

    Protected Sub New(api As TrelloApi)
      _Api = api
    End Sub

    Protected ReadOnly Property Api As TrelloApi
      Get
        Return _Api
      End Get
    End Property

    Protected Function GetFilterExpression(includeArchived As Boolean) As String
      If (includeArchived) Then
        Return "filter=all"
      Else
        Return "filter=open"
      End If
    End Function

    Private Function BuildUrl(urlSubpath As String, fieldFilterExpression As String, Optional urlQuery As String = Nothing) As String
      Dim url = "https://api.trello.com/1/" + urlSubpath + "?"
      If (Not String.IsNullOrWhiteSpace(fieldFilterExpression)) Then
        url = url + fieldFilterExpression
      End If

      If (Not String.IsNullOrWhiteSpace(urlQuery)) Then
        If (Not url.EndsWith("&") AndAlso Not url.EndsWith("?")) Then
          url = url + "&"
        End If
        url = url + urlQuery
      End If

      If (Not url.EndsWith("&") AndAlso Not url.EndsWith("?")) Then
        url = url + "&"
      End If
      url = url + "key=" + Me.Api.ApplicationKey + "&token=" + Me.Api.AuthToken

      Return url
    End Function

    Friend Function ExecuteGet(Of T)(urlSubpath As String, Optional urlQuery As String = Nothing) As T
      Dim url = Me.BuildUrl(urlSubpath, GetType(T).GetFieldExpression, urlQuery)
      Try
        Dim rawResponseData = Me.Api.WebClient.DownloadString(url)
        Using responseStringReader As New StringReader(rawResponseData)
          Using responseJsonReader As New JsonTextReader(responseStringReader)
            Return Me.Api.Serializer.Deserialize(Of T)(responseJsonReader)
          End Using
        End Using
      Catch ex As Exception When ex.Message.Contains("(404)")
        Throw New KeyNotFoundException(ex.Message + " " + url, ex)
      End Try
    End Function

    Friend Function ExecutePut(Of T)(urlSubpath As String, requestContent As Object, Optional urlQuery As String = Nothing) As T
      Return Me.ExecutePut(Of T)(urlSubpath, requestContent, requestContent.GetType(), urlQuery)
    End Function

    Friend Function ExecutePut(Of T, TRequestContent)(urlSubpath As String, requestContent As TRequestContent, Optional urlQuery As String = Nothing) As T
      Return Me.ExecutePut(Of T)(urlSubpath, requestContent, GetType(TRequestContent), urlQuery)
    End Function

    Friend Function ExecutePut(Of T)(urlSubpath As String, requestContent As Object, tArg As Type, Optional urlQuery As String = Nothing) As T
      Dim url = Me.BuildUrl(urlSubpath, GetType(T).GetFieldExpression + "&" + Me.EncodeFieldsToQueryUrl(requestContent, tArg), urlQuery)
      Try


        'Dim rawRequestData As New StringBuilder
        'Using requestStringWriter As New StringWriter(rawRequestData)
        '  Using requestJsonWriter As New JsonTextWriter(requestStringWriter)
        '    _Serializer.Serialize(requestJsonWriter, requestContent, tArg)
        '  End Using
        'End Using

        'Dim rawRequestString As String = rawRequestData.ToString()
        Dim rawRequestString As String = ""
        Dim rawResponseData = Me.Api.WebClient.UploadString(url, "PUT", rawRequestString)

        Using responseStringReader As New StringReader(rawResponseData)
          Using responseJsonReader As New JsonTextReader(responseStringReader)
            Return Me.Api.Serializer.Deserialize(Of T)(responseJsonReader)
          End Using
        End Using
      Catch ex As Exception When ex.Message.Contains("(404)")
        Throw New KeyNotFoundException(ex.Message + " " + url, ex)
      End Try
    End Function
    Friend Function EncodeFieldsToQueryUrl(Of T)(extendee As T) As String
      Return Me.EncodeFieldsToQueryUrl(extendee, GetType(T))
    End Function

    Friend Function EncodeFieldsToQueryUrl(extendee As Object, objType As Type) As String
      Dim url As New StringBuilder

      For Each prop In objType.GetProperties()
        Dim propertyValue As Object = prop.GetValue(extendee)
        If (propertyValue IsNot Nothing) Then
          Dim propertyString As String
          Select Case prop.PropertyType
            Case GetType(Boolean)
              If (DirectCast(propertyValue, Boolean)) = True Then
                propertyString = "true"
              Else
                propertyString = "false"
              End If
            Case GetType(DateTime), GetType(Date)
              propertyString = DirectCast(propertyValue, DateTime).ToString("o")
            Case GetType(Nullable(Of DateTime))
              With DirectCast(propertyValue, Nullable(Of DateTime))
                If (Not .HasValue OrElse .Value.Year <= 1950) Then
                  propertyString = "null"
                Else
                  propertyString = .Value.ToString("o")

                End If
              End With


            Case Else
              propertyString = propertyValue.ToString()
          End Select

          Dim encodedPropertyString = HttpUtility.UrlEncode(propertyString)
          If (url.Length > 0) Then
            url.Append("&")
          End If
          url.Append(prop.Name.FirstToLower())
          url.Append("=")
          url.Append(encodedPropertyString)
        End If
      Next

      Return url.ToString()
    End Function

    Friend Function ExecutePost(Of T)(urlSubpath As String, requestContent As Object, Optional urlQuery As String = Nothing) As T
      Return Me.ExecutePost(Of T)(urlSubpath, requestContent, requestContent?.GetType(), urlQuery)
    End Function

    Friend Function ExecutePost(Of T, TRequestContent)(urlSubpath As String, requestContent As TRequestContent, Optional urlQuery As String = Nothing) As T
      Return Me.ExecutePost(Of T)(urlSubpath, requestContent, GetType(TRequestContent), urlQuery)
    End Function

    Friend Function ExecutePost(Of T)(urlSubpath As String, requestContent As Object, tArg As Type, Optional urlQuery As String = Nothing) As T


      Dim rawRequestString As String = ""
      Dim urlEncodedFields As String = ""
      If (requestContent IsNot Nothing) Then
        If (tArg Is Nothing) Then
          tArg = requestContent.GetType()
        End If
        urlEncodedFields = "&" + Me.EncodeFieldsToQueryUrl(requestContent, tArg)
      End If

      Dim url = Me.BuildUrl(urlSubpath, GetType(T).GetFieldExpression + urlEncodedFields, urlQuery)


      Try

        'Dim rawRequestData As New StringBuilder
        'Using requestStringWriter As New StringWriter(rawRequestData)
        '  Using requestJsonWriter As New JsonTextWriter(requestStringWriter)
        '    _Serializer.Serialize(requestJsonWriter, requestContent, tArg)
        '  End Using
        'End Using

        'Dim rawRequestString As String = rawRequestData.ToString()

        Dim rawResponseData = Me.Api.WebClient.UploadString(url, "POST", rawRequestString)

        Using responseStringReader As New StringReader(rawResponseData)
          Using responseJsonReader As New JsonTextReader(responseStringReader)
            Return Me.Api.Serializer.Deserialize(Of T)(responseJsonReader)
          End Using
        End Using

      Catch ex As Exception When ex.Message.Contains("(404)")
        Throw New KeyNotFoundException(ex.Message + " " + url, ex)
      End Try
    End Function

    Friend Sub ExecuteDelete(urlSubpath As String, Optional urlQuery As String = Nothing)
      Dim url = Me.BuildUrl(urlSubpath, "", urlQuery) ' Me.BuildUrl(urlSubpath, GetType(T).GetFieldExpression, urlQuery)

      Try
        Dim rawResponseData = Me.Api.WebClient.UploadString(url, "DELETE", String.Empty)
      Catch ex As Exception When ex.Message.Contains("(404)")
        Throw New KeyNotFoundException(ex.Message + " " + url, ex)
      End Try


      'Using responseStringReader As New StringReader(rawResponseData)
      '  Using responseJsonReader As New JsonTextReader(responseStringReader)
      '    Return _Serializer.Deserialize(Of T)(responseJsonReader)
      '  End Using
      'End Using
    End Sub

  End Class

End Namespace
