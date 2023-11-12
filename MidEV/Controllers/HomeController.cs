using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MidEV.Models;
using MidEV.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MidEV.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        public IActionResult SPA()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Specialists()
        {
            var _ctx = new CoreMidContext();
            return Json(_ctx.Specialists.OrderBy(x => x.SpecialistName).ToList());
        }

        [HttpGet]
        public IActionResult Doctors()
        {
            var _ctx = new CoreMidContext();
            var listDoctor = (from c in _ctx.Doctors
                               join d in _ctx.Specialists on c.SpecialistId equals d.SpecialistId
                               select new
                               {
                                   c.DoctorId,
                                   c.DoctorName,
                                   c.SpecialistId,
                                   c.ScheduleDate,
                                   c.Gender,
                                   c.Prescription,
                                   d.SpecialistName
                               }).ToList();
            return Json(listDoctor);
        }

        [HttpGet]
        public IActionResult Doctor(int id)
        {
            var _ctx = new CoreMidContext();
            return Json(_ctx.Doctors.Where(x => x.DoctorId == id).FirstOrDefault());
        }

        [RequestSizeLimit(2147483648)]
        [HttpPost]
        public IActionResult DoctorAdd([FromForm] VmDoctor doctor)
        {
            object res = null; var _ctx = new CoreMidContext();
            var oDoctor = _ctx.Doctors.Where(x => x.DoctorId == doctor.DoctorId).FirstOrDefault();
            if (oDoctor == null)
            {
                #region file
                string fileName = "";
                if (doctor.Prescription != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pics");
                    //create folder if not exist
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //get file extension
                    FileInfo fileInfo = new FileInfo(doctor.Prescription.FileName);
                    string newFileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    fileName = newFileName + fileInfo.Extension;
                    string fileNameWithPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        doctor.Prescription.CopyTo(stream);
                    }
                }
                #endregion
                oDoctor = new Doctor();
                oDoctor.DoctorName = doctor.DoctorName;
                oDoctor.SpecialistId = doctor.SpecialistId;
                oDoctor.ScheduleDate = doctor.ScheduleDate;
                oDoctor.Gender = doctor.Gender;
                oDoctor.Prescription = fileName;
                _ctx.Add(oDoctor);
                _ctx.SaveChanges();
                res = new
                {
                    resstate = true
                };
            }
            return Json(res);
        }

        [HttpPut]
        public IActionResult DoctorEdit([FromForm] VmDoctor doctor)
        {
            object res = null; var _ctx = new CoreMidContext();
            var oDoctor = _ctx.Doctors.Where(x => x.DoctorId == doctor.DoctorId).FirstOrDefault();
            if (oDoctor != null)
            {
                #region file
                string fileName = "";
                if (doctor.Prescription != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pics");
                    //create folder if not exist
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //get file extension
                    FileInfo fileInfo = new FileInfo(doctor.Prescription.FileName);
                    string newFileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    fileName = newFileName + fileInfo.Extension;
                    string fileNameWithPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        doctor.Prescription.CopyTo(stream);
                    }
                    // delete old file when upload new file
                    if (!string.IsNullOrEmpty(oDoctor.Prescription))
                    {
                        string fileNameWithPathDEL = Path.Combine(path, oDoctor.Prescription);
                        if (System.IO.File.Exists(fileNameWithPathDEL))
                        {
                            System.IO.File.Delete(fileNameWithPathDEL);
                        }
                    }
                }
                #endregion
                oDoctor.DoctorName = doctor.DoctorName;
                oDoctor.SpecialistId = doctor.SpecialistId;
                oDoctor.ScheduleDate = doctor.ScheduleDate;
                oDoctor.Gender = doctor.Gender;
                oDoctor.Prescription = fileName;
                _ctx.SaveChanges();
                res = new
                {
                    resstate = true
                };
            }
            return Json(res);
        }

        [HttpDelete]
        public IActionResult DoctorRemove([FromQuery] int id)
        {
            object res = null; var _ctx = new CoreMidContext();
            var oDoctor = _ctx.Doctors.Where(x => x.DoctorId == id).FirstOrDefault();
            if (oDoctor != null)
            {
                _ctx.Doctors.Remove(oDoctor);
                _ctx.SaveChanges();
                #region file
                // delete file when record deleted
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pics");
                if (!string.IsNullOrEmpty(oDoctor.Prescription))
                {
                    string fileNameWithPathDEL = Path.Combine(path, oDoctor.Prescription);
                    if (System.IO.File.Exists(fileNameWithPathDEL))
                    {
                        System.IO.File.Delete(fileNameWithPathDEL);
                    }
                }
                #endregion
                res = new
                {
                    resstate = true
                };
            }
            return Json(res);
        }
    }
}
