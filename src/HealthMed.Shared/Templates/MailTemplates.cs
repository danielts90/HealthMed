using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Shared.Templates
{
    public static class MailTemplates
    {
        public static string scheduledTemplate = @"<html>
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Confirmação de Agendamento</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 20px;
        }
        .container {
            max-width: 600px;
            background: #ffffff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
            text-align: center;
        }
        h2 {
            color: #333;
        }
        p {
            color: #555;
            font-size: 16px;
        }
        .details {
            background: #e0f7fa;
            padding: 10px;
            border-radius: 5px;
            margin-top: 15px;
        }
    </style>
</head>
<body>
    <div class=""container"">
        <h2>Confirmação de Agendamento</h2>
        <p>Olá,</p>
        <p>O paciente <strong>{{PACIENTE_NOME}}</strong> agendou uma consulta.</p>
        <div class=""details"">
            <p><strong>Data:</strong> {{DATA_CONSULTA}}</p>
            <p><strong>Horário:</strong> {{HORA_CONSULTA}}</p>
            <p><strong>Médico:</strong> {{MEDICO_NOME}}</p>
        </div>
    </div>
</body>
</html>
";

        public static string canceledTemplate = @"<html>
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Confirmação de Agendamento</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 20px;
        }
        .container {
            max-width: 600px;
            background: #ffffff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
            text-align: center;
        }
        h2 {
            color: #333;
        }
        p {
            color: #555;
            font-size: 16px;
        }
        .details {
            background: #e0f7fa;
            padding: 10px;
            border-radius: 5px;
            margin-top: 15px;
        }
    </style>
</head>
<body>
    <div class=""container"">
        <h2>Cancelamento de Agendamento</h2>
        <p>Olá,</p>
        <p>O paciente <strong>{{PACIENTE_NOME}}</strong> cancelou uma consulta.</p>
        <div class=""details"">
            <p><strong>Data:</strong> {{DATA_CONSULTA}}</p>
            <p><strong>Horário:</strong> {{HORA_CONSULTA}}</p>
            <p><strong>Médico:</strong> {{MEDICO_NOME}}</p>
            <p><strong>Motivo:</strong> {{MOTIVO}}</p>
        </div>
    </div>
</body>
</html>
";

        public static string appointmentUpdated = @"<!DOCTYPE html>
                <html lang=""pt-BR"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <title>Status da Consulta</title>
                </head>
                <body style=""font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;"">
                    <div style=""max-width: 600px; margin: auto; background: #fff; padding: 20px; border-radius: 10px; box-shadow: 0 0 10px rgba(0,0,0,0.1);"">
                        <h2 style=""color: #2c3e50;"">Status da sua consulta</h2>
                        <p>Olá, <strong>{{PACIENTE_NOME}}</strong>,</p>
                        <p>Gostaríamos de informar que sua consulta está foi {{STATUS_CONSULTA}}:</p>
                        <ul>
                            <li><strong>Data:</strong> {{DATA_CONSULTA}}</li>
                            <li><strong>Horário:</strong> {{HORA_CONSULTA}}</li>
                            <li><strong>Médico:</strong> {{MEDICO_NOME}}</li>
                        </ul>
                    </div>
                </body>
                </html>
                ";
    }
}
