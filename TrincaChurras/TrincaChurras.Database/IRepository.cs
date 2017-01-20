using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrincaChurras.Entities;

namespace TrincaChurras.Database
{
    public interface IRepository
    {
        Guid SaveOrUpdate(Churras pChurras);
        Churras GetById(Guid pIdChurras);
        List<Churras> Get(Func<Churras, bool> pPredicate = null);
        void AddOrUpdateParticipante(Participante pParticipante);
        void RemoveParticipante(Guid pIdParticipante);
    }
}
