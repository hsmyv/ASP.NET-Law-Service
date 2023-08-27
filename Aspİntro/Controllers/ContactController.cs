using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Aspİntro.Data;
using Aspİntro.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;

namespace Aspİntro.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ContactUs(ContactUsVM contactUsVM)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("hsmusayev@gmail.com");

                mail.To.Add("hsmusayev@gmail.com");

                mail.Subject = contactUsVM.Subject;

                mail.IsBodyHtml = true;

                string content = "Name : " + contactUsVM.Name;
                content += "<br/> Message : " + contactUsVM.Message;

                mail.Body = content;

                SmtpClient smtpClient = new SmtpClient("hsmusayev@gmail.com");

                NetworkCredential networkCredential = new NetworkCredential("hsmusayev@gmail.com", "Hasan");
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = networkCredential;
                smtpClient.Port = 25;
                smtpClient.EnableSsl = false;
                smtpClient.Send(mail);

                ViewBag.Message = "Mail Send";

                ModelState.Clear();
            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message.ToString();
            }

            return View();
        }

    }
}
