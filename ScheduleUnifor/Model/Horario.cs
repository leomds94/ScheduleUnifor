using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleUnifor.Model
{
    public class Horario
    {
        public int Id { get; set; }
        public TurnoNome TurnoNome { get; set; }
        public Periodo Periodo { get; set; }
    }
}
