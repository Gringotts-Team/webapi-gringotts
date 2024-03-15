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
        /// <param name="helper"></param>
        /// <param name="mapper"></param>
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
            try
            {
                mageModel.mag_birthdate = mageModel.mag_birthdate.ToUniversalTime();
                MageDTO mageDTO = _mapper.Map<MageDTO>(mageModel);
                mageDTO.mag_inscription = DateTime.UtcNow;


                if (_helper.IsYounger(mageDTO.mag_birthdate))
                {
                    var msg = "The mage must be at least 16 years old";
                    throw new ApiException(msg);
                }

                await _helper.AALNValidator(mageDTO);

                EntityEntry<MageDTO> mageCreated = await _context.mages.AddAsync(mageDTO);
                await _context.SaveChangesAsync();
                return mageCreated.Entity;
            }
            catch (ApiException ex)
            {
                var msg = ex.Message;
                throw new ApiException(msg);
            }


        }

        /// <summary>
        /// Retrieves a list of mages based on optional filtering criteria.
        /// </summary>
        /// <param name="mageName">The name or partial name of the mage to filter by.</param>
        /// <param name="AALN">The AALN of the mage to filter by.</param>
        /// <param name="minAge">The minimum age of the mage to filter by.</param>
        /// <param name="maxAge">The maximum age of the mage to filter by.</param>
        /// <param name="houseId">The ID of the house to filter by.</param>
        /// <param name="minRegDate">The minimum registration date of the mage to filter by.</param>
        /// <param name="maxRegDate">The maximum registration date of the mage to filter by.</param>
        /// <returns>A list of mages matching the provided criteria.</returns>
        public async Task<List<MageListModel>> GetMageList(
            string? mageName,
            string? AALN,
            int? minAge,
            int? maxAge,
            int? houseId,
            DateTime? minRegDate,
            DateTime? maxRegDate)
        {
            try
            {
                var allMages = await (from mag in _context.mages
                                      join hou in _context.houses on mag.mag_hou_id equals hou.hou_id
                                      orderby mag.mag_name
                                      select new MageListModel
                                      {
                                          mag_id = mag.mag_id,
                                          mag_aaln = mag.mag_aaln,
                                          mag_name = mag.mag_name,
                                          mag_age = _helper.CurrentAge(mag.mag_birthdate),
                                          mag_inscription = mag.mag_inscription,
                                          mag_house = new HouseModel
                                          {
                                              hou_id = hou.hou_id,
                                              hou_name = hou.hou_name
                                          }
                                      }).ToListAsync();

                var filteredMages = allMages.Where(mag =>
                        (mageName == null || mag.mag_name.Contains(mageName, StringComparison.OrdinalIgnoreCase)) &&
                        (AALN == null || mag.mag_aaln.Equals(AALN)) &&
                        (!minAge.HasValue || mag.mag_age >= minAge) &&
                        (!maxAge.HasValue || mag.mag_age <= maxAge) &&
                        (houseId == null || mag.mag_house.hou_id == houseId) &&
                        (!minRegDate.HasValue || mag.mag_inscription >= minRegDate) &&
                        (!maxRegDate.HasValue || mag.mag_inscription <= maxRegDate)
                    ).Select(mag => _mapper.Map<MageListModel>(mag)).ToList();

                return filteredMages;

            }
            catch (Exception ex)
            {
                throw new ApiException($"Error while getting mage list: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a mage by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the mage.</param>
        /// <returns>Returns the mage if found, otherwise returns a BadRequest.</returns>
        public async Task<MageDTO> GetMage(int id)
        {
            MageDTO? mage = await _context.mages.FindAsync(id);
            if (mage != null)
            {
                return mage;
            }
            else
            {
                var msg = "There is no mage with this id";
                throw new ApiException(msg);
            }

        }

        /// <summary>
        /// Updates an existing mage by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the mage to be updated.</param>
        /// <param name="mageModel">The mage model containing updated information.</param>
        /// <returns>Returns the updated mage if successful, otherwise returns a BadRequest.</returns>
        public async Task<MageDTO> UpdateMage(int id, MageModel mageModel)
        {
            try
            {
                MageDTO? mage = await _context.mages.FindAsync(id);

                if (mage != null)
                {

                    mage = _mapper.Map(mageModel, mage);

                    if (_helper.IsYounger(mage.mag_birthdate))
                    {
                        var msg = "The mage must be at least 16 years old";
                        throw new ApiException(msg);
                    }

                    await _helper.AALNValidator(mage);

                    await _context.SaveChangesAsync();
                    return mage;

                }
                else
                {
                    var msg = "There is no mage with this id";
                    throw new ApiException(msg);
                }
            }
            catch (ApiException ex)
            {
                var msg = ex.Message;
                throw new ApiException(msg);
            }


        }

    }
}
