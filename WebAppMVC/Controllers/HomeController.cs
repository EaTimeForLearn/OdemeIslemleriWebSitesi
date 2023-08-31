using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebAppMVC.Models;
using Business.Abstract;
using Newtonsoft.Json;
using Entities.Concrete;
using System.Web.Helpers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Business.Contants;
using AutoMapper;
using Entities.Dtos;
using static LinqToDB.Common.Configuration;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;
using static LinqToDB.DataProvider.SqlServer.SqlFn;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Text;

namespace WebAppMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IMusteriService musteriService;
        public HomeController(IMusteriService musteriService, IMapper mapper)
        {
            this._mapper = mapper;
            this.musteriService = musteriService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = musteriService.GetList();
            var musteriler = new List<MusteriDto>();
            foreach (Musteri m in result.Data)
            {
                musteriler.Add(_mapper.Map<MusteriDto>(m));
            }
            if (result.Success)
            {
                return View(musteriler);
            }
            return View();
        }

        [HttpGet]
        [Route("home/installments/{totalprice:decimal}")]
        public async Task<string[][]> GetInstallments(decimal totalprice)
        {
            string apiUrl = "https://localhost:44378/api/payment/installments";

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(new GetInstallmentModel() { BinNumber = "", Price = totalprice }.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    var paymentServiceResult = await response.Content.ReadAsStringAsync();
                    var paymentServiceResultModel = Newtonsoft.Json.JsonConvert.DeserializeObject<PaymentServiceResultModel<List<CreditCardData>>>(paymentServiceResult);
                    if (paymentServiceResultModel!=null && paymentServiceResultModel.Success)
                    {
                        var retval = new List<string[]>();
                        foreach (var bank in paymentServiceResultModel.Data)
                        {
                            var row=new List<string>();
                            row.Add(bank.cardFamilyName);
                            foreach (var installmentInfo in bank.installments)
                            {
                                if (installmentInfo.installmentNumber == 1)
                                {
                                    row.Add(installmentInfo.totalPrice.ToString("N2"));
                                }
                                else
                                {
                                    row.Add($"{installmentInfo.price.ToString("N2")} x {installmentInfo.installmentNumber} = {installmentInfo.totalPrice.ToString("N2")}");
                                }
                            }
                            retval.Add(row.ToArray());
                        }
                        return retval.ToArray();
                    }
                }
            }
            return null;
        }


        public IActionResult Payment()
        {
            return View();
        }

        public IActionResult UserInformation()
        {
            return View();
        }
    
        public IActionResult Privacy()
        {
            return View();
        }


    }
}