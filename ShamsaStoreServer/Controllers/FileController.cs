using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.File;
using System.IO;
using System.Threading.Tasks;
using System;

namespace ShamsaStoreServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {

        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileController(IWebHostEnvironment environment)
        {
            _webHostEnvironment = environment;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Post([FromForm] FileDto model)
        {
            try
            {
                var year = DateTime.Now.Year;

                var yearFolder = 
                    Path.Combine(_webHostEnvironment.WebRootPath, year.ToString());

                if (!Path.Exists(yearFolder))
                {
                    Directory.CreateDirectory(yearFolder);
                }

                var month = DateTime.Now.Month;

                var monthFolder = Path.Combine(yearFolder, month.ToString());

                if (!Path.Exists(monthFolder))
                {
                    Directory.CreateDirectory(monthFolder);
                }

                var day = DateTime.Now.Day;

                var dayFolder = 
                    Path.Combine(monthFolder, day.ToString());

                if (!Path.Exists(dayFolder))
                {
                    Directory.CreateDirectory(dayFolder);
                }

                if (model.File?.Length > 0)
                {
                    var imageName = model.File.FileName;

                    var extension = 
                        Path.GetExtension(imageName);


                    var fullPath =
                        Path.Combine(dayFolder, imageName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await model.File.CopyToAsync(stream);
                    }

                    return Ok($"{year}/{month}/{day}/{imageName}");
                }
                else
                {
                    return BadRequest("فایل وجود ندارد");
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
