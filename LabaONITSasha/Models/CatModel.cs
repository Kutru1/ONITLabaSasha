using System.ComponentModel.DataAnnotations;

namespace LabaONITSasha.Models
{
    public class CatModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public int Gender { get; set; }
        public DateTime DateOfPet { get; set; }
    }

}