// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
    production: false,
    auth0Domain: "memelydev.auth0.com",
    auth0Client_id: "KvZyPvtRblUBt2clTAmJx84RT4mwmZ3L",
    auth0Redirect_uri: `${window.location.origin}`,
    auth0Audience: "https://dev-santaponecentral-api.azurewebsites.net/api/",
    // API ENDPOINTS
    clientServiceEndpoint: "https://dev-spc-clientapi.azurewebsites.net",
    eventServiceEndpoint: "https://dev-spc-eventapi.azurewebsites.net",
    messageServiceEndpoint: "https://dev-spc-messageapi.azurewebsites.net",
    profileServiceEndpoint: "https://dev-spc-profileapi.azurewebsites.net",
    searchServiceEndpoint: "https://dev-spc-searchapi.azurewebsites.net",
    sharkTankServiceEndpoint: "https://dev-spc-sharktankapi.azurewebsites.net",
    surveyServiceEndpoint: "https://dev-spc-surveyapi.azurewebsites.net"
  };

  