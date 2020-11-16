﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DataServiceLibrary;
using DataServiceLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebService.Models;

namespace WebService.Controllers
{
    [ApiController]
    [Route("api")]
    public class HomeController: ControllerBase
    {

        IDataService _dataService;
        IMapper _mapper;
        private const int MaxPageSize = 25;

        public HomeController(IDataService dataService, IMapper mapper)
        {
            _dataService = dataService;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult CreateUser(UserForCreationOrUpdateDto newuser)
        {
            var user = _mapper.Map<User>(newuser);
            var address = _mapper.Map<Address>(newuser);

            _dataService.CreateNewUser(user, address);

            //var user1 = _dataService.GetUsers(0, 10).Last();
            var result = _mapper.Map<UserDto>(user);

            //test1

            return Created("", result);
        }
    }
}
