using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rpg_api.Services.CharacterService
{
    public interface ICharacterService
    {
        /// <summary>
        /// Get all characters
        /// </summary>
        /// <returns></returns>
        Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters(int userId);
        
        /// <summary>
        /// Gets a single character by id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns></returns>
        Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id);
        
        /// <summary>
        /// Add a new character
        /// </summary>
        /// <param name="character">AddCharacterDto</param>
        /// <returns>List of Characters</returns>
        Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto character);

        /// <summary>
        /// Updates a character
        /// </summary>
        /// <param name="updatedCharacterDto">UpdateCharacterDto</param>
        Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto character);

        /// <summary>
        /// Removes a character
        /// </summary>
        /// <param name="id">int</param>
        /// <returns></returns>
        Task<ServiceResponse<List<GetCharacterDto>>> RemoveCharacter(int id);
    }
}