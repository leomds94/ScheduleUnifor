using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleUnifor.Model
{
    public class Sala
    {
        public string NumeroSala { get; set; }
        
        public Bloco Bloco { get; set; }

        List<Reserva> Reservas { get; set; }

    }
}
