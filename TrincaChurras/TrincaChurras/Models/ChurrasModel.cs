using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TrincaChurras.Entities;
using TrincaChurras.Support.Attributes;

namespace TrincaChurras.Models
{
    
    public class ChurrasGridModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Data")]
        public DateTime Data { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Particiantes")]
        public int Participantes { get; set; }

        [Display(Name = "Total Arrecadado")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalArrecadado { get; set; }

        public static explicit operator ChurrasGridModel(Churras pChurras)
        {
            return new ChurrasGridModel
            {
                Id = pChurras.Id,
                Data = pChurras.Data,
                Descricao = pChurras.Descricao,
                Participantes = pChurras.Participantes.Count(),
                TotalArrecadado = pChurras.Participantes.Select(p => p.Contribuicao).Sum()
            };
        }
    }

    public class ChurrasModel
    {
        public Guid? Id { get; set; } = Guid.NewGuid();

        [Display(Name = "Data")]
        [Required(ErrorMessage = "Precisamos saber quando o churras vai acontecer!")]
        public DateTime Data { get; set; }

        [Display(Name = "Hora")]
        [Required(ErrorMessage = "Precisamos saber quando o churras vai acontecer!")]
        public TimeSpan Hora { get; set; }

        [Display(Name = "Porquê?")]
        public string Descricao { get; set; }

        [Display(Name = "Obs")]
        public string Observacao { get; set; }

        [Display(Name = "Com Bebida")]
        public decimal? ValorComBebida { get; set; }

        [Display(Name = "Sem Bebida")]
        public decimal? ValorSemBebida { get; set; }

        [Display(Name = "Número de participantes")]
        public int Participantes { get; set; }

        [Display(Name = "Valor a ser arrecadado")]
        public decimal ValorTotal { get; set; }

        [Display(Name = "Valor já pago")]
        public decimal ValorPago { get; set; }

        [Display(Name = "Valor faltante")]
        public decimal ValorFaltante { get; set; }

        [Display(Name = "Total de bebuns")]
        public int TotalComBebida { get; set; }

        [Display(Name = "Total de saudáveis")]
        public int TotalSemBebida { get; set; }

        public Churras ToEntity()
        {
            return new Churras
            {
                Id = Id ?? Guid.NewGuid(),
                Data = Data.Add(Hora),
                Descricao = string.IsNullOrWhiteSpace(Descricao) ? "Sem motivo" : Descricao,
                Observacao = Observacao,
                ValorComBebida = ValorComBebida ?? 30,
                ValorSemBebida = ValorSemBebida ?? 15
            };
        }

        public static explicit operator ChurrasModel(Churras pChurras)
        {
            var model = new ChurrasModel
            {
                Id = pChurras.Id,
                Data = pChurras.Data,
                Hora = pChurras.Data.TimeOfDay,
                Descricao = pChurras.Descricao,
                Observacao = pChurras.Observacao,
                ValorComBebida = pChurras.ValorComBebida,
                ValorSemBebida = pChurras.ValorSemBebida,
                Participantes = pChurras.Participantes.Count,
                TotalComBebida = pChurras.Participantes.Where(p => p.Bebida).Count(),
                TotalSemBebida = pChurras.Participantes.Where(p => !p.Bebida).Count(),
                ValorPago = pChurras.Participantes.Select(p => p.Contribuicao).Sum()
            };

            model.ValorTotal = ((model.TotalComBebida * model.ValorComBebida.Value) + (model.TotalSemBebida * model.ValorSemBebida.Value));
            model.ValorFaltante = Math.Max(model.ValorTotal - model.ValorPago, 0);

            return model;
        }
    }
}