using Microsoft.Extensions.Logging;
using NexPay.Payments.Services.Common;
using NexPay.Payments.Services.DTOS;
using System;
using System.IO;
using System.Net;

namespace NexPay.Payments.Services
{
    public interface ITransactionService
    {
        HttpStatusCode CreateTransaction(CreateTranscationRequest createTranscationRequest, out TransactionsApiResult transactionsApiResult);
    }

    public class TransactionService : ITransactionService
    {
        readonly ILogger<TransactionService> _log;

        public TransactionService(ILogger<TransactionService> log)
        {
            _log = log;
        }
        public HttpStatusCode CreateTransaction(CreateTranscationRequest createTranscationRequest, out TransactionsApiResult transactionsApiResult)
        {
            _log.LogInformation("\nTransactionService.CreateTransaction - Started. - " + DateTime.Now);

            transactionsApiResult = WriteTransaction(createTranscationRequest.AccountNumber, createTranscationRequest.BSB, createTranscationRequest.FirstName, createTranscationRequest.LastName, createTranscationRequest.Reference, createTranscationRequest.Amount);

            if (transactionsApiResult.Status != ApiStatusCode.Success)
            {
                _log.LogError(new EventId(), (Exception)transactionsApiResult.Data, "\nTransactionService.CreateTransaction - Failed. - " + DateTime.Now);

                transactionsApiResult.Data = new CreateTransactionResponse
                {
                    TransactionStatus = "Failed"
                };

                return HttpStatusCode.InternalServerError;
            }

            transactionsApiResult.Data = new CreateTransactionResponse
            {
                TransactionStatus = "Success"
            };

            _log.LogInformation("\nTransactionService.CreateTransaction - Ended. - " + DateTime.Now);
            return HttpStatusCode.OK;
        }

        private TransactionsApiResult WriteTransaction(string accountNumber, string bsb, string firstName, string lastName, string reference, double amount)
        {
            try
            {
                using (StreamWriter outputFile = File.AppendText(Path.GetFullPath(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"..\..\..\")) + "Logs" + @"\Transactions.txt"))
                {
                    outputFile.Write("\r\nLog Entry : ");
                    outputFile.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                        DateTime.Now.ToLongDateString());
                    outputFile.WriteLine("");
                    outputFile.WriteLine(" AccountNumber: {0}", accountNumber);
                    outputFile.WriteLine(" BSB: {0}", bsb);
                    outputFile.WriteLine(" First Name: {0}", firstName);
                    outputFile.WriteLine(" Last Name: {0}", lastName);
                    outputFile.WriteLine(" Reference: {0}", reference);
                    outputFile.WriteLine(" Amount: {0}", amount);
                    outputFile.WriteLine("-------------------------------");
                }

                return new TransactionsApiResult
                {
                    Status = ApiStatusCode.Success,
                    code = 30,
                    Message = "Transaction is Successful",
                    Data = ""
                };

            }
            catch (Exception ex)
            {
                return new TransactionsApiResult
                {
                    Status = ApiStatusCode.Fail,
                    code = 20,
                    Message = "Transaction has failed. We are sorry for the inconvinience caused. Please try after sometime.",
                    Data = ex
                };
            }
        }
    }
}