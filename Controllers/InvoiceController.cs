﻿using IntegracionDesarrollo3.Dtos;
using IntegracionDesarrollo3.Dtos.IntegracionDesarrollo3.Dtos;
using IntegracionDesarrollo3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IntegracionDesarrollo3.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IConfiguration _cfg;
        private readonly HttpClient _http;
        public InvoiceController(IConfiguration cfg, IHttpClientFactory factory)
        {
            _cfg = cfg;
            _http = factory.CreateClient();
            _http.BaseAddress = new Uri(_cfg.GetValue<string>("CoreBaseUrl") + "invoice/");
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceDTO invoiceDto)
        {
            Utils.RequestNeedsAuthentication(Request, _http);

            var response = await _http.PostAsJsonAsync("create", invoiceDto);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var createdInvoice = JsonConvert.DeserializeObject<Invoices>(content);
                return Ok(createdInvoice);
            }
            else
            {
                return StatusCode((int)response.StatusCode, new CoreApiError
                {
                    Message = content,
                    StatusCode = (int)response.StatusCode,
                });

            }
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetAllInvoices()
        {
            Utils.RequestNeedsAuthentication(Request, _http);

            var response = await _http.GetAsync("get");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var invoices = JsonConvert.DeserializeObject<IEnumerable<Invoices>>(content);
                return Ok(invoices);
            }
            else
            {
                return StatusCode((int)response.StatusCode, new CoreApiError
                {
                    Message = content,
                    StatusCode = (int)response.StatusCode,
                });
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            Utils.RequestNeedsAuthentication(Request, _http);
            var response = await _http.GetAsync($"get/{id}");
            if (response.IsSuccessStatusCode)
            {
                var productJson = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<Invoices>(productJson);
                return new JsonResult(product);
            }
            return StatusCode((int)response.StatusCode, new { Message = $"Failed to get the product by ID: {id}." });
        }

        [HttpGet("getByName/{client_name}")]
        public async Task<IActionResult> GetInvoiceByClientName(string client_name)
        {
            Utils.RequestNeedsAuthentication(Request, _http);


            var response = await _http.GetAsync($"getByName/{Uri.EscapeDataString(client_name)}");
            var content = await response.Content.ReadAsStringAsync();

            // Comprueba si la respuesta es exitosa
            if (response.IsSuccessStatusCode)
            {
                var invoices = JsonConvert.DeserializeObject<IEnumerable<Invoices>>(content);
                return Ok(invoices);
            }
            else
            {
                return StatusCode((int)response.StatusCode, new CoreApiError
                {
                    Message = content,
                    StatusCode = (int)response.StatusCode,
                });
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddItemsToInvoice([FromBody] AddItemsDTO addItemsDto)
        {
            Utils.RequestNeedsAuthentication(Request, _http);
            var response = await _http.PostAsJsonAsync("add", addItemsDto);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var invoice = JsonConvert.DeserializeObject<Invoices>(content);
                return Ok(invoice);
            }
            else
            {
                return StatusCode((int)response.StatusCode, new CoreApiError
                {
                    Message = content,
                    StatusCode = (int)response.StatusCode,
                });
            }
        }


        [HttpPut("update")]
        public async Task<IActionResult> UpdateInvoice([FromBody] UpdateInvoiceDTO invoiceDto)
        {
            Utils.RequestNeedsAuthentication(Request, _http);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            var response = await _http.PutAsJsonAsync("update", invoiceDto);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var updatedInvoice = JsonConvert.DeserializeObject<Invoices>(content);
                return Ok(updatedInvoice);
            }
            else
            {
                return StatusCode((int)response.StatusCode, new CoreApiError
                {
                    Message = content,
                    StatusCode = (int)response.StatusCode,
                });
            }
        }




    }

}
