using Newtonsoft.Json;
using SoftJail.Data.Common;
using SoftJail.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
//using System.Text.Json.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportDepartmentWithCellsDto
    {
        [Required]
        [MinLength(ValidationConstans.DepartmentNameMinLength)]
        [MaxLength(ValidationConstans.DepartmentNameMaxLength)]
        [JsonProperty(nameof(Name))]

        public string Name { get; set; }

        [JsonProperty(nameof(Cells))]

        public ImportDepartmentCellDto[] Cells { get; set; }
    }
}
