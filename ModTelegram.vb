Module ModTelegram

    Public pBott As Telegram.Bot.TelegramBotClient

    'real group: -195688192
    'test group: -366195838

    ' Telegram logging: ERROR, BRIEF, FULL, (DEBUG)

    Public Function ConnectTelegram(vTelegramBot As String) As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        ConnectTelegram = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  ConnectTelegram...", "", "ConnectTelegram", "PROGRAM")

            pBott = New Telegram.Bot.TelegramBotClient(vTelegramBot)

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  ConnectTelegram: " + executionTime.ToString, "", "ConnectTelegram", "PROGRAM")

        Catch ex As Exception
            ConnectTelegram = "ConnectTelegram: " + ex.Message
            LogSessionEntry("ERROR", "ConnectTelegram: " + ex.Message, "", "ConnectTelegram", "PROGRAM")
        End Try
    End Function


    Public Function TelegramMessage(vMessage As String, vGroup As String) As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        TelegramMessage = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  TelegramMessage...", "", "TelegramMessage", "PROGRAM")

            pBott.SendTextMessageAsync(vGroup, vMessage)
            'minor stop, but flood control will block certain messages
            'Thread.Sleep(10)

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  TelegramMessage: " + executionTime.ToString, "", "TelegramMessage", "PROGRAM")

        Catch ex As Exception
            TelegramMessage = "TelegramMessage: " + ex.Message
            LogSessionEntry("ERROR", "TelegramMessage: " + ex.Message, "", "TelegramMessage", "PROGRAM")
        End Try
    End Function

End Module