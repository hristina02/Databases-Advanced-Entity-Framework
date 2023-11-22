using SoftJail.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Officer")]
    public class ImportOfficersWithPrisonersDto
    {
        [Required]
        [MinLength(ValidationConstans.OfficerFullNameMinLength)]
        [MaxLength(ValidationConstans.OfficerFullNameMaxLength)]
        [XmlElement("Name")]
        public string FullName { get; set; }

        [Range(typeof(decimal),ValidationConstans.PrisonerBailMinValue,ValidationConstans.PrisonerBailMaxValue)]
        [XmlElement("Money")]
        public decimal Salary{  get; set; }

        [Required]
        [XmlElement("Position")]
        public string Position { get; set; }

        [Required]
        [XmlElement("Weapon")]
        public string Weapon { get; set;}

        [XmlElement("DepartmentId")]
        public int DepartmentId {  get; set; }

        [XmlArray("Prisoners")]

        public ImportOfficerPrisonerDto[] Prisoners { get; set;}
    
    }
}
