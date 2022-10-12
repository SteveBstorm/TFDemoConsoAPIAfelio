using DemoConsoAPI;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

string url = "http://localhost:5128/api/";

HttpClient client = new HttpClient();
client.BaseAddress = new Uri(url);

Movie movie = new Movie();
movie.Title = "E6K Adventure";
movie.Synopsis = "Trop de bruit des travaux tue l'envie";
movie.ReleaseYear = 2022;
movie.PEGI = 18;
//Ajout de données
//string jsonToSend = JsonConvert.SerializeObject(movie);
//HttpContent content = new StringContent(jsonToSend, Encoding.UTF8, "application/json");

//using (HttpResponseMessage message = client.PostAsync("movie", content).Result)
//{
//    if(message.IsSuccessStatusCode)
//    {
//        Console.WriteLine("Enregistrement OK");
//    }
//}

//Récupération de données
List<Movie> movieList = new List<Movie>();

using (HttpResponseMessage response = client.GetAsync(url + "movie").Result)
{

    if (response.IsSuccessStatusCode)
    {
        string toto = response.Content.ReadAsStringAsync().Result;
        movieList = JsonConvert.DeserializeObject<List<Movie>>(toto);
    }

}

foreach (Movie m in movieList)
{
    Console.WriteLine(m.Title);
}

LoginForm form = new LoginForm
{
    Email = "admin@test.com",
    Password = "test1234"
};

string json = JsonConvert.SerializeObject(form);
HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

ConnectedUser user = new ConnectedUser();
using (HttpResponseMessage message = client.PostAsync("user/login", content).Result)
{
    try
    {
        message.EnsureSuccessStatusCode();

        string receivedJson = message.Content.ReadAsStringAsync().Result;
        user = JsonConvert.DeserializeObject<ConnectedUser>(receivedJson);

    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }

}

client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
using (HttpResponseMessage message = client.DeleteAsync("movie/1").Result)
{
    try
    {
        message.EnsureSuccessStatusCode();

    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }

}
Console.WriteLine();
Console.WriteLine("----------Après delete------");
Console.WriteLine();
movieList = new List<Movie>();

using (HttpResponseMessage response = client.GetAsync(url + "movie").Result)
{

    if (response.IsSuccessStatusCode)
    {
        string toto = response.Content.ReadAsStringAsync().Result;
        movieList = JsonConvert.DeserializeObject<List<Movie>>(toto);
    }

}

foreach (Movie m in movieList)
{
    Console.WriteLine(m.Title);
}