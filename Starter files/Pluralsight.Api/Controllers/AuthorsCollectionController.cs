using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc;
using CourseLibrary.API.Services;
using CourseLibrary.API.Models;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Helpers;
using AutoMapper;
using System.Collections.Generic;

namespace Pluralsight.Api.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsCollectionController : ControllerBase{
        private readonly ICourseLibraryRepository repository;
        private readonly IMapper mapper;
        public AuthorsCollectionController(ICourseLibraryRepository _repository,
                                           IMapper _mapper)
        {
            this.repository = _repository ?? throw new NullReferenceException(nameof(_repository));
            this.mapper = _mapper ?? throw new NullReferenceException(nameof(_mapper));
        }

        [HttpGet("({ids})", Name="GetAuthorCollection")]
        public IActionResult GetAuthorCollection([FromRoute] [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids){
            if(ids == null){
                return BadRequest();
            }

            var authorEntities = repository.GetAuthors(ids);

            if(ids.Count() != authorEntities.Count()){
                return NotFound();
            }

            var authorsToReturn = mapper.Map<IEnumerable<AuthorDto>>(authorEntities);

            return Ok(authorsToReturn);
        }

        [HttpPost]
        public ActionResult<IEnumerable<AuthorDto>> CreateAuthors(IEnumerable<AuthorForCreationDto> authorsForCreation){
            var authors = mapper.Map<IEnumerable<Author>>(authorsForCreation);

            foreach(var author in authors){
                repository.AddAuthor(author);
            }
            repository.Save();

            var authorsToReturn = mapper.Map<IEnumerable<AuthorDto>>(authors);
            var idsAsString = string.Join(",", authorsToReturn.Select(a => a.Id));

            return CreatedAtRoute("GetAuthorCollection", new { ids = idsAsString }, authorsToReturn);
        }
    }
}