const config = (window as Window & { parthTradersConfig?: { apiUrl?: string } }).parthTradersConfig;
const apiUrl = config?.apiUrl || 'https://rparth07-parth-traders-api.onrender.com/';
export const AppSetting = { API_URL: apiUrl.replace(/\/+$/, '') + '/' };
