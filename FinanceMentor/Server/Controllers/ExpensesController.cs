using FinanceMentor.Server.Storage;
using FinanceMentor.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinanceMentor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IRepository<Expense> _expenseRepository;

        public ExpensesController(IRepository<Expense> expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        [HttpGet]
        public IEnumerable<Expense> Get()
        {
            return _expenseRepository.GetAll()
                .OrderBy(earning => earning.Date);
        }

        [HttpPost]
        public void Post(Expense expense)
        {
            _expenseRepository.Add(expense);
        }

        [HttpDelete("{id?}")]
        public void Delete(Guid id)
        {
            var entity = _expenseRepository.GetAll()
                .Single(item => item.Id == id);
            _expenseRepository.Remove(entity);
        }
    }
}
