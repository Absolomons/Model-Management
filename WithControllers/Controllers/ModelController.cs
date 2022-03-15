#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MM.Data;
using MM.Models;

namespace MM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        private readonly MMDb _context;

        public ModelController(MMDb context)
        {
            _context = context;
        }

        // GET: api/Model
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModelDTO>>> GetModel()
        {
            return await _context.Models.Select(x => ModelToDTO(x))
                .ToListAsync();
        }

        // GET: api/Model/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Model>> GetModel(int id) 
        {
            var model = await _context.Models.FindAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return model;

            //Der skal også hentes modellens job og expenses.
        }

        // PUT: api/Model/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModel(int id, ModelDTO modelDTO)
        {
            var model = await _context.Models.FindAsync(id);

            {
                model.FirstName = modelDTO.FirstName;
                model.LastName = modelDTO.LastName;
                model.Email = modelDTO.Email;
                model.PhoneNo = modelDTO.PhoneNo;
                model.AddresLine1 = modelDTO.AddresLine1;
                model.AddresLine2 = modelDTO.AddresLine2;
                model.Zip = modelDTO.Zip;
                model.City = modelDTO.City;
                model.BirthDay = modelDTO.BirthDay;
                model.Height = modelDTO.Height;
                model.HairColor = modelDTO.HairColor;
                model.ShoeSize = modelDTO.ShoeSize;
                model.Comments = modelDTO.Comments;
            }

            if (id != model.ModelId)
            {
                return BadRequest();
            }

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Todoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Model>> PostModel(ModelDTO modelDTO)
        {
            var model = new Model(modelDTO);
            _context.Models.Add(model);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetTodo", new { id = todo.Id }, todo);
            return CreatedAtAction(nameof(GetModel), new { id = model.ModelId }, model);
        }

        // DELETE: api/Model/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var model = await _context.Models.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            _context.Models.Remove(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoExists(int id)
        {
            return _context.Models.Any(e => e.ModelId == id);
        }

        private static ModelDTO ModelToDTO(Model model) =>
            new ModelDTO
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNo = model.PhoneNo,
                AddresLine1 = model.AddresLine1,
                AddresLine2 = model.AddresLine2,
                Zip = model.Zip,
                City = model.City,
                BirthDay = model.BirthDay,
                Height = model.Height,
                HairColor = model.HairColor,
                ShoeSize = model.ShoeSize,
                Comments = model.Comments
            };
    }
}
