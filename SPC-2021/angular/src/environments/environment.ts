export const environment = {
  production: false,
  auth: {
    domain: "memelydev.auth0.com",
    clientId: "KvZyPvtRblUBt2clTAmJx84RT4mwmZ3L",
    audience: "https://dev-santaponecentral-api.azurewebsites.net/api/",
    redirectUri: window.location.origin,
    errorPath: "/error",
  },
  httpInterceptor: {
    allowedList: [
      {
        uri: "https://dev-spc-clientapi.azurewebsites.net/*",
        allowAnonymous: true
      },
      {
        uri: "https://dev-spc-eventapi.azurewebsites.net/*",
        allowAnonymous: true
      },
      {
        uri: "https://dev-spc-messageapi.azurewebsites.net/*",
        allowAnonymous: true
      },
      {
        uri: "https://dev-spc-profileapi.azurewebsites.net/*",
        allowAnonymous: true
      },
      {
        uri: "https://dev-spc-searchapi.azurewebsites.net/*",
        allowAnonymous: true
      },
      {
        uri: "https://dev-spc-sharktankapi.azurewebsites.net/*",
        allowAnonymous: true
      },
      {
        uri: "https://dev-spc-surveyapi.azurewebsites.net/*",
        allowAnonymous: true
      }
    ]
  },
  // API ENDPOINTS
  clientServiceEndpoint: "https://dev-spc-clientapi.azurewebsites.net/api/",
  eventServiceEndpoint: "https://dev-spc-eventapi.azurewebsites.net/api/",
  messageServiceEndpoint: "https://dev-spc-messageapi.azurewebsites.net/api/",
  profileServiceEndpoint: "https://dev-spc-profileapi.azurewebsites.net/api/",
  searchServiceEndpoint: "https://dev-spc-searchapi.azurewebsites.net/api/",
  sharkTankServiceEndpoint: "https://dev-spc-sharktankapi.azurewebsites.net/api/",
  surveyServiceEndpoint: "https://dev-spc-surveyapi.azurewebsites.net/api/"
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
