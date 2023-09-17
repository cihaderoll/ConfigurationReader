using Configuration.Core.Concrete;
using Configuration.Domain.Abstract;
using Configuration.Domain.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Configuration.Test
{
    public class ConfigurationWebTest
    {
        private readonly ConfigurationService _configurationService;
        private readonly IOptions<Common.Dtos.OptionsDto> _configOptions;
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly ApplicationDbContext _context;
        private readonly Mock<IServiceProvider> _serviceProvider;

        public ConfigurationWebTest()
        {
            //_configOptions = new 
            //_context = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>(), )
            //_serviceProvider = new Mock<IServiceProvider>();

            //_serviceProvider.Setup(o => o.GetService(typeof(ApplicationDbContext))).Returns(null)

            //Mock<IRepository<List<Domain.Models.Configuration>>> _mockRepositoryService = new Mock<IRepository<List<Domain.Models.Configuration>>>();

            //Mock<IUnitOfWork> _mockIUnitOfWorkService =  new Mock<IUnitOfWork>();


            //_mockRepositoryService.Setup(e => e.).Returns(new List<Domain.Models.Configuration>
            //{

            //});

            //_mockIUnitOfWorkService.Setup(e => e.Configurations).Returns(new Domain.Concrete.ConfigurationRepository());

            //_configurationService = new ConfigurationService()
        }
    }
}
