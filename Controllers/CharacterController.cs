using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace rpg_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get()
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
            return Ok(await _characterService.GetAllCharacters(userId));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingle(int id)
        {
            return Ok(await _characterService.GetCharacterById(id));
        }

        [HttpPost("Add")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto addCharacterDto)
        {
            return Ok(await _characterService.AddCharacter(addCharacterDto));
        }

        [HttpPut("Update")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter(UpdateCharacterDto updatedCharacterDto)
        {
            var response = await _characterService.UpdateCharacter(updatedCharacterDto);
            return response.Data == null ? NotFound(response) : Ok(response);
        }

        [HttpDelete("Remove/{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> RemoveCharacter(int id)
        {
            var response = await _characterService.RemoveCharacter(id);
            return response.Data == null ? NotFound(response) : Ok(response);
        }
    }
}