''' <summary>
''' Class for sending emails (supports gmail and yahoo mail).
''' </summary>
''' <remarks></remarks>
Public Class Email
    Implements IDisposable

#Region "Enumerations"
    ''' <summary>
    ''' Mail servers.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum MailHostEnum
        ''' <summary>
        ''' Custom assigned mail server address.
        ''' </summary>
        ''' <remarks></remarks>
        CustomMail = 3
        ''' <summary>
        ''' Mail server : smtp.gmail.com
        ''' </summary>
        ''' <remarks></remarks>
        Gmail = 1
        ''' <summary>
        ''' Mail server : smtp.mail.yahoo.com
        ''' </summary>
        ''' <remarks></remarks>
        YahooMail = 2
        ''' <summary>
        ''' Mail server : mail.fms.com.ph
        ''' </summary>
        ''' <remarks></remarks>
        amsfms = 0
        ''' <summary>
        ''' Mail server : other mail servers
        ''' </summary>
        ''' <remarks></remarks>
        others = -1
    End Enum
#End Region

#Region "Constant Variables"
    Const gmail As String = "smtp.gmail.com"
    Const yahoomail As String = "smtp.mail.yahoo.com"
    Const fmsmail As String = "mail.fms.com.ph"
#End Region

#Region "Custom Classes"
    ''' <summary>
    ''' Derived class for email file attachments.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class MailAttachmentCollection
        Inherits CollectionBase
        Implements IDisposable

        ''' <summary>
        ''' Gets or sets file attachment in the specified index of the collection.
        ''' </summary>
        ''' <param name="index"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Default Public Property Files(ByVal index As Integer) As Attachment
            Get
                Return CType(List(index), Attachment)
            End Get
            Set(ByVal value As Attachment)
                List(index) = value
            End Set
        End Property

        ''' <summary>
        ''' Adds a file attachment in the collection.
        ''' </summary>
        ''' <param name="file"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Add(ByVal file As Attachment) As Integer
            Return List.Add(file)
        End Function

        ''' <summary>
        ''' Adds a file attachment in the collection.
        ''' </summary>
        ''' <param name="filename">File's path.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Add(ByVal filename As String) As Integer
            Dim file As New Attachment(filename)
            Return List.Add(file)
        End Function

        ''' <summary>
        ''' Validates whether the specified file is currently existing within the collection.
        ''' </summary>
        ''' <param name="file"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Contains(ByVal file As Attachment) As Boolean
            Return List.Contains(file)
        End Function

        ''' <summary>
        ''' Removes the specified email attachment within the collection.
        ''' </summary>
        ''' <param name="file"></param>
        ''' <remarks></remarks>
        Public Sub Remove(ByVal file As Attachment)
            If List.Contains(file) Then List.Remove(file)
        End Sub

#Region "IDisposable"
        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        ''' <summary>
        ''' Dispose off any resources used by the class to free up memory space.
        ''' </summary>
        ''' <param name="disposing"></param>
        ''' <remarks></remarks>
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: free other state (managed objects).
                    For Each a As Attachment In List
                        a.Dispose()
                    Next
                    Common.Simple.RefreshAndManageCurrentProcess()
                End If

                ' TODO: free your own state (unmanaged objects).
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

#Region " IDisposable Support "
        ' This code added by Visual Basic to correctly implement the disposable pattern.
        ''' <summary>
        ''' Dispose off any resources used by the class to free up memory space.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region
#End Region

    End Class
#End Region

#Region "Variable Declarations"
    Dim _mailhost As MailHostEnum = MailHostEnum.CustomMail
    Dim _uid As String = String.Empty
    Dim _pwd As String = String.Empty
    Dim _smtp As SmtpClient = Nothing
#End Region

#Region "Sub New"
    ''' <summary>
    ''' Creates a new instance of Email.
    ''' </summary>
    ''' <param name="host">Mail server hostname.</param>
    ''' <remarks></remarks>
    Sub New(ByVal host As String)
        _smtp = New SmtpClient(host) : _host = host
    End Sub

    ''' <summary>
    ''' Creates a new instance of Email.
    ''' </summary>
    ''' <param name="host">Mail server hostname.</param>
    ''' <param name="uid">Mail account user id.</param>
    ''' <param name="pwd">Mail account password.</param>
    ''' <remarks></remarks>
    Sub New(ByVal host As String, ByVal uid As String, ByVal pwd As String)
        _uid = uid : _pwd = pwd : _host = host
        _smtp = New SmtpClient(host) : _credential = New NetworkCredential(uid, pwd)
    End Sub

    ''' <summary>
    ''' Creates a new instance of Email.
    ''' </summary>
    ''' <param name="host">Mail server hostname.</param>
    ''' <param name="port">Mail server port number.</param>
    ''' <remarks></remarks>
    Sub New(ByVal host As String, ByVal port As String)
        _smtp = New SmtpClient(host, port) : _host = host : _port = port
    End Sub

    ''' <summary>
    ''' Creates a new instance of Email.
    ''' </summary>
    ''' <param name="host">Mail server hostname.</param>
    ''' <param name="port">Mail server port number.</param>
    ''' <param name="uid">Mail account user id.</param>
    ''' <param name="pwd">Mail account password.</param>
    ''' <remarks></remarks>
    Sub New(ByVal host As String, ByVal port As String, ByVal uid As String, ByVal pwd As String)
        _uid = uid : _pwd = pwd : _host = host : _port = port
        _smtp = New SmtpClient(host, port) : _credential = New NetworkCredential(uid, pwd)
    End Sub

    ''' <summary>
    ''' Creates a new instance of Email.
    ''' </summary>
    ''' <param name="mail">Predefined mail server. Value should not be a CustomMail.</param>
    ''' <remarks></remarks>
    Sub New(ByVal mail As MailHostEnum)
        _mailhost = mail
        Select Case mail
            Case MailHostEnum.others
            Case MailHostEnum.amsfms : _host = fmsmail
            Case MailHostEnum.Gmail
                _host = gmail : _port = 465
            Case MailHostEnum.YahooMail
                _host = yahoomail : _port = 587
            Case Else
                Throw New ArgumentException("Mail host enumeration is not qualified for the call Sub New(ByVal mail As MailHostEnum).", "mail", Nothing)
        End Select

        If mail = MailHostEnum.amsfms Then : _smtp = New SmtpClient(_host)
        Else
            _smtp = New SmtpClient(_host, _port)
            With _smtp
                .EnableSsl = True : .UseDefaultCredentials = False
            End With
        End If
    End Sub

    ''' <summary>
    ''' Creates a new instance of Email.
    ''' </summary>
    ''' <param name="mail">Predefined mail server. Value should not be a CustomMail.</param>
    ''' <param name="uid">Mail account user id.</param>
    ''' <param name="pwd">Mail account password.</param>
    ''' <remarks></remarks>
    Sub New(ByVal mail As MailHostEnum, ByVal uid As String, ByVal pwd As String)
        _uid = uid : _pwd = pwd : _mailhost = mail
        Select Case mail
            Case MailHostEnum.others
            Case MailHostEnum.amsfms : _host = fmsmail
            Case MailHostEnum.Gmail
                _host = gmail : _port = 465
            Case MailHostEnum.YahooMail
                _host = yahoomail : _port = 587
            Case Else
                Throw New ArgumentException("Mail host enumeration is not qualified for the call Sub New(ByVal mail As MailHostEnum).", "mail", Nothing)
        End Select

        If mail = MailHostEnum.amsfms Then : _smtp = New SmtpClient(_host)
        Else
            _smtp = New SmtpClient(_host, _port)
            With _smtp
                .EnableSsl = True : .UseDefaultCredentials = False
            End With
        End If

        _credential = New NetworkCredential(uid, pwd)
    End Sub
#End Region

#Region "Properties"
    Dim _attachments As New MailAttachmentCollection
    ''' <summary>
    ''' Gets the email's file attachment information.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Attachments() As MailAttachmentCollection
        Get
            Return _attachments
        End Get
    End Property

    Dim _bcc As New MailAddressCollection
    ''' <summary>
    ''' Gets email's blind carbon copy recipient(s) information.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property BCC() As MailAddressCollection
        Get
            Return _bcc
        End Get
    End Property

    Dim _body As String = String.Empty
    ''' <summary>
    ''' Gets or sets email's message contents.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Body() As String
        Get
            Return _body
        End Get
        Set(ByVal value As String)
            _body = value
        End Set
    End Property

    Dim _cc As New MailAddressCollection
    ''' <summary>
    ''' Gets email's carbon copy recipient(s) information.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property CC() As MailAddressCollection
        Get
            Return _cc
        End Get
    End Property

    Dim _credential As ICredentialsByHost = Nothing
    ''' <summary>
    ''' Gets or sets mail account login credentials.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Credentials() As ICredentialsByHost
        Get
            Return _credential
        End Get
        Set(ByVal value As ICredentialsByHost)
            _credential = value
        End Set
    End Property

    Dim _ssl As Boolean = False
    ''' <summary>
    ''' Gets or sets whether mail server's SSL will be enabled or not.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property EnableSSL() As Boolean
        Get
            Return _ssl
        End Get
        Set(ByVal value As Boolean)
            _ssl = value
            If _smtp IsNot Nothing Then _smtp.EnableSsl = value
        End Set
    End Property

    Dim _error As String = String.Empty
    ''' <summary>
    ''' Gets the error message of the last failed email sending attempt.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ErrorMessage() As String
        Get
            Return _error
        End Get
    End Property

    Dim _from As MailAddress = Nothing
    ''' <summary>
    ''' Gets or sets email sender's information.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property [From]() As MailAddress
        Get
            Return _from
        End Get
        Set(ByVal value As MailAddress)
            _from = value
        End Set
    End Property

    Dim _host As String = String.Empty
    ''' <summary>
    ''' Gets or sets mail server hostname.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Host() As String
        Get
            Return _host
        End Get
        Set(ByVal value As String)
            Select Case _mailhost
                Case MailHostEnum.CustomMail : _host = value
                Case Else
            End Select
        End Set
    End Property

    Dim _innerexception As String = String.Empty
    ''' <summary>
    ''' Gets the inner exception details of the last failed email sending attempt. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property InnerException() As String
        Get
            Return _innerexception
        End Get
    End Property

    Dim _ishtml As Boolean = True
    ''' <summary>
    ''' Gets or sets whether email message body will be threated as an html or not.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsBodyHTML() As Boolean
        Get
            Return _ishtml
        End Get
        Set(ByVal value As Boolean)
            _ishtml = value
        End Set
    End Property

    Dim _port As Integer = 0
    ''' <summary>
    ''' Gets or sets mail server port number.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Port() As Integer
        Get
            Return _port
        End Get
        Set(ByVal value As Integer)
            _port = value
        End Set
    End Property

    Dim _sent As Boolean = False
    ''' <summary>
    ''' Gets whether last email sending attempt is successfully sent or not.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Sent() As Boolean
        Get
            Return _sent
        End Get
    End Property

    Dim _subject As String = String.Empty
    ''' <summary>
    ''' Gets or sets email's subject / title.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Subject() As String
        Get
            Return _subject
        End Get
        Set(ByVal value As String)
            _subject = value
        End Set
    End Property

    Private _imgFile As String = String.Empty
    Public Property ImgFile As String
        Get
            Return _imgFile
        End Get
        Set(ByVal value As String)
            _imgFile = value
        End Set
    End Property

    Private _htmlBody As String = String.Empty
    Public Property HtmlBody As String
        Get
            Return _htmlBody
        End Get
        Set(ByVal value As String)
            _htmlBody = value
        End Set
    End Property

    Private _contentId As String
    Public Property ContentId As String
        Get
            Return _contentId
        End Get
        Set(ByVal value As String)
            _contentId = value
        End Set
    End Property




    Dim _to As New MailAddressCollection
    ''' <summary>
    ''' Gets email recipient(s) information.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property [To]() As MailAddressCollection
        Get
            Return _to
        End Get
    End Property

    Dim _defaultcredential As Boolean = True
    ''' <summary>
    ''' Gets or sets whether default network credentials shall be use to log on to the mail server or not.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Property UseDefaultCredentials() As Boolean
        Get
            Return _defaultcredential
        End Get
        Set(ByVal value As Boolean)
            _defaultcredential = value
            If _smtp IsNot Nothing Then _smtp.UseDefaultCredentials = value
        End Set
    End Property

#End Region

#Region "Methods and Functions"
    ''' <summary>
    ''' Attempts to send the email.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Send()
        With _smtp
            .Host = _host
            If _port > 0 Then .Port = _port
            .UseDefaultCredentials = _defaultcredential
            .EnableSsl = _ssl : .Credentials = _credential
        End With

        Dim mail As New MailMessage
        With mail
            If _from IsNot Nothing Then : .From = _from
            Else
                If Not String.IsNullOrEmpty(_uid.Trim) Then _from = New MailAddress(_uid)
            End If

            For Each ea As MailAddress In _to
                .To.Add(ea)
            Next

            For Each ea As MailAddress In _cc
                .CC.Add(ea)
            Next

            For Each ea As MailAddress In _bcc
                .Bcc.Add(ea)
            Next

            For Each fa As Attachment In _attachments
                .Attachments.Add(fa)
            Next

            .Subject = _subject : .SubjectEncoding = Encoding.UTF8
            .IsBodyHtml = _ishtml
            .Body = _body : .BodyEncoding = Encoding.UTF8
            If _imgFile <> "" Then
                Dim imgeresources As New LinkedResource(_imgFile)
                With imgeresources
                    .ContentId = _contentId
                    .TransferEncoding = Mime.TransferEncoding.Base64
                End With
                Dim htmlView As AlternateView
                htmlView = AlternateView.CreateAlternateViewFromString(_htmlBody, Nothing, "text/html")

                htmlView.LinkedResources.Add(imgeresources)
                .AlternateViews.Add(htmlView)
            End If
        End With

        _sent = False : _error = String.Empty : _innerexception = String.Empty

        Try
            _smtp.Send(mail) : _sent = True
        Catch ex As Exception
            _error = ex.Message
            If ex.InnerException IsNot Nothing Then _innerexception = ex.InnerException.Message
        Finally : mail.Dispose()
        End Try
    End Sub

    Dim delSend As Action = Nothing

    ''' <summary>
    ''' Calls the Send method and run it asynchronously. Must call the EndSend method once IAsynResult is finish.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function BeginSend() As IAsyncResult
        If delSend IsNot Nothing Then delSend = Nothing
        delSend = New Action(AddressOf Send)
        Return delSend.BeginInvoke(Nothing, delSend)
    End Function

    ''' <summary>
    ''' Finalized the BeginSend call using its produced IAsynResult interface.
    ''' </summary>
    ''' <param name="async"></param>
    ''' <remarks></remarks>
    Public Sub EndSend(ByVal async As IAsyncResult)
        If delSend IsNot Nothing Then delSend.EndInvoke(async)
        delSend = Nothing
    End Sub

#End Region

#Region "IDisposable"
    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    ''' <summary>
    ''' Dispose off any resources used by the class to free up memory space.
    ''' </summary>
    ''' <param name="disposing"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free other state (managed objects).
                If _smtp IsNot Nothing Then _smtp = Nothing
                If _credential IsNot Nothing Then _credential = Nothing
                _attachments.Dispose()

                _from = Nothing

                For Each ea As MailAddress In _to
                    ea = Nothing
                Next

                For Each ea As MailAddress In _cc
                    ea = Nothing
                Next

                For Each ea As MailAddress In _bcc
                    ea = Nothing
                Next

                Common.Simple.RefreshAndManageCurrentProcess()
            End If

            ' TODO: free your own state (unmanaged objects).
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    ''' <summary>
    ''' Dispose off any resources used by the class to free up memory space.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
