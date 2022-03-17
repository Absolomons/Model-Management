#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MM.Data;
using MM.Hubs;
using MM.Models;

namespace MM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly MMDb _context;
        private readonly IHubContext<MMHub, IExpense> _expenseContext;

        public ExpenseController(IHubContext<MMHub, IExpense> expenseContext ,MMDb context )
        {
            _expenseContext = expenseContext;
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetExpense(long id)
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
        public async Task<ActionResult<Expense>> PostExpense(Expense expense, long jobId, long modelId)
        {

            _context.Expenses.Add(expense);
            var job = await _context.Jobs.Where(x => x.JobId == jobId).Include(m => m.Models).Include(e => e.Expenses).FirstOrDefaultAsync();
            var model = await _context.Models.Where(x => x.ModelId == modelId).Include(j => j.Jobs).Include(e => e.Expenses).FirstOrDefaultAsync();

            model.Expenses.Add(expense);
            job.Expenses.Add(expense);

            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetTodo", new { id = todo.Id }, todo);
            await _expenseContext.Clients.All.NewExpense(expense);
            return CreatedAtAction(nameof(GetExpense), new { id = expense.ExpenseId }, expense);
        }
    }
}
