using Purchases.API.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Purchases.API.Models.DTOs;

namespace Purchases.API.Services
{
    public interface IPurchasesService
    {
        public Task<Transaction> GetTransactionById(UserDTO user, int id);
        public Task<ICollection<Transaction>> GetTransactions(UserDTO user);
        //TODO: в строке возврщатаь id транзакции
        public Task AddTransaction(UserDTO user, Transaction transaction);
        //TODO: возвращать false, если пробуют изменить транзакцию магазина (не соблюдение правил)
        public Task<(string,bool)> UpdateTransaction(UserDTO user, UpdateTransaction transaction);
    }
}
