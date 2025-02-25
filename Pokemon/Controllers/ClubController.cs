using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pokemon.Data;
using Pokemon.Interface;
using Pokemon.Models;
using Pokemon.Repository;
using Pokemon.ViewModels;

namespace Pokemon.Controllers
{
    public class ClubController : Controller
    {
        private readonly IPhotoSevice _photoSevice;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IClubRepository ClubRepository { get; }

        public ClubController(IClubRepository clubRepository, IPhotoSevice photoSevice,IHttpContextAccessor httpContextAccessor)
        {
           
            ClubRepository = clubRepository;
            _photoSevice = photoSevice;
            _httpContextAccessor = httpContextAccessor;
        }

       

        public async Task<IActionResult> Index()
        {
            var clubs = await ClubRepository.GetAll();
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Club club = await ClubRepository.GetByIdAsync(id);
            return View(club);
        }

        public IActionResult Create()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var createClubViewModel = new CreateClubViewModel
            {
                AppUserId = curUserId
            };
            return View(createClubViewModel);
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoSevice.AddPhotoAsync(clubVM.Image);

                var club = new Club
                {
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    AppUserId = clubVM.AppUserId,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        Street = clubVM.Address.Street,
                        City = clubVM.Address.City,
                        State = clubVM.Address.State
                    }
                   
                };
                ClubRepository.Add(club);
                return RedirectToAction("Index");


            }
            else
            {
                ModelState.AddModelError("", "photo upload failed");
            }
            return View();
            
        }

        public async Task<IActionResult> Edit(int id )

        {

            var club = await ClubRepository.GetByIdAsync(id);

            if(club == null)
            {
                return View("Error");
            }

            var clubVM = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                Address = club.Address,
                URL = club.Image,
                ClubCategory = club.ClubCategory,
                AddressId = club.AddressId

            };
            return View(clubVM);

        }

        [HttpPost]

        public async Task<IActionResult> Edit(int id,EditClubViewModel clubVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "failed to edit ");
                return View("Edit",clubVM);

            }

            var userClub = await ClubRepository.GetByIdAsyncNoTracking(id);

            if (userClub != null)
            {
                try
                {
                    await _photoSevice.DeletePhotoAsync(userClub.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "could not delete photo");
                    return View(clubVM);
                }
                

            }
            var photoresult = await _photoSevice.AddPhotoAsync(clubVM.Image);

            var club = new Club
            {
                Id = id,
                Title = clubVM.Title,
                Description = clubVM.Description,
                Address = clubVM.Address,
                Image = photoresult.Url.ToString(),
                AddressId = clubVM?.AddressId

            };
            ClubRepository.Update(club);
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Delete(int id)
        {
            var club = await ClubRepository.GetByIdAsync(id);
            if (club != null)
            {
                return View(club);

            }
            return View("Error");
        }

        [HttpPost,ActionName("Delete")]
        public  async Task<IActionResult> DeleteClub(int id)
        {
            var club = await ClubRepository.GetByIdAsync(id);
            if (club != null)
            {
                
                  


                
                ClubRepository.Delete(club);
                ClubRepository.Save();
                return RedirectToAction("Index");
            }
            return View("Error");
        }

    }
}
