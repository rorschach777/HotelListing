using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    public class HotelController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HotelController> _logger;
        private readonly IMapper _mapper;

        public HotelController(IUnitOfWork unitOfWork, ILogger<HotelController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotels()
        {
            try {
                // Unit of work method. 
                var hotels = await _unitOfWork.Hotels.GetAll();
                // take the domain objects and coverts them to Country DTOs. 
                var results = _mapper.Map<IList<HotelDTO>>(hotels);
                // return status
                return Ok(results);
            }
            catch (Exception ex){
                _logger.LogError(ex, $"An error occured with {nameof(GetHotels)}");
                // return status
                return StatusCode(500, "Internal Server Error. Please try again later.");
                throw;

            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotel(int id)
        {
            try
            {
                // Unit of work method. 
                var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id);
                // take the domain objects and coverts them to Country DTOs. 
                var result = _mapper.Map<HotelDTO>(hotel);
                // return status
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occured with {nameof(GetHotel)}");
                // return status
                return StatusCode(500, "Internal Server Error. Please try again later.");
                throw;

            }
        }

    }
}
