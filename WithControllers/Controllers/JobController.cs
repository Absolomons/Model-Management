#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using MM.Data;
using MM.Models;

namespace MM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly MMDb _context;

        public JobController(MMDb context)
        {
            _context = context;
        }

        // GET: api/JobController
        // Get all jobs in list, mustn't include expenses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobDTO>>> GetJob()
        {
            return await _context.Jobs
                .Include(x => x.Models)
                .Select(job => new JobDTO
                    {
                    Customer = job.Customer,
                    StartDate = job.StartDate,
                    Days = job.Days,
                    Location = job.Location,
                    Comments = job.Comments,
                    Models = job.Models.Select(model => new ModelDTO()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName
                    }).ToList()
                })
                .ToListAsync();
        }

        // GET: api/Jobs/5
        // Get all jobs for specific model, no expenses.
        [HttpGet("model/{modelId}")]
        public async Task<IList<Object>> GetJobModel(long modelId)
        {
            //var model = await _context.Models.Where(x => x.ModelId == id).Include(model => model.Jobs).FirstOrDefaultAsync();
            //var options = new JsonSerializerOptions(JsonSerializerDefaults.Web){ReferenceHandler = ReferenceHandler.IgnoreCycles};
            //return Results.Json(model.Jobs, options);

            IList<object> model = new[]
            {
                await _context.Models
                    .Where(x => x.ModelId == modelId)
                    .Select(y => new
                    {
                        y.FirstName,
                        y.LastName,
                        Jobs = y.Jobs.Select(x => new
                        {
                            x.Customer,
                            x.StartDate,
                            x.Location,
                            x.Comments,
                            x.Days
                        }).ToList()
                    }).ToListAsync()
            };
            return model;
        }

        // GET: api/Jobs/5
        //Get job with expenses
        [HttpGet("{id}")]
        public async Task<ActionResult<Job>> GetJob(long id)
        {

            return await _context.Jobs
                .Include(x => x.Models)
                .Include(x => x.Expenses).FirstOrDefaultAsync();
        }

        // PUT: api/Jobs/5
        // Kun opdatere grunddata in job.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJob(long id, JobDTO jobdto)
        {
            var job = await _context.Jobs.FindAsync(id);

            //Læg jobdto data over i job
            job.StartDate = jobdto.StartDate;
            job.Days = jobdto.Days;
            job.Location = jobdto.Location;
            job.Comments = jobdto.Comments;

            _context.Entry(job).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(id))
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

        // POST: api/Jobs
        // Create new job
        [HttpPost]
        public async Task<ActionResult<Job>> PostJob(JobDTO jobdto)
        {

            var job = new Job();

            job.StartDate = jobdto.StartDate;
            job.Days = jobdto.Days;
            job.Location = jobdto.Location;
            job.Comments = jobdto.Comments;
            job.Customer = jobdto.Customer;

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetTodo", new { id = todo.Id }, todo);
            return CreatedAtAction(nameof(GetJob), new { id = job.JobId }, job);
        }

        // PUT: api/Jobs/modelName
        // Add model to a job
        [HttpPut("model/{modelId}")]
        public async Task<IActionResult> PutJobModel(long jobId, long modelId)
        {
            var job = await _context.Jobs.Where(x => x.JobId==jobId).Include(m => m.Models).FirstOrDefaultAsync();
            var model = await _context.Models.Where(x => x.ModelId==modelId).Include(j => j.Jobs).FirstOrDefaultAsync();

            job.Models.Add(model);
            model.Jobs.Add(job);

            _context.Entry(job).State = EntityState.Modified;
            _context.Entry(model).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        // DELETE: api/Todoes/5// Delete whole job
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(long id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return NoContent();
        } 
        //DELETE: api/Jobs/modelName
        // Delete model from a job
       [HttpDelete("model/{modelId}")]
        public async Task<IActionResult> DeleteJobModel(long jobId, long modelId)
        {

            var job = await _context.Jobs.Where(x => x.JobId == jobId).Include(m => m.Models).FirstOrDefaultAsync();
            var model = await _context.Models.Where(x => x.ModelId == modelId).Include(j => j.Jobs).FirstOrDefaultAsync();
            job.Models.Remove(model);
            _context.Jobs.Update(job);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool JobExists(long id)
        {
            return _context.Jobs.Any(e => e.JobId == id);
        }
    }
}
