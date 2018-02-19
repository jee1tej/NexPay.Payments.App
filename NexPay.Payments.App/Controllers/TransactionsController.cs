using System;
using System.Collections.Generic;
using System.Net;
using Lychee.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NexPay.Payments.App.Models;
using NexPay.Payments.Services;
using NexPay.Payments.Services.Common;
using NexPay.Payments.Services.DTOS;

namespace NexPay.Payments.App.Controllers
{
    [Produces("application/json")]
    [Route("api/Transactions")]
    public class TransactionsController : Controller
    {
        private ITransactionService _transactionService;
        readonly ILogger<TransactionsController> _log;

        public TransactionsController(ITransactionService transactionService, ILogger<TransactionsController> log)
        {
            _transactionService = transactionService;
            _log = log;
        }

        [HttpPost("{AccountNumber:int}")]
        public IActionResult Create([FromBody] CreateTransactionRequestModel request)
        {
            if (request == null)
            {
                return BadRequest(GetInvalidRequestResponse(new Exception("Request is empty.")));
            }

            try
            {
                Precondition.CheckArgument(!string.IsNullOrEmpty(request.AccountNumber), "AccountNumber", "Account Number cannot be empty.");
                Precondition.CheckArgument(!string.IsNullOrEmpty(request.BSB), "BSB", "BSB cannot be empty.");
                Precondition.CheckArgument(!string.IsNullOrEmpty(request.FirstName), "First Name", "First Name cannot be empty.");
                Precondition.CheckArgument(!string.IsNullOrEmpty(request.AccountNumber), "Last Name", "Last Name cannot be empty.");
                Precondition.CheckArgument(!(request.Amount <= 1.00), "Amount", "Amount is invalid.");

                _log.LogInformation("\nTransactionController.Create - Started. - " + DateTime.Now);

                TransactionsApiResult transactionsApiResult;
                var httpStatusCode = _transactionService.CreateTransaction(
                    new CreateTranscationRequest
                    {
                        AccountNumber = request.AccountNumber,
                        BSB = request.BSB,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Reference = request.Reference,
                        Amount = request.Amount
                    }, out transactionsApiResult);

                _log.LogInformation("\nTransactionController.Create - Ended. - " + DateTime.Now);
                return StatusCode(httpStatusCode.GetHashCode(), transactionsApiResult);                
            }
            catch (ArgumentException ArgEx)
            {
                _log.LogError(ArgEx, "\nTransactionController.Create failed. - "+ DateTime.Now);
                return BadRequest(new TransactionsApiResult
                {
                    Status = ApiStatusCode.Error,
                    code = 10,
                    Message = ArgEx.Message,
                    Data = ArgEx.InnerException
                });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "TransactionController.Create failed.");
                return StatusCode(500, GetInternalServerErrorResponse(ex));
            }
        }

        private TransactionsApiResult GetInvalidRequestResponse(Exception ex)
        {
            var errors = new List<string> { ex.Message };

            if (ex.InnerException != null)
            {
                errors.Add(ex.InnerException.Message);
            }

            return new TransactionsApiResult
            {
                Status = ApiStatusCode.Error,
                code = 10,
                Message = "Invalid Request.",
                Data = errors
            };
        }

        private TransactionsApiResult GetInternalServerErrorResponse(Exception ex)
        {
            var errors = new List<string> { ex.Message };

            if (ex.InnerException != null)
            {
                errors.Add(ex.InnerException.Message);
            }

            return new TransactionsApiResult
            {
                Status = ApiStatusCode.Error,
                code = 20,
                Message = "Unexpected Error.",
                Data = errors
            };
        }
    }
}