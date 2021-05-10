// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
    production: false,
    apiUrl: "https://localhost:5001/api/",
    auth0Domain: "memelydev.auth0.com",
    auth0Client_id: "KvZyPvtRblUBt2clTAmJx84RT4mwmZ3L",
    auth0Redirect_uri: `${window.location.origin}`,
    auth0Audience: "https://dev-santaponecentral-api.azurewebsites.net/api/"
  };

  