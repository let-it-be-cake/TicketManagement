using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using ThirdPartyEventEditor.Exceptions;
using ThirdPartyEventEditor.Filters;
using ThirdPartyEventEditor.Models;
using ThirdPartyEventEditor.Services.Interfaces;
using ThirdPartyEventEditor.ServiceTables;

[assembly: InternalsVisibleTo("ThirdPartyEventEditor.IntegrationTests")]

namespace ThirdPartyEventEditor.Controllers
{
    [ExecutionTime]
    public class EventsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProxyService<Event> _eventProxy;

        public EventsController(IProxyService<Event> eventProxy, IMapper mapper)
        {
            _eventProxy = eventProxy ?? throw new ArgumentNullException(nameof(eventProxy));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Create()
        {
            var createEventDateTime = new DateTime(2021, 01, 01, 12, 00, 00);

            var thirdPartyEventViewModel = new ThirdPartyEventViewModel
            {
                StartDate = createEventDateTime,
                EndDate = createEventDateTime,
            };

            return View(thirdPartyEventViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ThirdPartyEventViewModel model)
        {
            try
            {
                model.PosterImage = await UploadSampleImage(model.Image);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Image == null)
            {
                ModelState.AddModelError("", "Upload an image.");
                return View(model);
            }

            var @event = new Event
            {
                Name = model.Name,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                PosterImage = model.PosterImage,
            };
            try
            {
                _eventProxy.Add(@event);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Event @event = _eventProxy.Read(id);
            return View(_mapper.Map<ThirdPartyEventViewModel>(@event));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ThirdPartyEventViewModel eventView)
        {
            if (!ModelState.IsValid)
            {
                return View(eventView);
            }

            try
            {
                var @event = new Event
                {
                    Id = eventView.Id,
                    Name = eventView.Name,
                    Description = eventView.Description,
                    StartDate = eventView.StartDate,
                    EndDate = eventView.EndDate,
                    PosterImage = await UploadSampleImage(eventView.Image),
                };

                _eventProxy.Change(@event);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(eventView);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            Event evnt = _eventProxy.Read(id);
            return View(_mapper.Map<ThirdPartyEventViewModel>(evnt));
        }

        [HttpGet]
        public ActionResult DeleteInfo(int id)
        {
            var viewModel = _mapper.Map<ThirdPartyEventViewModel>(_eventProxy.Read(id));
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _eventProxy.Delete(id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Index");
            }

            return RedirectToAction("Index");
        }

        private async Task<string> UploadSampleImage(HttpPostedFileBase file)
        {
            if (file == null)
            {
                throw new FileNullException("You must upload the file.");
            }
            var supportedTypes = new[] { "jpg" };
            var fileExt = Path.GetExtension(file.FileName).Substring(1);
            if (!supportedTypes.Contains(fileExt))
            {
                throw new ExtentionFileException($"A file with the {fileExt} extension cannot be downloaded.");
            }

            var memoryStream = new MemoryStream();
            await file.InputStream.CopyToAsync(memoryStream);
            return "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
        }
    }
}