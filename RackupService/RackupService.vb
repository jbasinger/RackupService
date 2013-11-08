Imports System.Diagnostics
Imports System.IO

Public Class RackupService

  Dim server As Process
  Dim rackup As String
  Dim sites As String
  Dim cli As String

  Private Sub LoadSettings()

    rackup = My.MySettings.Default.Binary
    sites = My.MySettings.Default.WebsiteLocation
    cli = My.MySettings.Default.Args

    If rackup = String.Empty Then
      rackup = Commands.Where("rackup.bat")
    End If

    If sites = String.Empty Then
      Log("Can't figure out where your sites live. Put a directory in the WebsiteLocation setting!")
    End If

  End Sub

  Public Sub Start(args() As String)
    Try

      LoadSettings()

      server = New Process

      Directory.SetCurrentDirectory(sites)

      Dim info As New ProcessStartInfo()
      info.FileName = rackup
      info.WindowStyle = ProcessWindowStyle.Hidden
      info.Arguments = cli
      info.RedirectStandardError = True
      info.RedirectStandardOutput = True
      info.UseShellExecute = False

      server.StartInfo = info
      server.EnableRaisingEvents = True

      AddHandler server.ErrorDataReceived, AddressOf ServerError
      AddHandler server.OutputDataReceived, AddressOf ServerOutput
      AddHandler server.Exited, AddressOf ServerExited

      Log(String.Format("Starting Rackup Server like this - {0} {1}", info.FileName, info.Arguments))

      server.Start()
      server.BeginErrorReadLine()
      server.BeginOutputReadLine()

    Catch ex As Exception
      Log(ex.ToString)
    End Try

  End Sub

  Protected Overrides Sub OnStart(ByVal args() As String)
    Start(args)
  End Sub

  Protected Overrides Sub OnStop()
    Log("Attempting to kill Thin Server")
    If Not server.HasExited Then
      server.Kill()
      Log("Rackup Server killed!")
    End If
  End Sub

  Protected Sub ServerExited(sender As Object, e As EventArgs)
    server.WaitForExit()
    Log("Rackup Server exited")
  End Sub

  Protected Sub ServerOutput(sender As Object, data As DataReceivedEventArgs)
    Log(String.Format("Rackup Server Output: {0}", data.Data))
  End Sub

  Protected Sub ServerError(sender As Object, data As DataReceivedEventArgs)
    Log(String.Format("Rackup Server Error: {0}", data.Data))
  End Sub

  Private Sub Log(msg As String)

    Using sw As StreamWriter = New StreamWriter(Path.Combine(sites, "log.txt"), True)
      sw.WriteLine(msg)
    End Using

  End Sub

End Class
