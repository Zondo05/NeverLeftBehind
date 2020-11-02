using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeverLeftBehind.Models
{
    [Table("responses")]
    public class Response
    {
        [Key]
        public int ResponseId {get;set;}
        public int UserId {get;set;}
        public int ResourceId {get;set;}
    }
}