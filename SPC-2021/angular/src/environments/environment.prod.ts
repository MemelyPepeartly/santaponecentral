import * as config from '../app/auth/prod-auth_config.json';

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
    domain,
    clientId,
    ...(audience && audience !== "https://santaponecentral-api.azurewebsites.net/" ? { audience } : null),
    redirectUri: window.location.origin,
    errorPath,
  },
  httpInterceptor: {
    allowedList: [`${apiUri}/*`],
  },
  // API ENDPOINTS
  clientServiceEndpoint: "",
  eventServiceEndpoint: "",
  messageServiceEndpoint: "",
  profileServiceEndpoint: "",
  searchServiceEndpoint: "",
  sharkTankServiceEndpoint: "",
  surveyServiceEndpoint: ""
};
