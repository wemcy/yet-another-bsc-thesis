using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using System.Net.Mail;
using Wemcy.RecipeApp.Backend.Controllers.ErrorHandler;
using Wemcy.RecipeApp.Backend.Exceptions;

namespace Wemcy.RecipeApp.Backend.Tests.UnitTest.Controllers.ErrorHandler;

public class EntityNotFoundHandlerTest
{
    //private ExceptionContext exceptionContext;
    //private readonly Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
    //private readonly Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
    //private readonly Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
    //public EntityNotFoundHandlerTest()
    //{
    //    sut = new CustomHandleErrorAttribute
    //    {
    //        ExceptionType = typeof(HttpException)
    //    };

    //    mockContext.Setup(htx => htx.Request)
    //                   .Returns(mockRequest.Object);

    //    mockContext.Setup(htx => htx.Response)
    //               .Returns(mockResponse.Object);

    //    mockContext.Setup(htx => htx.IsCustomErrorEnabled)
    //               .Returns(true);

    //    exceptionContext = new ExceptionContext()
    //    {
    //        HttpContext = mockContext.Object,
    //        Exception = new HttpException()
    //    };
    //}
    //[Fact]
    //void OnValidExpetion() 
    //{ 
    //    // Arrange
    //    var handler = new EntityNotFoundHandler();
    //    var moqExceptionContext = new Mock<ExceptionContext>();
    //    var exception = new EntityNotFoundExeption("Entity not found");
    //    moqExceptionContext.Setup(e => e.Exception).Returns(exception);

    //    // Act
    //    handler.OnException(moqExceptionContext.Object);
    //    // Assert
    //    moqExceptionContext.VerifySet(e => e.ExceptionHandled = true, Times.Once);


    //}
 

}
