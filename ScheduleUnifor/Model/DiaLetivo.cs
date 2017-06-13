using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleUnifor.Model
{
    public class DiaLetivo
    {
        public int Id { get; set; }
        public NomeDiaSemana NomeDiaSemana { get; set; }
        public DateTime Dia { get; set; }
    }
}
