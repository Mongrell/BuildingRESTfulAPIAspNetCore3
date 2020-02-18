using System;
using Microsoft.AspNetCore.Mvc;
using CourseLibrary.API.Services;

namespace Pluralsight.Api.Controllers{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController: ControllerBase{
        private readonly ICourseLibraryRepository repository;
        public AuthorsController(ICourseLibraryRepository _repository)
        {
            this.repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
        }
        
        //[HttpGet("api/authors")]
        public IActionResult GetAuthors(){
            var authors = repository.GetAuthors();

            if(authors == null){
                return NoContent();
            }
            return new JsonResult(authors);
        }
    }
}