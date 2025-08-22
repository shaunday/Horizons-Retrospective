using AssetsFlowWeb.Services.Filtering;
using HsR.Journal.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsFlowWeb.Services.Models
{
    public class UserDataDTO
    {
        [Required]
        public Guid UserId { get; set; }

        //public ICollection<string>? SavedSectors { get; set; }

        public ICollection<string>? Symbols { get; set; }

        public ICollection<FilterDefinition>? AvailableFilters { get; set; } = FilterDefinitionFactory.Create();
    }
}
