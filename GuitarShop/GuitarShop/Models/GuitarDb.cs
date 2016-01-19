using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace GuitarShop.Models
{
    public class GuitarDb
    {
        public List<Guitar> Guitars { get; set; }
         
        public GuitarDb()
        {
            Guitars = new List<Guitar>
            {
                new Guitar {Id = 1, Name = "B.C. Rich Bich", Price = 2000, Description = File.ReadAllText($"{HttpRuntime.AppDomainAppPath}\\Content\\Text\\BcRichBich.txt") }, 
                new Guitar {Id = 2, Name = "Fender Stratocaster", Price = 1500, Description = File.ReadAllText($"{HttpRuntime.AppDomainAppPath}\\Content\\Text\\FenderStratocaster.txt")},
                new Guitar {Id = 3, Name = "Gibson Flying V", Price = 3000, Description = File.ReadAllText($"{HttpRuntime.AppDomainAppPath}\\Content\\Text\\GibsonFlyingV.txt")}
            };
        }

        public Guitar GetGuitarById(int guitarId) => Guitars.FirstOrDefault(w => w.Id.Equals(guitarId));
    }
}
