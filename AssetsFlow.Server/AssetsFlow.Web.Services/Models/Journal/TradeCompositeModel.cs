﻿using HsR.Journal.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HsR.Web.Services.Models.Journal
{
    public class TradeCompositeModel
    {
        [Required]
        public int Id { get; set; }
        public ICollection<TradeElementModel> TradeElements { get; set; } = null!;

        public TradeElementModel? Summary { get; set; }

        public ICollection<string> Sectors { get; set; } = null!;

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TradeStatus Status { get; set; } 

        public DateTime? OpenedAt { get; set; }

        public DateTime? ClosedAt { get; set; }
    }
}
