using System.Net;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using CourseLibrary.API.Services;
using CourseLibrary.API.Models;
using CourseLibrary.API.Entities;
using AutoMapper;

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

        [HttpGet("{courseId}")]
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
    }
}