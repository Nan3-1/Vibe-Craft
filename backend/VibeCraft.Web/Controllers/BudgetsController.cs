using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VibeCraft.Data;
using VibeCraft.Models.Entities;

namespace VibeCraft.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BudgetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 💰 GET: api/budgets/event/5
        // ВЗЕМИ БЮДЖЕТ ПО СЪБИТИЕ
        [HttpGet("event/{eventId}")]
        public async Task<ActionResult<Budget>> GetBudgetForEvent(int eventId)
        {
            var budget = await _context.Budgets
                .FirstOrDefaultAsync(b => b.EventId == eventId);

            if (budget == null)
            {
                return NotFound();
            }

            return budget;
        }

        // ➕ POST: api/budgets
        // СЪЗДАЙ БЮДЖЕТ
        [HttpPost]
        public async Task<ActionResult<Budget>> CreateBudget(Budget budget)
        {
            budget.CreatedAt = DateTime.UtcNow;
            budget.UpdatedAt = DateTime.UtcNow;

            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBudgetForEvent), 
                new { eventId = budget.EventId }, budget);
        }

        // 📈 PUT: api/budgets/5/add-spent
        // ДОБАВИ РАЗХОД КЪМ БЮДЖЕТА
        [HttpPut("{id}/add-spent")]
        public async Task<IActionResult> AddSpentAmount(int id, [FromBody] decimal amount)
        {
            var budget = await _context.Budgets.FindAsync(id);
            if (budget == null)
            {
                return NotFound();
            }

            budget.SpentAmount += amount;
            budget.UpdatedAt = DateTime.UtcNow;

            _context.Entry(budget).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { 
                budgetId = budget.Id, 
                totalAmount = budget.TotalAmount,
                spentAmount = budget.SpentAmount,
                remaining = budget.TotalAmount - budget.SpentAmount
            });
        }
    }
}