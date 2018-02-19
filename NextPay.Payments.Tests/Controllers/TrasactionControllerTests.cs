using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NexPay.Payments.App.Controllers;
using NexPay.Payments.App.Models;
using NexPay.Payments.Services;
using NexPay.Payments.Services.Common;
using NexPay.Payments.Services.DTOS;
using NSubstitute;
using NUnit.Framework;
using System.Net;

namespace NextPay.Payments.Tests.Controllers
{
    [TestFixture]
    public class TrasactionControllerTests
    {
        private ITransactionService _transactionService;
        private ILogger<TransactionsController> _log;
        [SetUp]
        public void setUp()
        {
            _transactionService = Substitute.For<ITransactionService>();
            _log = Substitute.For<ILogger<TransactionsController>>();

        }

        [Test]
        public void CreateTransaction_Should_Return_BadRequest_When_There_Is_No_RequestBody()
        {
            var transactionCOntroller = new TransactionsController(_transactionService, _log);
            Substitute.For<HttpRequest>();
            var response = (HttpWebResponse)transactionCOntroller.Create(new CreateTransactionRequestModel());

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public void CreateTransaction_Should_Return_Ok()
        {
            var transactionApiResult = new TransactionsApiResult();
            var transactionCOntroller = new TransactionsController(_transactionService, _log);
            Substitute.For<HttpRequest>();

            _transactionService.CreateTransaction(Arg.Any<CreateTranscationRequest>(), out transactionApiResult)
                .Returns(HttpStatusCode.OK);

            var response = (HttpWebResponse)transactionCOntroller.Create(Arg.Any<CreateTransactionRequestModel>());

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }
    }
}
