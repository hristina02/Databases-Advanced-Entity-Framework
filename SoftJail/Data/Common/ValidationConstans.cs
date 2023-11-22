using System;
using System.Collections.Generic;
using System.Text;

namespace SoftJail.Data.Common
{
    public class ValidationConstans
    {
        public const int PrisonerFullNameMinLength = 3;
        public const int PrisonerFullNameMaxLength = 20;

        public const int OfficerFullNameMinLength = 3;
        public const int OfficerFullNameMaxLength = 30;

        public const int DepartmentNameMinLength = 3;
        public const int DepartmentNameMaxLength = 25;

        public const int PersonerAgeMinValue = 18;
        public const int PersonerAgeMaxValue= 65;

        public const int CellNumberMinValue = 1;
        public const int CellNumberMaxValue = 1000;


        public const string PrisonerBailMaxValue = "79228162514264337593543950335";
        public const string MailAddressRegex = @"^([A-Za-z\s0-9]+?)(\sstr\.)$";

        public const string PrisonerBailMinValue = "0";

        public const string PrisonerNicknameRegex = @"^(The\s)([A-Z][a-z]*)$";

    }
}
