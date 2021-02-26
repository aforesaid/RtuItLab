using System.Collections.Generic;
using System.Threading.Tasks;
using ServicesDtoModels.Models.Identity;
using ServicesDtoModels.Models.Purchases;

namespace Purchases.Domain.Services
{
    public interface IPurchasesService
    {
        public Task<Transaction> GetTransactionById(User user, int id);
        public Task<ICollection<Transaction>> GetTransactions(User user);
        //TODO: в строке возврщатаь id транзакции
        public Task AddTransaction(User user, Transaction transaction);
        //TODO: возвращать false, если пробуют изменить транзакцию магазина (не соблюдение правил)
        public Task<(string,bool)> UpdateTransaction(User user, UpdateTransaction transaction);
    }
}
