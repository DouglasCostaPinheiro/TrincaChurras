using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrincaChurras.Entities
{
    [Serializable]
    public class Participante
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid IdChurras { get; set; }
        public string Nome { get; set; }
        public decimal Contribuicao { get; set; }
        public bool Pago { get; set; }
        public bool Bebida { get; set; }
        public string Observacao { get; set; }
    }
}
