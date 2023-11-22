namespace SoftJail.DataProcessor
{

    using System;
    using System.IO;
    using System.Text;
    using System.Linq;
    using System.Globalization;

    using Newtonsoft.Json;
    using System.Xml.Serialization;

    using Data;
   

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context
                 .Prisoners
                 .Where(p => ids.Contains(p.Id))
                 .Select(p => new
                 {
                     Id = p.Id,
                     Name = p.FullName,
                     CellNumber = p.Cell.CellNumber,
                     Officers = p.PrisonerOfficers
                     .Select(po => new
                     {
                         OfficerName = po.Officer.FullName,
                         Department = po.Officer.Department.Name,

                     })
                     .ToArray(),
                     TotalOfficerSalary = decimal.Parse(p.PrisonerOfficers.Sum(po => po.Officer.Salary).ToString("f2"))



                 })
                 .OrderBy(p => p.Name)
                 .ThenBy(p => p.Id)
                 .ToArray();

            string json=JsonConvert.SerializeObject(prisoners,Formatting.Indented);
            return json;
                
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            throw new NotImplementedException();
        }
    }
}