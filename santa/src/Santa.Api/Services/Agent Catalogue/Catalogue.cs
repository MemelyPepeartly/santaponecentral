using Santa.Api.Models.Catalogue_Models;
using Santa.Api.Services.Searcher_Service;
using Santa.Logic.Interfaces;
using Santa.Logic.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.Services.Agent_Catalogue
{
    public class Catalogue : ICatalogue
    {
        private readonly IRepository repository;

        public Catalogue(IRepository _repository)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
        }

        public Task<List<Client>> searchClientByQuery(searchQueryModel searchQuery)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
    }
}
