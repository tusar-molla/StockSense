using Microsoft.EntityFrameworkCore;
using StockSense.Data;
using StockSense.Interface;
using StockSense.Models;

namespace StockSense.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly BaseService _baseService;

        public CompanyService(AppDbContext context,IWebHostEnvironment webHostEnvironment,BaseService baseService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
            _baseService = baseService;
        }
        public async Task<List<Company>> GetAllAsync()
        {
            return await _context.Companies.ToListAsync();
        }
        public async Task<bool> CreateCompanyAsync(Company company)
        {
            try
            {
                if (company == null)
                    throw new ArgumentNullException(nameof(company));                   
                if (company.LogoImage != null)
                {
                    company.LogoPath = await UploadFileAsync(company.LogoImage);
                }
                await _context.AddAsync(company);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateCompanyAsync(Company company)
        {
            try
            {
                if (company == null)
                    throw new ArgumentNullException(nameof(company));

                var existingCompany = await _context.Companies.FirstOrDefaultAsync(c => c.Id == company.Id);
                if (existingCompany == null)
                {
                    return false;
                }
                if (company.LogoImage != null)
                {
                    if (!string.IsNullOrEmpty(existingCompany.LogoPath))
                    {
                        DeleteFile(existingCompany.LogoPath);
                    }
                    existingCompany.LogoPath = await UploadFileAsync(company.LogoImage);
                }

                existingCompany.Name = company.Name;
                existingCompany.TagLine = company.TagLine;
                existingCompany.VatRegistrationNo = company.VatRegistrationNo;
                existingCompany.TinNo = company.TinNo;
                existingCompany.WebsiteLink = company.WebsiteLink;
                existingCompany.Email = company.Email;
                existingCompany.ContactNumber = company.ContactNumber;
                existingCompany.Address = company.Address;
                existingCompany.Remarks = company.Remarks;
                _context.Update(existingCompany);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return string.Empty;

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/images/{uniqueFileName}";
        }
        private void DeleteFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return;

            string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, filePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
