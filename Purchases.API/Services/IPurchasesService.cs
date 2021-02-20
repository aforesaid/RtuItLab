using Purchases.API.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Purchases.API.Services
{
    public interface IPurchasesService
    {
        public Task<Transaction> GetTransactionById(int id);
        public Task<ICollection<Transaction>> GetTransactions();
        public Task<(string,bool)> AddTransaction(Transaction transaction);
        public Task<string> UpdateTransaction(Transaction transaction);
    }
}
