using ScheduleUnifor.Data;
using ScheduleUnifor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleUnifor.Helpers
{
    public class ConvertImportToObject
    {
        //private readonly ScheduleContext context;

        public static List<Reserva> Convert(ImportResponse response)
        {

            List<Reserva> reservas = new List<Reserva>();

            for (int i = 0; i < response.Salas.Count; i++)
            {
                List<AulaHorario>  aulaHorarioList = ReadDictionary(response.Horarios[i]);
                foreach (AulaHorario aulaHorario in aulaHorarioList)
                {
                    char bloco;
                    string sala;

                    if (response.Salas[i] == "999")
                    {
                        sala = "99";
                        bloco = '9';
                    }
                    else
                    {
                        bloco = response.Salas[i][0];
                        sala = response.Salas[i].Substring(1);
                    }
                    reservas.Add(new Reserva() {
                        AulaHorario = aulaHorario,
                        Sala = new Sala()
                        {
                            NumeroSala = sala,
                            Bloco = new Bloco(bloco)
                        }
                    });
                }
            }

            return reservas;
        }

        private static List<AulaHorario> ReadDictionary(Dictionary<int, List<string>> dic)
        {
            List<AulaHorario> aulaHorarioList = new List<AulaHorario>();
            foreach(KeyValuePair<int, List<string>> c in dic)
            {
                List<Horario> horarios = ReadTurnoPeriodo(c.Value);
                foreach (Horario horario in horarios)
                {
                    aulaHorarioList.Add(new AulaHorario() {
                        Horario = horario,
                        DiaLetivo = new DiaLetivo() {
                            NomeDiaSemana = (NomeDiaSemana) Enum.Parse(typeof(NomeDiaSemana), c.Key.ToString())
                        }
                    });
                }
            }
            return aulaHorarioList;
        }

        private static List<Horario> ReadTurnoPeriodo(List<string> listHorario)
        {
            List<Horario> horarios = new List<Horario>();
            foreach (string horario in listHorario)
            {
                horarios.Add(new Horario() {
                    TurnoNome = (TurnoNome) Enum.Parse(typeof(TurnoNome), horario[0].ToString()),
                    Periodo = (Periodo) Enum.Parse(typeof(Periodo), horario[1].ToString())
                });
            }
            return horarios;
        }
    }
}
