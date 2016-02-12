﻿using SampleMVC.ViewModels.Account;
using SampleMVCTest.Extensions;
using SampleMVCTest.IntegrationTests;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace SampleMVCTest
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class IntegrationTest: BaseIntegrationTest
    {

        [Fact]
        public async Task CreateNewUser()
        {
            var vm = new RegisterViewModel()
            {
                ConfirmPassword = "Iasdf654842384!",
                Password = "Iasdf654842384!",
                Email = string.Format("myemail{0}@a.com", Guid.NewGuid().ToString().Substring(0, 5)),
            };
            var registerUri = "/Account/Register";
            using (var th = InitTestServer())
            {
                var client = th.CreateClient();
                var result = await client.GetAsync(registerUri);
                result.EnsureSuccessStatusCode();
                var cookieVal = result.GetCookie(AntiForgeryCookieName).ToString();
                var formTokenVal = await result.GetAntiForgeryFormToken(AntiForgeryFormTokenName);
                client.DefaultRequestHeaders.Add("Cookie", cookieVal);
                result = await client.PostFormDataAsync<RegisterViewModel>(registerUri, vm, formTokenVal);
                Assert.Equal(HttpStatusCode.Found, result.StatusCode);
            }
        }
    }
}
