namespace Database.Models;

public class WeatherForecast
{
    public DateTime Date { get; set; }
    public string Summary { get; set; }
    public int TemperatureC { get; set; }

    public double TemperatureF => 32.0 + TemperatureC / 0.5556;

    public WeatherForecast(DateTime date, int temperatureC, string summary)
    {
        Date = date;
        TemperatureC = temperatureC;
        Summary = summary;
    }
}