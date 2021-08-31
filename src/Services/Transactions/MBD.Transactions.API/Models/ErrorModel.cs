using System.Collections.Generic;
using MBD.Application.Core.Response;

namespace MBD.Transactions.API.Models
{
    public class ErrorModel
    {
        public List<string> Errors { get; set; } = new List<string>();

        public ErrorModel(IResult result)
        {
            Errors.AddRange(result.Message.Split("\n"));
        }

        public ErrorModel() { }
    }
}