using career_wide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace career_wide.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _environment;

    public ProfileController(UserManager<ApplicationUser> userManager, IWebHostEnvironment environment)
    {
        _userManager = userManager;
        _environment = environment;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return Challenge();
        }

        var model = new ProfileViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            Location = user.Location,
            ProfessionalTitle = user.ProfessionalTitle,
            ProfessionalSummary = user.ProfessionalSummary,
            Skills = user.Skills,
            LinkedInUrl = user.LinkedInUrl,
            PortfolioUrl = user.PortfolioUrl,
            CurrentCvFileName = user.CvFileName
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ProfileViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return Challenge();
        }

        model.CurrentCvFileName = user.CvFileName;

        string? fileExtension = null;

        if (model.CvFile is not null)
        {
            fileExtension = Path.GetExtension(model.CvFile.FileName).ToLowerInvariant();

            var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };

            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError(
                    nameof(model.CvFile),
                    "Please upload a PDF or Word document.");
            }

            if (model.CvFile.Length == 0 || model.CvFile.Length > 5 * 1024 * 1024)
            {
                ModelState.AddModelError(
                    nameof(model.CvFile),
                    "The CV must be smaller than 5 MB.");
            }
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.PhoneNumber = model.PhoneNumber;
        user.Location = model.Location;
        user.ProfessionalTitle = model.ProfessionalTitle;
        user.ProfessionalSummary = model.ProfessionalSummary;
        user.Skills = model.Skills;
        user.LinkedInUrl = model.LinkedInUrl;
        user.PortfolioUrl = model.PortfolioUrl;

        var oldStorageName = user.CvStorageName;
        string? savedFilePath = null;
        var uploadDirectory = Path.Combine(
            _environment.ContentRootPath,
            "App_Data",
            "ApplicantCvs");

        if (model.CvFile is not null && fileExtension is not null)
        {
            Directory.CreateDirectory(uploadDirectory);

            var storageName = $"{Guid.NewGuid():N}{fileExtension}";
            savedFilePath = Path.Combine(uploadDirectory, storageName);

            await using (var fileStream = new FileStream(savedFilePath, FileMode.CreateNew))
            {
                await model.CvFile.CopyToAsync(fileStream);
            }

            user.CvFileName = Path.GetFileName(model.CvFile.FileName);
            user.CvStorageName = storageName;
            user.CvUploadedOn = DateTime.UtcNow;
        }

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            if (savedFilePath is not null && System.IO.File.Exists(savedFilePath))
            {
                System.IO.File.Delete(savedFilePath);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        if (model.CvFile is not null &&
            !string.IsNullOrWhiteSpace(oldStorageName) &&
            Path.GetFileName(oldStorageName) == oldStorageName)
        {
            var oldFilePath = Path.Combine(uploadDirectory, oldStorageName);

            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }
        }

        TempData["SuccessMessage"] = "Profile updated successfully.";
        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> DownloadCv()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null ||
            string.IsNullOrWhiteSpace(user.CvStorageName) ||
            string.IsNullOrWhiteSpace(user.CvFileName) ||
            Path.GetFileName(user.CvStorageName) != user.CvStorageName)
        {
            return NotFound();
        }

        var filePath = Path.Combine(
            _environment.ContentRootPath,
            "App_Data",
            "ApplicantCvs",
            user.CvStorageName);

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        var contentType = Path.GetExtension(user.CvFileName).ToLowerInvariant() switch
        {
            ".pdf" => "application/pdf",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            _ => "application/octet-stream"
        };

        return PhysicalFile(filePath, contentType, Path.GetFileName(user.CvFileName));
    }
}