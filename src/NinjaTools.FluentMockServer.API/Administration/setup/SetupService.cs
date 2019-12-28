using NinjaTools.FluentMockServer.API.Models;

namespace NinjaTools.FluentMockServer.API.Administration.setup
{
    public class SetupService
    {
        private readonly ISetupRepository _repository;

        public SetupService(ISetupRepository repository)
        {
            _repository = repository;
        }

        void Add(Setup setup) => _repository.Add(setup);
    }
}
