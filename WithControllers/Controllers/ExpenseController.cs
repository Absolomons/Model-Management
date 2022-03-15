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
    public class MMController : ControllerBase
    {
        private readonly MMDb _context;

        public MMController(MMDb context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetExpense(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);

            if (expense == null)
            {
                return NotFound();
            }

            return expense;

            //Der skal også hentes modellens job og expenses.
        }

        // POST: api/Todoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Expense>> PostExpense(Expense expense, int jobId, int modelId)
        {

            _context.Expenses.Add(expense);
            var model = await _context.Models.FindAsync(modelId);
            var job = await _context.Jobs.FindAsync(jobId);

            model.Expenses.Add(expense);
            job.Expenses.Add(expense);

            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetTodo", new { id = todo.Id }, todo);
            return CreatedAtAction(nameof(GetExpense), new { id = expense.ExpenseId }, expense);
        }
    }
}
