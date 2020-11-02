using System.ComponentModel.DataAnnotations;

namespace NeverLeftBehind.Models
{
    public class Edit
    {
        [Key]
        public int EditId {get;set;}
        public int UserId {get;set;}
        public int ResourceId {get;set;}
    }
}