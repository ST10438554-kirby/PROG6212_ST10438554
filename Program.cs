using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

// Your project namespaces
using ClaimSystem.Data;
using ClaimSystem.Models;
using ClaimSystem.Services;
using ClaimSystem.Controllers;

public class ClaimsControllerTests
{
    [Fact]
    public async Task SubmitClaimController_SavesClaimAndDocument()
    {
        // 1️⃣ In-memory EF Core
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        using var db = new ApplicationDbContext(options);

        // 2️⃣ Create a test WebRootPath folder dynamically
        var testRoot = Path.Combine(Directory.GetCurrentDirectory(), "TestWebRoot");
        var uploadFolder = Path.Combine(testRoot, "uploads");
        Directory.CreateDirectory(uploadFolder); // ✅ Ensure folder exists

        // 3️⃣ Mock IWebHostEnvironment
        var envMock = new Mock<IWebHostEnvironment>();
        envMock.Setup(e => e.WebRootPath).Returns(testRoot);

        // 4️⃣ FileUploadOptions via IOptions
        var fileOptions = new FileUploadOptions
        {
            AllowedExtensions = new[] { ".pdf" },
            MaxFileSizeBytes = 10 * 1024 * 1024,
            MaxFilesPerClaim = 5,
            UploadFolder = "uploads"
        };
        IOptions<FileUploadOptions> opts = Options.Create(fileOptions);

        // 5️⃣ Create service
        var service = new ClaimService(db, envMock.Object, opts);

        // 6️⃣ Create controller and inject service + options
        var controller = new ClaimsController(service, opts);

        // 7️⃣ Fake claim + fake file
        var claim = new Claim
        {
            ClaimMonth = DateTime.UtcNow,
            HoursWorked = 2,
            HourlyRate = 50
        };

        var stream = new MemoryStream(new byte[10]);
        stream.Position = 0;
        IFormFile formFile = new FormFile(stream, 0, stream.Length, "file", "test.pdf");

        // 8️⃣ Call controller action
        // Make sure your controller has a method like:
        // Task<Claim> SubmitClaim(Claim claim, IFormFile[] files)
        var result = await controller.SubmitClaim(claim, new[] { formFile });

        // 9️⃣ Assertions
        Assert.NotNull(result);
        Assert.Equal(100, result.ClaimAmount);
        Assert.Single(db.SupportingDocuments);

        // 🔟 Cleanup test folder
        if (Directory.Exists(testRoot))
        {
            Directory.Delete(testRoot, true);
        }
    }
}
