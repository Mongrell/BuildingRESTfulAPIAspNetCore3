using System;
using System.ComponentModel.DataAnnotations;
using CourseLibrary.API.Models;

namespace CourseLibrary.API.ValidationAttributes{
    public class CourseTitleMustBeDifferentFromDescriptionAttribute : ValidationAttribute{
        protected override ValidationResult IsValid(object value, ValidationContext validationContext){
            var course = (CourseForManipulation)validationContext.ObjectInstance;

            if(course.Title == course.Description){
                return new ValidationResult("The description should not be the same as the title.",new[] {nameof(CourseForManipulation)});
            }

            return ValidationResult.Success;
        }
    }
}