using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Q2.Models;

namespace Q2.Pages.List
{
    public class IndexModel : PageModel
    {
        private readonly PRN221_TrialContext _context;
        public string Room { get; set; }
        public List<Service> Services { get; set; } = new List<Service>();
        public IndexModel(PRN221_TrialContext context)
        {
            _context = context;
        }
        public void OnGet(string room)
        {
            Room = room;
            if (string.IsNullOrEmpty(Room))
            {
                var currentMonth = DateTime.Now.Month;
                Services = _context.Services.Include(x => x.EmployeeNavigation).Where(x => x.Month == currentMonth).ToList();
            }
            else
            {
                Services = _context.Services.Include(x => x.EmployeeNavigation).Where(x => x.RoomTitle.Contains(Room)).ToList();
            }
        }
        public void OnPost(IFormFile inputFile)
        {
            string fileContent = string.Empty;
            using (var reader = new StreamReader(inputFile.OpenReadStream()))
            {
                fileContent = reader.ReadToEnd();
            }
            if (!string.IsNullOrEmpty(fileContent))
            {
                var listOfService = JsonConvert.DeserializeObject<List<Service>>(fileContent);
                if (listOfService.Count > 0)
                {
                    foreach (var service in listOfService)
                    {
                        service.Id = 0;
                    }
                    _context.Services.AddRange(listOfService);
                    _context.SaveChanges();
                }
            }
            Services = _context.Services.Include(x => x.EmployeeNavigation).ToList();

        }
    }
}
