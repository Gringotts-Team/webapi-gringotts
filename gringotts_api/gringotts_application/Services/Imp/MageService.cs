using AutoMapper;
using gringotts_application.DTOs;
using gringotts_application.Exceptions;
using gringotts_application.Helpers;
using gringotts_application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace gringotts_application.Services.Imp
{
    /// <summary>
    /// Implementation of the mage service interface (IMageService).
    /// </summary>
    public class MageService : IMageService
    {
        private readonly GringottsDbContext _context;
        private readonly ServiceHelper _helper;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for MageService.
        /// </summary>
        /// <param name="context">Database context for Gringotts.</param>
        public MageService(GringottsDbContext context, ServiceHelper helper, IMapper mapper)
        {
            _context = context;
            _helper = helper;
            _mapper = mapper;
        }

        /// <summary>
        /// Create a new mage using autoMapper.
        /// </summary>
        /// <param name="mageModel">mage to be created</param>
        /// <returns>mage created</returns>
        /// 
        public async Task<MageDTO> CreateMage(MageModel mageModel)
        {
            mageModel.mag_birthdate = mageModel.mag_birthdate.ToUniversalTime();
            MageDTO mageDTO = _mapper.Map<MageDTO>(mageModel);
            mageDTO.mag_inscription = DateTime.UtcNow;


            if (_helper.IsYounger(mageDTO.mag_birthdate))
            {
                var msg = "The mage must be at least 16 years old";
                throw new ApiException(msg);
            }

            string[] aaln = mageDTO.mag_aaln.Split("-");
            string schoolPart = aaln[0];
            string housePart = aaln[1];
            string numericPart = aaln[2];

            var houseInitials = await _context.houses.Where(h => h.hou_id == mageDTO.mag_hou_id)
            .Select(h => h.hou_name.Substring(0, 2))
            .FirstOrDefaultAsync();

            houseInitials = houseInitials?.ToUpper();

            if (houseInitials != null && houseInitials != housePart)
            {
                var msg = "The house indicated in the aaln does not coincide with the house of the mage";
                throw new ApiException(msg);
            }

            if (houseInitials == "OT" && schoolPart != "OT")
            {
                var msg = "For the OT house, the school initials should be OT";
                throw new ApiException(msg);
            }
            else if (houseInitials != "OT" && schoolPart != "HG")
            {
                var msg = "For " + houseInitials + " house, the school initials should be HG";
                throw new ApiException(msg);
            }

            var exists = await _context.mages.AnyAsync(m => m.mag_aaln.Contains(numericPart));

            if (exists)
            {
                var msg = "AALN already exists";
                throw new ApiException(msg);
            }
            else
            {
                EntityEntry<MageDTO> mageCreated = await _context.mages.AddAsync(mageDTO);
                await _context.SaveChangesAsync();
                return mageCreated.Entity;
            }


        }

        /// <summary>
        /// Retrieves a list of mages based on the provided filtering criteria.
        /// </summary>
        /// <param name="filter">The model containing the filtering parameters.</param>
        /// <returns>A list of MageListModel objects representing the filtered mages.</returns>
        public async Task<List<MageListModel>> GetMageList(MageFilterModel filter)
        {
            try
            {
                var allMages = await (from mag in _context.mages
                                      join hou in _context.houses on mag.mag_hou_id equals hou.hou_id
                                      orderby mag.mag_name
                                      select new MageListModel
                                      {
                                          mag_aaln = mag.mag_aaln,
                                          mag_name = mag.mag_name,
                                          mag_age = _helper.CurrentAge(mag.mag_birthdate),
                                          mag_inscription = mag.mag_inscription,
                                          mag_house = new HouseModel
                                          { hou_id = hou.hou_id,
                                            hou_name = hou.hou_name
                                          }
                                      }).ToListAsync();

                var filteredMages = allMages.Where(mag =>
                    (filter.mageName == null || mag.mag_name.Contains(filter.mageName, StringComparison.OrdinalIgnoreCase)) &&
                    (filter.AALN == null || mag.mag_aaln.Equals(filter.AALN)) &&
                    (!filter.minAge.HasValue || mag.mag_age >= filter.minAge) &&
                    (!filter.maxAge.HasValue || mag.mag_age <= filter.maxAge) &&
                    (filter.houseId == null || mag.mag_house.hou_id == filter.houseId) &&
                    (!filter.minRegDate.HasValue || mag.mag_inscription >= filter.minRegDate) &&
                    (!filter.maxRegDate.HasValue || mag.mag_inscription <= filter.maxRegDate)
                ).ToList();


                return filteredMages;

            }
            catch (Exception ex)
            {
                throw new ApiException($"Error while getting mage list: {ex.Message}");
            }
        }
    }
}
