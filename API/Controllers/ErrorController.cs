using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Praksa.BLL.Exceptions;

namespace Praksa.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            var exception = (BusinessException)context!.Error;

            var message = exception?.Message;

            if (message is not null)
            {
                return StatusCode((int)exception!.StatusCode, message);
            }

            return StatusCode(500, $"{nameof(ErrorController)}: Unexpected server error occured. Log message is not found.");
        }
    }
}
