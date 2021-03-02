using System.Collections.Generic;
using System.Threading.Tasks;
using RtuItLab.Infrastructure.Models.Identity;
using RtuItLab.Infrastructure.Models.Purchases;

namespace Purchases.Domain.Services
{
    public interface IPurchasesService
    {
        public Task<Transaction> GetTransactionById(User user, int id);
        public Task<ICollection<Transaction>> GetTransactions(User user);
        public Task AddTransaction(User user, Transaction transaction);
        public Task<(string,bool)> UpdateTransaction(User user, UpdateTransaction transaction);
    }
}
