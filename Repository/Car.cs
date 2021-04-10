using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Repository
{
    public class Car
    {
        public int Id { get; set; }
        public string CarName { get; set; }
        public string Color { get; set; }
        public DateTime Time { get; set; }
        public int Cost { get; set; }
        public List<Photo> Photos { get; set; }
    }
    public class Photo
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Car))]
        public int CarId { get; set; }
        public string LocationName { get; set; }
        public Car car { get; set; }
    }
}
