using ArtPatio.Models;
using ArtPatio.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public IActionResult History(DateTime? startDate, DateTime? endDate, string transactionType)
        {
            int userId = HttpContext.Session.GetInt32("Id").GetValueOrDefault();

            // Fetch all transactions for the user
            var transactions = _transactionRepository.GetTransactionHistory(userId);

            // If startDate and endDate are provided, handle filtering
            if (startDate.HasValue && endDate.HasValue)
            {
                // Adjust endDate for same day case
                if (startDate.Value.Date == endDate.Value.Date)
                {
                    endDate = endDate.Value.Date.AddDays(1).AddTicks(-1);
                }

                // Filter transactions between startDate and endDate
                transactions = transactions.FindAll(t => t.TransactionDate >= startDate.Value && t.TransactionDate <= endDate.Value);

                // Preserve the dates in the ViewData for form repopulation
                ViewData["startDate"] = startDate.Value.ToString("yyyy-MM-dd");
                ViewData["endDate"] = endDate.Value.ToString("yyyy-MM-dd");
            }

            // Filter by transaction type if specified
            if (!string.IsNullOrEmpty(transactionType))
            {
                transactions = transactions.FindAll(t => t.TransactionType == transactionType);
                ViewData["transactionType"] = transactionType; // Preserve selected transaction type
            }

            return View(transactions); // Pass the filtered transactions to the view
        }

    }
}
