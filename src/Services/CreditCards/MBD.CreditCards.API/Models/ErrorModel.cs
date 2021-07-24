using System.Collections.Generic;
using MBD.Application.Core.Response;

namespace MBD.CreditCards.API.Models
{
    public class ErrorModel
    {
        public List<string> Errors { get; set; } = new List<string>();

        public ErrorModel(IResult result)
        {
            Errors.AddRange(result.Message.Split("\r\n"));
        }

        public ErrorModel() { }
    }
}