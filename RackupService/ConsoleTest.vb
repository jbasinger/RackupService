Imports System.IO

Module ConsoleTest
  Sub Main(args() As String)

    Dim service As New RackupService()

    If (Environment.UserInteractive) Then

      service.Start(args)
      Console.WriteLine("Press enter to continue...")
      Console.ReadLine()

      service.Stop()

    End If

  End Sub
End Module
