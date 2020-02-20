using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using CourseLibrary.API.Services;
using CourseLibrary.API.Models;
using CourseLibrary.API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Pluralsight.Api.Controllers{
    [Route("api/authors/{authorId}/courses")]
    [ApiController]
    public class CoursesController : ControllerBase{
        private readonly ICourseLibraryRepository courseRepo;
        private readonly IMapper mapper;
        public CoursesController(ICourseLibraryRepository _courseRepo,
                                 IMapper _mapper)
        {
            this.courseRepo = _courseRepo ?? throw new NullReferenceException(nameof(_courseRepo));
            this.mapper = _mapper ?? throw new NullReferenceException(nameof(_mapper));
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<CourseDto>> GetCoursesForAuthor(Guid authorId){
            if(!courseRepo.AuthorExists(authorId)){
                return NotFound();
            }
            var courses = courseRepo.GetCourses(authorId);
            return Ok(mapper.Map<IEnumerable<CourseDto>>(courses));
        }

        [HttpGet("{courseId}", Name="GetCourseForAuthor")]
        public ActionResult<CourseDto> GetCourseForAuthor(Guid authorId, Guid courseId){
            if (!courseRepo.AuthorExists(authorId))
            {
                return NotFound();
            }
            var course = courseRepo.GetCourse(authorId, courseId);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<CourseDto>(course));
        }

        [HttpPost]
        public ActionResult<CourseDto> CreateCourseForAuthor(Guid authorId, CourseForCreationDto courseForCreation){
            if(!courseRepo.AuthorExists(authorId)){
                return NotFound();
            }
            
            var course = mapper.Map<Course>(courseForCreation);
            courseRepo.AddCourse(authorId, course);
            courseRepo.Save();

            var courseResult = mapper.Map<CourseDto>(course);

            return CreatedAtRoute("GetCourseForAuthor", new { authorId = authorId, courseId = courseResult.Id }, courseResult);
        }

        [HttpPut("{courseId}")]
        public IActionResult UpdateCourseForAuthor(Guid authorId, Guid courseId, CourseForUpdateDto courseForUpdate){
            if(!courseRepo.AuthorExists(authorId)){
                return NotFound();
            }
            var courseForAuthor = courseRepo.GetCourse(authorId, courseId);
            if(courseForAuthor == null){
                var courseToAdd = mapper.Map<Course>(courseForUpdate);
                courseToAdd.Id = courseId;

                courseRepo.AddCourse(authorId, courseToAdd);

                courseRepo.Save();

                var courseToReturn = mapper.Map<CourseDto>(courseToAdd);

                return CreatedAtRoute("GetCourseForAuthor", new[] { authorId, courseId = courseToReturn.Id }, courseToReturn);
            }

            mapper.Map(courseForUpdate, courseForAuthor);

            courseRepo.UpdateCourse(courseForAuthor);

            courseRepo.Save();

            return NoContent();
        }

        [HttpPatch("{courseId}")]
        public IActionResult PartiallyUpdateCourseForAuthor(Guid authorId, Guid courseId, JsonPatchDocument<CourseForUpdateDto> patchDocument){
            if(!courseRepo.AuthorExists(authorId)){
                return NotFound();
            }

            var courseForAuthorRepo = courseRepo.GetCourse(authorId, courseId);

            if(courseForAuthorRepo == null){
                var courseDto = new CourseForUpdateDto();
                patchDocument.ApplyTo(courseDto, ModelState);

                if(!TryValidateModel(courseDto)){
                    return ValidationProblem(ModelState);
                }

                var courseToAdd = mapper.Map<Course>(courseDto);
                courseToAdd.Id = courseId;

                courseRepo.AddCourse(authorId, courseToAdd);
                courseRepo.Save();

                var courseToReturn = mapper.Map<CourseDto>(courseToAdd);

                return CreatedAtRoute("GetCourseForAuthor", new[] { authorId, courseId = courseToAdd.Id }, courseToReturn);
            }

            var courseToPatch = mapper.Map<CourseForUpdateDto>(courseForAuthorRepo);
            //Validation needs to be added
            patchDocument.ApplyTo(courseToPatch, ModelState);

            if(!TryValidateModel(courseToPatch)){
                return ValidationProblem(ModelState);
            }

            mapper.Map(courseToPatch, courseForAuthorRepo);

            courseRepo.UpdateCourse(courseForAuthorRepo);

            courseRepo.Save();

            return NoContent();
        }

        [HttpDelete("{courseId}")]
        public ActionResult DeleteCourseForAuthor(Guid authorId, Guid courseId){
            if(!courseRepo.AuthorExists(authorId)){
                return NotFound();
            }
            
            var courseToDelete = courseRepo.GetCourse(authorId, courseId);

            if(courseToDelete == null){
                return NotFound();
            }

            courseRepo.DeleteCourse(courseToDelete);
            courseRepo.Save();

            return NoContent();
        }

        public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary){
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext); 
        } 
    }
}