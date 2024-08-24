Module ModTelegram

    Public pBott As Telegram.Bot.TelegramBotClient

    'real group: -195688192
    'test group: -366195838

    ' Telegram logging: ERROR, BRIEF, FULL, (DEBUG)

    Public Function ConnectTelegram(vTelegramBot As String) As String

        ConnectTelegram = "OK"
        Try
            pBott = New Telegram.Bot.TelegramBotClient(vTelegramBot)

        Catch ex As Exception
            ConnectTelegram = "ConnectTelegram: " + ex.Message
            LogSessionEntry("ERROR", "ConnectTelegram: " + ex.Message, "", "ConnectTelegram", "PROGRAM")
        End Try
    End Function


    Public Function TelegramMessage(vMessage As String, vGroup As String) As String

        TelegramMessage = "OK"
        Try
            pBott.SendTextMessageAsync(vGroup, vMessage)
            'minor stop, but flood control will block certain messages
            'Thread.Sleep(10)
        Catch ex As Exception
            TelegramMessage = "TelegramMessage: " + ex.Message
            LogSessionEntry("ERROR", "TelegramMessage: " + ex.Message, "", "TelegramMessage", "PROGRAM")
        End Try
    End Function

End Module