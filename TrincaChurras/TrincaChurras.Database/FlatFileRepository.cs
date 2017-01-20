using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using TrincaChurras.Entities;

namespace TrincaChurras.Database
{
    public class FlatFileRepository : IRepository
    {
        private string _flatFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["PathFlatFileDatabase"]);

        public FlatFileRepository()
        {
            if (!Directory.Exists(Path.GetDirectoryName(_flatFilePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(_flatFilePath));
        }

        public void AddOrUpdateParticipante(Participante pParticipante)
        {
            var list = Get();
            var itemToEdit = list.Find(c => c.Id.Equals(pParticipante.IdChurras));

            if (itemToEdit.Participantes.Any(p => p.Nome.Equals(pParticipante.Nome, StringComparison.InvariantCultureIgnoreCase) && !p.Id.Equals(pParticipante.Id)))
                throw new Exception("Já existe um participante com este nome.");

            itemToEdit.Participantes.RemoveAll(p => p.Id.Equals(pParticipante.Id));
            itemToEdit.Participantes.Add(pParticipante);

            using (Stream stream = File.Open(_flatFilePath, FileMode.Create))
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, list);
            }
        }

        public Churras GetById(Guid pIdChurras)
        {
            return Get(c => c.Id.Equals(pIdChurras)).FirstOrDefault();
        }

        public List<Churras> Get(Func<Churras, bool> pPredicate = null)
        {
            if (!File.Exists(_flatFilePath))
                return new List<Churras>();

            using (Stream st = File.Open(_flatFilePath, FileMode.Open))
            {
                var binaryFormatter = new BinaryFormatter();
                var list = (List<Churras>)binaryFormatter.Deserialize(st);

                if (pPredicate != null)
                    return list.Where(pPredicate).ToList();

                return list;
            }
        }

        public void RemoveParticipante(Guid pIdParticipante)
        {
            throw new NotImplementedException();
        }

        public Guid SaveOrUpdate(Churras pChurras)
        {
            var list = Get();

            if (list.Any(c => c.Id.Equals(pChurras.Id)))
            {
                var itemToEdit = list.Find(c => c.Id.Equals(pChurras.Id));
                itemToEdit.Data = pChurras.Data;
                itemToEdit.Descricao = pChurras.Descricao;
                itemToEdit.Observacao = pChurras.Observacao;
                itemToEdit.ValorComBebida = pChurras.ValorComBebida;
                itemToEdit.ValorSemBebida = pChurras.ValorSemBebida;
            }
            else
                list.Add(pChurras);

            using (Stream stream = File.Open(_flatFilePath, FileMode.Create))
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, list);
            }

            return pChurras.Id;
        }
    }
}
