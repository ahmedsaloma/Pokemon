using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Pokemon.Interface;
using Pokemon.Models;
using Pokemon.ViewModels;

namespace Pokemon.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IPhotoSevice _photoSevice;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardController(IDashboardRepository dashboardRepository,IPhotoSevice photoSevice,IHttpContextAccessor httpContextAccessor)
        {
            _dashboardRepository = dashboardRepository;
            _photoSevice = photoSevice;
            _httpContextAccessor = httpContextAccessor;
        }

        private void MapUserEdit(AppUser appUser,EditProfileViewModel editProfileViewModel,ImageUploadResult imageUploadResult)
        {
            appUser.Id = editProfileViewModel.Id;
            appUser.Pace = editProfileViewModel.Pace;
            appUser.Mileage = editProfileViewModel.Mileage;
            appUser.ProfileImageUrl = imageUploadResult.Url.ToString(); 
            appUser.City = editProfileViewModel.City;
            appUser.State = editProfileViewModel.State; 
           

        }
        public async Task<IActionResult> Index()
        {
            var userRaces = await _dashboardRepository.GetAllUserRaces();
            var userClubs = await _dashboardRepository.GetAllUserClubs();
            DashboardViewModel dashboardViewModel =  new  DashboardViewModel 
            {
                Races =  userRaces,
                Clubs =  userClubs
            };
            return View(dashboardViewModel);

        }
        public async Task<IActionResult> EditUserProfile()
        {
            var curUser = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRepository.GetUserById(curUser);
            if (user == null)
            {
                return NotFound();
            }

            var editProfileViewModel = new EditProfileViewModel
            {
                Id = user.Id,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.City,
                State = user.State
            };
            return View(editProfileViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditProfileViewModel editProfileViewModel)
        {
            if (!ModelState.IsValid) {
                ModelState.AddModelError("", "failed");
                return View("EditUserProfile", editProfileViewModel);
            }
            var user = await _dashboardRepository.GetByIdNoTracking(editProfileViewModel.Id);

            if (user.ProfileImageUrl == null || user.ProfileImageUrl == "")
            {
                var photoResult = await _photoSevice.AddPhotoAsync(editProfileViewModel.Image);
                MapUserEdit(user, editProfileViewModel, photoResult);
                _dashboardRepository.Update(user);
                return RedirectToAction("Index");

            }

            else
            {
                try
                {
                    await _photoSevice.DeletePhotoAsync(editProfileViewModel.ProfileImageUrl);
                }

                catch (Exception ex)
                {
                    ModelState.AddModelError("","failed");
                    return View(editProfileViewModel);
                }

                var photoResult = await _photoSevice.AddPhotoAsync(editProfileViewModel.Image);
                MapUserEdit(user, editProfileViewModel, photoResult);
                _dashboardRepository.Update(user);
                return RedirectToAction("Index");
            }

        }


    }
}
