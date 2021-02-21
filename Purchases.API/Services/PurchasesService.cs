using Microsoft.EntityFrameworkCore;
using Purchases.API.Data;
using Purchases.API.Helpers;
using Purchases.API.Models.ContextModels;
using Purchases.API.Models.DTOs;
using Purchases.API.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Purchases.API.Services
{
    public class PurchasesService : IPurchasesService
    {
        private readonly PurchasesDbContext _context; 
        public PurchasesService(PurchasesDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction> GetTransactionById(UserDTO user, int id)
        {
            var customer = await _context.Customers.Include(item => item.Transactions)
                                                   .ThenInclude(item => item.Products)
                                                   .FirstOrDefaultAsync(item => item.CustomerId == user.Id);
            var transaction = customer?.Transactions.FirstOrDefault(item => item.Id == id);
            return transaction.ToTransactionDto();
        }

        public async Task AddTransaction(UserDTO user, Transaction transaction)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(item => item.CustomerId == user.Id);
            customer.Transactions.Add(transaction.ToTransactionContext());
            await _context.SaveChangesAsync();
        }

        public async Task<(string, bool)> UpdateTransaction(UserDTO user, UpdateTransaction transaction)
        {
            var customer = await _context.Customers.Include(item => item.Transactions)
                .ThenInclude(item => item.Products)
                .FirstOrDefaultAsync(item => item.CustomerId == user.Id);
            var currentTransaction = customer?.Transactions.FirstOrDefault(item => item.Id == transaction.Id);
            var response = UpdateTransaction(currentTransaction, transaction);
            return response is null
                ? (null, true)
                : (response, false);
        }
        public async Task<ICollection<Transaction>> GetTransactions(UserDTO user)
        {
            var customer = await _context.Customers.Include(item => item.Transactions)
                .ThenInclude(item => item.Products)
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.CustomerId == user.Id);
            return customer.Transactions.Select(item => item.ToTransactionDto()).ToList();
        }

        private string UpdateTransaction(TransactionContext transactionContext, UpdateTransaction updateTransaction)
        {
            if (transactionContext.IsShopCreate)
            {
                if (updateTransaction.Products != null || updateTransaction.Date != new DateTime())
                    return "You can't change current shop's transaction!";
                transactionContext.TransactionType = updateTransaction.TransactionType;
            }
            else
                UpdateUserTransaction(transactionContext, updateTransaction);
            return null;

        }
        private void UpdateUserTransaction(TransactionContext transactionContext, UpdateTransaction updateTransaction)
        {
            transactionContext.TransactionType = updateTransaction.TransactionType;
            if (updateTransaction.Products != null)
                transactionContext.Products = updateTransaction.Products.Select(item => item.ToProductContext()).ToList();
            if (updateTransaction.Date != new DateTime())
                transactionContext.Date = updateTransaction.Date;
        }
    }
}
