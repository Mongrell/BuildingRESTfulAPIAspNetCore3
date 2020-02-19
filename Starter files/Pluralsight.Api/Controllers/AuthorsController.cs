using System;
using Microsoft.AspNetCore.Mvc;
using CourseLibrary.API.Services;
using CourseLibrary.API.Models;
using CourseLibrary.API.Helpers;
using System.Collections.Generic;
using CourseLibrary.API.ResourceParameters;
using AutoMapper;

namespace Pluralsight.Api.Controllers{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController: ControllerBase{
        private readonly ICourseLibraryRepository repository;
        private readonly IMapper mapper;
        public AuthorsController(ICourseLibraryRepository _repository,
                                 IMapper _mapper)
        {
            this.repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
            this.mapper = _mapper ?? throw new NullReferenceException(nameof(_mapper));
        }
        
        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors([FromQuery] AuthorsResourceParameters parameters){
            var authors = repository.GetAuthors(parameters);

            if(authors == null){
                return NotFound();
            }

            return Ok(mapper.Map<IEnumerable<AuthorDto>>(authors));
        }

        [HttpGet("{authorId}")]
        public ActionResult<AuthorDto> GetAuthor(Guid authorId){
            var author = repository.GetAuthor(authorId);
            if (author == null){
                return NotFound();
            }

            return Ok(mapper.Map<AuthorDto>(author));
        }
    }
}