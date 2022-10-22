using Microsoft.AspNetCore.Mvc;
using RPGSystem_API.Models.DTOs;
using RPGSystem_API.Data___TEMP;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.JsonPatch;

namespace RPGSystem_API.Controllers
{
    [ApiController]
    [Route("api/Character")]
    public class RPGSystemAPIController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<CharacterDTO>> GetCharacters()
        {
            return Ok(CharacterStore.ListCharacters);
        }

        [HttpGet("{id:int}", Name = "GetCharacter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CharacterDTO> GetCharacter(int id)
        {
            var character = CharacterStore.ListCharacters.FirstOrDefault(c => c.Id == id);
            if (character == null)
                return NotFound();

            return Ok(character);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CharacterDTO> CreateCharacter([FromBody] CharacterDTO newCharacter)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (newCharacter == null)
                return BadRequest();

            if (newCharacter.Id > 0)
                return StatusCode(StatusCodes.Status500InternalServerError);

            newCharacter.Id = CharacterStore.ListCharacters.Max(u => u.Id) + 1;
            CharacterStore.ListCharacters.Add(newCharacter);
            return CreatedAtRoute(nameof(GetCharacter), new { id = newCharacter.Id }, newCharacter);
        }

        [HttpDelete("{id:int}", Name = "DeleteCharacter")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteCharacter(int id)
        {
            if (id == 0)
                return BadRequest();

            var character = CharacterStore.ListCharacters.FirstOrDefault(c => c.Id == id);

            if (character == null)
                return NotFound();

            CharacterStore.ListCharacters.Remove(character);
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateCharacter")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdateCharacter(int id, [FromBody] CharacterDTO characterDTO)
        {
            if (id != characterDTO.Id || characterDTO == null)
                return BadRequest();

            var character = CharacterStore.ListCharacters.FirstOrDefault(c => c.Id == id);
            character.Name = characterDTO.Name;
            character.Age = characterDTO.Age;

            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialCharacter")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdatePartialCharacter(int id, JsonPatchDocument<CharacterDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
                return BadRequest();

            var character = CharacterStore.ListCharacters.FirstOrDefault(c => c.Id == id);

            if (character == null)
                return BadRequest();

            patchDTO.ApplyTo(character, ModelState);
            if (!ModelState.IsValid)
                return BadRequest();

            return NoContent();
        }
    }
}
