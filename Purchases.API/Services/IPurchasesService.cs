using Purchases.API.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Purchases.API.Services
{
    public interface IPurchasesService
    {
        public Task<Transaction> GetTransactionById(int id);
        public Task<ICollection<Transaction>> GetTransactions();
        //TODO: в строке возврщатаь id транзакции
        public Task<string> AddTransaction(Transaction transaction);
        //TODO: возвращать false, если пробуют изменить транзакцию магазина (не соблюдение правил)
        public Task<(string,bool)> UpdateTransaction(UpdateTransaction transaction);
    }
}
