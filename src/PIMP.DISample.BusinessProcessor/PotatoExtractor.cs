using PIMP.DISample.Common.Entity;
using PIMP.DISample.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMP.DISample.BusinessProcessor
{
    public class PotatoExtractor : IPotatoExtractor
    {

        private IRepository<Potato> potatoRepository { get; }

        public PotatoExtractor(IRepository<Potato> potatoRepository)
        {
            this.potatoRepository = potatoRepository;
        }
        
        public int CountPotatoYield()
        {
            var potatos = this.potatoRepository.Get(p => p.Weight / 3 > -45);
            var count = potatos.Count();
            return count;
        }

    }
}
