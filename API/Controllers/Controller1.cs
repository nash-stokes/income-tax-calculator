﻿using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class Controller1 : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}