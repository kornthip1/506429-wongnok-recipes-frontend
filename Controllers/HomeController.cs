using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using wongnok_dev.Models;

namespace wongnok_dev.Controllers
{
    public class HomeController : Controller
    {
        private static string endpoint = "https://localhost:44377/api/values/";
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Main()
        {
            ViewData["Message"] = "welcome";

            return View();
        }
        public IActionResult RecipeManage()
        {
            ViewData["Message"] = "welcome";

            return View("RecipeManage");
        }


        public IActionResult RecipeEdit()
        {
            ViewData["Message"] = "welcome";

            return View("RecipeEdit");
        }


        [HttpPost]
        public async Task<IActionResult> GetAllRecipe()
        {
            List<RecipDataModel> list = new List<RecipDataModel>();
            try
            {

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var data = "";
                var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data); // Serialize object to JSON
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                     (sender, cert, chain, sslPolicyErrors) => true;

                HttpResponseMessage response = await httpClient.GetAsync(endpoint + "Recipe");

                //// Check for success
                if (response.IsSuccessStatusCode)
                {

                        String json = response.Content.ReadAsStringAsync().Result;

                        list =  JsonConvert.DeserializeObject<List<RecipDataModel>>(json);
                }
                    


                }
                catch (Exception ex)
            {
                ex.ToString();
            }


            
            return Json(list);
           // return Json(new { success = true, data = list  });

        }



        [HttpPost]
        public IActionResult SubmitText(MainModel model)
        {


            ViewData["Message"] = "Success";


            return View("Main"); // or RedirectToAction(...)
        }



        [HttpPost]
        public async Task<IActionResult> SubmitRegis([FromBody] MainModel model)
        {
     
            String strStatus = "";
            Boolean isSuccess = true;

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var data = new
            {
               
                Uername = model.UserNameInput, 
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.PasswordReg
            };

            var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data); // Serialize object to JSON
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                 (sender, cert, chain, sslPolicyErrors) => true;

                HttpResponseMessage response = await httpClient.PostAsync(endpoint + "Register", content);

              

                //// Check for success
                if (response.IsSuccessStatusCode)
                {
                    
                   

                    string escapedJson = response.Content.ReadAsStringAsync().Result.ToString();

                    string json = JsonConvert.DeserializeObject<string>(escapedJson);

                    ResponeModel list = JsonConvert.DeserializeObject<ResponeModel>(json);

                    if (list.message.Equals("Already User"))
                    {
                        isSuccess = false;
                        strStatus = "มีสมัครใช้งานด้วย email นี้แล้ว!";
                    }
                    else
                    {
                        isSuccess = true;
                        strStatus = "สมัครเรียบร้อย";
                    }
                   

                }
                else
                {
                    strStatus = "Fails";
                    isSuccess = false;
                    Console.WriteLine("Error: " + response.StatusCode + " - " + response.ReasonPhrase);
                    
                }

              

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                //return false;
            }


            ViewData["UserName"] = model.UserNameInput;

            ViewData["Message"] = strStatus;


            return Json(new { success = isSuccess, message = strStatus , username = model.UserNameInput });
        }

        [HttpPost]
        public async Task<IActionResult> Rating([FromBody] RecipDataModel model)
        {

            String msgResult = "";
            bool iSComplete = false;
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var data = new
            {

                UserOwner = model.UserOwner,
                Rating = model.Rating,
                RecipeID = model.RecipeID,

            };

            var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data); // Serialize object to JSON
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                 (sender, cert, chain, sslPolicyErrors) => true;

                HttpResponseMessage response = await httpClient.PostAsync(endpoint + "SetRating", content);


                //// Check for success
                if (response.IsSuccessStatusCode)
                {
                    string escapedJson = response.Content.ReadAsStringAsync().Result.ToString();

                    string json = JsonConvert.DeserializeObject<string>(escapedJson);

                    ResponeModel list = JsonConvert.DeserializeObject<ResponeModel>(json);

                    if (list.message.Equals("Exists"))
                    {
                        msgResult = "คุณเคยให้คะแนนสูตรอาหารนี้แล้ว!";
                    }
                    else
                    {
                        msgResult = "ขอบคุณ";
                        iSComplete = true;
                    }
                   

                }
                else
                {

                    Console.WriteLine("Error: " + response.StatusCode + " - " + response.ReasonPhrase);

                }



            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                //return false;
            }


            return Json(new { success = iSComplete, message = msgResult });
        }



        [HttpPost]
        public async Task<IActionResult> SubmitRecipeAsync(RecipeModel model)
        {

           
            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);

            var filePath = Path.Combine(uploads, model.ImageRecipe.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.ImageRecipe.CopyToAsync(stream);
            }



            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var data = new
            {
                MenuName = model.MenuName,
                Ingredient = model.Ingredient,
                Step = model.Step,
                Times = model.Time,
                Levels = model.Level,
                ImageRecipePath = model.ImageRecipe.FileName,
                UserOwner = model.UserID,
                RecipeID = model.RecipeID
            };

            var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data); // Serialize object to JSON
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                 (sender, cert, chain, sslPolicyErrors) => true;


                HttpResponseMessage response = await httpClient.PostAsync(endpoint + "AddRecipe", content);


                //// Check for success
                if (response.IsSuccessStatusCode)
                {

                    ViewData["MessageResult"] = "บันทึกเรียบร้อย" ;
                }
                else
                {

                    Console.WriteLine("Error: " + response.StatusCode + " - " + response.ReasonPhrase);

                }

            }
            catch (Exception ex)
            {

            }

            String directpag = "";
            if (model.RecipeID != null)
            {
                directpag = "RecipeEdit";
            }
            else
            {
                directpag = "RecipeManage";
            }

                return View(directpag); // or RedirectToAction(...)
        }








        [HttpPost]
        public async Task<IActionResult> Login([FromBody] MainModel model)
        {

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string username = "";
            string userID = "";
            bool isSuccess = false;
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                 (sender, cert, chain, sslPolicyErrors) => true;


                HttpResponseMessage response = await httpClient.GetAsync(endpoint + "Login/?username=" + model.UserNameInput + "&password=" + model.PasswordInput + "");

                //// Check for success
                if (response.IsSuccessStatusCode)
                {

                    String json = response.Content.ReadAsStringAsync().Result;

                    List<User> list = JsonConvert.DeserializeObject<List<User>>(json);

                    if (list.Count>0)
                    {
                        isSuccess = true;
                        username = list[0].Uername;
                        userID = list[0].UerID;
                    }


                }
                else
                {
                    //strStatus = "Fails";
                    //isSuccess = false;
                    Console.WriteLine("Error: " + response.StatusCode + " - " + response.ReasonPhrase);

                }

            }
            catch (Exception ex)
            {

            }


            return Json(new { success = isSuccess, username = username, userID = userID });// or RedirectToAction(...)
        }






        [HttpPost]
        public async Task<IActionResult> GetMyRecipe([FromBody]  string  id)
        {


            List<RecipDataModel> list = new List<RecipDataModel>();

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));



            //var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data); // Serialize object to JSON
            //var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                 (sender, cert, chain, sslPolicyErrors) => true;


                HttpResponseMessage response = await httpClient.GetAsync(endpoint + id);

                //    //// Check for success
                if (response.IsSuccessStatusCode)
                {
                    //        //strStatus = "success";
                    //        //isSuccess = true;
                    String json = response.Content.ReadAsStringAsync().Result;

                    list = JsonConvert.DeserializeObject<List<RecipDataModel>>(json);
                }
                else
                {
                    //        //strStatus = "Fails";
                    //        //isSuccess = false;
                    //        Console.WriteLine("Error: " + response.StatusCode + " - " + response.ReasonPhrase);

                }

            }
            catch (Exception ex)
            {

            }


            return Json(list);// or RedirectToAction(...)
        }




        [HttpPost]
        public async Task<IActionResult> Del([FromBody] string id)
        {

            List<RecipDataModel> list = new List<RecipDataModel>();
            try
            {

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var data = "";
                var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data); // Serialize object to JSON
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");


                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                     (sender, cert, chain, sslPolicyErrors) => true;

                HttpResponseMessage response = await httpClient.DeleteAsync(endpoint + id);

                //// Check for success
                if (response.IsSuccessStatusCode)
                {

                    String json = response.Content.ReadAsStringAsync().Result;

                    list = JsonConvert.DeserializeObject<List<RecipDataModel>>(json);
                }



            }
            catch (Exception ex)
            {
                ex.ToString();
            }



        

            return Json(new { success = true, message = "Invalid data"});
        }






        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



    }
}
