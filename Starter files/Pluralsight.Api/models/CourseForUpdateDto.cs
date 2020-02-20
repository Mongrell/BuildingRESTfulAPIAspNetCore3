using System.ComponentModel.DataAnnotations;
namespace CourseLibrary.API.Models{
    public class CourseForUpdateDto : CourseForManipulation{
        [Required(ErrorMessage = "You have to fill in a description")]
        public override string Description { get => base.Description; set => base.Description = value; }
    }
}