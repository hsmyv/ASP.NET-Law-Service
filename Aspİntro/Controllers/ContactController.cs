using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Aspİntro.Data;
using Aspİntro.Models;
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
        [HttpPost]
        public IActionResult ContactUs(ContactUs contactUs)
        {
            //try
            //{
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("hsmusayev@gmail.com");

            mail.To.Add(new MailAddress("hsmusayev@gmail.com"));

            mail.Subject = contactUs.Subject;

            mail.IsBodyHtml = true;

            string content = "Name : " + contactUs.Name;
            content += "<br/> Message : " + contactUs.Message;

            mail.Body = content;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Credentials = new NetworkCredential("hsmusayev@gmail.com", "lozpdostnrutxjit"),
                UseDefaultCredentials = false,
                Port = 587,
                EnableSsl = true,
            };


            smtpClient.Send(mail);

                ViewBag.Message = "Mail Send";

                ModelState.Clear();
            return View();
            }
            
            //}
            //catch (Exception ex)
            //{

            //    ViewBag.Message = ex.Message.ToString();
            //}

    }
}
