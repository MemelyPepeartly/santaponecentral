using Santa.Api.Models.Catalogue_Models;
using Santa.Logic.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.Services.Searcher_Service
{
    public interface ICatalogue
    {
        Task<List<Client>> searchClientByQuery(searchQueryModel searchQuery);
    }
}
