﻿using AutoMapper;
using DataServiceLibrary;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Models;

namespace WebService.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SimpleSearchController: ControllerBase
    {
        IDataService _dataService;
        IMapper _mapper;
        private const int MaxPageSize = 25;

        public SimpleSearchController(IDataService dataService, IMapper mapper)
        {
            _dataService = dataService;
            _mapper = mapper;
        }
        [HttpGet(Name = nameof(SimpleSearch))]
        public IActionResult SimpleSearch(string search, int page = 0, int pageSize = 10)
        {
            try
            {
                if (Program.CurrentUser == null)
                {
                    return Unauthorized();
                }

                pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;

                var simpleSearch = _dataService.StringSearch(Program.CurrentUser.UserId, search, page, pageSize);

                if (simpleSearch == null)
                {
                    return NotFound();
                }
                SimpleSearchDto simpleSearchDto = new SimpleSearchDto();

                IList<SimpleSearchDto> items = new List<SimpleSearchDto>();

                foreach (var s in simpleSearch)
                {
                    simpleSearchDto.Search = Url.Link(nameof(TitleController.GetTitle), new { Id = s.TitleConst.Trim() });
                    items.Add(simpleSearchDto);
                }

                var count = _dataService.NumberOfStringSearchMatched(search, Program.CurrentUser.UserId);

                string prev = null;

                if (page > 0)
                {
                    prev = Url.Link(nameof(SimpleSearch), new { page = page - 1, pageSize });
                }

                string next = null;

                if (page < (int)Math.Ceiling((double)count / pageSize) - 1)
                {
                    next = Url.Link(nameof(SimpleSearch), new { page = page + 1, pageSize });
                }

                var cur = Url.Link(nameof(SimpleSearch), new { page, pageSize });

                var result = new
                {
                    prev,
                    next,
                    cur,
                    count,
                    items
                };

                return Ok(result);

            } 
            catch (ArgumentException)
            {
                return Unauthorized();
            }
        }

       
    }
}
