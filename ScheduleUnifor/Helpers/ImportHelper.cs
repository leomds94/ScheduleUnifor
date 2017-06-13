using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using OfficeOpenXml;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace ScheduleUnifor.Helpers
{
    public class ImportHelper
    {
        public static ImportResponse Import(string sWebRootFolder, string sFileName, string scheme, HostString host)
        {
            List<Dictionary<int, List<string>>> horarios = new List<Dictionary<int, List<string>>>();
            List<String> salas = new List<string>();
            List<String> disciplinas = new List<string>();
            bool flagNum = false;

            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            try
            {
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    StringBuilder sb = new StringBuilder();
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int rowCount = worksheet.Dimension.Rows;
                    int ColCount = worksheet.Dimension.Columns;

                    Dictionary<int, List<string>> horariocomp = new Dictionary<int, List<string>>();
                    List<string> certos = new List<string>();
                    List<int> dias = new List<int>();

                    //bool bHeaderRow = true;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        disciplinas.Add(worksheet.Cells[row, 3].Value.ToString());
                        salas.Add(worksheet.Cells[row, 14].Value.ToString());

                        string horario = worksheet.Cells[row, 13].Value.ToString();
                        horariocomp = new Dictionary<int, List<string>>();
                        certos = new List<string>();
                        dias = new List<int>();

                        char turnoAtual = ' ';

                        for (int pos = 0; pos < horario.Length; pos++)
                        {
                            if (horario != "A FIXAR")
                            {
                                if (horario[pos] == 'M' || horario[pos] == 'T' || horario[pos] == 'N')
                                {
                                    if (turnoAtual != ' ')
                                    {
                                        foreach (int dia in dias)
                                        {
                                            if (horariocomp.ContainsKey(dia))
                                            {
                                                foreach (KeyValuePair<int, List<String>> a in horariocomp)
                                                {
                                                    if (a.Key == dia)
                                                    {
                                                        a.Value.AddRange(certos);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                horariocomp.Add(dia, certos);
                                            }
                                        }
                                        certos = new List<string>();
                                        dias = new List<int>();
                                    }
                                    turnoAtual = horario[pos];
                                }
                                else if (char.IsNumber(horario[pos]))
                                {
                                    if (flagNum)
                                    {
                                        foreach (int dia in dias)
                                        {
                                            if (horariocomp.ContainsKey(dia))
                                            {
                                                foreach (KeyValuePair<int, List<String>> a in horariocomp)
                                                {
                                                    if (a.Key == dia)
                                                    {
                                                        a.Value.AddRange(certos);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                horariocomp.Add(dia, certos);
                                            }
                                        }
                                        flagNum = false;
                                        certos = new List<string>();
                                        dias = new List<int>();
                                    }
                                    dias.Add(int.Parse(horario[pos].ToString()));
                                }
                                else if (horario[pos] == 'A' ||
                                        horario[pos] == 'B' ||
                                        horario[pos] == 'C' ||
                                        horario[pos] == 'D' ||
                                        horario[pos] == 'E' ||
                                        horario[pos] == 'F')
                                {
                                    certos.Add(string.Format("{0}{1}", turnoAtual, horario[pos]));
                                    flagNum = true;
                                }
                            }
                            else
                            {
                                if (flagNum)
                                {
                                    foreach (int dia in dias)
                                    {
                                        if (horariocomp.ContainsKey(dia))
                                        {
                                            foreach (KeyValuePair<int, List<String>> a in horariocomp)
                                            {
                                                if (a.Key == dia)
                                                {
                                                    a.Value.AddRange(certos);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            horariocomp.Add(dia, certos);
                                        }
                                    }
                                    certos = new List<string>();
                                    dias = new List<int>();
                                    turnoAtual = ' ';
                                }
                            }
                        }
                        if (flagNum)
                        {
                            foreach (int dia in dias)
                            {
                                if (horariocomp.ContainsKey(dia))
                                {
                                    foreach (KeyValuePair<int, List<String>> a in horariocomp)
                                    {
                                        if (a.Key == dia)
                                        {
                                            a.Value.AddRange(certos);
                                        }
                                    }
                                }
                                else
                                {
                                    horariocomp.Add(dia, certos);
                                }
                            }
                        }
                        if (row == 2869)
                        {

                        }
                        horarios.Add(horariocomp);
                    }

                    string WebRootFolder = sWebRootFolder;
                    string FileName = @"demo.xlsx";
                    string URL = string.Format("{0}://{1}/{2}", scheme, host, FileName);
                    FileInfo sfile = new FileInfo(Path.Combine(WebRootFolder, FileName));
                    if (sfile.Exists)
                    {
                        sfile.Delete();
                        sfile = new FileInfo(Path.Combine(WebRootFolder, FileName));
                    }
                    using (ExcelPackage spackage = new ExcelPackage(sfile))
                    {
                        // add a new worksheet to the empty workbook
                        ExcelWorksheet mon = spackage.Workbook.Worksheets.Add("Segunda");
                        ExcelWorksheet tue = spackage.Workbook.Worksheets.Add("Terça");
                        ExcelWorksheet wed = spackage.Workbook.Worksheets.Add("Quarta");
                        ExcelWorksheet thu = spackage.Workbook.Worksheets.Add("Quinta");
                        ExcelWorksheet fri = spackage.Workbook.Worksheets.Add("Sexta");
                        ExcelWorksheet sat = spackage.Workbook.Worksheets.Add("Sábado");
                        //First add the headers

                        List<string> hs = new List<string> { "MA", "MB", "MC", "MD", "ME", "MF", "TA", "TB", "TC", "TD", "TE", "TF", "NA", "NB", "NC", "ND" };


                        foreach (ExcelWorksheet day in spackage.Workbook.Worksheets)
                        {
                            int d = 2;
                            foreach (string h in hs)
                            {
                                day.Cells[d, 1].Value = h;
                                d++;
                            }
                        }

                        Dictionary<int, string> salaAdded = new Dictionary<int, string>();

                        int salaIndex;
                        for (int i = 2; i < horarios.Count; i++)
                        {
                            if (salaAdded.ContainsValue(salas.ElementAt(i - 2)))
                            {
                                salaIndex = salaAdded.FirstOrDefault(x => x.Value == salas.ElementAt(i - 2)).Key;
                            }
                            else
                            {
                                salaAdded.Add(salaAdded.Count + 2, salas[i - 2]);
                                salaIndex = salaAdded.Count + 1;
                                mon.Cells[1, salaIndex].Value = salas[i - 2];
                            }

                            foreach (KeyValuePair<int, List<String>> a in horarios.ElementAt(i))
                            {
                                if (a.Key == 2)
                                {
                                    foreach (string h in a.Value)
                                    {
                                        for (int d = 0; d < hs.Count; d++)
                                        {
                                            if (h == hs[d])
                                            {
                                                mon.Cells[d + 2, salaIndex].Value = "X";
                                            }
                                        }
                                    }
                                }
                                if (a.Key == 3)
                                {
                                    foreach (string h in a.Value)
                                    {
                                        for (int d = 0; d < hs.Count; d++)
                                        {
                                            if (h == hs[d])
                                            {
                                                tue.Cells[d + 2, salaIndex].Value = "X";
                                            }
                                        }
                                    }
                                }
                                if (a.Key == 4)
                                {
                                    foreach (string h in a.Value)
                                    {
                                        for (int d = 0; d < hs.Count; d++)
                                        {
                                            if (h == hs[d])
                                            {
                                                wed.Cells[d + 2, salaIndex].Value = "X";
                                            }
                                        }
                                    }
                                }
                                if (a.Key == 5)
                                {
                                    foreach (string h in a.Value)
                                    {
                                        for (int d = 0; d < hs.Count; d++)
                                        {
                                            if (h == hs[d])
                                            {
                                                thu.Cells[d + 2, salaIndex].Value = "X";
                                            }
                                        }
                                    }
                                }
                                if (a.Key == 6)
                                {
                                    foreach (string h in a.Value)
                                    {
                                        for (int d = 0; d < hs.Count; d++)
                                        {
                                            if (h == hs[d])
                                            {
                                                fri.Cells[d + 2, salaIndex].Value = "X";
                                            }
                                        }
                                    }
                                }
                                if (a.Key == 7)
                                {
                                    foreach (string h in a.Value)
                                    {
                                        for (int d = 0; d < hs.Count; d++)
                                        {
                                            if (h == hs[d])
                                            {
                                                sat.Cells[d + 2, salaIndex].Value = "X";
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        spackage.Save(); //Save the workbook.
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return new ImportResponse() {
                Salas = salas,
                Horarios = horarios

            };
        }
    }
}
