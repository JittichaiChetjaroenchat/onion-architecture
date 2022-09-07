using System;

namespace Domain.Models.Token
{
    public class TokenInfo
    {
        public string Value { get; set; }

        public DateTime Expires { get; set; }
    }
}