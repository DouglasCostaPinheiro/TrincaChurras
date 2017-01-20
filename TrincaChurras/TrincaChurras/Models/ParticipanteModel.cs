using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TrincaChurras.Entities;

namespace TrincaChurras.Models
{
    public class ParticipanteGridModel {
        public Guid Id { get; set; }
        [Display(Name = "Nome")]
        public string Nome { get; set; }
        [Display(Name = "Contribuição")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Contribuicao { get; set; }
        [Display(Name = "Bebida")]
        public bool Bebida { get; set; }
        [Display(Name = "Pago")]
        public bool Pago { get; set; }
        [Display(Name = "Observação")]
        public string Observacao { get; set; }

        public static explicit operator ParticipanteGridModel(Participante pParticipante)
        {
            return new ParticipanteGridModel
            {
                Id = pParticipante.Id,
                Nome = pParticipante.Nome,
                Contribuicao = pParticipante.Contribuicao,
                Bebida = pParticipante.Bebida,
                Pago = pParticipante.Pago,
                Observacao = pParticipante.Observacao
            };
        }
    }

    public class ParticipanteModel
    {
        public Guid IdParticipante { get; set; } = Guid.NewGuid();
        public Guid IdChurras { get; set; }
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Nome { get; set; }
        [Display(Name = "Valor da Contribuição")]
        public decimal Contribuicao { get; set; }
        [Display(Name = "Pago")]
        public bool Pago { get; set; }
        [Display(Name = "Bebida?")]
        public bool Bebida { get; set; }
        [Display(Name = "Obs")]
        public string Observacao { get; set; }

        public Participante ToEntity() {
            return new Participante {
                Id = IdParticipante,
                IdChurras = IdChurras,
                Nome = Nome,
                Contribuicao = Contribuicao,
                Pago = Pago,
                Bebida = Bebida,
                Observacao = Observacao
            };
        }

        public static explicit operator ParticipanteModel(Participante pParticipante)
        {
            return new ParticipanteModel
            {
                IdParticipante = pParticipante.Id,
                IdChurras = pParticipante.IdChurras,
                Nome = pParticipante.Nome,
                Contribuicao = pParticipante.Contribuicao,
                Pago = pParticipante.Pago,
                Bebida = pParticipante.Bebida,
                Observacao = pParticipante.Observacao
            };
        }
    }
}