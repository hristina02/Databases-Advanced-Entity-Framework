﻿using Newtonsoft.Json;
using SoftJail.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportDepartmentCellDto
    {

        [Range(ValidationConstans.CellNumberMinValue,ValidationConstans.CellNumberMaxValue)]
        [JsonProperty(nameof(CellNumber))]
         public int CellNumber {  get; set; }

        [JsonProperty(nameof(HasWindow))]
        public bool HasWindow { get; set; }
    }
}
