export const environment = {
  production: true,
  auth: {
    domain: "santaponecentral.us.auth0.com",
    clientId: "U1PyIC5MkHe4fy8Rf9V0vDUQRlnbA8NS",
    audience: "https://santaponecentral-api.azurewebsites.net/",
    redirectUri: window.location.origin,
    errorPath: "/error",
  },
  httpInterceptor: {
    allowedList: [
      {
        uri: "https://prod-spc-clientapi.azurewebsites.net/*",
        allowAnonymous: true
      },
      {
        uri: "https://prod-spc-eventapi.azurewebsites.net/*",
        allowAnonymous: true
      },
      {
        uri: "https://prod-spc-messageapi.azurewebsites.net/*",
        allowAnonymous: true
      },
      {
        uri: "https://prod-spc-profileapi.azurewebsites.net/*",
        allowAnonymous: true
      },
      {
        uri: "https://prod-spc-searchapi.azurewebsites.net/*",
        allowAnonymous: true
      },
      {
        uri: "https://prod-spc-sharktankapi.azurewebsites.net/*",
        allowAnonymous: true
      },
      {
        uri: "https://prod-spc-surveyapi.azurewebsites.net/*",
        allowAnonymous: true
      }
    ],
  },
  // API ENDPOINTS
  clientServiceEndpoint: "https://prod-spc-clientapi.azurewebsites.net/api/",
  eventServiceEndpoint: "https://prod-spc-eventapi.azurewebsites.net/api/",
  messageServiceEndpoint: "https://prod-spc-messageapi.azurewebsites.net/api/",
  profileServiceEndpoint: "https://prod-spc-profileapi.azurewebsites.net/api/",
  searchServiceEndpoint: "https://prod-spc-searchapi.azurewebsites.net/api/",
  sharkTankServiceEndpoint: "https://prod-spc-sharktankapi.azurewebsites.net/api/",
  surveyServiceEndpoint: "https://prod-spc-surveyapi.azurewebsites.net/api/"
};
