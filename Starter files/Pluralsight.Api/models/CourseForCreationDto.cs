using System.ComponentModel.DataAnnotations;
using CourseLibrary.API.ValidationAttributes;
using System;
using System.Collections.Generic;

namespace CourseLibrary.API.Models{
    public class CourseForCreationDto: CourseForManipulation{
        
        

        // public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        // {
        //     if (Title == Description){
        //         yield return new ValidationResult("The description should be different from the title.", new[] { "CourseForCreationDto" });
        //     }
        // }
    }
}