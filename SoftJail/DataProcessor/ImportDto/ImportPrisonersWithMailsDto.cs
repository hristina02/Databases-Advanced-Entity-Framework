using Newtonsoft.Json;
using SoftJail.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportPrisonersWithMailsDto
    {
        [Required]
        [MinLength(ValidationConstans.PrisonerFullNameMinLength)]
        [MaxLength(ValidationConstans.PrisonerFullNameMaxLength)]
        [JsonProperty(nameof(FullName))]

        public string FullName {  get; set; }

        [Required]
        [RegularExpression(ValidationConstans.PrisonerNicknameRegex)]
        [JsonProperty(nameof(NickName))]
        public string NickName { get; set; }

        [Range(ValidationConstans.PersonerAgeMinValue, ValidationConstans.PersonerAgeMaxValue)]
        [JsonProperty(nameof(Age))]
        public int Age {  get; set; }

        [Required]
        [JsonProperty(nameof(IncarcerationDate))]
        public string IncarcerationDate {  get; set; }

        [JsonProperty(nameof(ReleaseDate))]
        public string ReleaseDate { get; set; }

        [Range(typeof(decimal), ValidationConstans.PrisonerBailMinValue,ValidationConstans.PrisonerBailMaxValue)]
        [JsonProperty(nameof(Bail))]
        
        public decimal? Bail {  get; set; }

        [JsonProperty(nameof(CellId))]
        public int? CellId {  get; set; }
        [JsonProperty(nameof(Mails))]
        public ImportPrisonerEmailDto[] Mails { get; set; }
    }
}
