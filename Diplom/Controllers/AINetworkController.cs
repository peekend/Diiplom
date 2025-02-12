using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using nnnn;

namespace Diplom.Controllers
{
    //протетсировать
    public class AINetworkController : Controller
    {
        private byte[] ConvertToByteArray(IFormFile file)
        { 
            var memoryStream = new MemoryStream();
            file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
        [HttpGet]
        public IActionResult Scan()
        {
            return View("~/Views/AINetwork/Scan.cshtml");
        }
        [HttpPost]
        public IActionResult Scan(IFormFile image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));
            byte[] imageData = ConvertToByteArray(image);
            var result = NeuronNetwork.NeuronPredict(imageData);
            return Content(result);
        }
    }
}
