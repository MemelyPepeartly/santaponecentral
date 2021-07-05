import * as config from '../app/auth/dev-auth_config.json';

const { domain, clientId, audience, apiUri, errorPath } = config as {
  domain: string;
  clientId: string;
  audience?: string;
  apiUri: string;
  errorPath: string;
};

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
    allowedList: [`http://localhost:3010/*`, `https://localhost:6001/*`],
  },
    // // API ENDPOINTS
    // clientServiceEndpoint: "https://localhost:5001/api/",
    // eventServiceEndpoint: "https://dev-spc-eventapi.azurewebsites.net/api/",
    // messageServiceEndpoint: "https://dev-spc-messageapi.azurewebsites.net/api/",
    // profileServiceEndpoint: "https://localhost:8001/api/",
    // searchServiceEndpoint: "https://dev-spc-searchapi.azurewebsites.net/api/",
    // sharkTankServiceEndpoint: "https://localhost:7001/api/",
    surveyServiceEndpoint: "https://localhost:6001/api/",

    // API ENDPOINTS
    clientServiceEndpoint: "https://dev-spc-clientapi.azurewebsites.net/api/",
    eventServiceEndpoint: "https://dev-spc-eventapi.azurewebsites.net/api/",
    messageServiceEndpoint: "https://dev-spc-messageapi.azurewebsites.net/api/",
    profileServiceEndpoint: "https://dev-spc-profileapi.azurewebsites.net/api/",
    searchServiceEndpoint: "https://dev-spc-searchapi.azurewebsites.net/api/",
    sharkTankServiceEndpoint: "https://dev-spc-sharktankapi.azurewebsites.net/api/",
    //surveyServiceEndpoint: "https://dev-spc-surveyapi.azurewebsites.net/api/"
  };

  