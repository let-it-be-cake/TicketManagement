using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestEase;
using TicketManagement.Entities.Tables;
using TicketManagement.UserInterface.Clients.EventApi;
using TicketManagement.UserInterface.Clients.VenueApi;
using TicketManagement.UserInterface.Helper;
using TicketManagement.UserInterface.Models.ViewModels;
using TicketManagement.UserInterface.Services;

namespace TicketManagement.UserInterface.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class EditEventsController : Controller
    {
        private readonly IEventClient _eventClient;
        private readonly ILayoutClient _layoutClient;

        private readonly IMapToViewModel _mapHelper;

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly ITokenService _tokenService;

        private readonly ILogger<EditEventsController> _logger;

        public EditEventsController(IEventClient eventClient,
                                    ILayoutClient layoutClient,
                                    IMapToViewModel mapHelper,
                                    IWebHostEnvironment webHostEnvironment,
                                    ITokenService tokenService,
                                    ILogger<EditEventsController> logger)
        {
            _eventClient = eventClient;
            _layoutClient = layoutClient;
            _mapHelper = mapHelper;
            _webHostEnvironment = webHostEnvironment;
            _tokenService = tokenService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "ShowEvents");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _eventClient.DeleteAsync(id, _tokenService.GetToken());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Index");
            }

            return new OkResult();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            return View(await GenerateEventViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EventViewModel eventView)
        {
            List<Layout> layouts = null;
            try
            {
                layouts = await _layoutClient.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Non-caught edit event error.");
                return View(eventView);
            }

            List<LayoutViewModel> layoutsViewModel = await _mapHelper.LayoutToViewModel(layouts);

            eventView.Layouts = layoutsViewModel;

            if (!ModelState.IsValid)
            {
                return View(eventView);
            }

            var @event = new Event
            {
                Id = eventView.Id,
                Name = eventView.Name,
                Description = eventView.Description,
                LayoutId = eventView.LayoutId,
                DateTimeStart = eventView.StartEvent,
                DateTimeEnd = eventView.EndEvent,
                ImageUrl = eventView.ImageUrl,
            };

            try
            {
                await _eventClient.UpdateAsync(@event, _tokenService.GetToken());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(await GenerateEventViewModel(eventView.Id));
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            List<Layout> layouts = null;
            try
            {
                layouts = await _layoutClient.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Non-caught add (get) event error.");
                return View();
            }

            List<LayoutViewModel> layoutsViewModel = await _mapHelper.LayoutToViewModel(layouts);

            var createEventDateTime = new DateTime(2021, 01, 01, 12, 00, 00);

            var eventViewModel = new EventViewModel
            {
                StartEvent = createEventDateTime,
                EndEvent = createEventDateTime,
                Layouts = layoutsViewModel,
            };

            return View(eventViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(EventViewModel model)
        {
            List<Layout> layouts = null;
            try
            {
                layouts = await _layoutClient.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Non-caught add (post) event error.");
                return View(model);
            }

            List<LayoutViewModel> layoutsViewModel = await _mapHelper.LayoutToViewModel(layouts);

            var eventViewModel = new EventViewModel
            {
                Name = model.Name,
                Description = model.Description,
                StartEvent = model.StartEvent,
                EndEvent = model.EndEvent,
                Layouts = layoutsViewModel,
                ImageUrl = model.ImageUrl,
                Image = model.Image,
            };

            if (!ModelState.IsValid)
            {
                return View(eventViewModel);
            }

            string uniqueFileName = UploadedFile(model);

            if (uniqueFileName == null)
            {
                ModelState.AddModelError("", "Upload an image.");
                return View(eventViewModel);
            }

            var @event = new Event
            {
                Name = model.Name,
                Description = model.Description,
                DateTimeStart = model.StartEvent,
                DateTimeEnd = model.EndEvent,
                LayoutId = model.LayoutId,
                ImageUrl = uniqueFileName,
            };
            try
            {
                await _eventClient.AddAsync(@event, _tokenService.GetToken());
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                ModelState.AddModelError("", ex.Content.ToString());
                return View(eventViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                _logger.LogWarning(ex, "Non-caught add event error.");
                return View(eventViewModel);
            }

            return RedirectToAction("Index");
        }

        private async Task<EventViewModel> GenerateEventViewModel(int eventId)
        {
            Event @event = await _eventClient.GetAsync(eventId);

            List<Layout> layouts = null;
            try
            {
                layouts = await _layoutClient.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Non-caught generate event view model event error.");
                return null;
            }

            var eventViewModel = new EventViewModel
            {
                Id = @event.Id,
                Name = @event.Name,
                Description = @event.Description,
                StartEvent = @event.DateTimeStart,
                EndEvent = @event.DateTimeEnd,
                LayoutId = @event.LayoutId,
                Layouts = await _mapHelper.LayoutToViewModel(layouts),
                ImageUrl = @event.ImageUrl,
            };

            return eventViewModel;
        }

        private string UploadedFile(EventViewModel model)
        {
            string uniqueFileName = null;

            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}
