using MM.Models;

namespace MM.Hubs

{
    public interface IExpense
    {
        Task NewExpense(Expense expense);
    }
}