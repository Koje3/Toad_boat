# Unity Google Spreadsheet Downloader and Parser

## Usage

- Publish your Google Spreadsheet to the web.
- Create a `SpreadsheetDownloaderConfig` asset via `Assets > Create > Spreadsheet Downloader Config` menu.
- Fill in all required information needed for the download process. 
	- Spreadsheet Id in published link: `docs.google.com/spreadsheets/d/e/<SPREADSHEET_ID>/pubhtml`
	- Sheet Gid in the URL: `docs.google.com/spreadsheets/d/<...>/edit#gid=<SHEET_GID>`

- Download your (CSV) files to a Resource -folder from the SpreadsheetDownloaderConfig -asset.
- Copy the spreadsheet -header row names (strict) to relevant variables in RowDefinitions.cs. Nullable variables mean optional cells.
- In GameData.cs create 'public static array' for each of your your spreadsheet RowDefinitions/BaseRow

- Call GameData.Load() when initializing at runtime. Check the example asset in 'GoogleSpreadsheetData/data/Examples/Example Data Loader' if you need an idea. There is also a 'Canvas_Data_to_UiElement' -prefab to test how you could load text, sprites or audioPrefabs from data.