using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HotelListing.IRepository;
using Microsoft.Extensions.Logging;
using AutoMapper;
using HotelListing.Models;

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
            catch (Exception ex){
                // for internal usage and info. 
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCountries)}");
                // 500 server issue
                return StatusCode(500, "Internal Server Error. Please try again later.");
                throw;
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCountry(int id)
        {
            try
            {
                var country = await _unitOfWork.Countries.Get(q => q.Id == id, new List<string> { "Hotels"});
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
    }
}
