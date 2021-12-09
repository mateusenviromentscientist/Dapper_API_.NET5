using System;
using System.Threading.Tasks;
using DapperAPI.Contracts;
using DapperAPI.Dto;
using DapperAPI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DapperAPI.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository _repository;

        public CompaniesController(ICompanyRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult> GetCompanies()
        {
            try
            {
                var companies = await _repository.GetCompanies();
                return Ok(companies);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}", Name = "CompanyById")]
        public async Task<IActionResult> GetCompany(int id)
        {
            try
            {
                var company = await _repository.GetCompany(id);
                if (company == null)
                    NotFound();
                return Ok(company);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany(CompanyForCreationDto company)
        {
            try
            {
                var createdCompany = await _repository.CreateCompany(company);
                return CreatedAtRoute("CompanyById", new {id = createdCompany.Id}, createdCompany);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, CompanyForUpdateDto company)
        {
            try
            {
                var dbCompany = await _repository.GetCompany(id);
                if (dbCompany == null)
                    return NotFound();

                await _repository.UpdateCompany(id, company);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            try
            {
                var dbCompany = await _repository.GetCompany(id);
                if (dbCompany == null)
                    return NotFound();

                await _repository.DeleteCompany(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
    }
}