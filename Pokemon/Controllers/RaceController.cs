using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pokemon.Data;
using Pokemon.Interface;
using Pokemon.Models;
using Pokemon.Repository;
using Pokemon.Services;
using Pokemon.ViewModels;

namespace Pokemon.Controllers
{
    public class RaceController : Controller
    {
        private readonly IPhotoSevice _photoSevice;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IRaceRepository RaceRepository { get; }

        public RaceController(IRaceRepository raceRepository,IPhotoSevice photoSevice,IHttpContextAccessor httpContextAccessor)
        {
            RaceRepository = raceRepository;
            _photoSevice = photoSevice;
            _httpContextAccessor = httpContextAccessor;
        }



        public async Task < IActionResult> Index()
        {
            var races = await RaceRepository.GetAll();
            return View(races);
        }
        public async Task< IActionResult> Detail(int id)
        {
            Race race = await RaceRepository.GetByIdAsync(id); 
            return View(race);
        }

        public IActionResult Create()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var createRaceViewModel = new CreateRaceViewModel
            {
                AppUserId = curUserId
            };

            return View(createRaceViewModel);
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateRaceViewModel raceVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoSevice.AddPhotoAsync(raceVM.Image);

                var race = new Race
                {
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    AppUserId = raceVM.AppUserId,
                    Image = result.Url.ToString(),

                    Address = new Address
                    {
                        Street = raceVM.Address.Street,
                        City = raceVM.Address.City,
                        State = raceVM.Address.State
                    }

                };
                RaceRepository.Add(race);
                return RedirectToAction("Index");


            }
            else
            {
                ModelState.AddModelError("", "photo upload failed");
            }
            return View();
        }

        public async Task<IActionResult> Edit(int id)

        {

            var race = await RaceRepository.GetByIdAsync(id);

            if (race == null)
            {
                return View("Error");
            }

            var raceVM = new EditRaceViewModel
            {
                Title = race.Title,
                Description =   race.Description,
                Address = race.Address,
                URL = race.Image,
                RaceCategory = race.RaceCategory,
                AddressId = race.AddressId

            };
            return View(raceVM);

        }

        [HttpPost]

        public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "failed to edit ");
                return View("Edit", raceVM);

            }

            var userRace = await RaceRepository.GetByIdAsyncNoTtracking(id);
            if (userRace != null)
            {
                try
                {
                    await _photoSevice.DeletePhotoAsync(userRace.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "could not delete photo");
                    return View(raceVM);
                }


            }
            var photoresult = await _photoSevice.AddPhotoAsync(raceVM.Image);

            var race = new Race
            {
                Id = id,
                Title = raceVM.Title,
                Description = raceVM.Description,
                Address = raceVM.Address,
                Image = photoresult.Url.ToString(),
                AddressId = raceVM.AddressId

            };
            RaceRepository.Update(race);
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Delete(int id)
        {
            var race = await RaceRepository.GetByIdAsync(id);
            if (race  != null)
            {
                return View(race);

            }
            return View("Error");
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteRace(int id)
        {
            var club = await RaceRepository.GetByIdAsync(id);
            if (club != null)
            {
                RaceRepository.Delete(club);
                RaceRepository.Save();
                return RedirectToAction("Index");
            }
            return View("Error");
        }
    }
}
