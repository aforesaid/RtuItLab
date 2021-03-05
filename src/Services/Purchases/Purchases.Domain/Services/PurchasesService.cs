using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Purchases.DAL.ContextModels;
using Purchases.DAL.Data;
using Purchases.Domain.Helpers;
using RtuItLab.Infrastructure.Exceptions;
using RtuItLab.Infrastructure.MassTransit;
using RtuItLab.Infrastructure.Models.Identity;
using RtuItLab.Infrastructure.Models.Purchases;

namespace Purchases.Domain.Services
{
    public class PurchasesService : IPurchasesService
    {
        private readonly PurchasesDbContext _context;

        public PurchasesService(PurchasesDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseMassTransit<Transaction>> GetTransactionById(User user, int id)
        {
            await CheckUserIsCreate(user);
            var response = new ResponseMassTransit<Transaction>();
            var customer = await _context.Customers.Include(item => item.Transactions)
                .ThenInclude(item => item.Products)
                .FirstOrDefaultAsync(item => item.CustomerId == user.Id);
            var transaction = customer?.Transactions.FirstOrDefault(item => item.Id == id);
            if (transaction is null)
                response.Exception = new NotFoundException("Transaction not found!");
            else
                response.Content = transaction.ToTransactionDto();
            return response;
        }

        public async Task<ResponseMassTransit<BaseResponseMassTransit>> AddTransaction(User user,
            Transaction transaction)
        {
            await CheckUserIsCreate(user);
            var response = new ResponseMassTransit<BaseResponseMassTransit>();
            var customer = await _context.Customers.FirstOrDefaultAsync(item => item.CustomerId == user.Id);
            customer.Transactions.Add(transaction.ToTransactionContext());
            await _context.SaveChangesAsync();
            response.Content = new BaseResponseMassTransit();
            return response;
        }

        public async Task<ResponseMassTransit<BaseResponseMassTransit>> UpdateTransaction(User user,
            UpdateTransaction transaction)
        {
            await CheckUserIsCreate(user);
            var response = new ResponseMassTransit<BaseResponseMassTransit>();
            try
            {
                var customer = await _context.Customers.Include(item => item.Transactions)
                    .ThenInclude(item => item.Products)
                    .FirstOrDefaultAsync(item => item.CustomerId == user.Id);
                var currentTransaction = customer?.Transactions.FirstOrDefault(item => item.Id == transaction.Id);
                if (currentTransaction is null)
                    throw new NotFoundException("Transaction that is being updated was not found"); 
                await UpdateTransaction(currentTransaction, transaction);
                response.Content = new BaseResponseMassTransit();
            }
            catch (Exception e)
            {
                response.Exception = e;
            }
            return response;
        }

        public async Task<ResponseMassTransit<ICollection<Transaction>>> GetTransactions(User user)
        {
            await CheckUserIsCreate(user);
            var response = new ResponseMassTransit<ICollection<Transaction>>();
            var customer = await _context.Customers.Include(item => item.Transactions)
                .ThenInclude(item => item.Receipt)
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.CustomerId == user.Id);
            response.Content = customer.Transactions.Select(item => item.ToTransactionDto()).ToList();
            return response;
        }

        private async Task CheckUserIsCreate(User user)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(item => item.CustomerId == user.Id);
            if (customer is null)
            {
                await _context.Customers.AddAsync(new CustomerContext
                {
                    CustomerId = user.Id
                });
                await _context.SaveChangesAsync();
            }
        }

        private async Task UpdateTransaction(TransactionContext transactionContext,
            UpdateTransaction updateTransaction)
        {
            if (transactionContext.IsShopCreate)
            {
                if (updateTransaction.Products != null || updateTransaction.Date != new DateTime())
                    throw new BadRequestException("You can't change current shop's transaction!");
                transactionContext.TransactionType = updateTransaction.TransactionType;
                await _context.SaveChangesAsync();
            }
            else
                await UpdateUserTransaction(transactionContext, updateTransaction);
        }

        private async Task UpdateUserTransaction(TransactionContext transactionContext,
            UpdateTransaction updateTransaction)
        {
            transactionContext.TransactionType = updateTransaction.TransactionType;
            if (updateTransaction.Products != null)
                transactionContext.Products =
                    updateTransaction.Products.Select(item => item.ToProductContext()).ToList();
            if (updateTransaction.Date != new DateTime())
                transactionContext.Date = updateTransaction.Date;
            await _context.SaveChangesAsync();
        }
    }
}