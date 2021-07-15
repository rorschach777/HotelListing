using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HotelListing.IRepository;
using Microsoft.Extensions.Logging;
using AutoMapper;
using HotelListing.Models;
using Microsoft.AspNetCore.Http;
using HotelListing.Data;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;

        // this is part of the configuration for setting up the controller
        public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var countries = await _unitOfWork.Countries.GetAll();
                // take the domain objects and coverts them to Country DTOs. 
                var results = _mapper.Map<IList<CountryDTO>>(countries);
                // return types are very important.
                // returning a status of 200, with the data
                return Ok(results);
            }
            catch (Exception ex) {
                // for internal usage and info. 
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCountries)}");
                // 500 server issue
                return StatusCode(500, "Internal Server Error. Please try again later.");
                throw;
            }
        }

        [Authorize]
        [HttpGet("{id:int}", Name = "GetCountry")]
        public async Task<IActionResult> GetCountry(int id)
        {
            try
            {
                var country = await _unitOfWork.Countries.Get(q => q.Id == id, new List<string> { "Hotels" });
                // take the domain objects and coverts them to Country DTOs. 
                var results = _mapper.Map<CountryDTO>(country);
                // return types are very important.
                // returning a status of 200, with the data
                return Ok(results);
            }
            catch (Exception ex)
            {
                // for internal usage and info. 
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCountry)}");
                // 500 server issue
                return StatusCode(500, "Internal Server Error. Please try again later.");
                throw;
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDTO countryDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateCountry)}");
                return BadRequest(ModelState);
            }
            try
            {
                var country = _mapper.Map<Country>(countryDTO);
                await _unitOfWork.Countries.Insert(country);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetCountry", new { id = country.Id}, country);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"ISomthing went wrong in the {nameof(CreateCountry)}.");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }


        [Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDTO countryDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateCountry)}");
                return BadRequest(ModelState);
            }
            try
            {
                var country = await _unitOfWork.Countries.Get(q => q.Id == id);
                if (country == null)
                {
                    _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateCountry)}");
                    return BadRequest("Sumbitted data is invalid");
                }

                // this is where the update occurs.
                // whatever the DTO contains, put it in the model (hotel)
                _mapper.Map(countryDTO, country);
                _unitOfWork.Countries.Update(country);
                await _unitOfWork.Save();

                // This returns a 204, we don't have anything to show... 
                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something wen wrong in the {nameof(UpdateCountry)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }


        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteCountry)}");
                return BadRequest();
            }
            try
            {
                var hotel = await _unitOfWork.Countries.Get(q => q.Id == id);
                if (hotel == null)
                {
                    _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteCountry)}");
                    return BadRequest("Submitted data is invalid");
                }
                await _unitOfWork.Countries.Delete(id);
                await _unitOfWork.Save();

                return NoContent();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteCountry)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

    }
}
