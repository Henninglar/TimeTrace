using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTrace.Data;
using TimeTrace.Models;

namespace TimeTrace.Controllers;

public class ManageController : Controller
{
    
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _um;

    public ManageController(ApplicationDbContext context, UserManager<ApplicationUser> um)
    {
        _context = context;
        _um = um;
    }
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Index()
    {
    
        return View();
    }
    
    
    [HttpPost]
    public IActionResult Index(email sendmail)


    {

        var user = _um.GetUserAsync(User).Result.Id;
        var test = _context.Users.Where(u => user == u.employeer_id).ToList();

        string combinedString = string.Join( ",", test);
        if (!ModelState.IsValid) return View();
        try
        {

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("Thetimetraceproject@hotmail.com");
            
            foreach (var data in combinedString.Split(new [] {","}, StringSplitOptions.RemoveEmptyEntries))
            {
                mail.To.Add(combinedString);
            }

            mail.Subject = sendmail.Subject;
            mail.IsBodyHtml = true;

            string content = sendmail.Message;

            mail.Body = content;
            SmtpClient smtpClient = new SmtpClient("smtp.office365.com");
            NetworkCredential networkCredential = new NetworkCredential("Thetimetraceproject@hotmail.com", "Password1.2.3.01@");
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = networkCredential;
            smtpClient.Port = 587; 
            smtpClient.EnableSsl = true;
            smtpClient.Send(mail);

            ViewBag.Message = "Epost sendt";
            ModelState.Clear();

        }
        catch (Exception ex)
        {
            ViewBag.Message = ex.Message.ToString();
        }
        return View();
    }
    
}