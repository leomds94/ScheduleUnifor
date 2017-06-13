using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleUnifor.Helpers
{
    public class ImportResponse
    {
        public List<Dictionary<int, List<string>>> Horarios { get; set; }
        public List<String> Salas { get; set; }
    }
}
