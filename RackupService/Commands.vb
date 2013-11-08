Public Class Commands
  Public Shared Function Where(arg As String)

    Dim info As New ProcessStartInfo()
    info.FileName = "where"
    info.Arguments = arg
    info.WindowStyle = ProcessWindowStyle.Hidden
    info.RedirectStandardOutput = True
    info.UseShellExecute = False

    Dim whr As New Process
    whr.StartInfo = info
    whr.Start()
    whr.WaitForExit()

    Return (From x In whr.StandardOutput.ReadToEnd.Split(vbCrLf) Select x).FirstOrDefault

  End Function
End Class
