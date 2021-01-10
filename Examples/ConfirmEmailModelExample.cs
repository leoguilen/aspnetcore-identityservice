using AuthenticationClientService.Models;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace AuthenticationClientService.Examples
{
    public class ConfirmEmailModelExample : IExamplesProvider<ConfirmEmailModel>
    {
        public ConfirmEmailModel GetExamples()
        {
            return new ConfirmEmailModel
            {
                Email = "example.swagger@email.com",
                Token = "CfDJ8BU+jHD1zMRCofIsGe3LunZzKK5Akuw1wiOL/HUE/xg4B/Rs8lRz+ToB/pIsbIh/9gft/Y3ZJySk5QJO1qHNkiAa3gbiVtipuYN6eCI0PyQ7Pymmafvl0+fWSg3xTGjnCCwXDbtgtA5qEmMYI6NLDXF/HH71GmQmgbLjTc3lbBAV7PL6aUwwd+kFmtqqWpZgQVfvMz/zakZObIvXDvynOLynVrt+iDlO+eUJWN+BaQ15Am1iJmYHUBK2hbtoFyZqHA=="
            };
        }
    }
}
