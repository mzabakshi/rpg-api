using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using rpg_api.Data;

namespace rpg_api.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        /// <summary>
        /// Add a new character
        /// </summary>
        /// <param name="character">AddCharacterDto</param>
        /// <returns>List of Characters</returns>
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto character)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var mappedCharacter = _mapper.Map<Character>(character);
            _context.Characters.Add(mappedCharacter);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceResponse;
        }

        /// <summary>
        /// Get all characters
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var characterList = await _context.Characters.ToListAsync();
            serviceResponse.Data = characterList.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        /// <summary>
        /// Gets a single character by id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns></returns>
        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var character = await _context.Characters.FirstOrDefaultAsync(x => x.Id == id);
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
                var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == updatedCharacterDto.Id);

                if (character == null)
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
                var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);

                if (character == null)
                    throw new Exception($"Character with Id '{id}' not found.");

                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(await _context.Characters.ToListAsync());
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}