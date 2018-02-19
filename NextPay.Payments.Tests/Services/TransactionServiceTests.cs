using Microsoft.Extensions.Logging;
using NexPay.Payments.Services;
using NexPay.Payments.Services.Common;
using NexPay.Payments.Services.DTOS;
using NSubstitute;
using NUnit.Framework;
using System.Net;

namespace NextPay.Payments.Tests.Services
{
    [TestFixture]
    public class TransactionsServiceTests
    {
        private ITransactionService _transactionService;
        private ILogger<TransactionService> _log;

        [SetUp]
        public void setUp()
        {
            _transactionService = Substitute.For<ITransactionService>();
            _log = Substitute.For<ILogger<TransactionService>>();
        }

        [Test]
        public void CreateTransaction_Should_Return_Success_When_Transaction_Is_Success()
        {
            _transactionService = new TransactionService(_log);
            var transactionApiResult = new TransactionsApiResult();
            _transactionService.CreateTransaction(Arg.Any<CreateTranscationRequest>(), out transactionApiResult)
                .Returns(HttpStatusCode.OK);

            var httpStatusCode = _transactionService.CreateTransaction(new CreateTranscationRequest(), out transactionApiResult);

            Assert.IsTrue(httpStatusCode == HttpStatusCode.OK);
            Assert.IsTrue(transactionApiResult.Status == ApiStatusCode.Success);
            Assert.IsNotNull(transactionApiResult.Data);
            Assert.IsTrue(((CreateTransactionResponse)transactionApiResult.Data).TransactionStatus == "Success");

        }

        [Test]
        public void CreateTransaction_Should_Return_Error_When_Transaction_Failed()
        {
            _transactionService = new TransactionService(_log);
            var transactionApiResult = new TransactionsApiResult();
            _transactionService.CreateTransaction(Arg.Any<CreateTranscationRequest>(), out transactionApiResult)
                .Returns(HttpStatusCode.InternalServerError);

            var httpStatusCode = _transactionService.CreateTransaction(new CreateTranscationRequest(), out transactionApiResult);

            Assert.IsTrue(httpStatusCode == HttpStatusCode.InternalServerError);
            Assert.IsTrue(transactionApiResult.Status == ApiStatusCode.Error);
            Assert.IsNotNull(transactionApiResult.Data);
            Assert.IsTrue(((CreateTransactionResponse)transactionApiResult.Data).TransactionStatus == "Failed");

        }
    }
}
