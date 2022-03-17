using Microsoft.AspNetCore.SignalR;
using MM.Models;

namespace MM.Hubs
{
    public class MMHub : Hub<IExpense>
    {
        public async Task NewExpense(Expense expense)
        {
            await Clients.All.NewExpense(expense);
        }
    }   
}
