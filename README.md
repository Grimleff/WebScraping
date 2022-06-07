### Sample of scraping amazon web pages

Simple web scraping of amazon web pages to get customer review on product list
Store them in SQLite code first database and expose through API, database ensure created at startup (API or Worker) and embedded default dataset (2 products)

- To test solution, each project can be started individually, for documentation API provide swagger index, worker is standalone (cycle of scraping is configurable in appsettings)

    - API :
Put in build directory (after build project) and start `dotnet WebScrapingAPI.dll` (listen on port 5000)
    - Worker : 
Put in build directory (after build project) and start command `dotnet WebScrapingWorker.dll` (listen on port 5005 for SignalR hub => notif for new reviews)

    Or debug project directly :
    - API / Worker :
Put in project directory directory and start command `dotnet watch run` (same port above)