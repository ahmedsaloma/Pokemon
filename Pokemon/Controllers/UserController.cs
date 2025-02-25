using Microsoft.AspNetCore.Mvc;
using Pokemon.Interface;
using Pokemon.Repository;
using Pokemon.ViewModels;

namespace Pokemon.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _user;

        public UserController(IUserRepository user)
        {
            _user = user;
        }
        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {
            var users = await _user.GetAllUsers();
            List<UserViewModel> result = new List<UserViewModel>();
            foreach (var user in users)
            {
                var userViewModel = new UserViewModel()
                {
                    Id = user.Id,
                    Pace = user.Pace,
                    Mileage = user.Mileage,
                    UserName = user.UserName,
                    ProfileImageUrl = user.ProfileImageUrl, 
                   
                };
                result.Add(userViewModel);
            }
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var user = await _user.GetUserById(id);
            if (user == null)
            {
                return RedirectToAction("Index", "Users");
            }

            var userDetailViewModel = new UserDetailViewModel()
            {
                Id = user.Id,
                Pace = user.Pace,
                Mileage = user.Mileage,
                UserName = user.UserName,
               
            };
            return View(userDetailViewModel);
        }
    }
}
