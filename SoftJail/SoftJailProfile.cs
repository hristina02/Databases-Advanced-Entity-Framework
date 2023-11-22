namespace SoftJail
{
    using AutoMapper;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ImportDto;

    public class SoftJailProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public SoftJailProfile()
        {
            //this.CreateMap<ImportDepartmentCellDto, Cell>();
            this.CreateMap<ImportPrisonerEmailDto,Mail>();
            //this.CreateMap<ImportOfficerPrisonerDto,OfficerPrisoner>()
            //    .ForMember(d=>d.PrisonerId,mo=>mo.MapFrom(s=>s.Id));
        }
    }
}
