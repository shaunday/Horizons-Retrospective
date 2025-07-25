﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using HsR.Journal.Entities;

namespace HsR.Web.Services.Models.Journal
{
    public class DataElementModel
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ComponentType ComponentType { get; set; }

        [Required]
        public ContentRecord? ContentWrapper { get; set; }
        public ICollection<ContentRecord>? History { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ValueRelevance? TotalCostRelevance { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ValueRelevance? UnitPriceRelevance { get; set; }

        public bool IsRelevantForTradeOverview { get; set; }
        public bool IsRelevantForLocalOverview { get; set; }

        public bool SectorRelevance { get; set; } 
        public ICollection<string>? Restrictions { get; set; }

        [Required]
        public int TradeElementFK { get; set; }
        [Required]
        public int CompositeFK { get; set; }

        public override string ToString() => $"Id={Id}, Title={Title}, Content={ContentWrapper?.ContentValue}";
    }
}
