using Microsoft.EntityFrameworkCore;
using Purchases.DAL.ContextModels;
using Purchases.DAL.Data;
using Purchases.Domain.Services;
using RtuItLab.Infrastructure.Exceptions;
using RtuItLab.Infrastructure.Models.Identity;
using RtuItLab.Infrastructure.Models.Purchases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Purchases.UnitTests.Services
{
    public class PurchasesServiceUnitTests
    {
        private readonly PurchasesService _purchasesService;
        private readonly PurchasesDbContext _purchasesContext;
        public PurchasesServiceUnitTests()
        {
            var options = new DbContextOptionsBuilder<PurchasesDbContext>()
                .UseInMemoryDatabase("test_purchases");
            _purchasesContext = new PurchasesDbContext(options.Options);
          
            InitialContext();
            _purchasesService = new PurchasesService(_purchasesContext);
        }
        [Fact]
        public async Task GetTransactionById_Expect_Throw_NotFoundException()
        {
           var transaction = await _purchasesService.GetTransactionById(new User
           {
               Id = "UserId"
           }, 100);
           if (transaction != null & transaction?.Exception != null)
               Assert.IsType<NotFoundException>(transaction.Exception);
        }
        [Fact]
        public async Task GetTransactionById_Expected_Added_Transaction()
        {
            var transaction = await _purchasesService.GetTransactionById(new User
            {
                Id = "UserId"
            }, 1);
            Assert.NotNull(transaction.Content);
            Assert.NotNull(transaction.Content.Products);
            Assert.NotNull(transaction.Content.Receipt);
        }
        [Fact]
        public async Task UpdateTransaction_Expected_Throw_NotFoundException()
        {
            var response = await _purchasesService.UpdateTransaction(new User
            {
                Id = "UserId"
            },
                new UpdateTransaction { Id = 12312,Date = DateTime.MaxValue});
            Assert.NotNull(response.Exception);
            Assert.IsType<NotFoundException>(response.Exception);
        }
        [Fact]
        public async Task UpdateTransaction_Expected_Throw_BadRequestException()
        {
            var response = await _purchasesService.UpdateTransaction(new User
                {
                    Id = "UserId"
                },
                new UpdateTransaction { Id = 1, Date = DateTime.MaxValue });
            Assert.NotNull(response.Exception);
            Assert.IsType<BadRequestException>(response.Exception);
        }
        [Fact]
        public async Task UpdateTransaction_Expected_Success()
        {
            var response = await _purchasesService.UpdateTransaction(new User
                {
                    Id = "UserId"
                },
                new UpdateTransaction { Id = 2, Date = DateTime.MaxValue });
            Assert.Null(response.Exception);
        }
        [Fact]
        public async Task GetTransactions_Expected_Two_elements_without_receipt_and_products()
        {
            var expectedCountProduct = 0;
            var response = await _purchasesService.GetTransactions(new User
                {
                    Id = "UserId"
                });
            Assert.NotNull(response.Content);
            Assert.Equal(expectedCountProduct, response.Content.First().Products.Count);
            Assert.Null(response.Content.First().Receipt);
        }

        private void InitialContext()
        {
            _purchasesContext.Customers.Add(new CustomerContext
            {
                CustomerId = "UserId",
                Transactions = new List<TransactionContext> {new TransactionContext
                {
                    Products = new List<ProductContext>
                    {
                        new ProductContext()
                    },
                    Date = DateTime.MaxValue,
                    TransactionType = TransactionTypes.InCash,
                    IsShopCreate = true,
                    Receipt = new ReceiptContext
                    {
                        Count = 1,
                        Cost = 1
                    }
                },
                    new TransactionContext
                    {
                        Products = new List<ProductContext>
                        {
                            new ProductContext()
                        },
                        Date = DateTime.MaxValue,
                        TransactionType = TransactionTypes.InCash,
                        IsShopCreate = false
                    }
                }
            });
            _purchasesContext.SaveChanges();
        }
    }
}
