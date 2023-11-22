namespace SoftJail.DataProcessor
{

    using Data;
    using Microsoft.EntityFrameworkCore.Internal;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using AutoMapper;
    using System.Globalization;
    using System.Xml.Serialization;
    using System.IO;
    using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
    using SoftJail.Data.Models.Enums;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data";

        private const string SuccessfullyImportedDepartment = "Imported {0} with {1} cells";

        
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportDepartmentWithCellsDto[] departmentDtos = JsonConvert.DeserializeObject<ImportDepartmentWithCellsDto[]>(jsonString);

            List<Department> departments = new List<Department>();

            foreach (ImportDepartmentWithCellsDto departmentDto in departmentDtos)
            {
                if (!IsValid(departmentDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Department d = new Department()
                {
                    Name = departmentDto.Name
                };

                bool isDepValid = true;
                foreach (ImportDepartmentCellDto cellDto in departmentDto.Cells)
                {
                    if (!IsValid(cellDto))
                    {
                        isDepValid = false;
                        break;
                    }

                    d.Cells.Add(new Cell()
                    {
                        CellNumber = cellDto.CellNumber,
                        HasWindow = cellDto.HasWindow
                    });
                }

                if (!isDepValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (d.Cells.Count == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                departments.Add(d);
                sb.AppendLine(String.Format(SuccessfullyImportedDepartment, d.Name, d.Cells.Count));
            }

            context.Departments.AddRange(departments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportPrisonersWithMailsDto[] prisonerDtos = JsonConvert.DeserializeObject<ImportPrisonersWithMailsDto[]>(jsonString);

            List<Prisoner> priosoners = new List<Prisoner>();
            ICollection<Prisoner>validPrisoners=new List<Prisoner>();

            foreach (var pDto in prisonerDtos)
            {
                if (!IsValid(pDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                if(pDto.Mails.Any(mDto=>!IsValid(mDto)))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                bool isIncarcerationDateValid = DateTime.TryParseExact(pDto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                 DateTimeStyles.None ,  out DateTime IncarcerationDate);
                if(!isIncarcerationDateValid )
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                DateTime? releaseDate = null;
                if(!String.IsNullOrWhiteSpace(pDto.ReleaseDate))
                {
                    bool isReleaseDateValid = DateTime.TryParseExact(pDto.ReleaseDate, "dd/MM/yyyy"
                        , CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime releaseDateValue);

                    if(!isReleaseDateValid ) 
                    {
                        sb.AppendLine("Invalid Data");
                        continue;
                    
                    }

                    releaseDate = releaseDateValue;
                }

                Prisoner prisoner = new Prisoner()
                {  
                    FullName=pDto.FullName,
                    Nickname=pDto.NickName,
                    Age=pDto.Age,
                    IncarcerationDate=IncarcerationDate,
                    ReleaseDate=releaseDate,
                    Bail=pDto.Bail,
                    CellId=pDto.CellId

                
                };

                foreach (var mDto in pDto.Mails) 
                {
                    Mail mail=Mapper.Map<Mail>(mDto);
                    prisoner.Mails.Add(mail);
                }

                validPrisoners.Add(prisoner);
                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");

            }  

            context.Prisoners.AddRange(validPrisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlRootAttribute xmlRoot=new XmlRootAttribute("Officers");
          XmlSerializer xmlSerializer=new XmlSerializer(typeof(ImportOfficersWithPrisonersDto[]),xmlRoot);
            StringReader reader=new StringReader(xmlString);
            ImportOfficersWithPrisonersDto[] oDtos = (ImportOfficersWithPrisonersDto[])
                xmlSerializer.Deserialize(reader);

            ICollection<Officer>validOfficers= new List<Officer>();

            foreach (var oDto in oDtos) 
            {
                if(!IsValid(oDto))
                {
                    sb.AppendLine($"Invalid Data");
                    continue;
                }

                bool isPositionEnumValid =
                    Enum.TryParse(typeof(Position), oDto.Position, out object posObj);
                if( !isPositionEnumValid )
                {
                    sb.AppendLine($"Invalid Data");
                    continue;
                }

                bool isWeaponEnuumValid=Enum.TryParse(typeof(Weapon),oDto.Weapon, out object weaponObj);

                Enum.TryParse(typeof(Weapon),oDto.Weapon, out weaponObj);
                if (!isWeaponEnuumValid)
                {
                    sb.AppendLine($"Invalid Data");
                    continue;
                }
                //if(!context.Departments.Any(d=>d.Id==oDto.DepartmentId))
                //{
                //    sb.AppendLine($"Invalid Data");
                //    continue;
                //}

                Officer officer = new Officer()
                {
                    FullName = oDto.FullName,
                    Salary = oDto.Salary,
                    Position = (Position)posObj,
                    Weapon = (Weapon)weaponObj,
                    DepartmentId = oDto.DepartmentId
                };

                foreach(var pDto in oDto.Prisoners )
                {
                    OfficerPrisoner op = new OfficerPrisoner()
                    {
                        Officer = officer,
                        PrisonerId = pDto.Id
                    };
                    officer.OfficerPrisoners.Add(op);
                }
                validOfficers.Add(officer);
                sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");
             
            }

            context.Officers.AddRange(validOfficers);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}