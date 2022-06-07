### Sample of scraping amazon web pages

Simple web scraping of amazon web pages to get customer review on product list
Store them in SQLite code first database and expose through API

- To test solution, each project can be started individually

    - API :
Put in build directory (after build project) and start `dotnet WebScrapingAPI.dll`
    - Worker : 
Put in build directory (after build project) and start command `dotnet WebScrapingWorker.dll`

    Or debug project directly :
    - API / Worker :
Put in project directory directory and start command `dotnet watch run`