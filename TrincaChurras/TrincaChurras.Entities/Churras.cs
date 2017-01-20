using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrincaChurras.Entities
{
    [Serializable]
    public class Churras
    {
        public Guid Id { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public string Observacao { get; set; }
        public decimal ValorComBebida { get; set; }
        public decimal ValorSemBebida { get; set; }
        public List<Participante> Participantes { get; set; } = new List<Participante>();
    }
}
