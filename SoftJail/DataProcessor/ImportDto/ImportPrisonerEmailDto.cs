using Newtonsoft.Json;
using SoftJail.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportPrisonerEmailDto
    {

        [Required]
        [JsonProperty(nameof(Description))]
        public string Description {  get; set; }

        [Required]
        [JsonProperty(nameof(Sender))]
        public string Sender { get; set; }

        [Required]
        [RegularExpression(ValidationConstans.MailAddressRegex)]
        [JsonProperty(nameof(Address))]
        public string Address {  get; set; }
    }
}
