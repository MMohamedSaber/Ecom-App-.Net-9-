
using System.ComponentModel;

namespace Ecom.Core.Sharing
{
    public class EmailStringBody
    {
        public static string Send(string email, string token, string component, string message)
        {
            string encodeToken=Uri.EscapeDataString(token);
            return $@"
    <html>
        <head>
 <style>
        .button {{
            border: none;
            border-radius: 10px;
            padding: 15px 30px;
            color: #fff;
            background: #007bff;
            cursor: pointer;
            text-decoration: none;
            box-shadow: 0 4px 15px rgba(0, 0, 0, 0.2);
            transition: all 0.3s ease;
            font-size: 16px;
            font-weight: bold;
            font-family: 'Arial', sans-serif;
            animation: glow 1.5s infinite alternate;
        }}

        .button:hover {{
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(0, 0, 0, 0.25);
        }}

        @keyframes glow {{
            from {{
                box-shadow: 0 0 5px rgba(0, 123, 255, 0.5);
            }}
            to {{
                box-shadow: 0 0 20px rgba(0, 123, 255, 0.8);
            }}
        }}
    </style>
        </head>
        <body>
            <h1>{message}</h1>
            <hr>
            <br>
            <a href=""https://localhost:7076/api/Accounts/{component}?email={email}&code={encodeToken}"">
                {message}
            </a>
        </body>
    </html>
    ";
        }
    }
}
