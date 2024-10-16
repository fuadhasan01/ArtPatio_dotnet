using ArtPatio.Models;
using ArtPatio.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ArtPatio.Controllers
{
    public class TransactionController : BaseController
    {
        private readonly TransactionRepository _transactionRepository;

        public TransactionController(TransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        // GET: /Transaction/History
        public IActionResult History()
        {
            int userId = HttpContext.Session.GetInt32("Id").GetValueOrDefault();
            var transactions = _transactionRepository.GetTransactionHistory(userId);
            return View(transactions); // Pass the transactions to the History view
        }
    }
}
