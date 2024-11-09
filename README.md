# Custom.Weather plugin for Wox
 A customizable weather plugin for Wox v1.\*.\*</br>
 Weather data retrieval from free and open-source weather API [Open-Meteo](https://open-meteo.com).

## Installation
Install by typing ```wpm install Custom.Weather``` into Wox.  
Alternatively, you can download it from the Releases tab here or from the [wox plugin site](http://www.wox.one/plugin/430).

## Look and Settings
<img width="600" src="Images\\example.png"/>

## Forecast
View a weather forecast of 7 days by adding a ``!`` after the city or simply by clicking/pressing the Enter key on a result.
</br></br>
<img width="600" src="Images\\forecast.png"/>
</br></br>
If the city name is ambiguous, you can also add a number after the ``!``.  
In this example, the second result in the list is selected, which shows a forecast for London in Canada, rather than for London in the UK.
</br></br>
<img width="600" src="Images\\forecast_selection.png"/>

## TODO ##
- [x] Retrieve and show more weather data
- [x] Cache Geocoding data to reduce requests
- [x] Implement home town and add more settings
- [x] Allow customization of data display
- [ ] Add support for non-latin scripts
- [x] Show forecast
- [x] Upgrade .NET Framework (now 4.8.1)
- [ ] Implement Wox Proxy
