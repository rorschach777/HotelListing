using System;
using AutoMapper;
using HotelListing.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelListing.Models;
using HotelListing.Services;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
        //private readonly SignInManager<ApiUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;



        public AccountController(UserManager<ApiUser> userManager,
            ILogger<AccountController> logger,
            IMapper mapper,
            IAuthManager authManager
            )
        {
            _userManager = userManager;
            //_signInManager = signInManager;
            _logger = logger;
            _mapper = mapper;
            _authManager = authManager;
        }
        // BOTH HTTP verbs mentioned here are HttpPost
        // They are both identitical in terms of what the are expecting...
        // the only way to differentiate is the routes. 



        // When this API gets hit, look in the body for the information that match the DTO
        // At minimum it needs the requried information, so the DTO can sanitize it etc...
        // You would not want to send sensitive information via the URL... only the Body. 
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            _logger.LogInformation($"Registration Attempt for {userDTO.Email}");
            if (!ModelState.IsValid)
            {
                // Standards not met. 
                return BadRequest(ModelState);
            }
            try {
                // It automatically takes the user, and writes the logic for us. 
                var user = _mapper.Map<ApiUser>(userDTO);
                user.UserName = userDTO.Email;
                // The password automatically gets hashed. 
                var result = await _userManager.CreateAsync(user, userDTO.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
                // Adds roles to the user: 
                await _userManager.AddToRolesAsync(user, userDTO.Roles);
                // Success! 
                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Register)}");
                return Problem($"Something went wrong in the {nameof(Register)}", statusCode: 500);
            }
        }


        // When we sanitize what we want,
        // the user could have included malicious informaiton in the body,
        // so here we are only interested in what the DTO requires. otherwise it just ignores it. 
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO userDTO)
        {
            _logger.LogInformation($"Login Attempt for {userDTO.Email}");
            if (!ModelState.IsValid)
            {
                // Standards not met. 
                return BadRequest(ModelState);
            }
            try
            {

                if (!await _authManager.ValidateUser(userDTO))
                {
                    // not authoraized. 
                    return Unauthorized();
                }

                // 204, Success! 
                return Accepted(new { Token = await _authManager.CreateToken() });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Login)}");
                return Problem($"Something went wrong in the {nameof(Login)}", statusCode: 500);
            }
        }




    }
}
