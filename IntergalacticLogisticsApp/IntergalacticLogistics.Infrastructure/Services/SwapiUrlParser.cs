using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntergalacticLogistics.Infrastructure.Services
{
    public static class SwapiUrlParser
    {
        public static string ExtractId(string? url)
        {
            if (string.IsNullOrEmpty(url)) return string.Empty;
            var lastSegment = url.TrimEnd('/').Split('/').LastOrDefault();
            return int.TryParse(lastSegment, out _) ? lastSegment ?? string.Empty : string.Empty;
        }
    }

}
