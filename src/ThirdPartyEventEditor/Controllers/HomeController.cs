using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using AutoMapper;
using ThirdPartyEventEditor.Filters;
using ThirdPartyEventEditor.Models;
using ThirdPartyEventEditor.Services.Interfaces;
using ThirdPartyEventEditor.ServiceTables;

[assembly: InternalsVisibleTo("ThirdPartyEventEditor.IntegrationTests")]

namespace ThirdPartyEventEditor.Controllers
{
    [ExecutionTime]
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProxyService<Event> _eventProxy;

        public HomeController(IMapper mapper, IProxyService<Event> eventProxy)
        {
            _mapper = mapper;
            _eventProxy = eventProxy;
        }

        public ActionResult Index()
        {
            var viewModel = _mapper.Map<List<ThirdPartyEventViewModel>>(_eventProxy.ReadAll());
            return View(viewModel);
        }

        public ActionResult Exception() => throw new Exception("This is a test message to scare our admin.");
    }
}