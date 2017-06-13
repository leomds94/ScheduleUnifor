using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleUnifor.Model
{
    public class Bloco
    {
        int Id { get; set; }
        char Descricao { get; set; }

        public Bloco()
        {

        }

        public Bloco(char d)
        {
            this.Descricao = d;
        }
    }
}
