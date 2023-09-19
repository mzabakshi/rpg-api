using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using rpg_api.Data;

namespace rpg_api.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        private async Task<List<GetCharacterDto>> GetCharactersForCurrentUserAsync() => await _context.Characters
                                                                                                    .Include(c => c.Weapon)
                                                                                                    .Include(c => c.Skills)
                                                                                                    .Where(c => c.User!.Id == GetUserId())
                                                                                                    .Select(c => _mapper.Map<GetCharacterDto>(c))
                                                                                                    .ToListAsync();

        /// <summary>
        /// Add a new character
        /// </summary>
        /// <param name="character">AddCharacterDto</param>
        /// <returns>List of Characters</returns>
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto character)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var mappedCharacter = _mapper.Map<Character>(character);
            mappedCharacter.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            _context.Characters.Add(mappedCharacter);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Characters
                                                .Where(c => c.User!.Id == GetUserId())
                                                .Select(c => _mapper.Map<GetCharacterDto>(c))
                                                .ToListAsync();
            return serviceResponse;
        }

        /// <summary>
        /// Get all characters
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            return new ServiceResponse<List<GetCharacterDto>>()
            {
                Data = await GetCharactersForCurrentUserAsync()
            };
        }

        /// <summary>
        /// Gets a single character by id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns></returns>
        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var character = await _context.Characters
                                        .Include(c => c.Weapon)
                                        .Include(c => c.Skills)
                                        .FirstOrDefaultAsync(x => x.Id == id && x.User!.Id == GetUserId());
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            return serviceResponse;
        }

        /// <summary>
        /// Updates a character
        /// </summary>
        /// <param name="updatedCharacterDto">UpdateCharacterDto</param>
        /// <returns></returns>
        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacterDto)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await _context.Characters
                                .Include(u => u.User)
                                .FirstOrDefaultAsync(c => c.Id == updatedCharacterDto.Id);

                if (character == null || character.User!.Id != GetUserId())
                    throw new Exception($"Character with Id '{updatedCharacterDto.Id}' not found.");

                _mapper.Map(updatedCharacterDto, character);
                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        /// <summary>
        /// Removes a character
        /// </summary>
        /// <param name="id">int</param>
        /// <returns></returns>
        public async Task<ServiceResponse<List<GetCharacterDto>>> RemoveCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());

                if (character == null)
                    throw new Exception($"Character with Id '{id}' not found.");

                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();
                serviceResponse.Data = await GetCharactersForCurrentUserAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        /// <summary>
        /// Add new character skill
        /// </summary>
        /// <param name="newCharacterSkill"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await _context.Characters
                                        .Include(c => c.Weapon)
                                        .Include(c => c.Skills)
                                        .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId
                                        && c.User!.Id == GetUserId());

                if (character is null)
                {
                    response.Success = false;
                    response.Message = "Character not found";
                    return response;
                }

                var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);

                if (skill is null)
                {
                    response.Success = false;
                    response.Message = "Skill not found";
                    return response;
                }

                character.Skills!.Add(skill);
                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}